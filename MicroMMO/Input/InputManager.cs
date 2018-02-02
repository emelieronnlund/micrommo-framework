using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace MicroMMO
{
    class InputManager : GameComponent
    {
        public InputManager(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs e);
        public delegate void MouseEventHandler(object sender, MouseEventArgs e);
        //public delegate void TextEventHandler(object sender, TextInputEventArgs)
        public event KeyboardEventHandler KeyDown;
        public event KeyboardEventHandler KeyUp;

        public event MouseEventHandler MouseMotion;
        public event MouseEventHandler MouseButtonDown;
        public event MouseEventHandler MouseButtonUp;
        public event MouseEventHandler MouseButtonPressed;

        Keys[] lastKeysPressed = new Keys[0];

        void RaiseKeyboardEvents()
        {
            KeyboardState keyboard = Keyboard.GetState();

            Keys[] currentKeysPressed = keyboard.GetPressedKeys();

            Keys[] keyDowns = currentKeysPressed.Except(lastKeysPressed).ToArray<Keys>();
            foreach (Keys key in keyDowns)
            {
                OnKeyDown(new KeyboardEventArgs() { Key = key });
            }

            Keys[] keyUps = lastKeysPressed.Except(currentKeysPressed).ToArray<Keys>();
            foreach (Keys key in keyUps)
            {
                OnKeyUp(new KeyboardEventArgs() { Key = key });
            }

            lastKeysPressed = currentKeysPressed;
        }

        struct MouseButtonStruct : IEnumerable
        {
            public ButtonState LeftMouseButton;
            public ButtonState RightMouseButton;
            public ButtonState MiddleMouseButton;
            public ButtonState XButton1;
            public ButtonState XButton2;
            public Point Position;

            public IEnumerator GetEnumerator()
            {
                yield return LeftMouseButton;
                yield return RightMouseButton;
                yield return MiddleMouseButton;
                yield return XButton1;
                yield return XButton2;
            }

            public object this[int i]
            {
                get
                {
                    switch(i)
                    {
                        case 0: return LeftMouseButton;
                        case 1: return RightMouseButton;
                        case 2: return MiddleMouseButton;
                        case 3: return XButton1;
                        case 4: return XButton2;

                        default:
                            return null;
                    }
                }
            }
        }

        MouseButtonStruct lastMouseButtons = new MouseButtonStruct();

        void RaiseMouseEvents()
        {
            MouseState mouse = Mouse.GetState();

            MouseButtonStruct currentMouseButtons = new MouseButtonStruct()
            {
                LeftMouseButton = mouse.LeftButton,
                RightMouseButton = mouse.RightButton,
                MiddleMouseButton = mouse.MiddleButton,
                XButton1 = mouse.XButton1,
                XButton2 = mouse.XButton2,
                Position = mouse.Position
            };

            // OnMousePressed
            if (currentMouseButtons.LeftMouseButton == ButtonState.Pressed)
            {
                OnMousePressed(new MouseEventArgs()
                {
                    Button = MouseButton.LeftButton,
                    Position = mouse.Position
                });
            }

            if (currentMouseButtons.RightMouseButton == ButtonState.Pressed)
            {
                OnMousePressed(new MouseEventArgs()
                {
                    Button = MouseButton.RightButton,
                    Position = mouse.Position
                });
            }

            if (currentMouseButtons.MiddleMouseButton == ButtonState.Pressed)
            {
                OnMousePressed(new MouseEventArgs()
                {
                    Button = MouseButton.Middle,
                    Position = mouse.Position
                });
            }

            if (currentMouseButtons.XButton1 == ButtonState.Pressed)
            {
                OnMousePressed(new MouseEventArgs()
                {
                    Button = MouseButton.X1,
                    Position = mouse.Position
                });
            }

            if (currentMouseButtons.XButton2 == ButtonState.Pressed)
            {
                OnMousePressed(new MouseEventArgs()
                {
                    Button = MouseButton.X2,
                    Position = mouse.Position
                });
            }

            // OnMouseUp & OnMouseDown
            if (currentMouseButtons.LeftMouseButton != lastMouseButtons.LeftMouseButton)
            {
                if (currentMouseButtons.LeftMouseButton == ButtonState.Released)
                {
                    OnMouseUp(new MouseEventArgs() { Button = MouseButton.LeftButton, Position = mouse.Position });
                }
                else
                {
                    OnMouseDown(new MouseEventArgs() { Button = MouseButton.LeftButton, Position = mouse.Position });
                }
            }

            if (currentMouseButtons.RightMouseButton != lastMouseButtons.RightMouseButton)
            {
                if (currentMouseButtons.RightMouseButton == ButtonState.Released)
                {
                    OnMouseUp(new MouseEventArgs() { Button = MouseButton.RightButton, Position = mouse.Position });
                }
                else
                {
                    OnMouseDown(new MouseEventArgs() { Button = MouseButton.RightButton, Position = mouse.Position });
                }
            }

            if (currentMouseButtons.MiddleMouseButton != lastMouseButtons.MiddleMouseButton)
            {
                if (currentMouseButtons.MiddleMouseButton == ButtonState.Released)
                {
                    OnMouseUp(new MouseEventArgs() { Button = MouseButton.Middle, Position = mouse.Position });
                }
                else
                {
                    OnMouseDown(new MouseEventArgs() { Button = MouseButton.Middle, Position = mouse.Position });
                }
            }

            if (currentMouseButtons.XButton1 != lastMouseButtons.XButton1)
            {
                if (currentMouseButtons.XButton1 == ButtonState.Released)
                {
                    OnMouseUp(new MouseEventArgs() { Button = MouseButton.X1, Position = mouse.Position });
                }
                else
                {
                    OnMouseDown(new MouseEventArgs() { Button = MouseButton.X1, Position = mouse.Position });
                }
            }

            if (currentMouseButtons.XButton2 != lastMouseButtons.XButton2)
            {
                if (currentMouseButtons.XButton2 == ButtonState.Released)
                {
                    OnMouseUp(new MouseEventArgs() { Button = MouseButton.X2, Position = mouse.Position });
                }
                else
                {
                    OnMouseDown(new MouseEventArgs() { Button = MouseButton.X2, Position = mouse.Position });
                }
            }

            // OnMousePosition
            if(currentMouseButtons.Position != lastMouseButtons.Position)
            {
                OnMouseMotion(new MouseEventArgs() { Button = MouseButton.None, Position = currentMouseButtons.Position });
            }
            lastMouseButtons = currentMouseButtons;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Game.IsActive)
            {
                RaiseKeyboardEvents();
                RaiseMouseEvents();

            }
        }

        protected virtual void OnKeyDown(KeyboardEventArgs e)
        {
            KeyDown?.Invoke(this, e);
        }

        protected virtual void OnKeyUp(KeyboardEventArgs e)
        {
            KeyUp?.Invoke(this, e);
        }

        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            MouseButtonDown?.Invoke(this, e);
        }

        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            MouseButtonUp?.Invoke(this, e);
        }

        protected virtual void OnMouseMotion(MouseEventArgs e)
        {
            MouseMotion?.Invoke(this, e);
        }

        protected virtual void OnMousePressed(MouseEventArgs e)
        {
            MouseButtonPressed?.Invoke(this, e);
        }


        private void RaiseTextInputEvents()
        {

        }
            
        private bool letsCollectText = false;
        protected bool IsReceivingTextInput
        {
            get
            {
                return letsCollectText;
            }
            set
            {
                letsCollectText = value;
            }
        }
    }
}
