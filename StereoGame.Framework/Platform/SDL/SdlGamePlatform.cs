namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Audio;
    using StereoGame.Framework.Graphics;
    using StereoGame.Framework.Input;
    using StereoGame.Framework.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    internal class SdlGamePlatform : GamePlatform
    {
        private int isExiting;
        private readonly SdlGameWindow view;
        private SdlGraphicsDevice? renderer;
        private SdlAudioDevice? audio;
        private readonly List<Keys> keys = new();
        private IntPtr textMem;
        private readonly byte[] textBuffer = new byte[64];
        public SdlGamePlatform(Game game) : base(game)
        {
            string dllDir = Path.Combine(Environment.CurrentDirectory, IntPtr.Size == 4 ? "x86" : "x64");
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + dllDir);
            if (CurrentPlatform.OS == OS.Windows && Debugger.IsAttached)
            {
                Sdl.SetHint("SDL_WINDOWS_DISABLE_THREAD_NAMING", "1");
            }
            Sdl.SetMainReady();
            Sdl.Init((int)(Sdl.InitFlags.Audio | Sdl.InitFlags.Video | Sdl.InitFlags.Joystick | Sdl.InitFlags.GameController | Sdl.InitFlags.Haptic));
            Sdl.DisableScreenSaver();
            SDL2Image.Init(SDL2Image.IMG_InitFlags.IMG_INIT_PNG);
            SDL2TTF.Init();
            SDL2Mixer.Init(SDL2Mixer.MIX_InitFlags.MIX_INIT_MP3 | SDL2Mixer.MIX_InitFlags.MIX_INIT_OGG | SDL2Mixer.MIX_InitFlags.MIX_INIT_MID);
            //Sdl.SetHint("SDL_HINT_RENDER_DRIVER", "opengl");
            Sdl.SetHint("SDL_HINT_RENDER_BATCHING", "1");
            Sdl.SetHint("SDL_HINT_MOUSE_FOCUS_CLICKTHROUGH", "1");
            Sdl.SetHint("SDL_HINT_RENDER_SCALE_QUALITY", "1");
            Keyboard.SetKeys(keys);
            textMem = Marshal.AllocHGlobal(64);
            Window = view = new SdlGameWindow(game);

        }

        public override GameRunBehavior DefaultRunBehavior => GameRunBehavior.Synchronous;

        public override void BeforeInitialize()
        {
            base.BeforeInitialize();
        }

        public override void RunLoop()
        {
            Sdl.Window.ShowWindow(Window.Handle);
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
                        {
                            var key = KeyboardUtil.ToXna(ev.Key.Keysym.Sym);
                            if (!keys.Contains(key))
                            {
                                keys.Add(key);
                            }
                            view.OnKeyDown(new InputKeyEventArgs(key));
                            break;
                        }
                    case Sdl.EventType.KeyUp:
                        {
                            var key = KeyboardUtil.ToXna(ev.Key.Keysym.Sym);
                            keys.Remove(key);
                            view.OnKeyUp(new InputKeyEventArgs(key));
                            break;
                        }
                    case Sdl.EventType.TextInput:
                        Marshal.StructureToPtr(ev, textMem, false);
                        Marshal.Copy(textMem, textBuffer, 0, 56);
                        int length = 0;
                        while (textBuffer[length + 12] != 0 && length < 32)
                        {
                            length++;
                        }
                        if (length > 0)
                        {
                            string str = Encoding.UTF8.GetString(textBuffer, 12, length);
                            if (!string.IsNullOrEmpty(str))
                            {
                                view.OnTextInput(new TextInputEventArgs(str));
                            }
                        }
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
            renderer = new SdlGraphicsDevice(Game, this, view, pp);
            BeginScreenDeviceChange(pp.IsFullScreen);
            EndScreenDeviceChange(string.Empty, pp.BackBufferWidth, pp.BackBufferHeight);
            return renderer;
        }

        public override AudioDevice CreateAudioDevice()
        {
            audio = new SdlAudioDevice(Game);
            return audio;
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
            SDL2Mixer.Quit();
            SDL2TTF.Quit();
            SDL2Image.Quit();
            Sdl.Quit();
            Marshal.FreeHGlobal(textMem);
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

        public override TextFont? LoadFont(string path, int ySize)
        {
            IntPtr font = SDL2TTF.OpenFont(path, ySize);
            if (font != IntPtr.Zero)
            {
                SdlTextFont textFont = new(font, ySize);
                return textFont;
            }
            return null;
        }

        public override TextFont? LoadFont(string name, byte[] data, int ySize)
        {
            int size = data.Length;
            IntPtr mem = Marshal.AllocHGlobal(size);
            Marshal.Copy(data, 0, mem, size);
            IntPtr rw = Sdl.RWFromMem(mem, size);
            if (rw != IntPtr.Zero)
            {
                IntPtr font = SDL2TTF.OpenFontRW(rw, 1, ySize);
                if (font != IntPtr.Zero)
                {
                    SdlTextFont textFont = new(font, ySize, mem);
                    return textFont;
                }
                else
                {
                    Sdl.FreeRW(rw);
                    Marshal.FreeHGlobal(mem);
                }
            }
            return null;
        }


        public override void Log(string message)
        {
            Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff "));
            Console.WriteLine(message);
        }
    }
}
