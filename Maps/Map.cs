using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wisdom
{
    public class Map
    {
        // Constantes do mapa
        public const int MapX = 8;
        public const int MapY = 8;
        public const int MapS = 64;

        // Dados do mapa
        private int[] map = new int[]
        {
            1,1,1,1,1,1,1,1,
            1,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,1,
            1,0,0,0,0,1,0,1,
            1,0,0,0,0,0,0,1,
            1,1,1,1,1,1,1,1,
        };

        private Texture2D pixel;

        public Map(GraphicsDevice graphicsDevice)
        {
            // Criar uma textura de 1x1 pixel para desenhar
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
        }

        public int GetMapValue(int x, int y)
        {
            if (x >= 0 && x < MapX && y >= 0 && y < MapY)
            {
                return map[y * MapX + x];
            }
            return 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < MapY; y++)
            {
                for (int x = 0; x < MapX; x++)
                {
                    int xo = x * MapS;
                    int yo = y * MapS;

                    Color color = (map[y * MapX + x] == 1) ? Color.White : Color.Black;

                    Rectangle rect = new Rectangle(xo + 1, yo + 1, MapS - 2, MapS - 2);
                    spriteBatch.Draw(pixel, rect, color);
                }
            }
        }
    }
}