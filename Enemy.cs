using Raylib_cs;
using System.Drawing;

internal class Enemy
{
    public Raylib_cs.Rectangle EnemyRect { get; private set; }
    public float Velocity { get; private set; }
    public Point Pos { get; set; }

    public Enemy(int size, int xPos, float velocity)
    {
        var pos = new Point(xPos, -size);
        EnemyRect = new Raylib_cs.Rectangle(pos.X, pos.Y, size, size);
        Velocity = velocity;
        Pos = pos;
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