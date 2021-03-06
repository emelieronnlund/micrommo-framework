﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MicroMMO
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GUIManager gui;
        Camera camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //this.Window.Handle;
        }

        TilemapManager mapManager;
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            camera = new Camera(this);
            Components.Add(camera);

            InputManager inputManager = new InputManager(this);
            Components.Add(inputManager);

            TilemapRendererDimetric dimetric = new TilemapRendererDimetric(this);
            Components.Add(dimetric);

            mapManager = new TilemapManager(this, camera, inputManager);
            Components.Add(mapManager);
            mapManager.GenerateMapChunks(5, 5);

            gui = new GUIManager(this, inputManager);
            Components.Add(gui);

            DeveloperConsole devcon = new DeveloperConsole(this, inputManager);

            Components.Add(devcon);

            Components.Add(new FPS_Counter(this));

            inputManager.KeyUp += InputManager_KeyUp;

            base.Initialize();
        }

        private void InputManager_KeyUp(object sender, KeyboardEventArgs e)
        {
            if(e.Key == Keys.Escape)
            {
                Exit();
            }
        }

        private bool WindowSizeIsBeingChanged = false;
        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            WindowSizeIsBeingChanged = !WindowSizeIsBeingChanged;
            if (WindowSizeIsBeingChanged)
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                graphics.ApplyChanges();
            }

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //mapManager.GenerateMapChunks(5, 5);
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);

            base.Draw(gameTime);
        }
    }
}
