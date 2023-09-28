namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IGuiSystem
    {
        int ScreenWidth { get; }
        int ScreenHeight { get; }
        GraphicsDevice GraphicsDevice { get; }

    }
}
