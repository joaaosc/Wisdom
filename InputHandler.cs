using Microsoft.Xna.Framework.Input;

namespace Wisdom
{
    public class InputHandler
    {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;
        public bool Exit;

        public void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            Up = keyboardState.IsKeyDown(Keys.W);
            Down = keyboardState.IsKeyDown(Keys.S);
            Left = keyboardState.IsKeyDown(Keys.A);
            Right = keyboardState.IsKeyDown(Keys.D);
            Exit = keyboardState.IsKeyDown(Keys.Escape);
        }
    }
}