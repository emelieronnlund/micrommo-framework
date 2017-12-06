using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MicroMMO
{
    partial class GUIManager : DrawableGameComponent
    {

        List<Button> buttons;
        SpriteBatch spriteBatch;
        Texture2D debugTexture;
        InputManager input;
        public GUIManager(Game game, InputManager inputManager) : base(game)
        {
            buttons = new List<Button>();
            input = inputManager;
        }

        public override void Initialize()
        {
            base.Initialize();

            input.MouseButtonUp += Input_MouseButtonUp;
            input.MouseMotion += Input_MouseMotion;
            HelloWorldButton hButton = new HelloWorldButton() {
                Box = new Rectangle(50,50,200,200)
            };
            buttons.Add(hButton);
        }

        private void Input_MouseMotion(object sender, MouseEventArgs e)
        {
            foreach(var button in buttons)
            {
                if(button.Box.Contains(e.Position))
                {
                    if(button.MouseOver == false)
                    {
                        button.MouseOver = true;
                        button.OnMouseEnter();
                    }

                }
                else
                {
                    if(button.MouseOver == true)
                    {
                        button.MouseOver = false;
                        button.OnMouseExit();
                    }
                }
            }
        }

        private void Input_MouseButtonUp(object sender, MouseEventArgs e)
        {
            foreach(var button in buttons)
            {
                if(button.Box.Contains(e.Position))
                {
                    if(e.Button == MouseButton.LeftButton)
                    {
                        button.OnClick();
                    }
                }
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            debugTexture = Debug.CreateDebugTexture(graphics: GraphicsDevice, color: Color.White);
        }

        void HandleMouseOver(Point pos, MouseState mouse)
        {
            foreach(var button in buttons)
            {
                if (button.Box.Contains(pos))
                {
                    if(button.MouseOver == false)
                    {
                        button.MouseOver = true;
                        button.OnMouseEnter();
                    }
                }
                else
                {
                    if(button.MouseOver == true)
                    {
                        button.MouseOver = false;
                        button.OnMouseExit();
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();

            foreach(var button in buttons)
            {
                spriteBatch.Draw(debugTexture, button.Box, Color.Red);
            }

            spriteBatch.End();
        }
    }
}
