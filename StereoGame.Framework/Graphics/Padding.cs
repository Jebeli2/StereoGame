using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Graphics
{
    public struct Padding : IEquatable<Padding>
    {
        private int left;
        private int top;
        private int right;
        private int bottom;

        public static readonly Padding Empty = new Padding(0);

        public Padding(int all)
        {
            left = all;
            top = all;
            right = all;
            bottom = all;
        }

        public Padding(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public int All
        {
            get
            {
                if ((left == top) && (left == right) && (left == bottom)) return left;
                return -1;
            }
            set
            {
                left = value;
                top = value;
                right = value;
                bottom = value;
            }
        }
        public int Left
        {
            get { return left; }
            set { left = value; }
        }

        public int Top
        {
            get { return top; }
            set { top = value; }
        }

        public int Right
        {
            get { return right; }
            set { right = value; }
        }

        public int Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }

        public int Horizontal => left + right;
        public int Vertical => top + bottom;

        public Size Size => new Size(Horizontal, Vertical);

        public bool Equals(Padding other)
        {
            return left == other.left && top == other.top && right == other.right && bottom == other.bottom;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is Padding other) { return Equals(other); }
            return false;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(left, top, right, bottom);
        }

        public static bool operator ==(Padding left, Padding right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Padding left, Padding right)
        {
            return !(left == right);
        }
    }
}
