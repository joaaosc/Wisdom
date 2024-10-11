using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Wisdom
{
    public class Player
    {
        // Variáveis do jogador
        public float px, py, pdx, pdy, pa;

        private Texture2D pixel;

        public Player(GraphicsDevice graphicsDevice)
        {
            // Inicializar posição e direção do jogador
            px = 150;
            py = 400;
            pa = 90;
            pdx = (float)Math.Cos(DegToRad(pa));
            pdy = -(float)Math.Sin(DegToRad(pa));

            // Criar uma textura de 1x1 pixel para desenhar
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
        }

        public void Update(float deltaTime, InputHandler input)
        {
            // Atualizar ângulo
            if (input.Left)
            {
                pa += 90 * deltaTime;
                pa = FixAng(pa);
                pdx = (float)Math.Cos(DegToRad(pa));
                pdy = -(float)Math.Sin(DegToRad(pa));
            }
            if (input.Right)
            {
                pa -= 90 * deltaTime;
                pa = FixAng(pa);
                pdx = (float)Math.Cos(DegToRad(pa));
                pdy = -(float)Math.Sin(DegToRad(pa));
            }

            // Atualizar posição
            if (input.Up)
            {
                px += pdx * 100 * deltaTime;
                py += pdy * 100 * deltaTime;
            }
            if (input.Down)
            {
                px -= pdx * 100 * deltaTime;
                py -= pdy * 100 * deltaTime;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Desenhar posição do jogador
            Color color = Color.Yellow;

            // Desenhar ponto
            Rectangle rect = new Rectangle((int)px - 4, (int)py - 4, 8, 8);
            spriteBatch.Draw(pixel, rect, color);

            // Desenhar linha de direção
            Vector2 start = new Vector2(px, py);
            Vector2 end = new Vector2(px + pdx * 20, py + pdy * 20);
            DrawLine(spriteBatch, start, end, color, 4);
        }

        private float DegToRad(float a)
        {
            return a * (float)Math.PI / 180.0f;
        }

        private float FixAng(float a)
        {
            if (a > 359)
                a -= 360;
            if (a < 0)
                a += 360;
            return a;
        }

        private void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color, int width)
        {
            // Desenhar uma linha usando uma textura de pixel
            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);

            float angle = (float)Math.Atan2(dy, dx);

            Rectangle rect = new Rectangle((int)start.X, (int)start.Y, (int)length, width);

            sb.Draw(pixel, rect, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
