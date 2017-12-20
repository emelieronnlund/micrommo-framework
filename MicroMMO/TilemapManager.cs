//#define DEBUG_DRAW_TILEMAP



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

        public int CellWidth { get; } = 32;
        public int CellHeight { get; } = 32;

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

        public TileRef[] GetTiles(Rectangle screenRectangle)
        {
            Tilemap[] tileChunks;

            screenRectangle.X += CellWidth / 2;
            screenRectangle.Y += CellHeight / 2;
            //screenRectangle.Width += CellWidth  / 2;
            //screenRectangle.Height += CellHeight / 2;
            //screenRectangle.Width += 9;
            //screenRectangle.Height += 9;

            Point topLeft = new Point(screenRectangle.X, screenRectangle.Y);
            Point bottomRight = new Point(screenRectangle.Right, screenRectangle.Bottom);

            Tilemap topLeftChunk = ScreenToChunk(topLeft, camera);
            Tilemap bottomRightChunk = ScreenToChunk(bottomRight, camera);

            int numberOfTiles = (screenRectangle.Width / CellWidth) * (screenRectangle.Height / CellHeight);

            if (topLeftChunk == bottomRightChunk)
            {
                tileChunks = new Tilemap[1];
                tileChunks[0] = topLeftChunk;
            }
            else
            {
                int numberOfChunks = (screenRectangle.Width / SizeInPixels.X) * (screenRectangle.Height / SizeInPixels.Y);
                tileChunks = new Tilemap[numberOfChunks];

                Point currentChunk = new Point();
                int chunk_i = 0;
                for (int y = topLeft.Y; y < (bottomRight.Y-SizeInPixels.Y); y += SizeInPixels.Y)
                {
                    for (int x = topLeft.X; x < (bottomRight.X - SizeInPixels.X); x += SizeInPixels.X, chunk_i++)
                    {
                        currentChunk.X = x;
                        currentChunk.Y = y;
                        tileChunks[chunk_i] = ScreenToChunk(currentChunk, camera);
                    }
                }
            }

            TileRef[] returnTiles = new TileRef[numberOfTiles];

            int i = 0;
            Point tileScreenCoords = Point.Zero;

            for(int y = screenRectangle.Y; y < (screenRectangle.Bottom-CellHeight); y += CellHeight)
            {
                for(int x = screenRectangle.X; x < (screenRectangle.Right-CellWidth); x += CellWidth, i++)
                {
                    //Console.WriteLine("GETTILES x: {0}, y: {1}", x, y);
                    tileScreenCoords.X = x;
                    tileScreenCoords.Y = y;

                    TileRef currentTileRef = ScreenToTileRef(tileScreenCoords);
                    returnTiles[i] = currentTileRef;
                    //Console.WriteLine(currentTileRef);
                }
            }
            //Console.WriteLine("GETTILES i: " + i);
            //for(int i = 0; i < numberOfTiles; i++)
            //{
            //    //Tilemap chunk = 
            //    //returnTiles[i] = new TileRef() { Chunk = };
            //}
            return returnTiles;
        }

        public void SetTileGroup(TileRef[] tileRefs, int tileId)
        {
            for (int i = 0; i < tileRefs.Length; i++)
            {
                //tileRefs[i] = tileId;
                TileRef t = tileRefs[i];

                if(t.Chunk != null)
                {
                    int? index = t.Chunk.TileCoordinatesToTileIndex(t.TileCoordinates);

                    if (index.HasValue)
                    {
                        t.Chunk.Tiles[(int)index] = tileId;
                    }
                }
            }
        }

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
                    Tilemap map = new Tilemap(Game, CellWidth, CellHeight, GridWidth, GridHeight, 16, 16);
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
            map.ChunkCoords = chunkCoords;

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

        private void InputManager_MouseButtonDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("MouseButtonDown {0} at {1}", e.Button.ToString(), e.Position);
            if (e.Button == MouseButton.Middle)
            {
                scrollButtonDown = true;
            }


        }

        private void InputManager_MouseButtonPressed(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButton.LeftButton)
            {
                PlaceTile(2, e.Position);
            }

            if (e.Button == MouseButton.RightButton)
            {
                Rectangle select = new Rectangle(e.Position.X, e.Position.Y, 200, 200);

                // center rectangle on mouse position
                select.X = select.X - select.Width / 2;
                select.Y = select.Y - select.Height / 2;

                TileRef[] selectedTiles = GetTiles(select);
                if (selectedTiles != null)
                {

                    SetTileGroup(selectedTiles, 5);
                }
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

#if DEBUG_DRAW_TILEMAP
            foreach(var map in MapsToDraw)
            {
               TilemapRendererExtensions.DebugDrawTilemap(Game.GraphicsDevice, map, camera);
            }
#endif
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
                    else
                    {
                        Tilemap neighbour;

                        if(x > map.GridWidth)
                        {
                            if(y > map.GridHeight)
                            {
                                //neighbour = map.ChunkNeighbourSouthEast;
                            }
                            else if(y < 0)
                            {
                                //neighbour = map.ChunkNeighbourNorthEast;
                            }
                            else
                            {
                                neighbour = map.ChunkNeighbourEast;
                            }
                        }
                        else if(x < 0)
                        {

                        }
                    }
                }
            }
        }

        TileRef ScreenToTileRef(Point screenPosition)
        {
            TileRef tile = new TileRef();

            Tilemap chunk = ScreenToChunk(screenPosition, camera);
            if(chunk != null)
            {
                Point tileCoords = chunk.ScreenToTileCoordinates(screenPosition, camera);
                Point chunkCoords = chunk.ChunkCoords;
                tile.Chunk = chunk;
                tile.ChunkCoordinates = chunkCoords;
                tile.TileCoordinates = tileCoords;
            }
            else
            {
                tile.Chunk = null;
            }
            return tile;
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
                //Console.WriteLine(chunkIndex);
                //Console.WriteLine(allMaps[chunkIndex].Bounds);
                return allMaps[chunkIndex];
            }
            else return null;
        }
    }
}
