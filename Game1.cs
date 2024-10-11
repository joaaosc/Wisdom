using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.InteropServices;

namespace Wisdom
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private bool isMouseInsideWindow;

        public int WindowWidth => GraphicsDevice.Viewport.Width;    // obtém a largura da janela
        public int WindowHeight => GraphicsDevice.Viewport.Height;  // obtém a altura da janela

        private Int64 debug = 0; // Defina para 1 para abrir o console, ou 0 para não abrir

        // Instâncias das classes criadas
        private Map map;
        private Player player;
        private Raycaster raycaster;
        private InputHandler inputHandler;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            if (debug == 1) OpenConsole();
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 510;
            graphics.ApplyChanges();

            // Inicializar as instâncias
            map = new Map(GraphicsDevice);
            player = new Player(GraphicsDevice);
            raycaster = new Raycaster(GraphicsDevice, map, player);
            inputHandler = new InputHandler();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            inputHandler.Update();

            if (inputHandler.Exit)
                Exit();

            CheckMouseFocus();

            // Atualizar o jogador
            player.Update((float)gameTime.ElapsedGameTime.TotalSeconds, inputHandler);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            spriteBatch.Begin();

            // Desenhar mapa
            map.Draw(spriteBatch);

            // Desenhar jogador
            player.Draw(spriteBatch);

            // Desenhar raios e paredes
            raycaster.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CheckMouseFocus()
        {
            // Obter a posição do mouse em coordenadas de tela
            var mousePosition = GetMousePosition();

            // Obter o handle da janela do jogo
            var windowHandle = this.Window.Handle;

            // Obter as coordenadas da janela na tela
            if (GetWindowRect(windowHandle, out RECT windowRect))
            {
                // Verificar se o mouse está dentro da janela
                if (mousePosition.X >= windowRect.Left && mousePosition.X <= windowRect.Right &&
                    mousePosition.Y >= windowRect.Top && mousePosition.Y <= windowRect.Bottom)
                {
                    if (!isMouseInsideWindow)
                    {
                        Console.WriteLine("focused");
                        isMouseInsideWindow = true;
                    }
                }
                else
                {
                    if (isMouseInsideWindow)
                    {
                        Console.WriteLine("unfocused");
                        isMouseInsideWindow = false;
                    }
                }
            }
        }

        // Método para obter a posição do mouse em coordenadas de tela
        private Point GetMousePosition()
        {
            GetCursorPos(out POINT lpPoint);
            return new Point(lpPoint.X, lpPoint.Y);
        }

        // Método para abrir o console
        private void OpenConsole()
        {
            AllocConsole();
        }

        // Importações e estruturas necessárias
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;    // x mínimo
            public int Top;     // y mínimo
            public int Right;   // x máximo
            public int Bottom;  // y máximo
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
    }
}
