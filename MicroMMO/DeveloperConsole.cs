using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MicroMMO
{
    class DeveloperConsole : DrawableGameComponent
    {

        private SpriteFont _font;
        private Texture2D _backgroundTexture;
        private Color _textColour = new Color(255, 255, 255, 125);
        private SpriteBatch spriteBatch;
        private Rectangle srcConsoleRect;
        private List<string> lineBuffer = new List<string>();
        private Hashtable commands;
        private Vector2 lineSpacing = new Vector2(0, 20);
        private bool consoleEnabled = false;

        public DeveloperConsole(Game game, InputManager input, Texture2D backgroundTexture = null) : base(game)
        {
            if(backgroundTexture == null)
            {
                backgroundTexture = Debug.CreateDebugTexture(this.GraphicsDevice, Color.White);
            }

            _backgroundTexture = backgroundTexture;
            _textColour = Color.Black;
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            srcConsoleRect = new Rectangle(0, 0, this.Game.Window.ClientBounds.Width, this.Game.Window.ClientBounds.Height / 2);

            input.KeyUp += Input_KeyUp;

            commands = new Hashtable();
        }

        private void Input_KeyUp(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Keys.OemPipe)
            {
                consoleEnabled = !consoleEnabled;
            }

            if(e.Key == Keys.G)
            {
                AddCommand("echo", Echo);
            }

            if(e.Key == Keys.R)
            {
                ProcessCommand("echo");
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _font = Game.Content.Load<SpriteFont>("Arial");
        }

        private Vector2 lineCursor = new Vector2();
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (consoleEnabled)
            {

                spriteBatch.Begin();

                spriteBatch.Draw(_backgroundTexture, srcConsoleRect, Color.White);

                Vector2 v = new Vector2(0, srcConsoleRect.Bottom);

                for (int i = 0; i < lineBuffer.Count; i++)
                {
                    lineCursor = (-lineSpacing) * i + (v - lineSpacing);
                    spriteBatch.DrawString(_font, lineBuffer[lineBuffer.Count - (i + 1)], lineCursor, _textColour);
                }

                spriteBatch.End();
            }
        }

        public void ProcessCommand(string command)
        {
            string[] tokens = command.Split(' ');
            Delegate method = (Delegate) commands[tokens[0]];
            method.DynamicInvoke();
        }

        public override void Initialize()
        {
            base.Initialize();

            this.Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            srcConsoleRect = new Rectangle(0, 0, this.Game.Window.ClientBounds.Width, this.Game.Window.ClientBounds.Height / 2);
            PushLine(string.Format("Changed resolution to {0} x {1}",
                Game.Window.ClientBounds.Width,
                Game.Window.ClientBounds.Height));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Echo()
        {
            PushLine("Hello, world!");
        }

        public delegate void CommandCallback();

        public void AddCommand(string invocation, CommandCallback callback)
        {

            commands.Add(invocation, callback);
        }

        public void PushLine(string line)
        {
            lineBuffer.Add(line);
        }
    }
}
