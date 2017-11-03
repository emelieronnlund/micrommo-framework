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

        Vector2 oldCameraOffset = new Vector2(0.0f, 0.0f);
        Vector2 CameraOffset = new Vector2(0.0f, 0.0f);
        bool Scrolling = false;

        Texture2D TileAtlas;
        List<Rectangle> TileAtlasIndex;
        SpriteBatch spriteBatch;

        public Tilemap(Game game) : base(game) { }

        protected override void LoadContent()
        {
            base.LoadContent();

            TileAtlas = Game.Content.Load<Texture2D>("monotiles");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
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

                if (mouse.MiddleButton == ButtonState.Pressed)
                {
                    if (Scrolling != true)
                    {
                        Scrolling = true;
                        oldCameraOffset.X = mouse.Position.X - CameraOffset.X;
                        oldCameraOffset.Y = mouse.Position.Y - CameraOffset.Y;
                    }

                    CameraOffset.X = mouse.Position.X - oldCameraOffset.X;
                    CameraOffset.Y = mouse.Position.Y - oldCameraOffset.Y;
                }

                if (mouse.MiddleButton == ButtonState.Released)
                {
                    Scrolling = false;
                }

                if (Scrolling)
                {
                    CameraOffset.X = mouse.Position.X - oldCameraOffset.X;
                    CameraOffset.Y = mouse.Position.Y - oldCameraOffset.Y;
                }


                if (keyb.IsKeyDown(Keys.R))
                {
                    Randomize();
                }

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (!LeftMouseDown)
                    {
                        int? tile = ScreenToTileIndex(mouse.Position);
                        if (tile != null)
                        {
                            Tiles[(int)tile] = 0;
                        }
                        //LeftMouseDown = true;

                        Console.WriteLine("Point: {0}", ScreenToTileCoordinates(mouse.Position));

                    }
                }
                else
                {
                    LeftMouseDown = false;
                }

                if (keyb.IsKeyDown(Keys.D1))
                {
                    int? tile = ScreenToTileIndex(mouse.Position);
                    if (tile != null && tile > GridWidth && tile < Tiles.Length - GridWidth - 1)
                    {
                        Tiles[(int)tile - 1] = 1;
                        Tiles[(int)tile - 0] = 1;
                        Tiles[(int)tile + 1] = 1;

                        Tiles[(int)tile - 1 + GridWidth] = 1;
                        Tiles[(int)tile - 0 + GridWidth] = 1;
                        Tiles[(int)tile + 1 + GridWidth] = 1;

                        Tiles[(int)tile - 1 - GridWidth] = 1;
                        Tiles[(int)tile - 0 - GridWidth] = 1;
                        Tiles[(int)tile + 1 - GridWidth] = 1;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);


            spriteBatch.Begin();

            for (int y = 0; y < GridHeight; y++)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    int tileIndex = Tiles[(y * GridWidth) + x];
                    Rectangle dst = new Rectangle(x * CellWidth + (int)CameraOffset.X, y * CellHeight + (int)CameraOffset.Y,
                        CellWidth, CellHeight);
                    spriteBatch.Draw(TileAtlas, dst, TileAtlasIndex[tileIndex], Color.White);
                }
            }

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

        void Randomize()
        {
            Random rng = new Random();

            for (int i = 0; i < Tiles.Length; i++)
            {
                Tiles[i] = rng.Next(0, TileAtlasIndex.Count);
            }
        }

        Point ScreenToTileCoordinates(Point screenCoordinates)
        {
            Point result = new Point();

            result.X = screenCoordinates.X - (int)CameraOffset.X;
            result.Y = screenCoordinates.Y - (int)CameraOffset.Y;

            Point sizeOfMap = new Point(CellWidth * GridWidth, CellHeight * GridHeight);

            if (result.X < 0 || result.Y < 0 || result.X > sizeOfMap.X || result.Y > sizeOfMap.Y)
            {
                return new Point(-1, -1);
            }

            result.X = result.X / CellWidth;
            result.Y = result.Y / CellHeight;

            return result;
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
    }
}
