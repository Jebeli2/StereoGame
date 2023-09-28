using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public class ToggleButton : Button
    {
        public ToggleButton(string? text = null)
          : this(null, text)
        {

        }
        public ToggleButton(Control? parent, string? text = null)
            : base(parent, text)
        {
        }

        public override bool OnPointerUp(PointerEventArgs args)
        {
            base.OnPointerUp(args);
            if (BoundingRectangle.Contains(args.X,args.Y)) 
            {
                Checked = !Checked;
            }
            return true;
        }
    }
}
