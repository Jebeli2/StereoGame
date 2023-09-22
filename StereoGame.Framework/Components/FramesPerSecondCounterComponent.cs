using StereoGame.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Components
{
    public class FramesPerSecondCounterComponent : DrawableGameComponent
    {
        private readonly FramesPerSecondCounter fpsCounter;
        public FramesPerSecondCounterComponent(Game game)
            : base(game)
        {
            fpsCounter = new FramesPerSecondCounter(game.TargetFPS);
        }

        public int FramesPerSecond => fpsCounter.FramesPerSecond;
        public string FramesPerSecondText => fpsCounter.FramesPerSecondText;
        public override void Update(GameTime gameTime)
        {
            fpsCounter.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            fpsCounter.Draw(gameTime);
        }

    }
}
