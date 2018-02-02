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

        private Rectangle inputLineRect;
        private Texture2D _inputLineTexture;

        public DeveloperConsole(Game game, InputManager input, Texture2D backgroundTexture = null, Texture2D inputLineTexture = null) : base(game)
        {
            if(backgroundTexture == null)
            {
                backgroundTexture = Debug.CreateDebugTexture(this.GraphicsDevice, Color.White);
            }

            _backgroundTexture = backgroundTexture;

            if (inputLineTexture == null)
            {
                _inputLineTexture = Debug.CreateDebugTexture(this.GraphicsDevice, Color.Red);
            }
            else
            {
                _inputLineTexture = inputLineTexture;
            }

            _textColour = Color.Black;
            spriteBatch = new SpriteBatch(this.GraphicsDevice);

            ResetConsoleWindow();

            input.KeyUp += Input_KeyUp;

            commands = new Hashtable();
        }

        private void Input_KeyUp(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Keys.OemPipe)
            {
                consoleEnabled = !consoleEnabled;
            }

            if(e.Key == Keys.R)
            {
                ProcessCommand("echo");
            }

            if(e.Key == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(inputLine))
                {
                    ProcessCommand(inputLine);
                    inputLine = "";
                }
            }

            if (e.Key == Keys.Back)
            {
                if(inputLine.Length > 0)
                {
                    inputLine = inputLine.Remove(inputLine.Length - 1);
                }
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _font = Game.Content.Load<SpriteFont>("Arial");
        }

        private Vector2 lineCursor = new Vector2();
        private Vector2 inputLineTextEntryPosition = new Vector2();

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (consoleEnabled)
            {
                spriteBatch.Begin(blendState: BlendState.AlphaBlend);

                spriteBatch.Draw(_backgroundTexture, srcConsoleRect, Color.White);

                Vector2 v = new Vector2(0, srcConsoleRect.Bottom);

                for (int i = 0; i < lineBuffer.Count; i++)
                {
                    lineCursor = (-lineSpacing) * i + (v - lineSpacing);
                    spriteBatch.DrawString(_font, lineBuffer[lineBuffer.Count - (i + 1)], lineCursor, _textColour);
                }

                spriteBatch.Draw(_inputLineTexture, inputLineRect, Color.White);
                if (!string.IsNullOrEmpty(inputLine))
                {
                    spriteBatch.DrawString(_font, inputLine, inputLineTextEntryPosition, Color.White);
                }

                spriteBatch.End();
            }
        }

        public void ProcessCommand(string command)
        {
            string[] tokens = command.Split(' ');
            
            Delegate method = (Delegate) commands[tokens[0]];
            if (method != null)
            {
                method.DynamicInvoke();
            }
            else PushLine(string.Format("No such command: {0}", tokens[0]));
        }

        public override void Initialize()
        {
            base.Initialize();

            this.Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
            this.Game.Window.TextInput += Window_TextInput;
            LoadCommands();

        }

        private string inputLine = "";
        private void Window_TextInput(object sender, TextInputEventArgs e)
        {


            if (char.IsLetterOrDigit(e.Character) || e.Character == ' ')
            {
                inputLine += e.Character;
                //PushLine(inputLine);
            }
        }

        private void ResetConsoleWindow()
        {
            int width = this.Game.Window.ClientBounds.Width;
            int height = this.Game.Window.ClientBounds.Height / 3;

            int inputLineHeight = 20;

            srcConsoleRect = new Rectangle(0, 0, width, height);

            inputLineRect.X = 0;
            inputLineRect.Y = height;
            inputLineRect.Width = width;
            inputLineRect.Height = inputLineHeight;

            inputLineTextEntryPosition.X = inputLineRect.X;
            inputLineTextEntryPosition.Y = inputLineRect.Y;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            ResetConsoleWindow();
            PushLine(string.Format("Resolution set to {0}x{1}",
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

        void LoadCommands()
        {
            AddCommand("echo", Echo);
        }
    }
}
