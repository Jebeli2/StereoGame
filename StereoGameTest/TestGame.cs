namespace StereoGameTest
{
    using StereoGame.Extended.Gui;
    using StereoGame.Framework;
    using StereoGame.Framework.Audio;
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
        private Music? music;
        private readonly FramesPerSecondCounterComponent fps;

        private GuiSystem gui;
        private Screen? mainScreen;
        private Window? titleWindow;

        public TestGame()
        {
            Content.AddResourceManager(Properties.Resources.ResourceManager);

            TargetFPS = 120;
            fps = new FramesPerSecondCounterComponent(this);
            Components.Add(fps);
            gui = new GuiSystem(this);
            Components.Add(gui);
        }


        protected override void Initialize()
        {
            base.Initialize();
            InitGui();
        }

        protected override void LoadContent()
        {
            //bg = Content.Load<Texture>(@"c:\Local\mods\fantasycore\images\menus\backgrounds\badlands.png");
            bg = Content.Load<Texture>(@"badlands");
            //font = Content.Load<TextFont>(@"c:\Local\mods\fantasycore\fonts\LiberationSans-Regular.ttf", 16);
            font = Content.Load<TextFont>(@"LiberationSans-Regular", 16);
            music = Content.Load<Music>(@"JesuJoy");

            AudioDevice.PlayMusic(music);


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

        private void InitGui()
        {
            mainScreen = new Screen();
            titleWindow = new Window(mainScreen);
            titleWindow.X = 20;
            titleWindow.Y = 50;
            titleWindow.Width = 200;
            titleWindow.Height = 200;

            var button1 = new Button(titleWindow) { X = 10, Y = 10, Width = 180, Height = 30 };
            var button2 = new Button(titleWindow) { X = 10, Y = 50, Width = 180, Height = 30 };
            var button3 = new Button(titleWindow) { X = 10, Y = 90, Width = 180, Height = 30 };

            gui.ActiveScreen = mainScreen;
        }

    }
}
