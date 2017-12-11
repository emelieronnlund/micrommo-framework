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
    class Tilemap : DrawableGameComponent
    {
        public int CellWidth { get; } = 16;
        public int CellHeight { get; } = 16;

        public int GridWidth { get; } = 32;
        public int GridHeight { get; } = 32;

        public int[] Tiles { private set; get; }

        //public Vector2 oldCameraOffset = new Vector2(0.0f, 0.0f);
        //public Vector2 CameraOffset = new Vector2(0.0f, 0.0f);
        bool Scrolling = false;

        public Texture2D TileAtlas;
        public List<Rectangle> TileAtlasIndex;
        SpriteBatch spriteBatch;

        public Rectangle Bounds;
        public Camera camera;

        public Tilemap ChunkNeighbourEast;
        public Tilemap ChunkNeighbourWest;
        public Tilemap ChunkNeighbourNorth;
        public Tilemap ChunkNeighbourSouth;

        public Tilemap(Game game) : base(game)
        {
            //Point _pointCameraOffset = CameraOffset.ToPoint();

            //Bounds = new Rectangle(_pointCameraOffset.X, _pointCameraOffset.Y, CellWidth * GridWidth, CellHeight * GridHeight);
            Bounds = Rectangle.Empty;
        }

        public Point SizeInPixels {
            get
            {
                return new Point(CellWidth * GridWidth, CellHeight * GridHeight);
            }
        }
        
        protected override void LoadContent()
        {
            base.LoadContent();

            TileAtlas = Game.Content.Load<Texture2D>("monotiles");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public void LoadStuff()
        {
            LoadContent();
        }
        public override void Initialize()
        {
            base.Initialize();
            Tiles = new int[GridWidth * GridHeight];
            TileAtlasIndex = GenerateTileIndex(TileAtlas);
            Randomize();
        }

        bool LeftMouseDown = false;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Game.IsActive)
            {
                MouseState mouse = Mouse.GetState();
                KeyboardState keyb = Keyboard.GetState();

                //if (mouse.MiddleButton == ButtonState.Pressed)
                //{
                //    if (Scrolling != true)
                //    {
                //        Scrolling = true;
                //        oldCameraOffset.X = mouse.Position.X - CameraOffset.X;
                //        oldCameraOffset.Y = mouse.Position.Y - CameraOffset.Y;
                //    }

                //    CameraOffset.X = mouse.Position.X - oldCameraOffset.X;
                //    CameraOffset.Y = mouse.Position.Y - oldCameraOffset.Y;
                //}

                //if (mouse.MiddleButton == ButtonState.Released)
                //{
                //    Scrolling = false;
                //}

                //if (Scrolling)
                //{
                //    CameraOffset.X = mouse.Position.X - oldCameraOffset.X;
                //    CameraOffset.Y = mouse.Position.Y - oldCameraOffset.Y;
                //}

                //if (keyb.IsKeyDown(Keys.R))
                //{
                //    Randomize();
                //}

                //if (mouse.LeftButton == ButtonState.Pressed)
                //{
                //    if (!LeftMouseDown)
                //    {
                //        int? tile = ScreenToTileIndex(mouse.Position);
                //        if (tile != null)
                //        {
                //            Tiles[(int)tile] = 0;
                //        }
                //        //LeftMouseDown = true;

                //        //Console.WriteLine("Point: {0}", ScreenToTileCoordinates(mouse.Position));

                //    }
                //}
                //else
                //{
                //    LeftMouseDown = false;
                //}

                if (keyb.IsKeyDown(Keys.D1))
                {

                    const int brushWidth = 8;
                    const int brushHeight = 8;

                    
                    const int newTile = 1;
                    int? tile = ScreenToTileIndex(mouse.Position);
                    Point tileCoords = ScreenToTileCoordinates(mouse.Position);

                    //if(tile.HasValue)
                    //{
                    //    int beginX = tileCoords.X - brushWidth / 2;
                    //    int beginY = tileCoords.Y - brushHeight / 2;

                    //    int endX = brushWidth / 2 + tileCoords.X;
                    //    int endY = brushHeight / 2 + tileCoords.Y;

                    //    Point brushTileCoords = new Point();

                    //    for(int y = beginY; y < endY; y++)
                    //    {
                    //        for(int x = beginX; x < endX; x++)
                    //        {
                    //            brushTileCoords.X = x;
                    //            brushTileCoords.Y = y;

                    //            if(x < GridWidth && y < GridHeight && x >= 0 && y >= 0)
                    //            {
                    //                Tiles[(int)TileCoordinatesToTileIndex(brushTileCoords)] = newTile;
                    //            }
                    //        }
                    //    }
                    //}
                    //if (tile != null && tile > GridWidth && tile < Tiles.Length - GridWidth - 1)
                    //{
                    //    Tiles[(int)tile - 1] = newTile;
                    //    Tiles[(int)tile - 0] = newTile;
                    //    Tiles[(int)tile + 1] = newTile;

                    //    Tiles[(int)tile - 1 + GridWidth] = newTile;
                    //    Tiles[(int)tile - 0 + GridWidth] = newTile;
                    //    Tiles[(int)tile + 1 + GridWidth] = newTile;

                    //    Tiles[(int)tile - 1 - GridWidth] = newTile;
                    //    Tiles[(int)tile - 0 - GridWidth] = newTile;
                    //    Tiles[(int)tile + 1 - GridWidth] = newTile;
                    //}

                }

                //Bounds.X = (int) CameraOffset.X;
                //Bounds.Y = (int) CameraOffset.Y;

                //Bounds.X = (int)camera.Position.X + (int)CameraOffset.X;
                //Bounds.Y = (int)camera.Position.Y + (int)CameraOffset.Y;

                //Bounds.X = (int)camera.Position.X;
                //Bounds.Y = (int)camera.Position.Y;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);


            spriteBatch.Begin();

            spriteBatch.Draw(this);
            //for (int y = 0; y < GridHeight; y++)
            //    {
            //        for (int x = 0; x < GridWidth; x++)
            //        {
            //            int tileIndex = Tiles[(y * GridWidth) + x];
            //            Rectangle dst = new Rectangle(x * CellWidth + (int)CameraOffset.X, y * CellHeight + (int)CameraOffset.Y,
            //                CellWidth, CellHeight);
            //            spriteBatch.Draw(TileAtlas, dst, TileAtlasIndex[tileIndex], Color.White);
            //        }
            //    }

            spriteBatch.End();
        }

        List<Rectangle> GenerateTileIndex(Texture2D atlas)
        {
            List<Rectangle> index = new List<Rectangle>();

            int atlasWidth = atlas.Width / CellWidth;
            int atlasHeight = atlas.Height / CellHeight;

            for (int y = 0; y < atlasHeight; y++)
            {
                for (int x = 0; x < atlasWidth; x++)
                {
                    Rectangle tileRect = new Rectangle(x * CellWidth,
                        y * CellHeight,
                        CellWidth,
                        CellHeight);
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

        public Point ScreenToTileCoordinates(Point screenCoordinates)
        {
            Point result = new Point();

            result = screenCoordinates - camera.Position.ToPoint();

            Point sizeOfMap = new Point(CellWidth * GridWidth, CellHeight * GridHeight);

            if (result.X < 0 || result.Y < 0 || result.X > sizeOfMap.X || result.Y > sizeOfMap.Y)
            {
                return new Point(-1, -1);
            }

            result.X = result.X / CellWidth;
            result.Y = result.Y / CellHeight;

            return result;
        }

        public Point ScreenToTileCoordinates(Point screenCoordinates, Camera cam)
        {
            Point result = new Point();

            result.X = screenCoordinates.X + (int)cam.Bounds.X - Bounds.X;
            result.Y = screenCoordinates.Y + (int)cam.Bounds.Y - Bounds.Y;

            //result = screenCoordinates - cam.Position.ToPoint();

            Point sizeOfMap = new Point(CellWidth * GridWidth, CellHeight * GridHeight);

            if (result.X < 0 || result.Y < 0 || result.X > sizeOfMap.X || result.Y > sizeOfMap.Y)
            {
                return new Point(-1, -1);
            }

            result.X = result.X / CellWidth;
            result.Y = result.Y / CellHeight;

            return result;
        }

        //Point ScreenToTileCoordinates(Point screenCoordinates)
        //{
        //    Point result = new Point();

        //    result.X = screenCoordinates.X - (int)CameraOffset.X;
        //    result.Y = screenCoordinates.Y - (int)CameraOffset.Y;

        //    Point sizeOfMap = new Point(CellWidth * GridWidth, CellHeight * GridHeight);

        //    if (result.X < 0 || result.Y < 0 || result.X > sizeOfMap.X || result.Y > sizeOfMap.Y)
        //    {
        //        return new Point(-1, -1);
        //    }

        //    result.X = result.X / CellWidth;
        //    result.Y = result.Y / CellHeight;

        //    return result;
        //}
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
        int? ScreenToTileIndex(Point screenCoordinates)
        {
            Point tileCoords = ScreenToTileCoordinates(screenCoordinates);

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
