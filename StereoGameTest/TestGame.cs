namespace StereoGameTest
{
    using StereoGame.Framework;
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class TestGame : Game
    {
        private Texture? bg;
        protected override void LoadContent()
        {
            bg = GraphicsDevice.LoadTexture(@"c:\Local\mods\fantasycore\images\menus\backgrounds\badlands.png");
        }

        protected override void UnloadContent()
        {
            bg?.Dispose();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.DrawTexture(bg);
            GraphicsDevice.Color = Color.Wheat;
            GraphicsDevice.DrawRect(10,10,100,100);
        }
    }
}
