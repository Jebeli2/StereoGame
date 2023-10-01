namespace StereoGame.Extended.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Slider : PropControl
    {
        private Orientation orientation;
        private int min;
        private int max;
        private int value;
        public Slider(Control? parent, Orientation orientation, int max = 16, int min = 1, int value = 1)
            : base(parent)
        {
            this.orientation = orientation;
            this.min = min;
            this.max = max;
            this.value = value;
            FreeHoriz = orientation == Orientation.Horizontal;
            FreeVert = orientation == Orientation.Vertical;
            AdjustProp();
        }

        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    FreeHoriz = orientation == Orientation.Horizontal;
                    FreeVert = orientation == Orientation.Vertical;
                }
            }
        }
        public int Min
        {
            get { return min; }
            set
            {
                if (min != value)
                {
                    min = value;
                    AdjustProp();
                }
            }
        }

        public int Max
        {
            get { return max; }
            set
            {
                if (max != value)
                {
                    max = value;
                    AdjustProp();
                }
            }
        }

        public int Value
        {
            get { return value; }
            set
            {
                if (value > max) value = max;
                if (value < min) value = min;
                if (this.value != value)
                {
                    this.value = value;
                    AdjustProp();
                }
            }
        }

        protected override void OnPropChanged()
        {
            int total = max - min + 1;
            int pot = orientation == Orientation.Horizontal ? HorizPot : VertPot;
            value = FindSliderLevel(total, pot) + min;
            base.OnPropChanged();
        }

        private void AdjustProp()
        {
            int total = max - min + 1;
            FindSliderValues(total, value + min, out int body, out int pot);
            ModifyProp(FreeHoriz, FreeVert, pot, pot, body, body);
        }

        private static void FindSliderValues(int numLevels, int level, out int body, out int pot)
        {
            if (numLevels > 0)
            {
                body = MAXBODY / numLevels;
            }
            else
            {
                body = MAXBODY;
            }
            if (numLevels > 1)
            {
                pot = (MAXPOT * level) / (numLevels - 1);
            }
            else
            {
                pot = 0;
            }
        }

        private static int FindSliderLevel(int numLevels, int pot)
        {
            if (numLevels > 1)
            {
                return (pot * (numLevels - 1) + MAXPOT / 2) / MAXPOT;
            }
            else
            {
                return 0;
            }
        }
    }
}
