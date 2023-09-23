namespace StereoGameTest
{
    using StereoGame.Framework;
    using StereoGame.Framework.Audio;
    using StereoGame.Framework.Graphics;
    using StereoGame.Framework.Input;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class RainingBoxes : DrawableGameComponent
    {
        private Texture? box;
        private Sound? swish;
        private static readonly Random random = new();
        private const float GRAVITY = 750.0f;
        private readonly List<Square> squares = new();
        private double lastTime;
        private bool mousePressed;


        public RainingBoxes(Game game)
            : base(game) { }


        protected override void LoadContent()
        {
            box = Game.Content.Load<Texture>(nameof(Properties.Resources.box));
            swish = Game.Content.Load<Sound>(nameof(Properties.Resources.swish_11));
        }

        public override void Update(GameTime gameTime)
        {
            double time = gameTime.TotalGameTime.TotalMilliseconds / 1000;
            double timeSinceLastAdded = time - lastTime;
            MouseState ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed)
            {
                if (timeSinceLastAdded > 0.01)
                {
                    if (!mousePressed)
                    {
                        AudioDevice.PlaySound(swish);
                    }
                    mousePressed = true;
                    lastTime = time;
                    int addX = ms.X;
                    int addY = ms.Y;
                    squares.Add(new Square(addX, addY, time));
                }
            }
            else
            {
                mousePressed = false;
            }
            int index = 0;
            int h = Game.Window.Size.Height;
            while (index < squares.Count)
            {
                Square s = squares[index];
                float dT = (float)(time - s.lastUpdate);
                s.yvelocity += dT * GRAVITY;
                s.y += s.yvelocity * dT;
                s.x += s.xvelocity * dT;
                if (s.y > h - s.h)
                {
                    s.y = h - s.h;
                    s.xvelocity = 0;
                    s.yvelocity = 0;
                }
                s.lastUpdate = time;
                if (s.yvelocity <= 0 && s.lastUpdate > s.born + s.duration)
                {
                    squares.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendMode = BlendMode.Blend;
            foreach (Square s in squares)
            {
                GraphicsDevice.DrawTexture(box, (int)s.x, (int)s.y, (int)s.w, (int)s.h);
            }
        }

        private static int Rand() { return random.Next(); }
        private class Square
        {
            public Square(int x, int y, double time)
            {
                w = Rand() % 80 + 40;
                h = Rand() % 80 + 40;
                this.x = x - w / 2;
                this.y = y - h / 2;
                yvelocity = -10;
                xvelocity = Rand() % 100 - 50;
                born = time;
                lastUpdate = time;
                duration = Rand() % 4 + 1;
            }
            public float x;
            public float y;
            public float w;
            public float h;
            public float xvelocity;
            public float yvelocity;
            public double born;
            public double lastUpdate;
            public double duration;
        }
    }
}
