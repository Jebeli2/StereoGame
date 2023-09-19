namespace StereoGame.Framework.Platform.SDL
{
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
        public SdlGamePlatform(Game game) : base(game)
        {
            if (CurrentPlatform.OS == OS.Windows && Debugger.IsAttached)
            {
                Sdl.SetHint("SDL_WINDOWS_DISABLE_THREAD_NAMING", "1");
            }
            Sdl.Init((int)(Sdl.InitFlags.Video | Sdl.InitFlags.Joystick | Sdl.InitFlags.GameController | Sdl.InitFlags.Haptic));
            Sdl.DisableScreenSaver();
        }

        public override GameRunBehavior DefaultRunBehavior => GameRunBehavior.Synchronous;

        public override void BeforeInitialize()
        {
            base.BeforeInitialize();
        }

        public override void RunLoop()
        {

        }

        public override void StartRunLoop()
        {
            throw new NotSupportedException("The desktop platform does not support asynchronous run loops");
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
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            Sdl.Quit();
            base.Dispose(disposing);
        }

    }
}
