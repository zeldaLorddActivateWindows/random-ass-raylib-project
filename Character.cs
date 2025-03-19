using System;
using System.Drawing;
using Raylib_cs;

namespace main
{
    internal class Character
    {
        private Raylib_cs.Rectangle charRect;
        private string name;
        public Direction Facing { get; set; }

        public string Name
        {
            get => name;
            set => name = value == null ? throw new Exception("Invalid name input") :
                value.Length > 255 ? throw new Exception("Name too long") : value;
        }

        public ushort Health { get; set; }
        public ulong Score { get; set; }

        public Character(string name, Direction facing, ushort health, ulong score)
        {
            Name = name;
            Facing = facing;
            Health = health;
            Score = score;

            var pos = new Point(Random.Shared.Next(0, Program.width), Random.Shared.Next(0, Program.height));
            const int width = 10;
            const int height = 10;
            charRect = new Raylib_cs.Rectangle(pos.X, pos.Y, width, height);
        }

        public Character() : this("Greg", Direction.Up, 100, 0) { }

        public void UpdatePos(bool W, bool S, bool A, bool D)
        {
            int speed = 5;
            if (W && charRect.Y - speed >= 0) charRect.Y -= speed;
            if (S && charRect.Y + charRect.Height + speed <= Program.height) charRect.Y += speed;
            if (A && charRect.X - speed >= 0) charRect.X -= speed;
            if (D && charRect.X + charRect.Width + speed <= Program.width) charRect.X += speed;
            Draw();
        }

        private void Draw()
        {
            Raylib.DrawRectangleRec(charRect, Raylib_cs.Color.DarkPurple);
        }
    }
}