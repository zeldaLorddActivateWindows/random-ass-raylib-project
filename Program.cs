using System.Drawing;
using Raylib_cs;

namespace main
{
    enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    internal class Program
    {
        public static int width = 800;
        public static int height = 480;

        static void Main(string[] args)
        {
            var title = "Game";
            var character = new Character();
            var center = new Point(width / 2, height / 2);

            Raylib.InitWindow(width, height, title);
            Raylib.SetTargetFPS(60);

            while (!Raylib.WindowShouldClose())
            {
                // Clear the screen
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib_cs.Color.Black);

                // Draw the crosshair
                Raylib.DrawLine(center.X + width / 2, center.Y, center.X - width / 2, center.Y, Raylib_cs.Color.Gold);
                Raylib.DrawLine(center.X, center.Y + height / 2, center.X, center.Y - height / 2, Raylib_cs.Color.Gold);

                // Update character position
                character.UpdatePos(
                    Raylib.IsKeyDown(KeyboardKey.W),
                    Raylib.IsKeyDown(KeyboardKey.S),
                    Raylib.IsKeyDown(KeyboardKey.A),
                    Raylib.IsKeyDown(KeyboardKey.D)
                );

                // Draw the character
                character.Draw();

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}