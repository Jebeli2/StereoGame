namespace StereoGame.Framework.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TextInputEventArgs : EventArgs
    {
        private readonly string text;

        public TextInputEventArgs(string text)
        {
            this.text = text;
        }

        public string Text { get { return text; } }
    }
}
