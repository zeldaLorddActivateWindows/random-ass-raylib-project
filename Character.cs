using main;
using Raylib_cs;
using System.Drawing;

internal class Character
{
    public int Score { get; set; } = 0;
    public Raylib_cs.Rectangle CharacterRect { get; private set; }
    public double Speed { get; set; } = 5;
    public int Kills { get; set; } = 0;
    public float Health { get; set; } = 100;
    public double Regeneration { get; set; } = 0;
    public event EventHandler<EnemyKilledEventArgs> EnemyKilled;

    public Character()
    {
        InitCharacter();
    }

    public void InitCharacter()
    {
        var random = new Random();
        var pos = new Point(random.Next(0, Program.width - 40), random.Next(0, Program.height - 40));
        CharacterRect = new Raylib_cs.Rectangle(pos.X, pos.Y, 40, 40);
    }

    public void UpdatePos(bool moveUp, bool moveDown, bool moveLeft, bool moveRight)
    {
        var newRect = CharacterRect;

        if (moveUp) newRect.Y -= (float)Speed;
        if (moveDown) newRect.Y += (float)Speed;
        if (moveLeft) newRect.X -= (float)Speed;
        if (moveRight) newRect.X += (float)Speed;

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

    public void OnEnemyKilled(float enemySize)
    {
        EnemyKilled?.Invoke(this, new EnemyKilledEventArgs(enemySize));
    }
}