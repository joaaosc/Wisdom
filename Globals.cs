using Microsoft.Xna.Framework;


namespace Globals
{
    public static class Variables
    {
        public static int GameState = 1;
    }

    public class Camera
    {
        public Matrix Transform { get; private set; } // Agora a propriedade Transform é pública
        public Vector2 Position { get; set; }
        public float Zoom { get; set; } = 1f;
    }
}