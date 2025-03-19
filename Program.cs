using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Raylib_cs;
namespace main
{
  

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
            character.InitCharacter(character);
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.DrawLine(center.X + width / 2, center.Y, center.X - width / 2, center.Y, Raylib_cs.Color.Gold);
                Raylib.DrawLine(center.X, center.Y + height/2, center.X, center.Y - height/2, Raylib_cs.Color.Gold);
                Raylib.EndDrawing();
            }
            
            Raylib.CloseWindow();
        }
    }
}
