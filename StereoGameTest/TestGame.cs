namespace StereoGameTest
{
    using StereoGame.Extended.Gui;
    using StereoGame.Framework;
    using StereoGame.Framework.Audio;
    using StereoGame.Framework.Components;
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class TestGame : Game
    {
        private Texture? bg;
        private Texture? button;
        private TextFont? font;
        private TextFont? guiFont;
        private Music? music;
        private readonly FramesPerSecondCounterComponent fps;

        private GuiSystem gui;
        private Screen? mainScreen;
        private Window? titleWindow;

        private Screen? guiTestScreen;
        private Window? guiTestWindow;

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
            InitGui(guiFont);
        }

        protected override void LoadContent()
        {
            //bg = Content.Load<Texture>(@"c:\Local\mods\fantasycore\images\menus\backgrounds\badlands.png");
            bg = Content.Load<Texture>(nameof(Properties.Resources.badlands));
            button = Content.Load<Texture>(nameof(Properties.Resources.Button));
            //font = Content.Load<TextFont>(@"c:\Local\mods\fantasycore\fonts\LiberationSans-Regular.ttf", 16);
            font = Content.Load<TextFont>(nameof(Properties.Resources.LiberationSans_Regular), 16);
            guiFont = Content.Load<TextFont>(nameof(Properties.Resources.Roboto_Regular), 16);
            music = Content.Load<Music>(nameof(Properties.Resources.box));

            AudioDevice.PlayMusic(music);


        }

        protected override void UnloadContent()
        {
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DrawTexture(bg);
            base.Draw(gameTime);
            GraphicsDevice.DrawText(font, fps.FramesPerSecondText, 10, 10);
        }

        private void InitGui(TextFont? font)
        {
            Skin.DefaultSkin.DefaultFont = font;
            var buttonStyle = Skin.DefaultSkin.GetStyle(typeof(Button));
            if (button != null && buttonStyle != null)
            {
                NinePatchRegion npr = new NinePatchRegion(button, new Padding(2));
                buttonStyle.BackgroundRegion = npr;
                buttonStyle.BorderThickness = 0;
            }
            mainScreen = new Screen();
            titleWindow = new Window(mainScreen);
            titleWindow.X = 20;
            titleWindow.Y = 50;
            titleWindow.Width = 200;
            titleWindow.Height = 200;
            titleWindow.Title = "Stereo";

            var button1 = new Button(titleWindow, "Test GUI") { X = 10, Y = 10, Width = 180, Height = 30 };

            button1.Clicked += (s, e) => { gui.ActiveScreen = guiTestScreen; };

            var button2 = new Button(titleWindow, "Test MAP") { X = 10, Y = 50, Width = 180, Height = 30 };
            var button3 = new Button(titleWindow, "Test SOM") { X = 10, Y = 90, Width = 180, Height = 30 };

            gui.ActiveScreen = mainScreen;


            guiTestScreen = new Screen();
            guiTestWindow = new Window(guiTestScreen);
            guiTestWindow.X = 100;
            guiTestWindow.Y = 100;
            guiTestWindow.Width = 200;
            guiTestWindow.Height = 200;
            guiTestWindow.Title = "GUI Test";
            var b1 = new Button(guiTestWindow, "Buttons");
            var b2 = new Button(guiTestWindow, "Whatever");
            var b3 = new Button(guiTestWindow, "Back");

            b3.Clicked += (s, e) => { gui.ActiveScreen = mainScreen; };

        }

    }
}
