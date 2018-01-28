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

        Matrix World;
        Matrix View;
        Matrix Projection;
        VertexBuffer vertexBuffer;
        BasicEffect basicEffect;
        Texture2D texture;
        
        public TilemapRendererDimetric(Game game) : base(game)
        {
            World = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            View = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), Vector3.UnitY);

            Projection = Matrix.CreateOrthographic(Game.Window.ClientBounds.Width,
                Game.Window.ClientBounds.Height, 0.01f, 100.0f);
            //Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);
        }

        public override void Initialize()
        {
            base.Initialize();

            Game.Window.ClientSizeChanged += Window_ClientSizeChanged;

            //vertexBuffer = new VertexBuffer(GraphicsDevice, type)
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Projection = Matrix.CreateOrthographic(Game.Window.ClientBounds.Width,
                Game.Window.ClientBounds.Height, 0.0f, 100.0f);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            basicEffect = new BasicEffect(Game.GraphicsDevice);
            float X = 256;
            float Y = 256;
            float Z = 1;


            VertexPositionTexture[] verts = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(-X, -Y,Z), new Vector2(0f,1f)),
                new VertexPositionTexture(new Vector3(X, -Y,Z), new Vector2(1f,1f)),
                new VertexPositionTexture(new Vector3(X, Y,Z), new Vector2(1f,0f)),
                new VertexPositionTexture(new Vector3(-X, Y,Z), new Vector2(0f,0f))
            };
            VertexPositionColor[] vpc = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(0,1,0), Color.Red),
                new VertexPositionColor(new Vector3(+0.5f,0,0), Color.Green),
                new VertexPositionColor(new Vector3(-0.5f,0,0), Color.Blue),
            };

            short[] indexList = new short[]
            {
                0,1,2,
                2,3,0
            };

            vertexBuffer = new VertexBuffer(Game.GraphicsDevice, VertexPositionTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionTexture>(verts);

            indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, sizeof(short) * indexList.Length, BufferUsage.None);
            indexBuffer.SetData<short>(indexList);

            texture = Game.Content.Load<Texture2D>("cossacks");

        }
        IndexBuffer indexBuffer;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            basicEffect.World = World;
            basicEffect.View = View;
            basicEffect.Projection = Projection;
            basicEffect.Texture = texture;
            basicEffect.TextureEnabled = true;
            //basicEffect.VertexColorEnabled = true;
            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.Indices = indexBuffer;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;

            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
            }

            base.Draw(gameTime);

        }


        //void Draw(Model model, Matrix modelMatrix)
        //{

        //    //foreach (ModelMesh mesh in model.Meshes)
        //    //{
        //    //    foreach (BasicEffect effect in mesh.Effects)
        //    //    {
        //    //        effect.World = modelMatrix;
        //    //        effect.View = View;
        //    //        effect.Projection = Projection;
        //    //    }
        //    //    mesh.Draw();
        //    //}
            
        //}
    }
}
