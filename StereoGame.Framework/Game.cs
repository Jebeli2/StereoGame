namespace StereoGame.Framework
{
    using StereoGame.Framework.Audio;
    using StereoGame.Framework.Content;
    using StereoGame.Framework.Graphics;
    using System.Diagnostics;

    public class Game : IDisposable
    {
        private bool isDisposed;
        private static Game instance = null!;
        private readonly GameServiceContainer services;
        private readonly GameComponentCollection components;
        private readonly ContentManager content;
        private readonly GamePlatform platform;
        private readonly Stopwatch gameTimer = new();
        private bool initialzed;
        private bool isFixedTimeStep = true;
        private const double oneSecInMS = 1000.0;
        private TimeSpan targetElapsedTime = TimeSpan.FromTicks(166667);
        private TimeSpan inactiveSleepTime = TimeSpan.FromSeconds(0.02);
        private TimeSpan maxElapsedTime = TimeSpan.FromMilliseconds(500);

        private TimeSpan accumulatedElapsedTime;
        private readonly GameTime gameTime = new();
        private long previousTicks;
        private int updateFrameLag;

        private bool shouldExit;
        private bool suppressDraw;

        private IGraphicsDeviceManager? graphicsDeviceManager;
        private IGraphicsDeviceService? graphicsDeviceService;
        private GraphicsDevice? graphicsDevice;
        private AudioDevice? audioDevice;

        private readonly SortingFilteringCollection<IDrawable> drawables =
                   new(d => d.Visible,
                       (d, handler) => d.VisibleChanged += handler,
                       (d, handler) => d.VisibleChanged -= handler,
                       (d1, d2) => Comparer<int>.Default.Compare(d1.DrawOrder, d2.DrawOrder),
                       (d, handler) => d.DrawOrderChanged += handler,
                       (d, handler) => d.DrawOrderChanged -= handler);

        private readonly SortingFilteringCollection<IUpdateable> updateables =
            new(u => u.Enabled,
                (u, handler) => u.EnabledChanged += handler,
                (u, handler) => u.EnabledChanged -= handler,
                (u1, u2) => Comparer<int>.Default.Compare(u1.UpdateOrder, u2.UpdateOrder),
                (u, handler) => u.UpdateOrderChanged += handler,
                (u, handler) => u.UpdateOrderChanged -= handler);

        private static readonly Action<IDrawable, GameTime> DrawAction = (drawable, gameTime) => drawable.Draw(gameTime);
        private static readonly Action<IUpdateable, GameTime> UpdateAction = (updateable, gameTime) => updateable.Update(gameTime);

        public Game()
        {
            instance = this;
            services = new GameServiceContainer();
            components = new GameComponentCollection();
            content = new ContentManager(this);
            platform = GamePlatform.PlatformCreate(this);
            platform.Activated += OnActivated;
            platform.Deactivated += OnDeactivated;
            services.AddService(typeof(GamePlatform), platform);
        }

        ~Game()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    for (int i = 0; i < components.Count; i++)
                    {
                        if (components[i] is IDisposable disposable)
                        {
                            disposable.Dispose();
                        }
                    }
                    components.Clear();
                    content.Dispose();
                    platform.Activated -= OnActivated;
                    platform.Deactivated -= OnDeactivated;
                    services.RemoveService(typeof(GamePlatform));
                    platform.Dispose();
                }
                isDisposed = true;
                instance = null!;
            }
        }

        internal static Game Instance => instance!;

        public GameServiceContainer Services => services;
        public GameComponentCollection Components => components;
        public ContentManager Content => content;
        public GamePlatform Platform => platform;

        public AudioDevice AudioDevice
        {
            get
            {
                if (audioDevice == null)
                {
                    audioDevice = platform.CreateAudioDevice();
                }
                return audioDevice;
            }
        }

        public GraphicsDevice GraphicsDevice
        {
            get
            {
                if (graphicsDevice == null)
                {
                    if (graphicsDeviceService == null)
                    {
                        graphicsDeviceService = (IGraphicsDeviceService)Services.GetService(typeof(IGraphicsDeviceService))!;
                        if (graphicsDeviceService == null)
                        {
                            throw new InvalidOperationException("No Graphics Device Service");
                        }
                    }
                    graphicsDevice = graphicsDeviceService.GraphicsDevice;
                    if (graphicsDevice == null)
                    {
                        throw new InvalidOperationException("No Graphics Device");
                    }
                }
                return graphicsDevice;
            }
        }

        internal GraphicsDeviceManager GraphicsDeviceManager
        {
            get
            {
                graphicsDeviceManager ??= (IGraphicsDeviceManager)Services.GetService(typeof(IGraphicsDeviceManager))!;
                return (GraphicsDeviceManager)graphicsDeviceManager;
            }
            set
            {
                if (graphicsDeviceManager != null)
                {
                    throw new InvalidOperationException("GraphicsDeviceManager already registered for this Game object");
                }
                graphicsDeviceManager = value;
            }
        }

        public GameWindow Window
        {
            get { return platform.Window; }
        }
        public TimeSpan InactiveSleepTime
        {
            get { return inactiveSleepTime; }
            set
            {
                if (value < TimeSpan.Zero) throw new ArgumentException("The time must be positive.");
                inactiveSleepTime = value;
            }
        }

        public TimeSpan MaxElapsedTime
        {
            get { return maxElapsedTime; }
            set
            {
                if (value < TimeSpan.Zero) throw new ArgumentException("The time must be positive.");

                if (value < targetElapsedTime) throw new ArgumentException("The time must be at least TargetElapsedTime");

                maxElapsedTime = value;
            }
        }

        public bool IsActive => platform.IsActive;

        public TimeSpan TargetElapsedTime
        {
            get { return targetElapsedTime; }
            set
            {
                value = platform.TargetElapsedTimeChanging(value);

                if (value <= TimeSpan.Zero) throw new ArgumentException("The time must be positive and non-zero.");

                if (value > maxElapsedTime) throw new ArgumentException("The time can not be larger than MaxElapsedTime");

                if (value != targetElapsedTime)
                {
                    targetElapsedTime = value;
                    platform.TargetElapsedTimeChanged();
                }
            }
        }

        public int TargetFPS
        {
            get { return (int)(oneSecInMS / targetElapsedTime.TotalMilliseconds); }
            set { TargetElapsedTime = TimeSpan.FromMilliseconds(oneSecInMS / value); }

        }

        public bool IsFixedTimeStep
        {
            get => isFixedTimeStep;
            set => isFixedTimeStep = value;
        }

        public void Run()
        {
            Run(platform.DefaultRunBehavior);
        }

        public void Run(GameRunBehavior runBehavior)
        {
            AssertNotDisposed();
            if (!platform.BeforeRun())
            {
                BeginRun();
                gameTimer.Restart();
                return;
            }
            if (!initialzed)
            {
                DoInitialize();
                initialzed = true;
            }
            BeginRun();
            gameTimer.Restart();
            switch (runBehavior)
            {
                case GameRunBehavior.Asynchronous:
                    platform.AsyncRunLoopEnded += Platform_AsyncRunLoopEnded;
                    platform.StartRunLoop();
                    break;
                case GameRunBehavior.Synchronous:
                    DoUpdate(new GameTime());
                    platform.RunLoop();
                    EndRun();
                    DoExiting();
                    break;
                default:
                    throw new ArgumentException(string.Format("Handling for the run behavior {0} is not implemented.", runBehavior));
            }
        }

        public void Tick()
        {
            RetryTick:
            if (!IsActive && InactiveSleepTime.TotalMilliseconds >= 1.0)
            {
                Thread.Sleep((int)InactiveSleepTime.TotalMilliseconds);
            }
            var currentTicks = gameTimer.ElapsedTicks;
            accumulatedElapsedTime += TimeSpan.FromTicks(currentTicks - previousTicks);
            previousTicks = currentTicks;
            if (IsFixedTimeStep && accumulatedElapsedTime < TargetElapsedTime)
            {
                var sleepTime = (TargetElapsedTime - accumulatedElapsedTime).TotalMilliseconds;
                if (sleepTime >= 2.0)
                {
                    Thread.Sleep(1);
                }
                goto RetryTick;
            }
            if (accumulatedElapsedTime > maxElapsedTime) { accumulatedElapsedTime = maxElapsedTime; }
            if (IsFixedTimeStep)
            {
                gameTime.ElapsedGameTime = TargetElapsedTime;
                var stepCount = 0;

                while (accumulatedElapsedTime >= TargetElapsedTime && !shouldExit)
                {
                    gameTime.TotalGameTime += TargetElapsedTime;
                    accumulatedElapsedTime -= TargetElapsedTime;
                    ++stepCount;

                    DoUpdate(gameTime);
                }

                updateFrameLag += Math.Max(0, stepCount - 1);

                if (gameTime.IsRunningSlowly)
                {
                    if (updateFrameLag == 0)
                        gameTime.IsRunningSlowly = false;
                }
                else if (updateFrameLag >= 5)
                {
                    gameTime.IsRunningSlowly = true;
                }

                if (stepCount == 1 && updateFrameLag > 0)
                    updateFrameLag--;

                gameTime.ElapsedGameTime = TimeSpan.FromTicks(TargetElapsedTime.Ticks * stepCount);
            }
            else
            {
                gameTime.ElapsedGameTime = accumulatedElapsedTime;
                gameTime.TotalGameTime += accumulatedElapsedTime;
                accumulatedElapsedTime = TimeSpan.Zero;

                DoUpdate(gameTime);
            }

            if (suppressDraw)
                suppressDraw = false;
            else
            {
                DoDraw(gameTime);
            }

            if (shouldExit)
            {
                platform.Exit();
                shouldExit = false;
            }
        }

        public event EventHandler<EventArgs>? Activated;
        public event EventHandler<EventArgs>? Deactivated;
        public event EventHandler<EventArgs>? Disposed;
        public event EventHandler<EventArgs>? Exiting;

        private void Platform_AsyncRunLoopEnded(object? sender, EventArgs e)
        {
            AssertNotDisposed();
            if (sender is GamePlatform platform)
            {
                platform.AsyncRunLoopEnded -= Platform_AsyncRunLoopEnded;
            }
            EndRun();
            DoExiting();
        }

        protected virtual void OnActivated(object? sender, EventArgs args)
        {
            AssertNotDisposed();
            Activated?.Invoke(sender, args);
        }
        protected virtual void OnDeactivated(object? sender, EventArgs args)
        {
            AssertNotDisposed();
            Deactivated?.Invoke(sender, args);
        }

        protected virtual void OnExiting(object sender, EventArgs args)
        {
            Exiting?.Invoke(this, args);
        }
        protected virtual bool BeginDraw() { return true; }
        protected virtual void EndDraw()
        {
            platform.Present();
        }
        protected virtual void BeginRun() { }
        protected virtual void EndRun() { }
        protected virtual void LoadContent() { }
        protected virtual void UnloadContent() { }
        protected virtual void Initialize()
        {
            InitializeExistingComponents();
            graphicsDeviceService = (IGraphicsDeviceService)Services.GetService(typeof(IGraphicsDeviceService))!;

            if (graphicsDeviceService != null &&
                graphicsDeviceService.GraphicsDevice != null)
            {
                LoadContent();
            }
        }

        private void InitializeExistingComponents()
        {
            for (int i = 0; i < Components.Count; ++i)
                Components[i].Initialize();
        }
        internal void DoUpdate(GameTime gameTime)
        {
            AssertNotDisposed();
            if (platform.BeforeUpdate(gameTime))
            {
                //FrameworkDispatcher.Update();

                Update(gameTime);

                //The TouchPanel needs to know the time for when touches arrive
                //TouchPanelState.CurrentTimestamp = gameTime.TotalGameTime;
            }
        }

        internal void DoDraw(GameTime gameTime)
        {
            AssertNotDisposed();
            if (platform.BeforeDraw(gameTime) && BeginDraw())
            {
                Draw(gameTime);
                EndDraw();
            }
        }
        internal void DoInitialize()
        {
            AssertNotDisposed();
            if (graphicsDevice == null && graphicsDeviceManager != null)
            {
                graphicsDeviceManager.CreateDevice();
            }
            platform.BeforeInitialize();
            Initialize();

            CategorizeComponents();
            components.ComponentAdded += Components_ComponentAdded;
            components.ComponentRemoved += Components_ComponentRemoved;
            Log("Game Initialized");
        }

        private void Components_ComponentAdded(object? sender, GameComponentCollectionEventArgs e)
        {
            e.GameComponent.Initialize();
            CategorizeComponent(e.GameComponent);
        }

        private void Components_ComponentRemoved(object? sender, GameComponentCollectionEventArgs e)
        {
            DecategorizeComponent(e.GameComponent);
        }

        private void CategorizeComponents()
        {
            DecategorizeComponents();
            for (int i = 0; i < Components.Count; ++i)
                CategorizeComponent(Components[i]);
        }

        private void DecategorizeComponents()
        {
            updateables.Clear();
            drawables.Clear();
        }

        private void CategorizeComponent(IGameComponent component)
        {
            if (component is IUpdateable updateable) updateables.Add(updateable);
            if (component is IDrawable drawable) drawables.Add(drawable);
        }

        private void DecategorizeComponent(IGameComponent component)
        {
            if (component is IUpdateable updateable) updateables.Remove(updateable);
            if (component is IDrawable drawable) drawables.Remove(drawable);
        }
        internal void DoExiting()
        {
            OnExiting(this, EventArgs.Empty);
            UnloadContent();
            Log("Game Exited");
        }

        protected virtual void Draw(GameTime gameTime)
        {
            drawables.ForEachFilteredItem(DrawAction, gameTime);
        }

        protected virtual void Update(GameTime gameTime)
        {
            updateables.ForEachFilteredItem(UpdateAction, gameTime);
        }
        [DebuggerNonUserCode]
        private void AssertNotDisposed()
        {
            if (isDisposed)
            {
                string name = GetType().Name;
                throw new ObjectDisposedException(name, string.Format("The {0} object was used after being Disposed.", name));
            }
        }

        [Conditional("DEBUG")]
        protected void Log(string message)
        {
            platform?.Log(message);
        }


        private class SortingFilteringCollection<T> : ICollection<T>
        {
            private readonly List<T> _items;
            private readonly List<AddJournalEntry<T>> _addJournal;
            private readonly Comparison<AddJournalEntry<T>> _addJournalSortComparison;
            private readonly List<int> _removeJournal;
            private readonly List<T> _cachedFilteredItems;
            private bool _shouldRebuildCache;

            private readonly Predicate<T> _filter;
            private readonly Comparison<T> _sort;
            private readonly Action<T, EventHandler<EventArgs>> _filterChangedSubscriber;
            private readonly Action<T, EventHandler<EventArgs>> _filterChangedUnsubscriber;
            private readonly Action<T, EventHandler<EventArgs>> _sortChangedSubscriber;
            private readonly Action<T, EventHandler<EventArgs>> _sortChangedUnsubscriber;

            public SortingFilteringCollection(
                Predicate<T> filter,
                Action<T, EventHandler<EventArgs>> filterChangedSubscriber,
                Action<T, EventHandler<EventArgs>> filterChangedUnsubscriber,
                Comparison<T> sort,
                Action<T, EventHandler<EventArgs>> sortChangedSubscriber,
                Action<T, EventHandler<EventArgs>> sortChangedUnsubscriber)
            {
                _items = new List<T>();
                _addJournal = new List<AddJournalEntry<T>>();
                _removeJournal = new List<int>();
                _cachedFilteredItems = new List<T>();
                _shouldRebuildCache = true;

                _filter = filter;
                _filterChangedSubscriber = filterChangedSubscriber;
                _filterChangedUnsubscriber = filterChangedUnsubscriber;
                _sort = sort;
                _sortChangedSubscriber = sortChangedSubscriber;
                _sortChangedUnsubscriber = sortChangedUnsubscriber;

                _addJournalSortComparison = CompareAddJournalEntry;
            }

            private int CompareAddJournalEntry(AddJournalEntry<T> x, AddJournalEntry<T> y)
            {
                int result = _sort(x.Item, y.Item);
                if (result != 0) return result;
                return x.Order - y.Order;
            }

            public void ForEachFilteredItem<TUserData>(Action<T, TUserData> action, TUserData userData)
            {
                if (_shouldRebuildCache)
                {
                    ProcessRemoveJournal();
                    ProcessAddJournal();

                    _cachedFilteredItems.Clear();
                    for (int i = 0; i < _items.Count; ++i)
                        if (_filter(_items[i]))
                            _cachedFilteredItems.Add(_items[i]);

                    _shouldRebuildCache = false;
                }

                for (int i = 0; i < _cachedFilteredItems.Count; ++i)
                    action(_cachedFilteredItems[i], userData);

                if (_shouldRebuildCache)
                    _cachedFilteredItems.Clear();
            }

            public void Add(T item)
            {
                _addJournal.Add(new AddJournalEntry<T>(_addJournal.Count, item));
                InvalidateCache();
            }

            public bool Remove(T item)
            {
                if (_addJournal.Remove(AddJournalEntry<T>.CreateKey(item)))
                    return true;

                var index = _items.IndexOf(item);
                if (index >= 0)
                {
                    UnsubscribeFromItemEvents(item);
                    _removeJournal.Add(index);
                    InvalidateCache();
                    return true;
                }
                return false;
            }

            public void Clear()
            {
                for (int i = 0; i < _items.Count; ++i)
                {
                    _filterChangedUnsubscriber(_items[i], Item_FilterPropertyChanged);
                    _sortChangedUnsubscriber(_items[i], Item_SortPropertyChanged);
                }

                _addJournal.Clear();
                _removeJournal.Clear();
                _items.Clear();

                InvalidateCache();
            }

            public bool Contains(T item)
            {
                return _items.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                _items.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return _items.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _items.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return ((System.Collections.IEnumerable)_items).GetEnumerator();
            }

            private static readonly Comparison<int> RemoveJournalSortComparison = (x, y) => Comparer<int>.Default.Compare(y, x);
            private void ProcessRemoveJournal()
            {
                if (_removeJournal.Count == 0)
                    return;

                _removeJournal.Sort(RemoveJournalSortComparison);
                for (int i = 0; i < _removeJournal.Count; ++i)
                    _items.RemoveAt(_removeJournal[i]);
                _removeJournal.Clear();
            }

            private void ProcessAddJournal()
            {
                if (_addJournal.Count == 0)
                    return;

                _addJournal.Sort(_addJournalSortComparison);

                int iAddJournal = 0;
                int iItems = 0;

                while (iItems < _items.Count && iAddJournal < _addJournal.Count)
                {
                    var addJournalItem = _addJournal[iAddJournal].Item;
                    if (_sort(addJournalItem, _items[iItems]) < 0)
                    {
                        SubscribeToItemEvents(addJournalItem);
                        _items.Insert(iItems, addJournalItem);
                        ++iAddJournal;
                    }
                    ++iItems;
                }

                for (; iAddJournal < _addJournal.Count; ++iAddJournal)
                {
                    var addJournalItem = _addJournal[iAddJournal].Item;
                    SubscribeToItemEvents(addJournalItem);
                    _items.Add(addJournalItem);
                }

                _addJournal.Clear();
            }

            private void SubscribeToItemEvents(T item)
            {
                _filterChangedSubscriber(item, Item_FilterPropertyChanged);
                _sortChangedSubscriber(item, Item_SortPropertyChanged);
            }

            private void UnsubscribeFromItemEvents(T item)
            {
                _filterChangedUnsubscriber(item, Item_FilterPropertyChanged);
                _sortChangedUnsubscriber(item, Item_SortPropertyChanged);
            }

            private void InvalidateCache()
            {
                _shouldRebuildCache = true;
            }

            private void Item_FilterPropertyChanged(object? sender, EventArgs e)
            {
                InvalidateCache();
            }

            private void Item_SortPropertyChanged(object? sender, EventArgs e)
            {
                if (sender is T item)
                {
                    var index = _items.IndexOf(item);

                    _addJournal.Add(new AddJournalEntry<T>(_addJournal.Count, item));
                    _removeJournal.Add(index);

                    UnsubscribeFromItemEvents(item);
                    InvalidateCache();
                }
            }
        }

        private readonly struct AddJournalEntry<T>
        {
            public readonly int Order;
            public readonly T Item;

            public AddJournalEntry(int order, T item)
            {
                Order = order;
                Item = item;
            }

            public static AddJournalEntry<T> CreateKey(T item)
            {
                return new AddJournalEntry<T>(-1, item);
            }

            public override int GetHashCode()
            {
                return Item?.GetHashCode() ?? 0;
            }

            public override bool Equals(object? obj)
            {
                if (obj is AddJournalEntry<T> entry)
                {
                    return Equals(Item, entry.Item);
                }
                return false;
            }
        }
    }
}