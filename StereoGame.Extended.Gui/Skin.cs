using StereoGame.Framework.Collections;
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
        private static readonly Skin defaultSkin = CreateDefaultSkin();
        private readonly KeyedCollection<string, ControlStyle> styles = new KeyedCollection<string, ControlStyle>(s => s.Name);

        public Skin()
        {

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

        public ControlStyle? GetStyle(string name)
        {
            if (styles.TryGetValue(name, out var style)) return style;
            return null;
        }

        public ControlStyle? GetStyle(Type controlType)
        {
            return GetStyle(controlType.Name);
        }

        public void Apply(Control control)
        {
            var types = new List<Type>();
            var controlType = control.GetType();
            while (controlType != null)
            {
                types.Add(controlType);
                controlType = controlType.BaseType;
            }
            for (int i = types.Count - 1; i >= 0; i--)
            {
                var style = GetStyle(types[i]);
                style?.Apply(control);
            }
        }

        private static Skin CreateDefaultSkin()
        {
            return new Skin
            {
                Styles =
                {
                    new ControlStyle(typeof(Control))
                    {
                        BackgroundColor = Color.FromArgb(51,51,55),
                        BorderColor = Color.FromArgb(67,67,70),
                        BorderThickness =1,
                        TextColor = Color.FromArgb(241,241,241),
                        DisabledStyle = new ControlStyle
                        {
                            TextColor = Color.FromArgb(78,78,80)
                        }
                    },
                    new ControlStyle(typeof(LayoutControl))
                    {
                        BackgroundColor = Color.Transparent,
                        BorderColor = Color.Transparent,
                        BorderThickness = 0
                    },
                    new ControlStyle(typeof(Window))
                    {
                        BackgroundColor = Color.DarkGray,
                        BorderColor = Color.LightGray,
                        BorderThickness = 1
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
