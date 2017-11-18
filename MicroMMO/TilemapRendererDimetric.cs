using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MicroMMO
{
    class TilemapRendererDimetric : DrawableGameComponent
    {

        Matrix Model;
        Matrix View;
        Matrix Projection;

        public TilemapRendererDimetric(Game game) : base(game)
        {
            Model = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            View = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
            Projection = Matrix.CreateOrthographic(Game.Window.ClientBounds.Width,
                Game.Window.ClientBounds.Height, 0.0f, 100.0f);
        }

        public override void Initialize()
        {
            base.Initialize();

            Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Projection = Matrix.CreateOrthographic(Game.Window.ClientBounds.Width,
                Game.Window.ClientBounds.Height, 0.0f, 100.0f);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }


        void Draw(Model model, Matrix modelMatrix)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = modelMatrix;
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }
        }
    }
}
