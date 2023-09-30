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
                        BackgroundColor = Color.FromArgb(52,52,52),
                        BorderColor = Color.FromArgb(67,67,70),
                        BorderShineColor = Color.FromArgb(188,200, 200, 200),
                        BorderShadowColor = Color.FromArgb(188,10, 10, 10),
                        BorderThickness =1,
                        TextColor = Color.FromArgb(240,240,240),
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
                        BorderShineColor  = Color.Transparent,
                        BorderShadowColor= Color.Transparent,
                        BorderThickness = 0,
                        Padding = new Padding(0),
                        Margin = new Padding(0)
                    },
                    new ControlStyle(typeof(Window))
                    {
                        BackgroundColor = Color.FromArgb(188,100,100,100),
                        BorderColor = Color.FromArgb(188,100,100,100),
                        BorderShineColor = Color.FromArgb(200,200, 200, 200),
                        BorderShadowColor = Color.FromArgb(200,10, 10, 10),
                        BorderThickness = 0,
                        Padding = new Padding(4,30,4,4),
                        ActiveStyle = new ControlStyle
                        {
                            BorderColor = Color.FromArgb(200,62,92,154),
                        }
                    },
                    new ControlStyle(typeof(Label))
                    {
                        BackgroundColor = Color.Transparent,
                        BorderColor = Color.Transparent,
                        BorderShineColor = Color.Transparent,
                        BorderShadowColor= Color.Transparent,
                        BorderThickness = 0,
                        Padding = new Padding(0),
                        Margin = new Padding(5,0,5,0)
                    },
                    new ControlStyle(typeof(Button))
                    {
                        HoverStyle = new ControlStyle
                        {
                            BackgroundColor = Color.FromArgb(64,64,64),
                            BorderShineColor = Color.FromArgb(188,200, 200, 200),
                            BorderShadowColor = Color.FromArgb(188,10, 10, 10),
                            BorderThickness =1,
                        },
                        PressedStyle = new ControlStyle
                        {
                            BackgroundColor = Color.FromArgb(80,80,80),
                            BorderShineColor = Color.FromArgb(188,10, 10, 10),
                            BorderShadowColor = Color.FromArgb(188,200, 200, 200),
                            BorderThickness =1,
                        }
                    },
                    new ControlStyle(typeof(SysButton))
                    {
                        BackgroundColor = Color.Transparent,
                        BorderColor = Color.Transparent,
                        BorderShineColor  = Color.Transparent,
                        BorderShadowColor= Color.Transparent,
                        BorderThickness = 0,
                        Padding = new Padding(0),
                        Margin = new Padding(0),
                        HoverStyle = new ControlStyle
                        {
                            BackgroundColor = Color.FromArgb(46,200,200,200),
                        },
                        PressedStyle = new ControlStyle
                        {
                            BackgroundColor = Color.FromArgb(46,10,10,10),
                        }
                    },
                    new ControlStyle(typeof(ToggleButton))
                    {
                        CheckedStyle = new ControlStyle
                        {
                            BackgroundColor = Color.FromArgb(20,20,20),
                            BorderShineColor = Color.FromArgb(188,10, 10, 10),
                            BorderShadowColor = Color.FromArgb(188,200, 200, 200),
                            BorderThickness =1,
                        },
                        CheckedHoverStyle = new ControlStyle
                        {
                            BackgroundColor = Color.FromArgb(34,34,34),
                            BorderShineColor = Color.FromArgb(188,10, 10, 10),
                            BorderShadowColor = Color.FromArgb(188,200, 200, 200),
                            BorderThickness =1,

                        }
                    },
                    new ControlStyle(typeof(CheckBox))
                    {
                        //BorderThickness = 0,
                        BackgroundColor = Color.Transparent,
                        CheckedStyle = new ControlStyle
                        {
                            BorderShineColor = Color.FromArgb(188,200, 200, 200),
                            BorderShadowColor = Color.FromArgb(188,10, 10, 10),
                            BorderThickness =1,
                        },
                        HoverStyle = new ControlStyle
                        {
                            BackgroundColor = Color.FromArgb(64,64,64),
                            BorderShineColor = Color.FromArgb(188,200, 200, 200),
                            BorderShadowColor = Color.FromArgb(188,10, 10, 10),
                            BorderThickness =1,
                        },
                        CheckedHoverStyle = new ControlStyle
                        {
                            BackgroundColor = Color.FromArgb(64,64,64),
                            BorderShineColor = Color.FromArgb(188,200, 200, 200),
                            BorderShadowColor = Color.FromArgb(188,10, 10, 10),
                            BorderThickness =1,
                        },
                    },
                    new ControlStyle(typeof(PropControl))
                    {
                        BackgroundColor = Color.FromArgb(52,52,52),
                        KnobColor = Color.FromArgb(100,100,100),
                        BorderColor = Color.Transparent,
                        BorderShineColor  = Color.Transparent,
                        BorderShadowColor= Color.Transparent,
                        BorderThickness = 0,
                        HoverStyle = new ControlStyle
                        {
                            BackgroundColor = Color.FromArgb(64,64,64),
                            KnobColor = Color.FromArgb(120,120,120),
                        },

                    }

                }
            };
        }
    }
}
