using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MicroMMO
{
    class FPS_Counter : DrawableGameComponent
    {
        SpriteFont font;
        SpriteBatch spriteBatch;
        public Vector2 Position = new Vector2(10.0f, 20.0f);

        public FPS_Counter(Game game) : base(game)
        {

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            font = Game.Content.Load<SpriteFont>("Arial");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            float frameRate = 1000.0f / (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "FPS: " + frameRate, Position, Color.White);

            spriteBatch.End();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


        }

    }
}
