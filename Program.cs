using System;
using System.Collections.Generic;
using System.Drawing;
using Raylib_cs;

namespace main
{
    internal class Program
    {
        public static int width = 1000;
        public static int height = 1000;

        static void Main(string[] args)
        {
            var enemiesKilled = 0;
            var pointReq = 100;
            var title = "Game";
            var character = new Character();
            var center = new Point(width / 2, height / 2);
            var enemies = new List<Enemy>();
            var spawnTimer = 0;

            Raylib.InitWindow(width, height, title);
            Raylib.SetTargetFPS(60);

            while (!Raylib.WindowShouldClose())
            {
                spawnTimer++;
                if (spawnTimer >= 60)
                {
                    spawnTimer = 0;
                    var size = Raylib.GetRandomValue(5, 20);
                    var xPos = Raylib.GetRandomValue(0, width - size);
                    var velocity = (size * -1) * 0.5f + 21 + (character.Score / 100);
                    enemies.Add(new Enemy(size, xPos, velocity));
                }

                foreach (var enemy in enemies)
                {
                    enemy.Update();
                }

                enemies.RemoveAll(enemy => enemy.EnemyRect.Y > height);

                foreach (var enemy in enemies)
                {
                    if (character.CheckCollision(enemy.EnemyRect))
                    {
                        var value = (int)((20 - enemy.EnemyRect.Width) * 2);
                        character.Score += value;
                        enemies.Remove(enemy);
                        enemiesKilled++;
                        break;
                    }
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib_cs.Color.Black);

                Raylib.DrawText($"Score: {character.Score:00000}", 10, 10, 20, Raylib_cs.Color.RayWhite);
                Raylib.DrawText($"Kills: {enemiesKilled}", 10, 35, 20, Raylib_cs.Color.RayWhite);

                Raylib.DrawLine(center.X + width / 2, center.Y, center.X - width / 2, center.Y, Raylib_cs.Color.Gold);
                Raylib.DrawLine(center.X, center.Y + height / 2, center.X, center.Y - height / 2, Raylib_cs.Color.Gold);

                foreach (var enemy in enemies)
                {
                    enemy.Draw();
                }

                character.UpdatePos(
                    Raylib.IsKeyDown(KeyboardKey.W),
                    Raylib.IsKeyDown(KeyboardKey.S),
                    Raylib.IsKeyDown(KeyboardKey.A),
                    Raylib.IsKeyDown(KeyboardKey.D)
                );

                character.Draw();

                if (enemiesKilled % 10 == 0)
                {
                    character.Speed += (int)(5 / (character.Score / 500f + 1));
                    pointReq += (int)(pointReq * 0.25);
                }

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}