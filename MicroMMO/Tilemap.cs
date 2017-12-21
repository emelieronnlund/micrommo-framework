using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MicroMMO
{
    public class Tilemap
    {
        public int CellWidth;
        public int CellHeight;

        public int GridWidth;
        public int GridHeight;

        public int SrcCellWidth;
        public int SrcCellHeight;

        public int[] Tiles { private set; get; }

        public Texture2D TileAtlas;
        public List<Rectangle> TileAtlasIndex;

        public Rectangle Bounds;
        public Camera camera;

        public Tilemap ChunkNeighbourEast;
        public Tilemap ChunkNeighbourWest;
        public Tilemap ChunkNeighbourNorth;
        public Tilemap ChunkNeighbourSouth;

        public Point ChunkCoords { get; set; }
        public Tilemap(Game game, int cellWidth, int cellHeight, 
            int gridWidth, int gridHeight, 
            int srcCellWidth, int srcCellHeight,
            Texture2D tileAtlas)
        {
            CellWidth = cellWidth;
            CellHeight = cellHeight;
            GridWidth = gridWidth;
            GridHeight = gridHeight;

            SrcCellWidth = srcCellWidth;
            SrcCellHeight = srcCellHeight;
            Bounds = Rectangle.Empty;

            Tiles = new int[GridWidth * GridHeight];
        }

        public Point SizeInPixels {
            get
            {
                return new Point(CellWidth * GridWidth, CellHeight * GridHeight);
            }
        }
        
        public List<Rectangle> GenerateTileIndex(Texture2D atlas)
        {
            List<Rectangle> index = new List<Rectangle>();

            int atlasWidth = atlas.Width / SrcCellWidth;
            int atlasHeight = atlas.Height / SrcCellHeight;

            for (int y = 0; y < atlasHeight; y++)
            {
                for (int x = 0; x < atlasWidth; x++)
                {
                    Rectangle tileRect = new Rectangle(x * SrcCellWidth,
                        y * SrcCellHeight,
                        SrcCellWidth,
                        SrcCellHeight);
                    index.Add(tileRect);
                }
            }
            return index;
        }

        public void Randomize()
        {
            Random rng = new Random();

            for (int i = 0; i < Tiles.Length; i++)
            {
                Tiles[i] = rng.Next(0, TileAtlasIndex.Count);
            }
        }

        public Point ScreenToTileCoordinates(Point screenCoordinates, Camera cam)
        {
            Point result = new Point();

            result.X = screenCoordinates.X + cam.Bounds.X - Bounds.X;
            result.Y = screenCoordinates.Y + cam.Bounds.Y - Bounds.Y;

            Point sizeOfMap = new Point(CellWidth * GridWidth, CellHeight * GridHeight);

            if (result.X < 0 || result.Y < 0 || result.X > sizeOfMap.X || result.Y > sizeOfMap.Y)
            {
                return new Point(-1, -1);
            }

            result.X = result.X / CellWidth;
            result.Y = result.Y / CellHeight;

            return result;
        }

        public int? ScreenToTileIndex(Point screenCoordinates, Camera cam)
        {
            Point tileCoords = ScreenToTileCoordinates(screenCoordinates, cam);

            if (tileCoords.X >= 0 && tileCoords.Y >= 0 && tileCoords.X < GridWidth && tileCoords.Y < GridHeight)
            {
                return tileCoords.Y * GridWidth + tileCoords.X;
            }
            else
            {
                return null;
            }
        }

        public int? TileCoordinatesToTileIndex(Point tileCoords)
        {
            if (tileCoords.X >= 0 && tileCoords.Y >= 0 && tileCoords.X < GridWidth && tileCoords.Y < GridHeight)
            {
                return tileCoords.Y * GridWidth + tileCoords.X;
            }
            else
                return null;
        }
    }
}
