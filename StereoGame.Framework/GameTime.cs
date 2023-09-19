namespace StereoGame.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GameTime
    {
        private TimeSpan totalGameTime;
        private TimeSpan elapsedGameTime;
        private bool isRunningSlowly;
        public TimeSpan TotalGameTime
        {
            get => totalGameTime;
            internal set => totalGameTime = value;
        }
        public TimeSpan ElapsedGameTime
        {
            get => elapsedGameTime;
            internal set => elapsedGameTime = value;
        }

        public bool IsRunningSlowly
        {
            get => isRunningSlowly;
            internal set => isRunningSlowly = value;
        }

        public GameTime()
        {
            totalGameTime = TimeSpan.Zero;
            elapsedGameTime = TimeSpan.Zero;
            isRunningSlowly = false;
        }
        public GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime, bool isRunningSlowly = false)
        {
            this.totalGameTime = totalGameTime;
            this.elapsedGameTime = elapsedGameTime;
            this.isRunningSlowly = isRunningSlowly;
        }
    }
}
