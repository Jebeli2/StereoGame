using StereoGame.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public abstract class LayoutControl : Control
    {
        protected LayoutControl(Control? parent) : base(parent)
        {
        }
    }
}
