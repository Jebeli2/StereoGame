namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class Control
    {
        private Control? parent;
        private readonly List<Control> children = new();
        private int x;
        private int y;
        private int width;
        private int height;
        private int minWidth;
        private int minHeight;
        private int maxWidth;
        private int maxHeight;
        private bool enabled;
        private bool visible;
        private bool focused;
        private bool hovered;


        protected Control()
            : this(null)
        {
        }

        protected Control(Control? parent)
        {
            enabled = true;
            visible = true;
            maxWidth = int.MaxValue;
            maxHeight = int.MaxValue;
            parent?.Add(this);
        }

        public Control? Parent
        {
            get { return parent; }
        }

        public IEnumerable<Control> Children
        {
            get { return children; }
        }

        public bool Add(Control child)
        {
            if (!children.Contains(child))
            {
                children.Add(child);
                child.parent = this;
                return true;
            }
            return false;
        }

        public bool Remove(Control child)
        {
            if (children.Remove(child))
            {
                child.parent = null;
                return true;
            }
            return false;
        }


        public int X
        {
            get { return x; }
            set
            {
                if (x != value)
                {
                    x = value;
                }
            }
        }

        public int Y
        {
            get { return y; }
            set
            {
                if (y != value)
                {
                    y = value;
                }
            }
        }

        public int Width
        {
            get { return width; }
            set
            {
                if (width != value)
                {
                    width = value;
                }
            }
        }

        public int Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                }
            }
        }

        public int MinWidth
        {
            get { return minWidth; }
            set
            {
                if (minWidth != value)
                {
                    minWidth = value;
                }
            }
        }

        public int MinHeight
        {
            get { return minHeight; }
            set
            {
                if (minHeight != value)
                {
                    minHeight = value;
                }
            }
        }

        public int MaxWidth
        {
            get { return maxWidth; }
            set
            {
                if (maxWidth != value)
                {
                    maxWidth = value;
                }
            }
        }

        public int MaxHeight
        {
            get { return maxHeight; }
            set
            {
                if (maxHeight != value)
                {
                    maxHeight = value;
                }
            }
        }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                }
            }
        }

        public bool Visible
        {
            get { return visible; }
            set
            {
                if (visible != value)
                {
                    visible = value;
                }
            }
        }

        public bool Focused
        {
            get { return focused; }
            set
            {
                if (focused != value)
                {
                    focused = value;
                }
            }
        }

        public bool Hovered
        {
            get { return hovered; }
            set
            {
                if (hovered != value)
                {
                    hovered = value;
                }
            }
        }

        public virtual Rectangle GetBounds()
        {
            Rectangle rect = new Rectangle(x, y, width, height);
            if (parent != null)
            {
                Rectangle parentRect = parent.GetBounds();
                rect.Offset(parentRect.Location);
            }
            return rect;
        }

        public virtual bool Contains(int x, int y)
        {
            return GetBounds().Contains(x, y);  
        }

        public virtual void Update(IGuiSystem gui, GameTime gameTime) { }

        public virtual void Draw(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime)
        {
            renderer.DrawControl(this);
        }
    }
}
