using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MicroMMO
{
    class TilemapManager : DrawableGameComponent
    {
        List<Tilemap> LoadedMapChunks;
        Camera camera;

        public int CellWidth { get; } = 16;
        public int CellHeight { get; } = 16;

        public int GridWidth { get; } = 32;
        public int GridHeight { get; } = 32;

        private Point sizeInPixels;
        public Point SizeInPixels
        {
            get
            {
                return sizeInPixels;
            }
        }

        public Point ChunkGridSize = Point.Zero;
        List<Tilemap> allMaps; // including those without uninitialized tile data.

        private void SetChunkNeighbours()
        {
            for(int i = 0; i < allMaps.Count; i++)
            {
                if(i % ChunkGridSize.X == 0)
                {
                    allMaps[i].ChunkNeighbourWest = null;
                }
                else
                {
                    allMaps[i].ChunkNeighbourWest = allMaps[i-1];
                }

                if(i < ChunkGridSize.X)
                {
                    allMaps[i].ChunkNeighbourNorth = null;
                }
                else
                {
                    allMaps[i].ChunkNeighbourNorth = allMaps[i-ChunkGridSize.X];
                }

                if(i >= ChunkGridSize.X * (ChunkGridSize.Y-1))
                {
                    allMaps[i].ChunkNeighbourSouth = null;
                }
                else
                {
                    allMaps[i].ChunkNeighbourSouth = allMaps[i + ChunkGridSize.X];
                }

                if((i+1) % ChunkGridSize.X == 0)
                {
                    allMaps[i].ChunkNeighbourEast = null;
                }
                else
                {
                    allMaps[i].ChunkNeighbourEast = allMaps[i + 1];
                }
            }
        }
        public void GenerateMapChunks(int countX, int countY)
        {
            Random rng = new Random();

            for(int y = 0; y < countY; y++)
            {
                for(int x = 0; x < countX; x++)
                {
                    Tilemap map = new Tilemap(Game);
                    AddMapChunk(new Point(x, y), map);
                }
            }

            ChunkGridSize.X = countX;
            ChunkGridSize.Y = countY;

            SetChunkNeighbours();
        }

        public void AddMapChunk(Point chunkCoords, Tilemap map)
        {
            Point pixelOffset = chunkCoords * map.SizeInPixels;
            map.Bounds.X = pixelOffset.X;
            map.Bounds.Y = pixelOffset.Y;
            map.Bounds.Width = map.SizeInPixels.X;
            map.Bounds.Height = map.SizeInPixels.Y;

            //map.CameraOffset = pixelOffset.ToVector2();
            map.camera = camera;
            LoadedMapChunks.Add(map);
        }

        InputManager inputManager;
        public TilemapManager(Game game, Camera cam, InputManager input) : base(game)
        {
            LoadedMapChunks = new List<Tilemap>();
            camera = cam;
            inputManager = input;
            sizeInPixels = new Point(CellWidth * GridWidth, CellHeight * GridHeight);
            allMaps = LoadedMapChunks; // change allMaps to its own instance once we have unitialized maps.
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var map in LoadedMapChunks)
            {
                map.Initialize();
            }

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            inputManager.KeyDown += InputManager_KeyDown;
            inputManager.KeyUp += InputManager_KeyUp;
            inputManager.MouseButtonDown += InputManager_MouseButtonDown;
            inputManager.MouseButtonUp += InputManager_MouseButtonUp;
            inputManager.MouseMotion += InputManager_MouseMotion;
            inputManager.MouseButtonPressed += InputManager_MouseButtonPressed;
        }

        private void InputManager_MouseButtonPressed(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButton.LeftButton)
            {
                PlaceTile(2, e.Position);
            }
        }

        private void InputManager_MouseMotion(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("MouseMotion: {0}", e.Position);
            mousePosition = e.Position;
        }

        private void InputManager_MouseButtonUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("MouseButtonUp {0} at {1}", e.Button.ToString(), e.Position);
            if(e.Button == MouseButton.Middle)
            {
                Scrolling = false;
                scrollButtonDown = false;
            }
        }

        Vector2 oldCameraOffset;
        //Vector2 CameraOffset;
        bool scrollButtonDown = false;
        Point mousePosition = Point.Zero;

        private void InputManager_MouseButtonDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("MouseButtonDown {0} at {1}", e.Button.ToString(), e.Position);
            if (e.Button == MouseButton.Middle)
            {
                scrollButtonDown = true;

            }
        }

        private void InputManager_KeyUp(object sender, KeyboardEventArgs e)
        {
            Console.WriteLine("Key up: {0}", e.Key);
            if(e.Key == Keys.D1)
            {
                brushing = false;
            }
        }

        private void InputManager_KeyDown(object sender, KeyboardEventArgs e)
        {
            Console.WriteLine("Key down: {0}", e.Key);
            if(e.Key == Keys.D1)
            {
                brushing = true;
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            foreach (var map in LoadedMapChunks)
            {
                map.LoadStuff();
            }
        }

        List<Tilemap> MapsToDraw = new List<Tilemap>();
        bool Scrolling = false;
        bool brushing = false;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Rectangle cameraBounds = camera.Bounds;


            if (scrollButtonDown == true)
            {
                if (Scrolling != true)
                {
                    Scrolling = true;
                    oldCameraOffset.X = mousePosition.X - camera.Position.X;
                    oldCameraOffset.Y = mousePosition.Y - camera.Position.Y;
                }

                camera.Position.X = mousePosition.X - oldCameraOffset.X;
                camera.Position.Y = mousePosition.Y - oldCameraOffset.Y;
            }

            if(Scrolling)
            {
                camera.Position.X = mousePosition.X - oldCameraOffset.X;
                camera.Position.Y = mousePosition.Y - oldCameraOffset.Y;
            }

            if(brushing)
            {
                //brushing = false;
                Tilemap map = ScreenToChunk(mousePosition, camera);
                if (map != null)
                {
                    Point tileCoords = map.ScreenToTileCoordinates(mousePosition, camera);
                    UseSquareBrush(map, tileCoords,1, 32,32);
                }
            }

            foreach (var map in LoadedMapChunks)
            {
                map.Update(gameTime);

                //map.Bounds.X = (int)camera.Position.X + (int)map.CameraOffset.X;
                //map.Bounds.X = (int)camera.Position.X + (int)map.CameraOffset.Y;
                //todo: Calculate bounds instead of brute forcing it.
                if (cameraBounds.Intersects(map.Bounds))
                {
                    MapsToDraw.Add(map);
                }
            }
        }

        void PlaceTile(int tileNr, Point screenPos)
        {
            Tilemap map = ScreenToChunk(screenPos, camera);
            if (map != null)
            {

                int? tileIndex = map.ScreenToTileIndex(screenPos, camera);
                if(tileIndex.HasValue)
                {
                    map.Tiles[tileIndex.Value] = tileNr;
                }
                //Point tileCoords = map.ScreenToTileCoordinates(screenPos, camera);
                //UseSquareBrush(map, tileCoords, tileNr, 2,2);
            }
        }
        SpriteBatch spriteBatch;
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, blendState: null, samplerState: SamplerState.PointClamp);
            
            foreach(var map in MapsToDraw)
            {
                spriteBatch.Draw(map, camera);
            }

            spriteBatch.End();

            MapsToDraw.Clear();
        }

        void UseSquareBrush(Tilemap map, Point tileCoords, int tile = 1, int brushWidth = 4, int brushHeight = 4)
        {
            int beginX = 0;
            int beginY = 0;

            int endX = 1;
            int endY = 1;

            if (brushWidth > 1)
            {
                beginX = tileCoords.X - brushWidth / 2;
                endX = brushWidth / 2 + tileCoords.X;

            }

            if(brushHeight > 1)
            {
                beginY = tileCoords.Y - brushHeight / 2;
                endY = brushHeight / 2 + tileCoords.Y;
            }


            Point brushTileCoords = new Point();

            for(int y = beginY; y < endY; y++)
            {
                for(int x = beginX; x < endX; x++)
                {
                    brushTileCoords.X = x;
                    brushTileCoords.Y = y;

                    if(x < map.GridWidth && y < map.GridHeight && x >= 0 && y >= 0)
                    {
                        //Console.WriteLine("brushtilecoords: " + brushTileCoords);
                        //Console.WriteLine("tileindex : " + (int)map.TileCoordinatesToTileIndex(brushTileCoords));
                        map.Tiles[(int)map.TileCoordinatesToTileIndex(brushTileCoords)] = tile;
                    }
                }
            }
        }

        TileRef ScreenToTileRef(Point screenPosition)
        {
            


            throw new NotImplementedException();
        }

        int ScreenToChunkIndex(Point screenPosition, Camera _cam)
        {
            Point cameraScreenPos = Point.Zero;
            cameraScreenPos.X = screenPosition.X + _cam.Bounds.X;
            cameraScreenPos.Y = screenPosition.Y + _cam.Bounds.Y;

            Point chunkCoords = cameraScreenPos / sizeInPixels; // chunk coords

            //Console.WriteLine(chunkCoords);

            if (chunkCoords.X >= 0 && chunkCoords.X < ChunkGridSize.X &&
                chunkCoords.Y >= 0 && chunkCoords.Y < ChunkGridSize.Y)
            {
                return (chunkCoords.Y * ChunkGridSize.X) + chunkCoords.X;
            }
            else return -1;
        }

        Tilemap ScreenToChunk(Point screenPosition, Camera _cam)
        {
            int chunkIndex = ScreenToChunkIndex(screenPosition, _cam);
            if (chunkIndex != -1)
            {
                Console.WriteLine(chunkIndex);
                Console.WriteLine(allMaps[chunkIndex].Bounds);
                return allMaps[chunkIndex];
            }
            else return null;
        }
    }
}
