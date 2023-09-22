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

        public TestGame()
        {

        }

        protected override void Initialize()
        {
            GraphicsDevice.ResourceCreated += GraphicsDevice_ResourceCreated;
            GraphicsDevice.ResourceDestroyed += GraphicsDevice_ResourceDestroyed;
            base.Initialize();
        }



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
            GraphicsDevice.DrawTexture(bg);
            GraphicsDevice.Color = Color.BlanchedAlmond;
            GraphicsDevice.BlendMode = BlendMode.Blend;
            GraphicsDevice.DrawRect(10, 10, 100, 100);
            GraphicsDevice.DrawLine(10, 109, 109, 10);
            base.Draw(gameTime);
        }

        private void GraphicsDevice_ResourceDestroyed(object? sender, ResourceDestroyedEventArgs e)
        {
            Log($"{e.Resource} destroyed");
        }

        private void GraphicsDevice_ResourceCreated(object? sender, ResourceCreatedEventArgs e)
        {
            Log($"{e.Resource} created");
        }

    }
}
