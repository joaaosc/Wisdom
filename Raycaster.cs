using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Wisdom
{
    public class Raycaster
    {
        private Map map;
        private Player player;
        private Texture2D pixel;

        public Raycaster(GraphicsDevice graphicsDevice, Map map, Player player)
        {
            this.map = map;
            this.player = player;

            // Criar uma textura de 1x1 pixel para desenhar
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Desenhar teto e chão
            Rectangle ceiling = new Rectangle(526, 0, 480, 160);
            spriteBatch.Draw(pixel, ceiling, Color.Cyan);

            Rectangle floor = new Rectangle(526, 160, 480, 160);
            spriteBatch.Draw(pixel, floor, Color.Blue);

            int r, mx, my, mp, dof;
            // int side = 0; // Removido porque não está sendo usado
            float vx = 0, vy = 0, rx = 0, ry = 0, ra, xo = 0, yo = 0, disV, disH;

            ra = FixAng(player.pa + 30); // Ângulo do raio

            for (r = 0; r < 60; r++)
            {
                // --- Vertical ---
                dof = 0;
                // side = 0; // Removido porque não está sendo usado
                disV = 100000;
                float Tan = (float)Math.Tan(DegToRad(ra));
                if (Math.Cos(DegToRad(ra)) > 0.001)
                {
                    // Olhando para a direita
                    rx = (((int)player.px >> 6) << 6) + 64;
                    ry = (player.px - rx) * Tan + player.py;
                    xo = 64;
                    yo = -xo * Tan;
                }
                else if (Math.Cos(DegToRad(ra)) < -0.001)
                {
                    // Olhando para a esquerda
                    rx = (((int)player.px >> 6) << 6) - 0.0001f;
                    ry = (player.px - rx) * Tan + player.py;
                    xo = -64;
                    yo = -xo * Tan;
                }
                else
                {
                    // Olhando para cima ou para baixo. Sem acerto
                    rx = player.px;
                    ry = player.py;
                    dof = 8;
                    // Inicializar xo e yo
                    xo = 0;
                    yo = 0;
                }

                while (dof < 8)
                {
                    mx = (int)(rx) >> 6;
                    my = (int)(ry) >> 6;
                    mp = my * Map.MapX + mx;
                    if (mp > 0 && mp < Map.MapX * Map.MapY && map.GetMapValue(mx, my) == 1)
                    {
                        dof = 8;
                        disV = (float)(Math.Cos(DegToRad(ra)) * (rx - player.px) - Math.Sin(DegToRad(ra)) * (ry - player.py));
                    }
                    else
                    {
                        rx += xo;
                        ry += yo;
                        dof += 1;
                    }
                }
                vx = rx;
                vy = ry;

                // --- Horizontal ---
                dof = 0;
                disH = 100000;
                Tan = 1.0f / Tan;
                if (Math.Sin(DegToRad(ra)) > 0.001)
                {
                    // Olhando para cima
                    ry = (((int)player.py >> 6) << 6) - 0.0001f;
                    rx = (player.py - ry) * Tan + player.px;
                    yo = -64;
                    xo = -yo * Tan;
                }
                else if (Math.Sin(DegToRad(ra)) < -0.001)
                {
                    // Olhando para baixo
                    ry = (((int)player.py >> 6) << 6) + 64;
                    rx = (player.py - ry) * Tan + player.px;
                    yo = 64;
                    xo = -yo * Tan;
                }
                else
                {
                    // Olhando diretamente para a esquerda ou direita
                    rx = player.px;
                    ry = player.py;
                    dof = 8;
                    // Inicializar xo e yo
                    xo = 0;
                    yo = 0;
                }

                while (dof < 8)
                {
                    mx = (int)(rx) >> 6;
                    my = (int)(ry) >> 6;
                    mp = my * Map.MapX + mx;
                    if (mp > 0 && mp < Map.MapX * Map.MapY && map.GetMapValue(mx, my) == 1)
                    {
                        dof = 8;
                        disH = (float)(Math.Cos(DegToRad(ra)) * (rx - player.px) - Math.Sin(DegToRad(ra)) * (ry - player.py));
                    }
                    else
                    {
                        rx += xo;
                        ry += yo;
                        dof += 1;
                    }
                }

                Color color = new Color(0, 0.8f, 0);
                if (disV < disH)
                {
                    rx = vx;
                    ry = vy;
                    disH = disV;
                    color = new Color(0, 0.6f, 0);
                }

                // Desenhar raio 2D
                DrawLine(spriteBatch, new Vector2(player.px, player.py), new Vector2(rx, ry), color, 2);

                int ca = (int)FixAng(player.pa - ra);
                disH = disH * (float)Math.Cos(DegToRad(ca)); // Corrigir efeito fisheye

                int lineH = (int)((Map.MapS * 320) / disH);
                if (lineH > 320)
                    lineH = 320;

                int lineOff = 160 - (lineH >> 1);

                // Desenhar parede vertical
                Rectangle lineRect = new Rectangle(r * 8 + 530, lineOff, 8, lineH);
                spriteBatch.Draw(pixel, lineRect, color);

                ra = FixAng(ra - 1);
            }
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
