namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class Control
    {
        private static int nextId;

        private readonly int id;
        private Control? parent;
        private readonly List<Control> children = new();
        private int x;
        private int y;
        private int width;
        private int height;
        private int fixedWidth;
        private int fixedHeight;
        private int minWidth;
        private int minHeight;
        private int maxWidth;
        private int maxHeight;
        private Padding padding;
        private Padding margin;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch;
        private VerticalAlignment verticalAlignment = VerticalAlignment.Stretch;
        private HorizontalAlignment horizontalTextAlignment = HorizontalAlignment.Center;
        private VerticalAlignment verticalTextAlignment = VerticalAlignment.Center;
        private bool enabled;
        private bool visible;
        private bool focused;
        private bool hovered;
        private bool pressed;
        private bool _checked;
        private bool active;

        protected bool valid;
        protected bool superBitmap;
        private Texture? bitmap;

        protected ILayout? layout;
        private TextFont? font;
        private TextureRegion? backgroundRegion;
        private Color backgroundColor;
        private Color borderColor;
        private Color borderShineColor;
        private Color borderShadowColor;
        private int borderThickness;
        private Color textColor;


        private Skin skin;

        private string? text;
        private Icons icon;


        protected Control()
            : this(null)
        {
        }

        protected Control(Control? parent)
        {
            id = ++nextId;
            enabled = true;
            visible = true;
            maxWidth = int.MaxValue;
            maxHeight = int.MaxValue;
            skin = Skin.DefaultSkin;
            skin.Apply(this);
            parent?.Add(this);
        }

        public Skin Skin
        {
            get { return skin; }
            set
            {
                if (skin != value)
                {
                    skin = value;
                    skin.Apply(this);
                }
            }
        }

        public int Id => id;

        public Control? Parent
        {
            get { return parent; }
        }

        public Window? Window
        {
            get
            {
                Control? c = this;
                while (c != null)
                {
                    if (c is Window window) return window;
                    c = c.Parent;
                }
                return null;
            }
        }
        public Screen? Screen
        {
            get
            {
                Control? c = this;
                while (c != null)
                {
                    if (c is Screen screen) return screen;
                    c = c.Parent;
                }
                return null;
            }
        }

        public IEnumerable<Control> Children
        {
            get { return children; }
        }

        public bool Add(Control child)
        {
            if (!children.Contains(child))
            {
                children.Add(child);
                child.parent = this;
                return true;
            }
            return false;
        }

        public bool Remove(Control child)
        {
            if (children.Remove(child))
            {
                child.parent = null;
                return true;
            }
            return false;
        }

        public void ForEachChild(Action<Control, GameTime> action, GameTime gameTime)
        {
            for (int i = 0; i < children.Count; i++)
            {
                action(children[i], gameTime);
            }
        }
        public void ForEachChild(Action<Control, GameTime, int, int> action, GameTime gameTime, int x, int y)
        {
            for (int i = 0; i < children.Count; i++)
            {
                action(children[i], gameTime, x, y);
            }
        }
        public Size GetFixedSize()
        {
            return new Size(fixedWidth, fixedHeight);
        }

        public virtual Size GetPreferredSize(IGuiSystem context)
        {
            return layout?.GetPreferredSize(context, this) ?? new Size(width, height);
        }

        public void PerformLayout(IGuiSystem context)
        {
            if (layout != null)
            {
                layout.PerformLayout(context, this);
            }
            else
            {
                foreach (Control child in children)
                {
                    Size pref = child.GetPreferredSize(context);
                    Size fix = child.GetFixedSize();
                    int w = fix.Width != 0 ? fix.Width : pref.Width;
                    int h = fix.Height != 0 ? fix.Height : pref.Height;
                    child.Width = w;
                    child.Height = h;
                    child.PerformLayout(context);
                }
            }
            valid = false;
        }

        public virtual void Invalidate()
        {
            valid = false;
            parent?.Invalidate();
        }

        //public virtual void InvalidateLayout() { }


        public int X
        {
            get { return x; }
            set
            {
                if (x != value)
                {
                    x = value;
                }
            }
        }

        public int Y
        {
            get { return y; }
            set
            {
                if (y != value)
                {
                    y = value;
                }
            }
        }

        public int Width
        {
            get { return width; }
            set
            {
                if (width != value)
                {
                    width = value;
                }
            }
        }

        public int Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                }
            }
        }

        public int FixedWidth
        {
            get { return fixedWidth; }
            set
            {
                if (fixedWidth != value)
                {
                    fixedWidth = value;
                }
            }
        }

        public int FixedHeight
        {
            get { return fixedHeight; }
            set
            {
                if (fixedHeight != value)
                {
                    fixedHeight = value;
                }
            }
        }


        public int MinWidth
        {
            get { return minWidth; }
            set
            {
                if (minWidth != value)
                {
                    minWidth = value;
                }
            }
        }

        public int MinHeight
        {
            get { return minHeight; }
            set
            {
                if (minHeight != value)
                {
                    minHeight = value;
                }
            }
        }

        public int MaxWidth
        {
            get { return maxWidth; }
            set
            {
                if (maxWidth != value)
                {
                    maxWidth = value;
                }
            }
        }

        public int MaxHeight
        {
            get { return maxHeight; }
            set
            {
                if (maxHeight != value)
                {
                    maxHeight = value;
                }
            }
        }

        public Padding Margin
        {
            get { return margin; }
            set { margin = value; }
        }
        public Padding Padding
        {
            get { return padding; }
            set { padding = value; }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set
            {
                if (horizontalAlignment != value)
                {
                    horizontalAlignment = value;
                    Invalidate();
                }
            }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return verticalAlignment; }
            set
            {
                if (verticalAlignment != value)
                {
                    verticalAlignment = value;
                    Invalidate();
                }
            }
        }

        public HorizontalAlignment HorizontalTextAlignment
        {
            get { return horizontalTextAlignment; }
            set
            {
                if (horizontalTextAlignment != value)
                {
                    horizontalTextAlignment = value;
                    Invalidate();
                }
            }
        }

        public VerticalAlignment VerticalTextAlignment
        {
            get { return verticalTextAlignment; }
            set
            {
                if (verticalTextAlignment != value)
                {
                    verticalTextAlignment = value;
                    Invalidate();
                }
            }
        }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    skin.Apply(this);
                }
            }
        }

        public bool Visible
        {
            get { return visible; }
            set
            {
                if (visible != value)
                {
                    visible = value;
                    Invalidate();
                }
            }
        }

        public bool Focused
        {
            get { return focused; }
            set
            {
                if (focused != value)
                {
                    focused = value;
                    Invalidate();
                }
            }
        }

        public bool Active
        {
            get { return active; }
            set
            {
                if (active != value)
                {
                    active = value;
                    skin.Apply(this);
                }
            }
        }
        public bool Hovered
        {
            get { return hovered; }
            set
            {
                if (hovered != value)
                {
                    hovered = value;
                    skin.Apply(this);
                }
            }
        }

        public bool Pressed
        {
            get { return pressed; }
            set
            {
                if (pressed != value)
                {
                    pressed = value;
                    skin.Apply(this);
                }
            }
        }

        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    skin.Apply(this);
                }

            }
        }

        public TextureRegion? BackgroundRegion
        {
            get { return backgroundRegion; }
            set
            {
                if (backgroundRegion != value)
                {
                    backgroundRegion = value;
                    Invalidate();
                }
            }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                if (backgroundColor != value)
                {
                    backgroundColor = value;
                    Invalidate();
                }
            }
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                if (borderColor != value)
                {
                    borderColor = value;
                    Invalidate();
                }
            }
        }

        public Color BorderShineColor
        {
            get { return borderShineColor; }
            set
            {
                if (borderShineColor != value)
                {
                    borderShineColor = value;
                    Invalidate();
                }
            }
        }


        public Color BorderShadowColor
        {
            get { return borderShadowColor; }
            set
            {
                if (borderShadowColor != value)
                {
                    borderShadowColor = value;
                    Invalidate();
                }
            }
        }

        public int BorderThickness
        {
            get { return borderThickness; }
            set
            {
                if (borderThickness != value)
                {
                    borderThickness = value;
                    Invalidate();
                }
            }
        }

        public Color TextColor
        {
            get { return textColor; }
            set
            {
                if (textColor != value)
                {
                    textColor = value;
                    Invalidate();
                }
            }
        }

        public TextFont? Font
        {
            get
            {
                if (font != null) return font;
                if (parent != null) { return parent.Font; }
                if (skin != null) { return skin.DefaultFont; }
                return null;
            }
            set
            {
                if (font != value)
                {
                    font = value;
                    Invalidate();
                }
            }
        }

        public string? Text
        {
            get => text;
            set
            {
                if (text != value)
                {
                    text = value;
                    Invalidate();
                }
            }
        }

        public Icons Icon
        {
            get => icon;
            set
            {
                if (icon != value)
                {
                    icon = value;
                    Invalidate();
                }
            }
        }

        public bool SuperBitmap => superBitmap;
        public Texture? Bitmap => bitmap;

        public Rectangle BoundingRectangle
        {
            get
            {
                return GetBounds();
            }
        }

        public Rectangle ContentRectangle
        {
            get
            {
                var rect = BoundingRectangle;
                return new Rectangle(rect.Left + padding.Left, rect.Top + padding.Top, rect.Width - padding.Horizontal, rect.Height - padding.Vertical);
            }
        }

        public virtual Rectangle GetBounds()
        {
            Rectangle rect = new Rectangle(x, y, width, height);
            if (parent != null)
            {
                Rectangle parentRect = parent.GetBounds();
                rect.Offset(parentRect.Location);
            }
            return rect;
        }

        public bool Resize(IGuiSystem gui, int dx, int dy, HitTestResult hitTestResult)
        {
            int newX = x;
            int newY = y;
            int newWidth = width;
            int newHeight = height;
            switch (hitTestResult)
            {
                case HitTestResult.SizeLeft:
                    newX += dx;
                    newWidth -= dx;
                    break;
                case HitTestResult.SizeRight:
                    newWidth += dx;
                    break;
                case HitTestResult.SizeTop:
                    newY += dy;
                    newHeight -= dy;
                    break;
                case HitTestResult.SizeBottom:
                    newHeight += dy;
                    break;
            }
            if (newX < 0) { newX = 0; }
            if (newY < 0) { newY = 0; }
            if (parent != null)
            {
                int maxX = parent.Width- newWidth;
                int maxY = parent.Height - newHeight;
                if (newX > maxX) { newX = maxX; }
                if (newY > maxY) { newY = maxY; }
            }
            if (newX != x || newY != y || newWidth != width || newHeight != height)
            {
                x = newX;
                y = newY;
                width = newWidth;
                height = newHeight;
                fixedWidth = width;
                fixedHeight = height;
                PerformLayout(gui);
                //Invalidate();
                return true;
            }
            return false;
        }

        public bool Move(int dx, int dy)
        {
            int newX = x + dx;
            int newY = y + dy;
            if (newX < 0) { newX = 0; }
            if (newY < 0) { newY = 0; }
            if (parent != null)
            {
                int maxX = parent.Width - width;
                int maxY = parent.Height - height;
                if (newX > maxX) { newX = maxX; }
                if (newY > maxY) { newY = maxY; }
            }
            if (newX != x || newY != y)
            {
                x = newX;
                y = newY;
                return true;
            }
            return false;
        }

        public abstract Size GetContentSize(IGuiSystem context);

        //public virtual Size CalculateActualSize(IGuiSystem context)
        //{
        //    var desizredSize = GetContentSize(context) + margin.Size + padding.Size;
        //    if (desizredSize.Width < minWidth) { desizredSize.Width = minWidth; }
        //    if (desizredSize.Height < minHeight) { desizredSize.Height = minHeight; }
        //    if (desizredSize.Width > maxWidth) { desizredSize.Width = maxWidth; }
        //    if (desizredSize.Height > maxHeight) { desizredSize.Height = maxHeight; }
        //    var awidth = width == 0 ? desizredSize.Width : width;
        //    var aheight = height == 0 ? desizredSize.Height : height;
        //    return new Size(awidth, aheight);
        //}

        public virtual HitTestResult GetHitTestResult(int x, int y)
        {
            if (Contains(x, y))
            {
                return HitTestResult.Control;
            }
            return HitTestResult.None;
        }

        public virtual bool Contains(int x, int y)
        {
            return GetBounds().Contains(x, y);
        }

        public bool HasParent(Control control)
        {
            return parent != null && ((parent == control) || parent.HasParent(control));
        }
        public virtual void Update(IGuiSystem gui, GameTime gameTime) { }

        public virtual void Draw(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, int offsetX = 0, int offsetY = 0)
        {
            Rectangle rect = GetBounds();
            rect.Offset(offsetX, offsetY);
            if (backgroundRegion != null)
            {
                renderer.DrawRegion(backgroundRegion, rect);
            }
            else if (!backgroundColor.IsEmpty && backgroundColor != Color.Transparent)
            {
                renderer.FillRectangle(rect, backgroundColor);
            }
            if (borderThickness != 0)
            {
                renderer.DrawBorder(rect, borderShineColor, borderShadowColor, borderThickness);
            }
        }

        internal bool NeedsNewBitmap
        {
            get
            {
                return superBitmap && (!valid || bitmap == null);
            }
        }


        internal void PushBitmap(IGuiSystem gui)
        {
            CheckBitmap(gui);
            gui.GraphicsDevice.PushTarget(bitmap);
            gui.GraphicsDevice.Color = Color.FromArgb(0, 0, 0, 0);
            gui.GraphicsDevice.Clear();
        }

        internal void PopBitmap(IGuiSystem gui)
        {
            gui.GraphicsDevice.PopTarget();
            valid = true;
        }

        private void CheckBitmap(IGuiSystem gui)
        {
            if (bitmap == null || bitmap.Width < width || bitmap.Height < height)
            {
                InitBitmap(gui);
            }
        }

        private void InitBitmap(IGuiSystem gui)
        {
            bitmap?.Dispose();
            bitmap = gui.GraphicsDevice.CreateTexture(width, height);
            if (bitmap != null)
            {
                bitmap.BlendMode = BlendMode.Blend;
            }
        }

        public virtual bool OnPointerDown(PointerEventArgs args) { return true; }
        public virtual bool OnPointerUp(PointerEventArgs args) { return true; }
        public virtual bool OnPointerMove(PointerEventArgs args) { return true; }
        public virtual bool OnPointerEnter(PointerEventArgs args)
        {
            if (enabled && !hovered)
            {
                Hovered = true;
            }
            return true;
        }
        public virtual bool OnPointerLeave(PointerEventArgs args)
        {
            if (enabled && hovered)
            {
                Hovered = false;
            }
            return true;
        }

        protected void ToBack(Control control)
        {
            if (children.Count > 1)
            {
                int index = children.IndexOf(control);
                if (index >= 0)
                {
                    children.RemoveAt(index);
                    children.Insert(0, control);
                    Invalidate();
                }
            }
        }

        protected void ToFront(Control control)
        {
            if (children.Count > 1)
            {
                int index = children.IndexOf(control);
                if (index >= 0)
                {
                    children.RemoveAt(index);
                    children.Add(control);
                    Invalidate();
                }
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name} #{id}";
        }
    }
}
