﻿namespace StereoGameTest
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
        private Window? buttonDemo;
        private Window? propDemo;

        private Lines lines;
        private RainingBoxes boxes;

        public TestGame()
        {
            Content.AddResourceManager(Properties.Resources.ResourceManager);
            TargetFPS = 120;
            fps = new FramesPerSecondCounterComponent(this);
            Components.Add(fps);
            gui = new GuiSystem(this);
            Components.Add(gui);
            lines = new Lines(this);
            Components.Add(lines);
            boxes = new RainingBoxes(this);
            Components.Add(boxes);
        }

        public Lines Lines => lines;
        public RainingBoxes Boxes => boxes;


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
            music = Content.Load<Music>(nameof(Properties.Resources.JesuJoy));

            AudioDevice.PlayMusic(music);

            ToggleBoxes();
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

        private void ToggleBoxes()
        {
            if (Components.Contains(boxes))
            {
                boxes.Clear();
                Components.Remove(boxes);
            }
            else
            {
                boxes.Clear();
                Components.Add(boxes);
            }
        }

        private void ToggleLines()
        {
            if (Components.Contains(lines))
            {
                Components.Remove(lines);
            }
            else
            {
                Components.Add(lines);
            }
        }
        private void InitGui(TextFont? font)
        {
            Skin.DefaultSkin.DefaultFont = font;
            //var buttonStyle = Skin.DefaultSkin.GetStyle(typeof(Button));
            //if (button != null && buttonStyle != null)
            //{
            //    NinePatchRegion npr = new NinePatchRegion(button, new Padding(2));
            //    buttonStyle.BackgroundRegion = npr;
            //    buttonStyle.BorderThickness = 0;
            //}
            mainScreen = new Screen();
            titleWindow = new Window(mainScreen);
            titleWindow.MinWidth = 120;
            titleWindow.MinHeight = 120;
            titleWindow.Title = "Stereo";
            titleWindow.WindowClosed += (s, e) => { Exit(); };

            var button1 = new Button(titleWindow, "Test GUI");

            button1.Clicked += (s, e) => { gui.ActivateScreenAndWindow(guiTestScreen, guiTestWindow); };

            var button2 = new Button(titleWindow, "Test MAP");
            var button3 = new Button(titleWindow, "Test SOM");

            gui.ActivateScreenAndWindow(mainScreen, titleWindow);


            guiTestScreen = new Screen();
            guiTestWindow = new Window(guiTestScreen);
            guiTestWindow.MinWidth = 120;
            guiTestWindow.MinHeight = 120;
            guiTestWindow.Title = "GUI Test";
            guiTestWindow.WindowClosed += (s, e) => { gui.ActivateScreenAndWindow(mainScreen, titleWindow); };

            var b1 = new Button(guiTestWindow, "Buttons");
            var b2 = new Button(guiTestWindow, "Props");
            var b3 = new Button(guiTestWindow, "Back");

            b1.Clicked += (s, e) => { if (buttonDemo != null) { buttonDemo.Visible ^= true; } };
            b2.Clicked += (s, e) => { if (propDemo != null) { propDemo.Visible ^= true; } };
            b3.Clicked += (s, e) => { gui.ActivateScreenAndWindow(mainScreen, titleWindow); };


            buttonDemo = new Window(guiTestScreen);
            buttonDemo.SetPosition(200, 50);
            buttonDemo.MinWidth = 200;
            buttonDemo.MinHeight = 500;
            buttonDemo.Title = "Button Demo";
            buttonDemo.Visible = false;
            buttonDemo.DefaultWindowCloseAction = WindowCloseAction.Hide;
            _ = new Label(buttonDemo, "Push buttons");
            _ = new Button(buttonDemo, "Plain button");
            var sb = new Button(buttonDemo, "Styled");
            sb.Icon = Icons.ROCKET;
            _ = new Label(buttonDemo, "Toggle buttons");
            _ = new ToggleButton(buttonDemo, "Toggle me");
            _ = new Label(buttonDemo, "Radio buttons");
            _ = new Button(buttonDemo, "Radio button 1");
            _ = new Button(buttonDemo, "Radio button 2");
            _ = new Label(buttonDemo, "A tool palette");
            var p1 = new Panel(buttonDemo);
            var t1 = new ToggleButton(p1) { Icon = Icons.LINE_GRAPH, Checked = true };
            t1.CheckedStateChanged += (s, e) => { ToggleLines(); };
            var t2 = new ToggleButton(p1) { Icon = Icons.BOX, Checked = true };
            t2.CheckedStateChanged += (s, e) => { ToggleBoxes(); };
            _ = new ToggleButton(p1) { Icon = Icons.CIRCLE_WITH_PLUS };
            _ = new ToggleButton(p1) { Icon = Icons.DRIVE };
            _ = new Label(buttonDemo, "Check boxes");
            _ = new CheckBox(buttonDemo, "Check Box");

            propDemo = new Window(guiTestScreen);
            propDemo.SetPosition(400, 50);
            propDemo.MinWidth = 200;
            propDemo.MinHeight = 500;
            propDemo.Title = "Prop Demo";
            propDemo.Visible = false;
            propDemo.DefaultWindowCloseAction = WindowCloseAction.Hide;
            _ = new Label(propDemo, "Proportional Controls");
            var prop1 = new PropControl(propDemo);
            prop1.ModifyProp(true, false, 0x2222, 0, 0x5555, 0);
            var prop2 = new PropControl(propDemo);
            prop2.ModifyProp(true, false, 0x0, 0, 0x555, 0);
            var prop3 = new PropControl(propDemo);
            prop3.ModifyProp(true, true, 0x2000, 0x2000, 0x1000, 0x2000);

        }

    }
}
