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

    internal class Lines : DrawableGameComponent
    {

        private class Line
        {
            public int x1;
            public int y1;
            public int x2;
            public int y2;
        }
        private int lastWidth;
        private int lastHeight;
        private int linesPerQuadrant = 80;
        private int lineStep = 8;
        private static readonly TimeSpan lineSpeed = TimeSpan.FromMilliseconds(32);
        private static readonly TimeSpan colorSpeed = TimeSpan.FromMilliseconds(8);
        private TimeSpan lastLineTime;
        private TimeSpan lastColorTime;


        private int lineIndex = 0;
        private int lineCount;
        private IList<Line> lines = Array.Empty<Line>();
        private IList<Color> colors = Array.Empty<Color>();

        private int colShift = 0;
        public Lines(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            Init();
            colors = MakeColors(linesPerQuadrant);

        }

        public override void Update(GameTime gameTime)
        {

            Init();

            UpdateLines(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            PaintLines(GraphicsDevice);
        }

        private void PaintLines(GraphicsDevice gfx)
        {
            int lineNum = lineIndex;
            int colNum = (lineIndex + colShift) % colors.Count;
            for (int i = 0; i < lineCount && lineNum < lines.Count && colNum < colors.Count; ++i)
            {
                Line l = lines[lineNum];
                Color c = colors[colNum];
                gfx.Color = c;
                gfx.DrawLine(l.x1, l.y1, l.x2, l.y2);
                lineNum++;
                lineNum %= lines.Count;
                colNum++;
                colNum %= colors.Count;
            }
        }
        private void UpdateLines(GameTime gameTime)
        {
            if (gameTime.HasTimePassed(lineSpeed, ref lastLineTime))
            {
                lineIndex++;
                lineIndex %= lines.Count;
                lineCount++;
                if (lineCount > linesPerQuadrant * 2)
                {
                    lineCount = linesPerQuadrant * 2;
                }
            }
            if (gameTime.HasTimePassed(colorSpeed, ref lastColorTime))
            {
                colShift++;
                colShift %= colors.Count;
            }
        }
        private void Init()
        {
            if (lastWidth != Width || lastHeight != Height)
            {
                lastWidth = Width;
                lastHeight = Height;
                lines = MakeLines(linesPerQuadrant, lineStep, 0, 0, Width - 1, Height - 1);
                lineIndex = 0;
                lineCount = 0;
            }
        }

        private static IList<Color> MakeColors(int linesPerQuadrant)
        {
            Color[] list = new Color[linesPerQuadrant * 4];
            double hue = 180;
            double sat = 0.6;
            double val = 1.0;
            int alpha = 128;

            for (int i = 0; i < list.Length; i++)
            {
                list[i] = ColorExtensions.ColorFromHSV(hue, sat, val, alpha);
                Shift(ref hue, ref sat, ref val, ref alpha);
            }
            return list;
        }

        private static IList<Line> MakeLines(int linesPerQuadrant, int lineStep, int left, int top, int right, int bottom)
        {
            Line[] list = new Line[linesPerQuadrant * 4];
            int idx = 0;
            for (int q = 0; q < linesPerQuadrant; ++q)
            {
                list[idx++] = CreateLine(left, lineStep * (linesPerQuadrant - q), lineStep * q, top);
            }
            for (int q = 0; q < linesPerQuadrant; ++q)
            {
                list[idx++] = CreateLine(right - lineStep * (linesPerQuadrant - q), top, right, lineStep * q);
            }
            for (int q = 0; q < linesPerQuadrant; ++q)
            {
                list[idx++] = CreateLine(right, bottom - lineStep * (linesPerQuadrant - q), right - lineStep * q, bottom);
            }
            for (int q = 0; q < linesPerQuadrant; ++q)
            {
                list[idx++] = CreateLine(left + lineStep * (linesPerQuadrant - q), bottom, left, bottom - lineStep * q);
            }
            return list;
        }

        private static void Shift(ref double hue, ref double sat, ref double val, ref int alpha)
        {
            hue += 1;
            if (hue > 360) { hue = 0; }
            val += 0.1;
            if (val > 0.9) { val = 0.6; }
        }
        private static Line CreateLine(int x1, int y1, int x2, int y2)
        {
            return new Line { x1 = x1, y1 = y1, x2 = x2, y2 = y2 };
        }
    }
}
