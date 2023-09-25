using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public class ControlStyle
    {
        private readonly Dictionary<int, ControlStyle> previousStyles = new();
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

        public string Name => name;
        public Type TargetType => targetType;
        public Color? BackgroundColor { get; set; }
        public Color? BorderColor { get; set; }
        public int? BorderThickness { get; set; }
        public Color? TextColor { get; set; }
        public ControlStyle? HoverStyle { get; set; }
        public ControlStyle? DisabledStyle { get; set; }
        public ControlStyle? PressedStyle { get; set; }

        public void ApplyIf(Control control, bool prediacte)
        {
            if (prediacte)
            {
                Apply(control);
            }
            else
            {
                Revert(control);
            }
        }

        public ControlStyle Collect(Control control)
        {
            ControlStyle style = new ControlStyle(name, targetType);
            //style.BackgroundColor = control.BackgroundColor; 
            //style.BorderColor = control.BorderColor; 
            //style.BorderThickness = control.BorderThickness; 
            //style.TextColor = control.TextColor; 
            //style.HoverStyle = control.HoverStyle; 
            //style.DisabledStyle = control.DisabledStyle;
            //style.PressedStyle = control.PressedStyle;

            if (BackgroundColor != null) { style.BackgroundColor = control.BackgroundColor; }
            if (BorderColor != null) { style.BorderColor = control.BorderColor; }
            if (BorderThickness != null) { style.BorderThickness = control.BorderThickness; }
            if (TextColor != null) { style.TextColor = control.TextColor; }
            if (HoverStyle != null) { style.HoverStyle = control.HoverStyle; }
            if (DisabledStyle != null) { style.DisabledStyle = control.DisabledStyle; }
            if (PressedStyle != null) { style.PressedStyle = control.PressedStyle; }
            return style;
        }

        public void Apply(Control control)
        {
            var prev = Collect(control);
            previousStyles[control.Id] = prev;
            ApplyStyle(control, this);
            Console.WriteLine($"Apply {this} Prev = {prev}");
        }

        public void Revert(Control control)
        {
            if (previousStyles.TryGetValue(control.Id, out ControlStyle? style))
            {
                ApplyStyle(control, style);
                previousStyles.Remove(control.Id);
                Console.WriteLine($"Revert to {style}");
            }
            else
            {
                Console.WriteLine("Would like to revert to default, but there's nothing here...");
            }
        }

        private static void ApplyStyle(Control control, ControlStyle style)
        {
            if (style.BackgroundColor != null) { control.BackgroundColor = style.BackgroundColor.Value; }
            if (style.BorderColor != null) { control.BorderColor = style.BorderColor.Value; }
            if (style.BorderThickness != null) { control.BorderThickness = style.BorderThickness.Value; }
            if (style.TextColor != null) { control.TextColor = style.TextColor.Value; }
            if (style.HoverStyle != null) { control.HoverStyle = style.HoverStyle; }
            if (style.DisabledStyle != null) { control.DisabledStyle = style.DisabledStyle; }
            if (style.PressedStyle != null) { control.PressedStyle = style.PressedStyle; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name); sb.Append(':');
            if (BackgroundColor != null) { sb.Append("Background:"); sb.Append(BackgroundColor.Value); sb.Append('-'); }
            if (BorderColor != null) { sb.Append("Border:"); sb.Append(BorderColor.Value); sb.Append('-'); }
            if (BorderThickness != null) { sb.Append("Thickness:"); sb.Append(BorderThickness.Value); sb.Append('-'); }
            if (TextColor != null) { sb.Append("Text:"); sb.Append(TextColor.Value); sb.Append('-'); }
            if (HoverStyle != null) { sb.Append("Hover:"); sb.Append(HoverStyle); sb.Append('-'); }
            if (DisabledStyle != null) { sb.Append("Disabled:"); sb.Append(DisabledStyle); sb.Append('-'); }
            if (PressedStyle != null) { sb.Append("Pressed:"); sb.Append(PressedStyle); sb.Append('-'); }
            return sb.ToString();
        }
    }
}
