using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Utilities
{
    public class FramesPerSecondCounter
    {
        private static readonly TimeSpan oneSecondTimeSpan = TimeSpan.FromSeconds(1);
        private int framesCounter;
        private TimeSpan timer = oneSecondTimeSpan;
        private int fps;
        private string fpsText;

        public FramesPerSecondCounter(int fps = 60)
        {
            this.fps = fps;
            framesCounter = fps;
            fpsText = fps.ToString() + " fps";
        }

        public int FramesPerSecond
        {
            get { return fps; }
            private set
            {
                if (fps != value)
                {
                    fps = value;
                    fpsText = fps.ToString() + " fps";
                }
            }
        }
        public string FramesPerSecondText => fpsText;

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime;
            if (timer <= oneSecondTimeSpan)
                return;

            FramesPerSecond = framesCounter;
            framesCounter = 0;
            timer -= oneSecondTimeSpan;
        }

        public void Draw(GameTime gameTime)
        {
            framesCounter++;
        }
    }
}
