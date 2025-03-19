using System;
using System.Drawing;
using Raylib_cs;

namespace main
{
    internal class Enemy
    {
        public Raylib_cs.Rectangle EnemyRect { get; private set; }
        public float Velocity { get; private set; }

        public Enemy(int size, int xPos, float velocity)
        {
            var pos = new Point(xPos, Program.height);
            EnemyRect = new Raylib_cs.Rectangle(pos.X, pos.Y, size, size);
            Velocity = velocity;
        }

        public void Update()
        {
            var rect = EnemyRect;
            rect.Y += Velocity;
            EnemyRect = rect;
        }

        public void Draw()
        {
            Raylib.DrawRectangleRec(EnemyRect, Raylib_cs.Color.DarkPurple);
        }
    }
}