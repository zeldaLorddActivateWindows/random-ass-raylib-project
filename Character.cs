using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

enum Direction
{
    Left,
    Right,
    Up,
    Down
}

namespace main
{
    internal class Character
    {
        private string? name;
        public Direction Facing { get; set; }

        public string? Name
        {
            get => name;
            set => name = value == null ? throw new Exception("Invalid name input") :
                value.Length > 255 ? throw new Exception("Name too long") : value;
        }

        public UInt16 Health { get; set; }

        public UInt64 Score { get; set; }

        public Character(string? name, Direction facing, UInt16 health, UInt64 score)
        {
            Name = name;
            Facing = facing;
            Health = health;
            Score = score;
        }

        public Character() : this("Greg", Direction.Up, 100, 0) { }

        public void InitCharacter(Character character)
        {
            var pos = new Point(Random.Shared.Next(0, Program.width)-10, Random.Shared.Next(0, Program.height)-10);
            const int width = 10;
            const int height = 10;
            Raylib.DrawLine(pos.X - width / 2, pos.Y + height / 2, pos.X + width / 2, pos.Y + height / 2, Raylib_cs.Color.DarkPurple);
            Raylib.DrawLine(pos.X - width / 2, pos.Y - height / 2, pos.X + width / 2, pos.Y - height / 2, Raylib_cs.Color.DarkPurple);
            Raylib.DrawLine(pos.X - width / 2, pos.Y + height / 2, pos.X + width / 2, pos.Y - height / 2, Raylib_cs.Color.DarkPurple);
            Raylib.DrawLine(pos.X + width / 2, pos.Y + height / 2, pos.X - width / 2, pos.Y + height / 2, Raylib_cs.Color.DarkPurple);

        }
    }
}
