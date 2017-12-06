
using Microsoft.Xna.Framework;

namespace MicroMMO
{
    partial class GUIManager : DrawableGameComponent
    {
        abstract class Button
        {
            public Rectangle Box { get; set; }
            public virtual void OnClick() { }
            public virtual void OnMouseEnter() { }
            public virtual void OnMouseExit() { }
            public bool MouseOver { get; set; } = false;

            //enum GuiButtonState
            //{

            //}
        }
    }
}
