using Raylib_cs;
using System.Drawing;

namespace main
{
    internal class Character
    {
        public int Score { get; set; } = 0;
        public Raylib_cs.Rectangle CharacterRect { get; private set; }
        public int Speed { get; set; } = 5;

        public Character()
        {
            var pos = new Point(Program.width / 2, Program.height / 2);
            CharacterRect = new Raylib_cs.Rectangle(pos.X, pos.Y, 40, 40);
        }

        public void UpdatePos(bool moveUp, bool moveDown, bool moveLeft, bool moveRight)
        {
            var newRect = CharacterRect;

            if (moveUp) newRect.Y -= Speed;
            if (moveDown) newRect.Y += Speed;
            if (moveLeft) newRect.Y -= Speed;
            if (moveRight) newRect.Y += Speed;

            newRect.X = Math.Clamp(newRect.X, 0, Program.width - newRect.Width);
            newRect.Y = Math.Clamp(newRect.Y, 0, Program.height - newRect.Height);

            CharacterRect = newRect;
        }

        public void Draw()
        {
            Raylib.DrawRectangleRec(CharacterRect, Raylib_cs.Color.Green);
        }

        public bool CheckCollision(Raylib_cs.Rectangle otherRect)
        {
            return Raylib.CheckCollisionRecs(CharacterRect, otherRect);
        }
    }
}