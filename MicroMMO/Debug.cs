using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MicroMMO
{
    static class Debug
    {
        static public Texture2D CreateDebugTexture(GraphicsDevice graphics, Color color)
        {
            Texture2D texture = new Texture2D(graphics,1,1);
            texture.SetData<Color>(new Color[] { color });

            return texture;
        }
    }
}
