using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MicroMMO
{
    class Camera : GameComponent
    {
        public Rectangle Bounds
        {
            get
            {
                //Point _pos = Position.ToPoint();
                //return new Rectangle(_pos.X, _pos.Y,
                //    Game.Window.ClientBounds.Width,
                //    Game.Window.ClientBounds.Height);
                return _Bounds;
            }
        }
        public Vector2 Position = Vector2.Zero;
        private Rectangle _Bounds = new Rectangle();
        //public Point Position
        //{
        //    get
        //    {
        //        return new Point(Bounds.X, Bounds.Y);
        //    }
        //    set
        //    {
        //        Bounds.X = value.X;
        //        Bounds.Y = value.Y;
        //    }
        //}

        public Camera(Game game) : base(game)
        {
            //Bounds.Width = game.Window.ClientBounds.Width;
            //Bounds.Height = game.Window.ClientBounds.Height;
        }

        public override void Initialize()
        {
            base.Initialize();

            //Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        //private void Window_ClientSizeChanged(object sender, EventArgs e)
        //{
        //    Bounds.Width = Game.Window.ClientBounds.Width;
        //    Bounds.Height = Game.Window.ClientBounds.Height;
        //}

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _Bounds.X = (int)-Position.X;
            _Bounds.Y = (int)-Position.Y;
            _Bounds.Width = Game.Window.ClientBounds.Width;
            _Bounds.Height = Game.Window.ClientBounds.Height;


            //Point _position = Position.ToPoint();

            //Bounds.X = _position.X;
            //Bounds.Y = _position.Y;
        }
    }
}
