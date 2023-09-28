using StereoGame.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public class ControlStyle
    {
        //private readonly Dictionary<int, ControlStyle> previousStyles = new();
        private readonly string name;
        private readonly Type targetType;


        public ControlStyle()
            : this(typeof(Control))
        {

        }
        public ControlStyle(Type targetType)
            : this(targetType.Name, targetType)
        {
        }
        public ControlStyle(string name, Type targetType)
        {
            this.name = name;
            this.targetType = targetType;
        }

        public ControlStyle(ControlStyle other)
        {
            name = other.name;
            targetType = other.targetType;
            BackgroundRegion = other.BackgroundRegion;
            BackgroundColor = other.BackgroundColor;
            BorderColor = other.BorderColor;
            BorderShadowColor = other.BorderShadowColor;
            BorderThickness = other.BorderThickness;
            TextColor = other.TextColor;
            Padding = other.Padding;
        }

        public string Name => name;
        public Type TargetType => targetType;
        public TextureRegion? BackgroundRegion { get; set; }
        public Color? BackgroundColor { get; set; }
        public Color? BorderColor { get; set; }
        public Color? BorderShadowColor { get; set; }
        public int? BorderThickness { get; set; }
        public Color? TextColor { get; set; }
        public Padding? Padding { get; set; }
        public Padding? Margin { get; set; }

        public ControlStyle Combine(ControlStyle? other)
        {
            if (other != null)
            {
                if (other.BackgroundRegion != null) { BackgroundRegion = other.BackgroundRegion; }
                if (other.BackgroundColor != null) { BackgroundColor = other.BackgroundColor; }
                if (other.BorderColor != null) { BorderColor = other.BorderColor; }
                if (other.BorderShadowColor != null) { BorderShadowColor = other.BorderShadowColor; }
                if (other.BorderThickness != null) { BorderThickness = other.BorderThickness; }
                if (other.TextColor != null) { TextColor = other.TextColor; }
                if (other.Padding != null) { Padding = other.Padding; }
                if (other.Margin != null) { Margin = other.Margin; }
            }
            return this;
        }

        public ControlStyle Combine(Control control, ControlStyle other)
        {
            ControlStyle combi = new ControlStyle(this);
            combi.Combine(other);
            if (!control.Enabled) { combi.Combine(other.DisabledStyle); }
            if (control.Hovered && control.Checked) { combi.Combine(other.CheckedHoverStyle); }
            else if (control.Hovered) { combi.Combine(other.HoverStyle); }
            else if (control.Checked) { combi.Combine(other.CheckedStyle); }
            if (control.Pressed) { combi.Combine(other.PressedStyle); }
            return combi;
        }

        public ControlStyle? HoverStyle { get; set; }
        public ControlStyle? DisabledStyle { get; set; }
        public ControlStyle? PressedStyle { get; set; }
        public ControlStyle? CheckedStyle { get; set; }
        public ControlStyle? CheckedHoverStyle { get; set; }

        public void ApplyIf(Control control, bool prediacte)
        {
            if (prediacte)
            {
                Apply(control);
            }
        }

        public void Apply(Control control)
        {
            ApplyStyle(control, this);
        }

        private static void ApplyStyle(Control control, ControlStyle? style)
        {
            if (style == null) return;
            if (style.BackgroundRegion != null) { control.BackgroundRegion = style.BackgroundRegion; }
            if (style.BackgroundColor != null) { control.BackgroundColor = style.BackgroundColor.Value; }
            if (style.BorderColor != null) { control.BorderColor = style.BorderColor.Value; }
            if (style.BorderShadowColor != null) { control.BorderShadowColor = style.BorderShadowColor.Value; }
            if (style.BorderThickness != null) { control.BorderThickness = style.BorderThickness.Value; }
            if (style.TextColor != null) { control.TextColor = style.TextColor.Value; }
            if (style.Padding != null) { control.Padding = style.Padding.Value; }
            if (style.Margin != null) { control.Margin = style.Margin.Value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name); sb.Append(':');
            if (BackgroundColor != null) { sb.Append("Background:"); sb.Append(BackgroundColor.Value); sb.Append('-'); }
            if (BorderColor != null) { sb.Append("Border:"); sb.Append(BorderColor.Value); sb.Append('-'); }
            if (BorderThickness != null) { sb.Append("Thickness:"); sb.Append(BorderThickness.Value); sb.Append('-'); }
            if (TextColor != null) { sb.Append("Text:"); sb.Append(TextColor.Value); sb.Append('-'); }
            sb.Length -= 1;
            return sb.ToString();
        }
    }
}
