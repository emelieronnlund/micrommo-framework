using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MicroMMO
{
    static class TilemapRendererExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, Tilemap map)
        {
            for (int y = 0; y < map.GridHeight; y++)
            {
                for (int x = 0; x < map.GridWidth; x++)
                {
                    int tileIndex = map.Tiles[(y * map.GridWidth) + x];
                    Rectangle dst = new Rectangle(x * map.CellWidth + (int)map.CameraOffset.X, y * map.CellHeight + (int)map.CameraOffset.Y,
                        map.CellWidth, map.CellHeight);
                    spriteBatch.Draw(map.TileAtlas, dst, map.TileAtlasIndex[tileIndex], Color.White);
                }
            }
        }
    }
}
