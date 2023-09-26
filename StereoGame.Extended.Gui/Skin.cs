using StereoGame.Framework.Collections;
using StereoGame.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public class Skin
    {
        private static readonly Skin defaultSkin = CreateDefaultSkin(null);
        private readonly KeyedCollection<string, ControlStyle> styles = new KeyedCollection<string, ControlStyle>(s => s.Name);
        private TextFont? defaultFont;
        public Skin(TextFont? defaultFont)
        {
            this.defaultFont = defaultFont;
        }

        public KeyedCollection<string, ControlStyle> Styles
        {
            get { return styles; }
            set
            {
                styles.Clear();
                if (value != null)
                {
                    styles.AddRange(value.Values);
                }
            }
        }

        public static Skin DefaultSkin => defaultSkin;

        public TextFont? DefaultFont
        {
            get => defaultFont;
            set => defaultFont = value;
        }

        public ControlStyle? GetStyle(string name)
        {
            if (styles.TryGetValue(name, out var style)) return style;
            return null;
        }

        public ControlStyle? GetStyle(Type controlType)
        {
            return GetStyle(controlType.Name);
        }

        public ControlStyle? GetStyle(Control control)
        {
            var types = new List<Type>();
            var controlType = control.GetType();
            while (controlType != null)
            {
                types.Add(controlType);
                controlType = controlType.BaseType;
            }
            ControlStyle? cs = null;
            for (int i = types.Count - 1; i >= 0; i--)
            {
                var style = GetStyle(types[i]);
                if (style != null)
                {
                    if (cs == null)
                    {
                        cs = new ControlStyle(style);
                    }
                    else
                    {
                        cs = cs.Combine(control, style);
                    }
                }
            }
            return cs;
        }

        public void Apply(Control control)
        {
            ControlStyle? cs = GetStyle(control);
            cs?.Apply(control);
        }

        private static Skin CreateDefaultSkin(TextFont? font)
        {
            return new Skin(font)
            {
                Styles =
                {
                    new ControlStyle(typeof(Control))
                    {
                        BackgroundColor = Color.FromArgb(51,51,55),
                        BorderColor = Color.FromArgb(67,67,70),
                        BorderThickness =1,
                        TextColor = Color.FromArgb(241,241,241),
                        Padding = new Padding(5),
                        DisabledStyle = new ControlStyle
                        {
                            TextColor = Color.FromArgb(78,78,80)
                        }
                    },
                    new ControlStyle(typeof(LayoutControl))
                    {
                        BackgroundColor = Color.Transparent,
                        BorderColor = Color.Transparent,
                        BorderThickness = 0,
                        Padding = new Padding(0),
                        Margin = new Padding(0)
                    },
                    new ControlStyle(typeof(Window))
                    {
                        BackgroundColor = Color.FromArgb(128, Color.Black),
                        BorderColor = Color.DarkGray,
                        BorderThickness = 1,
                        Padding = new Padding(8)
                    },
                    new ControlStyle(typeof(Button))
                    {
                        HoverStyle = new ControlStyle
                        {
                            BackgroundColor = Color.FromArgb(62,62,64),
                            BorderColor = Color.WhiteSmoke
                        },
                        PressedStyle = new ControlStyle
                        {
                            BackgroundColor = Color.FromArgb(0,122,204)
                        }
                    }

                }
            };
        }
    }
}
