using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MicroMMO
{
    public enum MouseButton
    {
        LeftButton,
        RightButton,
        Middle,
        X1,
        X2,
        None
    }

    class MouseEventArgs : EventArgs
    {
        public Point Position;
        public MouseButton Button;
    }
}
