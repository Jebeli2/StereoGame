namespace StereoGameTest
{
    using StereoGame.Framework;
    using StereoGame.Framework.Components;
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
        private TextFont? font;
        private readonly FramesPerSecondCounterComponent fps;

        public TestGame()
        {
            TargetFPS = 120;
            fps = new FramesPerSecondCounterComponent(this);
            Components.Add(fps);
            Content.AddResourceManager(Properties.Resources.ResourceManager);
        }

        protected override void Initialize()
        {
            GraphicsDevice.ResourceCreated += GraphicsDevice_ResourceCreated;
            GraphicsDevice.ResourceDestroyed += GraphicsDevice_ResourceDestroyed;
            base.Initialize();
        }



        protected override void LoadContent()
        {
            //bg = Content.Load<Texture>(@"c:\Local\mods\fantasycore\images\menus\backgrounds\badlands.png");
            bg = Content.Load<Texture>(@"badlands");
            //font = Content.Load<TextFont>(@"c:\Local\mods\fantasycore\fonts\LiberationSans-Regular.ttf", 16);
            font = Content.Load<TextFont>(@"LiberationSans-Regular", 16);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DrawTexture(bg);
            //GraphicsDevice.Color = Color.BlanchedAlmond;
            //GraphicsDevice.BlendMode = BlendMode.Blend;
            //GraphicsDevice.DrawRect(10, 10, 100, 100);
            //GraphicsDevice.DrawLine(10, 109, 109, 10);
            base.Draw(gameTime);
            GraphicsDevice.DrawText(font, fps.FramesPerSecondText, 10, 10);
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
