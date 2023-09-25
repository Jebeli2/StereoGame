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
        private static int nextId;

        private readonly int id;
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
        private bool pressed;

        private Color backgroundColor;
        private Color borderColor;
        private int borderThickness;
        private Color textColor;

        private ControlStyle? hoverStyle;
        private ControlStyle? disabledStyle;
        private ControlStyle? pressedStyle;

        private Skin skin;


        protected Control()
            : this(null)
        {
        }

        protected Control(Control? parent)
        {
            id = ++nextId;
            enabled = true;
            visible = true;
            maxWidth = int.MaxValue;
            maxHeight = int.MaxValue;
            skin = Skin.DefaultSkin;
            skin.Apply(this);
            parent?.Add(this);
        }

        public Skin Skin
        {
            get { return skin; }
            set
            {
                if (skin != value)
                {
                    skin = value;
                    skin.Apply(this);
                }
            }
        }

        public int Id => id;

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

        public virtual void InvalidateLayout() { }


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
                    disabledStyle?.ApplyIf(this, !enabled);
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
                    hoverStyle?.ApplyIf(this, hovered);
                }
            }
        }

        public bool Pressed
        {
            get { return pressed; }
            set
            {
                if (pressed != value)
                {
                    pressed = value;
                    pressedStyle?.ApplyIf(this, pressed);
                }
            }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                if (backgroundColor != value)
                {
                    backgroundColor = value;
                }
            }
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                if (borderColor != value)
                {
                    borderColor = value;
                }
            }
        }

        public int BorderThickness
        {
            get { return borderThickness; }
            set
            {
                if (borderThickness != value)
                {
                    borderThickness = value;
                }
            }
        }

        public Color TextColor
        {
            get { return textColor; }
            set
            {
                if (textColor != value)
                {
                    textColor = value;
                }
            }
        }

        public ControlStyle? HoverStyle
        {
            get { return hoverStyle; }
            set
            {
                if (hoverStyle != value)
                {
                    hoverStyle = value;
                    hoverStyle?.ApplyIf(this, hovered);
                }
            }
        }

        public ControlStyle? DisabledStyle
        {
            get { return disabledStyle; }
            set
            {
                if (disabledStyle != value)
                {
                    disabledStyle = value;
                    disabledStyle?.ApplyIf(this, !enabled);
                }
            }
        }

        public ControlStyle? PressedStyle
        {
            get { return pressedStyle; }
            set
            {
                if (pressedStyle != value)
                {
                    pressedStyle = value;
                    pressedStyle?.ApplyIf(this, pressed);
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

        public bool HasParent(Control control)
        {
            return parent != null && ((parent == control) || parent.HasParent(control));
        }
        public virtual void Update(IGuiSystem gui, GameTime gameTime) { }

        public virtual void Draw(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime)
        {
            Rectangle rect = GetBounds();
            if (!backgroundColor.IsEmpty && backgroundColor != Color.Transparent)
            {
                renderer.FillRectangle(rect, backgroundColor);
            }
            if (borderThickness != 0)
            {
                renderer.DrawRectangle(rect, borderColor, borderThickness);
            }
        }

        public virtual bool OnPointerDown(PointerEventArgs args) { return true; }
        public virtual bool OnPointerUp(PointerEventArgs args) { return true; }
        public virtual bool OnPointerMove(PointerEventArgs args) { return true; }
        public virtual bool OnPointerEnter(PointerEventArgs args)
        {
            if (enabled && !hovered)
            {
                Hovered = true;
            }
            return true;
        }
        public virtual bool OnPointerLeave(PointerEventArgs args)
        {
            if (enabled && hovered)
            {
                Hovered = false;
            }
            return true;
        }

    }
}
