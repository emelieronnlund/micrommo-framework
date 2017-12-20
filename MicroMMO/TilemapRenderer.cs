//#define DEBUG_RENDER_TILEMAPS 

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
            //for (int y = 0; y < map.GridHeight; y++)
            //{
            //    for (int x = 0; x < map.GridWidth; x++)
            //    {
            //        int tileIndex = map.Tiles[(y * map.GridWidth) + x];
            //        Rectangle dst = new Rectangle(x * map.CellWidth + (int)map.CameraOffset.X, y * map.CellHeight + (int)map.CameraOffset.Y,
            //            map.CellWidth, map.CellHeight);
            //        spriteBatch.Draw(map.TileAtlas, dst, map.TileAtlasIndex[tileIndex], Color.White);
            //    }
            //}
        }
        public static void Draw(this SpriteBatch spriteBatch, Tilemap map, Camera cam)
        {
            //for (int y = 0; y < map.GridHeight; y++)
            //{
            //    for (int x = 0; x < map.GridWidth; x++)
            //    {
            //        int tileIndex = map.Tiles[(y * map.GridWidth) + x];
            //        Rectangle dst = new Rectangle(x * map.CellWidth + (int)map.Bounds.X + (int)cam.Position.X, y * map.CellHeight + (int)map.CameraOffset.Y + (int)cam.Position.Y,
            //            map.CellWidth, map.CellHeight);
            //        spriteBatch.Draw(map.TileAtlas, dst, map.TileAtlasIndex[tileIndex], Color.White);
            //    }
            //}

            for (int y = 0; y < map.GridHeight; y++)
            {
                for (int x = 0; x < map.GridWidth; x++)
                {
                    int tileIndex = map.Tiles[(y * map.GridWidth) + x];
                    Rectangle dst = new Rectangle((x * map.CellWidth) + map.Bounds.X + (int)cam.Position.X, (y * map.CellHeight) + map.Bounds.Y + (int)cam.Position.Y,
                        map.CellWidth, map.CellHeight);
                    spriteBatch.Draw(map.TileAtlas, dst, map.TileAtlasIndex[tileIndex], Color.White);
                }
            }
        }

        public static void DebugDrawTilemap(GraphicsDevice gd, Tilemap map, Camera cam)
        {
            for (int y = 0; y < map.GridHeight; y++)
            {
                for (int x = 0; x < map.GridWidth; x++)
                {
                    int tileIndex = map.Tiles[(y * map.GridWidth) + x];
                    Rectangle dst = new Rectangle((x * map.CellWidth) + map.Bounds.X + (int)cam.Position.X, (y * map.CellHeight) + map.Bounds.Y + (int)cam.Position.Y,
                        map.CellWidth, map.CellHeight);

                    gd.DrawUserPrimitives(PrimitiveType.LineStrip, new VertexPosition[] {
                        new VertexPosition(new Vector3(dst.X, dst.Y, 0.0f)),
                        new VertexPosition(new Vector3 (dst.Right, dst.Y, 0.0f)),
                        new VertexPosition(new Vector3 (dst.Right, dst.Bottom, 0.0f)),
                        new VertexPosition(new Vector3 (dst.X, dst.Bottom, 0.0f)) }, 0, 3);
                }
            }
        }
    }
}

