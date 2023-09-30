﻿namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class Control
    {
        public const int ICONWIDTH = 20;
        public const int ICONHEIGHT = 20;

        private static int nextId;

        private readonly int controlId;
        private Control? parent;
        private readonly List<Control> children = new();
        private int x;
        private int y;
        private int width;
        private int height;
        private int fixedWidth;
        private int fixedHeight;
        private int minWidth;
        private int minHeight;
        private int maxWidth;
        private int maxHeight;
        private Padding padding;
        private Padding margin;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch;
        private VerticalAlignment verticalAlignment = VerticalAlignment.Stretch;
        private HorizontalAlignment horizontalTextAlignment = HorizontalAlignment.Center;
        private VerticalAlignment verticalTextAlignment = VerticalAlignment.Center;
        private bool excludeFromLayout;
        private bool enabled;
        private bool visible;
        private bool focused;
        private bool hovered;
        private bool pressed;
        private bool _checked;
        private bool active;

        protected bool layoutNeeded;
        protected bool valid;
        protected bool superBitmap;
        private Texture? bitmap;

        protected ILayout? layout;
        protected bool alwaysUseIconSpace;
        protected bool textIsTitle;
        private TextFont? font;
        private TextureRegion? backgroundRegion;
        private Color backgroundColor;
        private Color borderColor;
        private Color borderShineColor;
        private Color borderShadowColor;
        private Color knobColor;
        private int borderThickness;
        private int additionalSizeIncrease;
        private Color textColor;


        private Skin skin;

        private string? text;
        private Icons icon;


        protected Control()
            : this(null)
        {
        }

        protected Control(Control? parent)
        {
            controlId = ++nextId;
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

        public ILayout? Layout
        {
            get { return layout; }
            set
            {
                if (layout != value)
                {
                    layout = value;
                    InvalidateLayout();
                }
            }
        }

        public int ControlId => controlId;

        public Control? Parent
        {
            get { return parent; }
        }

        public Window? Window
        {
            get
            {
                Control? c = this;
                while (c != null)
                {
                    if (c is Window window) return window;
                    c = c.Parent;
                }
                return null;
            }
        }
        public Screen? Screen
        {
            get
            {
                Control? c = this;
                while (c != null)
                {
                    if (c is Screen screen) return screen;
                    c = c.Parent;
                }
                return null;
            }
        }

        public virtual IEnumerable<Control> Children
        {
            get { return children; }
        }

        public bool Add(Control child)
        {
            if (!children.Contains(child))
            {
                children.Add(child);
                child.parent = this;
                InvalidateLayout();
                return true;
            }
            return false;
        }

        public bool Remove(Control child)
        {
            if (children.Remove(child))
            {
                child.parent = null;
                InvalidateLayout();
                return true;
            }
            return false;
        }

        public Size GetFixedSize()
        {
            return new Size(fixedWidth, fixedHeight);
        }

        public virtual Size GetPreferredSize(IGuiSystem context)
        {
            return layout?.GetPreferredSize(context, this) ?? new Size(width, height);
        }

        public virtual void PerformLayout(IGuiSystem context)
        {
            if (layout != null)
            {
                layout.PerformLayout(context, this);
            }
            else
            {
                foreach (Control child in children)
                {
                    Size pref = child.GetPreferredSize(context);
                    Size fix = child.GetFixedSize();
                    Size cs = pref.GetValidSize(ref fix);
                    child.SetSize(cs.Width, cs.Height);
                    if (child.LayoutNeeded) child.PerformLayout(context);
                }
            }
            additionalSizeIncrease = 0;
            layoutNeeded = false;
            valid = false;
        }

        public bool LayoutNeeded => layoutNeeded;

        public virtual void InvalidateLayout()
        {
            layoutNeeded = true;
            //valid = false;
        }

        public virtual void Invalidate()
        {
            valid = false;
            parent?.Invalidate();
        }

        public int X
        {
            get { return x; }
            //set
            //{
            //    if (x != value)
            //    {
            //        x = value;
            //    }
            //}
        }

        public int Y
        {
            get { return y; }
            //set
            //{
            //    if (y != value)
            //    {
            //        y = value;
            //    }
            //}
        }

        public int Width
        {
            get { return width; }
            //set { SetSize(value, height); }
        }

        public int Height
        {
            get { return height; }
            //set { SetSize(width, value); }
        }

        public int FixedWidth
        {
            get { return fixedWidth; }
            //set { SetFixedSize(value, fixedHeight); }
        }

        public int FixedHeight
        {
            get { return fixedHeight; }
            //set { SetFixedSize(fixedWidth, value); }
        }

        public bool SetFixedBounds(int x, int y, int width, int height)
        {
            ValidateSize(ref width, ref height);
            if (this.x != x || this.y != y || fixedWidth != width || fixedHeight != height)
            {
                this.x = x;
                this.y = y;
                fixedWidth = width;
                fixedHeight = height;
                if (fixedWidth > 0) { this.width = fixedWidth; }
                if (fixedHeight > 0) { this.height = fixedHeight; }
                InvalidateLayout();
                return true;
            }
            return false;
        }

        public bool SetBounds(int x, int y, int width, int height)
        {
            ValidateSize(ref width, ref height);
            if (this.x != x || this.y != y || this.width != width || this.height != height)
            {
                this.x = x;
                this.y = y;
                this.width = width;
                this.height = height;
                InvalidateLayout();
                return true;
            }
            return false;
        }

        public bool SetSize(int width, int height)
        {
            ValidateSize(ref width, ref height);
            if (this.width != width || this.height != height)
            {
                this.width = width;
                this.height = height;
                InvalidateLayout();
                return true;
            }
            return false;
        }

        public bool SetFixedSize(int width, int height)
        {
            ValidateSize(ref fixedWidth, ref fixedHeight);
            if (fixedWidth != width || fixedHeight != height)
            {
                fixedWidth = width;
                fixedHeight = height;
                if (fixedWidth > 0) { this.width = fixedWidth; }
                if (fixedHeight > 0) { this.height = fixedHeight; }
                InvalidateLayout();
                return true;
            }
            return false;
        }

        public bool SetPosition(int x, int y)
        {
            if (this.x != x || this.y != y)
            {
                this.x = x;
                this.y = y;
                return true;
            }
            return false;
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

        protected void ValidateSize(ref int width, ref int height)
        {
            if (width > maxWidth) { width = maxWidth; }
            if (height > maxHeight) { height = maxHeight; }
            if (width < minWidth && width > 0) { width = minWidth; }
            if (height < minHeight && height > 0) { height = minHeight; }

        }

        protected void ValidateBounds(ref int x, ref int y, ref int width, ref int height)
        {
            if (width > maxWidth) { width = maxWidth; }
            if (height > maxHeight) { height = maxHeight; }
            if (width < minWidth) { width = minWidth; }
            if (height < minHeight) { height = minHeight; }

        }

        public bool ExcludeFromLayout
        {
            get { return excludeFromLayout; }
            set
            {
                if (excludeFromLayout != value)
                {
                    excludeFromLayout = value;
                }
            }
        }

        public Padding Margin
        {
            get { return margin; }
            set { margin = value; }
        }
        public Padding Padding
        {
            get { return padding; }
            set { padding = value; }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set
            {
                if (horizontalAlignment != value)
                {
                    horizontalAlignment = value;
                    Invalidate();
                }
            }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return verticalAlignment; }
            set
            {
                if (verticalAlignment != value)
                {
                    verticalAlignment = value;
                    Invalidate();
                }
            }
        }

        public HorizontalAlignment HorizontalTextAlignment
        {
            get { return horizontalTextAlignment; }
            set
            {
                if (horizontalTextAlignment != value)
                {
                    horizontalTextAlignment = value;
                    Invalidate();
                }
            }
        }

        public VerticalAlignment VerticalTextAlignment
        {
            get { return verticalTextAlignment; }
            set
            {
                if (verticalTextAlignment != value)
                {
                    verticalTextAlignment = value;
                    Invalidate();
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
                    skin.Apply(this);
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
                    Invalidate();
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
                    Invalidate();
                }
            }
        }

        public bool Active
        {
            get { return active; }
            set
            {
                if (active != value)
                {
                    active = value;
                    skin.Apply(this);
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
                    skin.Apply(this);
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
                    skin.Apply(this);
                }
            }
        }

        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    skin.Apply(this);
                }

            }
        }

        public TextureRegion? BackgroundRegion
        {
            get { return backgroundRegion; }
            set
            {
                if (backgroundRegion != value)
                {
                    backgroundRegion = value;
                    Invalidate();
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
                    Invalidate();
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
                    Invalidate();
                }
            }
        }

        public Color BorderShineColor
        {
            get { return borderShineColor; }
            set
            {
                if (borderShineColor != value)
                {
                    borderShineColor = value;
                    Invalidate();
                }
            }
        }


        public Color BorderShadowColor
        {
            get { return borderShadowColor; }
            set
            {
                if (borderShadowColor != value)
                {
                    borderShadowColor = value;
                    Invalidate();
                }
            }
        }

        public Color KnobColor
        {
            get { return knobColor; }
            set
            {
                if (knobColor != value)
                {
                    knobColor = value;
                    Invalidate();
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
                    Invalidate();
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
                    Invalidate();
                }
            }
        }

        public TextFont? Font
        {
            get
            {
                if (font != null) return font;
                if (parent != null) { return parent.Font; }
                if (skin != null) { return skin.DefaultFont; }
                return null;
            }
            set
            {
                if (font != value)
                {
                    font = value;
                    Invalidate();
                }
            }
        }

        public string? Text
        {
            get => text;
            set
            {
                if (text != value)
                {
                    text = value;
                    Invalidate();
                }
            }
        }

        public Icons Icon
        {
            get => icon;
            set
            {
                if (icon != value)
                {
                    icon = value;
                    Invalidate();
                }
            }
        }

        public bool SuperBitmap => superBitmap;
        public Texture? Bitmap => bitmap;

        public Rectangle BoundingRectangle
        {
            get
            {
                return GetBounds();
            }
        }

        public Rectangle ContentRectangle
        {
            get
            {
                var rect = BoundingRectangle;
                return new Rectangle(rect.Left + padding.Left, rect.Top + padding.Top, rect.Width - padding.Horizontal, rect.Height - padding.Vertical);
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

        public bool Resize(Rectangle startRect, int dx, int dy, HitTestResult hitTestResult)
        {
            Rectangle newRect = startRect;
            switch (hitTestResult)
            {
                case HitTestResult.SizeLeft:
                    newRect.X += dx;
                    newRect.Width -= dx;
                    break;
                case HitTestResult.SizeRight:
                    newRect.Width += dx;
                    break;
                case HitTestResult.SizeTop:
                    newRect.Y += dy;
                    newRect.Height -= dy;
                    break;
                case HitTestResult.SizeBottom:
                    newRect.Height += dy;
                    break;
                case HitTestResult.SizeBottomLeft:
                    newRect.X += dx;
                    newRect.Width -= dx;
                    newRect.Height += dy;
                    break;
                case HitTestResult.SizeBottomRight:
                    newRect.Width += dx;
                    newRect.Height += dy;
                    break;
                case HitTestResult.SizeTopLeft:
                    newRect.X += dx;
                    newRect.Width -= dx;
                    newRect.Y += dy;
                    newRect.Height -= dy;
                    break;
                case HitTestResult.SizeTopRight:
                    newRect.Width += dx;
                    newRect.Y += dy;
                    newRect.Height -= dy;
                    break;
            }
            if (newRect.X < 0) { newRect.X = 0; }
            if (newRect.Y < 0) { newRect.Y = 0; }
            if (parent != null)
            {
                //int maxX = parent.Width- newWidth;
                //int maxY = parent.Height - newHeight;
                //if (newX > maxX) { newX = maxX; }
                //if (newY > maxY) { newY = maxY; }
            }
            if (newRect != startRect)
            {
                if (newRect.Width > width)
                {
                    additionalSizeIncrease = Math.Max(additionalSizeIncrease, newRect.Width - width);
                }
                if (newRect.Height > height)
                {
                    additionalSizeIncrease = Math.Max(additionalSizeIncrease, newRect.Height - height);
                }
                return SetFixedBounds(newRect.X, newRect.Y, newRect.Width, newRect.Height);
            }
            return false;
        }

        public bool Move(int dx, int dy)
        {
            int newX = x + dx;
            int newY = y + dy;
            if (newX < 0) { newX = 0; }
            if (newY < 0) { newY = 0; }
            if (parent != null)
            {
                int maxX = parent.Width - width;
                int maxY = parent.Height - height;
                if (newX > maxX) { newX = maxX; }
                if (newY > maxY) { newY = maxY; }
            }
            if (newX != x || newY != y)
            {
                x = newX;
                y = newY;
                return true;
            }
            return false;
        }

        //public abstract Size GetContentSize(IGuiSystem context);

        public virtual HitTestResult GetHitTestResult(int x, int y)
        {
            if (Contains(x, y))
            {
                return HitTestResult.Control;
            }
            return HitTestResult.None;
        }

        public virtual bool Contains(int x, int y)
        {
            return GetBounds().Contains(x, y);
        }

        public bool HasParent(Control control)
        {
            return parent != null && ((parent == control) || parent.HasParent(control));
        }
        public virtual void Update(IGuiSystem gui, GameTime gameTime)
        {
            if (layoutNeeded)
            {
                PerformLayout(gui);
            }
        }

        public virtual void Draw(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, int offsetX = 0, int offsetY = 0)
        {
            Rectangle rect = GetBounds();
            rect.Offset(offsetX, offsetY);
            DrawControl(gui, renderer, gameTime, ref rect);
        }

        protected virtual void DrawControl(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, ref Rectangle bounds)
        {
            DrawControlBorder(gui, renderer, gameTime, ref bounds);
            DrawControlText(gui, renderer, gameTime, ref bounds);
        }

        protected virtual Rectangle AdjustBorderBounds(ref Rectangle bounds)
        {
            return bounds;
        }

        protected void DrawControlBorder(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, ref Rectangle rect)
        {
            Rectangle bounds = AdjustBorderBounds(ref rect);
            if (backgroundRegion != null)
            {
                renderer.DrawRegion(backgroundRegion, bounds);
            }
            else if (!backgroundColor.IsEmpty && backgroundColor != Color.Transparent)
            {
                renderer.FillRectangle(bounds, backgroundColor);
            }
            if (borderThickness != 0)
            {
                renderer.DrawBorder(bounds, borderShineColor, borderShadowColor, borderThickness);
            }
        }

        protected void DrawControlText(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, ref Rectangle bounds)
        {
            if (textIsTitle) return;
            if (!string.IsNullOrEmpty(Text) && (Icon != Icons.NONE || alwaysUseIconSpace))
            {
                Rectangle textBounds = bounds;
                Rectangle iconBounds = bounds;
                iconBounds.Width = (ICONWIDTH * 3) / 2;
                switch (HorizontalTextAlignment)
                {
                    case HorizontalAlignment.Left:
                        textBounds.X += ICONWIDTH * 2;
                        textBounds.Width -= ICONWIDTH * 2;
                        break;
                    case HorizontalAlignment.Right:
                        break;
                }
                renderer.DrawText(Font, Text, textBounds, TextColor, HorizontalTextAlignment, VerticalTextAlignment);
                renderer.DrawIcon(Icon, iconBounds, TextColor, HorizontalAlignment.Center, VerticalTextAlignment);
            }
            else if (!string.IsNullOrEmpty(Text))
            {
                renderer.DrawText(Font, Text, bounds, TextColor, HorizontalTextAlignment, VerticalTextAlignment);
            }
            else if (Icon != Icons.NONE)
            {
                renderer.DrawIcon(Icon, bounds, TextColor, HorizontalTextAlignment, VerticalTextAlignment);
            }
        }


        internal bool NeedsNewBitmap
        {
            get
            {
                return superBitmap && (!valid || bitmap == null);
            }
        }


        internal void PushBitmap(IGuiSystem gui)
        {
            CheckBitmap(gui);
            gui.GraphicsDevice.PushTarget(bitmap);
            gui.GraphicsDevice.Color = Color.FromArgb(0, 0, 0, 0);
            gui.GraphicsDevice.Clear();
        }

        internal void PopBitmap(IGuiSystem gui)
        {
            gui.GraphicsDevice.PopTarget();
            valid = true;
        }

        private void CheckBitmap(IGuiSystem gui)
        {
            if (bitmap == null || bitmap.Width < width || bitmap.Height < height)
            {
                InitBitmap(gui);
            }
        }

        private void InitBitmap(IGuiSystem gui)
        {
            bitmap?.Dispose();
            bitmap = gui.GraphicsDevice.CreateTexture(ToString(), width + additionalSizeIncrease * 2, height + additionalSizeIncrease * 2);
            if (bitmap != null)
            {
                bitmap.BlendMode = BlendMode.Blend;
            }
        }
        public virtual bool OnPointerTimerTick(PointerEventArgs args) { return true; }
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

        protected void ToBack(Control control)
        {
            if (children.Count > 1)
            {
                int index = children.IndexOf(control);
                if (index >= 0)
                {
                    children.RemoveAt(index);
                    children.Insert(0, control);
                    Invalidate();
                }
            }
        }

        protected void ToFront(Control control)
        {
            if (children.Count > 1)
            {
                int index = children.IndexOf(control);
                if (index >= 0)
                {
                    children.RemoveAt(index);
                    children.Add(control);
                    Invalidate();
                }
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name} #{controlId}";
        }
    }
}
