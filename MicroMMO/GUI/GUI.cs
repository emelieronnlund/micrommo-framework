using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MicroMMO
{
    partial class GUI : DrawableGameComponent
    {

        List<Button> buttons;
        SpriteBatch spriteBatch;
        Texture2D debugTexture;

        public GUI(Game game) : base(game)
        {
            buttons = new List<Button>();
        }

        public override void Initialize()
        {
            base.Initialize();
            HelloWorldButton hButton = new HelloWorldButton() {
                Box = new Rectangle(50,50,200,200)
            };
            buttons.Add(hButton);
        }
        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            //debugTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            //debugTexture.SetData<Color>(new Color[] { Color.White });

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

                    if(mouse.LeftButton == ButtonState.Pressed)
                    {
                        button.LeftMouseDown = true;
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

                if(mouse.LeftButton == ButtonState.Released && button.LeftMouseDown)
                {
                    button.LeftMouseDown = false;
                    button.OnClick();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MouseState mouse = Mouse.GetState();

            HandleMouseOver(mouse.Position, mouse);

            if(mouse.LeftButton == ButtonState.Pressed)
            {

            }

            if (mouse.RightButton == ButtonState.Pressed)
            {

            }

            if (mouse.MiddleButton == ButtonState.Pressed)
            {

            }

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
