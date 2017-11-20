﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace MicroMMO
{
    class TilemapManager : DrawableGameComponent
    {
        List<Tilemap> LoadedMapChunks;
        Camera camera;

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
        }

        public void AddMapChunk(Point chunkCoords, Tilemap map)
        {
            Point pixelOffset = chunkCoords * map.SizeInPixels;
            map.CameraOffset = pixelOffset.ToVector2();

            LoadedMapChunks.Add(map);
        }

        InputManager inputManager;
        public TilemapManager(Game game, Camera cam, InputManager input) : base(game)
        {
            LoadedMapChunks = new List<Tilemap>();
            camera = cam;
            inputManager = input;
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
        }

        private void InputManager_MouseMotion(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("MouseMotion: {0}", e.Position);
        }

        private void InputManager_MouseButtonUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("MouseButtonUp {0} at {1}", e.Button.ToString(), e.Position);
        }

        private void InputManager_MouseButtonDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("MouseButtonDown {0} at {1}", e.Button.ToString(), e.Position);
        }

        private void InputManager_KeyUp(object sender, KeyboardEventArgs e)
        {
            Console.WriteLine("Key up: {0}", e.Key);
        }

        private void InputManager_KeyDown(object sender, KeyboardEventArgs e)
        {
            Console.WriteLine("Key down: {0}", e.Key);
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
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Rectangle cameraBounds = camera.Bounds;
            foreach (var map in LoadedMapChunks)
            {
                map.Update(gameTime);

                //todo: Calculate bounds instead of brute forcing it.
                if (cameraBounds.Intersects(map.Bounds))
                {
                    MapsToDraw.Add(map);
                }
            }

        }

        SpriteBatch spriteBatch;
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, blendState: null, samplerState: SamplerState.PointClamp);
            
            foreach(var map in MapsToDraw)
            {
                spriteBatch.Draw(map);
            }

            spriteBatch.End();

            MapsToDraw.Clear();
        }
    }
}
