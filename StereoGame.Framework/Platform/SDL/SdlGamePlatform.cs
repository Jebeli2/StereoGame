﻿namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Graphics;
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
                }
            }
        }

        public override void StartRunLoop()
        {
            throw new NotSupportedException("The desktop platform does not support asynchronous run loops");
        }

        public override GraphicsDevice CreateGraphicsDevice()
        {
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
