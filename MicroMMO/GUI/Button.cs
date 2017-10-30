
using Microsoft.Xna.Framework;

namespace MicroMMO
{
    partial class GUI : DrawableGameComponent
    {
        abstract class Button
        {
            public Rectangle Box { get; set; }
            public abstract void OnClick();
            public abstract void OnMouseEnter();
            public abstract void OnMouseExit();
            public bool MouseOver { get; set; } = false;
            public bool LeftMouseDown { get; set; } = false;

            enum GuiButtonState
            {

            }
        }
    }
}
