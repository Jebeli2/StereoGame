namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Graphics;
    using StereoGame.Framework.Input;
    using StereoGame.Framework.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SdlGamePlatform : GamePlatform
    {
        private int isExiting;
        private readonly SdlGameWindow view;
        private SdlGraphicsDevice? renderer;
        public SdlGamePlatform(Game game) : base(game)
        {
            if (CurrentPlatform.OS == OS.Windows && Debugger.IsAttached)
            {
                Sdl.SetHint("SDL_WINDOWS_DISABLE_THREAD_NAMING", "1");
            }
            Sdl.Init((int)(Sdl.InitFlags.Video | Sdl.InitFlags.Joystick | Sdl.InitFlags.GameController | Sdl.InitFlags.Haptic));
            Sdl.DisableScreenSaver();
            SDL2Image.IMG_Init(SDL2Image.IMG_InitFlags.IMG_INIT_PNG);
            Window = view = new SdlGameWindow(game);
        }

        public override GameRunBehavior DefaultRunBehavior => GameRunBehavior.Synchronous;

        public override void BeforeInitialize()
        {
            base.BeforeInitialize();
        }

        public override void RunLoop()
        {
            Sdl.Window.Show(Window.Handle);
            while (true)
            {
                SdlRunLoop();
                Game.Tick();
                //Threading
                if (isExiting > 0) break;
            }
        }

        private void SdlRunLoop()
        {
            while (Sdl.PollEvent(out var ev) == 1)
            {
                switch (ev.Type)
                {
                    case Sdl.EventType.Quit:
                        isExiting++;
                        break;
                    case Sdl.EventType.JoyDeviceAdded:
                        break;
                    case Sdl.EventType.JoyDeviceRemoved:
                        break;
                    case Sdl.EventType.ControllerDeviceRemoved:
                        break;
                    case Sdl.EventType.ControllerButtonUp:
                    case Sdl.EventType.ControllerButtonDown:
                    case Sdl.EventType.ControllerAxisMotion:
                        break;
                    case Sdl.EventType.KeyDown:
                        break;
                    case Sdl.EventType.KeyUp:
                        break;
                    case Sdl.EventType.MouseWheel:
                        const int wheelDelta = 120;
                        view.MouseState.ScrollWheelValue = ev.Wheel.X * wheelDelta;
                        view.MouseState.HorizontalScrollWheelValue = ev.Wheel.Y * wheelDelta;
                        break;
                    case Sdl.EventType.MouseButtonDown:
                        view.MouseState.Buttons |= SdlMouseButtons2Buttons((Sdl.Mouse.Button)ev.Button.Button);
                        break;
                    case Sdl.EventType.MouseButtonup:
                        view.MouseState.Buttons &= (byte)~SdlMouseButtons2Buttons((Sdl.Mouse.Button)ev.Button.Button);
                        break;
                    case Sdl.EventType.MouseMotion:
                        view.MouseState.X = ev.Button.X;
                        view.MouseState.Y = ev.Button.Y;
                        break;
                    case Sdl.EventType.WindowEvent:
                        if (ev.Window.WindowID != view.ID) break;
                        switch (ev.Window.EventID)
                        {
                            case Sdl.Window.EventId.Resized:
                            case Sdl.Window.EventId.SizeChanged:
                                view.ClientResize(ev.Window.Data1, ev.Window.Data2);
                                break;
                            case Sdl.Window.EventId.FocusGained:
                                IsActive = true;
                                break;
                            case Sdl.Window.EventId.FocusLost:
                                IsActive = false;
                                break;
                            case Sdl.Window.EventId.Moved:
                                view.Moved();
                                break;
                            case Sdl.Window.EventId.Close:
                                isExiting++;
                                break;
                        }
                        break;
                }
            }
        }

        private static byte SdlMouseButtons2Buttons(Sdl.Mouse.Button state)
        {
            byte res = 0;
            if ((state & Sdl.Mouse.Button.Left) != 0) { res |= MouseState.LeftButtonFlag; }
            if ((state & Sdl.Mouse.Button.Middle) != 0) { res |= MouseState.MiddleButtonFlag; }
            if ((state & Sdl.Mouse.Button.Right) != 0) { res |= MouseState.RightButtonFlag; }
            if ((state & Sdl.Mouse.Button.X1Mask) != 0) { res |= MouseState.XButton1Flag; }
            if ((state & Sdl.Mouse.Button.X2Mask) != 0) { res |= MouseState.XButton2Flag; }
            return res;
        }
        public override void StartRunLoop()
        {
            throw new NotSupportedException("The desktop platform does not support asynchronous run loops");
        }

        public override GraphicsDevice CreateGraphicsDevice(PresentationParameters pp)
        {
            view.Size = new System.Drawing.Size(pp.BackBufferWidth, pp.BackBufferHeight);
            renderer = new SdlGraphicsDevice(Game, view);
            return renderer;
        }

        public override void Exit()
        {
            Interlocked.Increment(ref isExiting);
        }

        public override bool BeforeUpdate(GameTime gameTime)
        {
            return true;
        }

        public override bool BeforeDraw(GameTime gameTime)
        {
            if (renderer != null)
            {
                renderer.Clear();
                return true;
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            view.Dispose();
            SDL2Image.IMG_Quit();
            Sdl.Quit();
            base.Dispose(disposing);
        }

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
            view.BeginScreenDeviceChange(willBeFullScreen);
        }

        public override void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight)
        {
            view.EndScreenDeviceChange(screenDeviceName, clientWidth, clientHeight);
        }

        public override void Present()
        {
            Game.GraphicsDevice.Present();
        }
    }
}
