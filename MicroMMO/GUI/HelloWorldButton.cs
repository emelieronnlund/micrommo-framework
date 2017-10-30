using System;

using Microsoft.Xna.Framework;

namespace MicroMMO
{
    partial class GUI : DrawableGameComponent
    {
        class HelloWorldButton : Button
        {
            public override void OnClick()
            {
                Console.WriteLine("Hello, world!");
            }

            public override void OnMouseEnter()
            {
                Console.WriteLine("Entered button..");
            }

            public override void OnMouseExit()
            {
                Console.WriteLine("Exited button..");
            }
        }
    }
}
