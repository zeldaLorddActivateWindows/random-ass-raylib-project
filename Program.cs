using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Raylib_cs;

namespace main
{
    internal class Program
    {
        static float pointReq = 100f;
        public static int width = 1000;
        public static int height = 800;

        static void Main(string[] args)
        {
            var kd = 1f;
            var title = "Game";
            var character = new Character();
            var center = new Point(width / 2, height / 2);
            var enemies = new ConcurrentQueue<Enemy>();
            var spawnTimer = 0;
            bool isPaused = false;
            Raylib.InitWindow(width, height, title);
            Raylib.SetTargetFPS(60);

            float cPointReq = pointReq;
            float cSpeedIncrease = 0f;

            character.EnemyKilled += (sender, e) =>
            {
                var value = (int)((20 - e.EnemySize) * 2);
                character.Score += value;
                character.Kills++;

                if (character.Score > cPointReq)
                {
                    cPointReq += (((cPointReq * 0.25f)) * kd) + 100;
                    kd += 0.025f;
                    cSpeedIncrease = (float)Math.Log(5 / ((double)character.Kills / 100 + 1f));
                    character.Regeneration += Math.Log(1)*Math.Pow(Math.E, -kd);
                    character.Speed += cSpeedIncrease;
                    character.Health += (int) ((float)Math.Pow(Math.E, character.Score*0.01) * Math.Log(5 / (float)character.Kills / 1000 +1f));
                }
            };

            while (!Raylib.WindowShouldClose())
            {
                if (Raylib.IsKeyPressed(KeyboardKey.E))
                {
                    isPaused = !isPaused;
                }

                if (!isPaused)
                {
                    spawnTimer++;
                    if (spawnTimer >= 60)
                    {
                        spawnTimer = 0;
                        var size = Raylib.GetRandomValue(5, 20);
                        var xPos = Raylib.GetRandomValue(0, width - size);
                        var velocity = (size * -1) * 0.5f + 21 + (character.Kills / 10);
                        enemies.Enqueue(new Enemy(size, xPos, velocity));
                    }

                    Parallel.ForEach(enemies, enemy => enemy.Update());


                    var enemiesToRemove = new List<Enemy>();
                    foreach (var enemy in enemies)
                    {
                        if (enemy.EnemyRect.Y > height)
                        {
                            character.Health =- enemy.EnemyRect.X * 0.1f;
                            enemiesToRemove.Add(enemy);
                        }
                    }
                    foreach (var enemy in enemiesToRemove)
                    {
                        enemies.TryDequeue(out _);
                    }

                    Parallel.ForEach(enemies, enemy =>
                    {
                        if (character.CheckCollision(enemy.EnemyRect))
                        {
                            character.OnEnemyKilled(enemy.EnemyRect.Width);
                            enemies.TryDequeue(out _);
                        }
                    });

                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Raylib_cs.Color.Black);

                    DisplayInfo(character, cPointReq);

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

                    Raylib.EndDrawing();
                }
                else
                {
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Raylib_cs.Color.Black);
                    DisplayPauseInfo(character);
                    Raylib.EndDrawing();
                }
            }

            Raylib.CloseWindow();
        }

        protected static void DisplayInfo(Character character, float cPointReq)
        {
            Raylib.DrawText($"Score: {character.Score:00000}", 10, 10, 20, Raylib_cs.Color.RayWhite);
            Raylib.DrawText($"Kills: {character.Kills}", 10, 35, 20, Raylib_cs.Color.Red);
            Raylib.DrawText($"Points until next upgrade: {(int)cPointReq - character.Score}", 10, 60, 20, Raylib_cs.Color.Gold);
            Raylib.DrawText($"Additional speed: +{Math.Round(character.Speed - 5, 1)}", 10, 85, 20, Raylib_cs.Color.SkyBlue);
        }

        protected static void DisplayPauseInfo(Character character) //protected modifier for fun idk lol
        {
            Raylib.DrawText("PAUSED", width / 2 - 50, height / 2 - 100, 40, Raylib_cs.Color.RayWhite);
            Raylib.DrawText($"Score: {character.Score}", width / 2 - 50, height / 2 - 50, 20, Raylib_cs.Color.RayWhite);
            Raylib.DrawText($"Kills: {character.Kills}", width / 2 - 50, height / 2 - 25, 20, Raylib_cs.Color.Red);
            Raylib.DrawText($"Speed: {Math.Round(character.Speed, 1)}", width / 2 - 50, height / 2, 20, Raylib_cs.Color.SkyBlue);
            Raylib.DrawText($"Regeneration: {character.Regeneration,00}", width / 2 - 50, height / 2 + 50 , 20, Raylib_cs.Color.DarkPurple);
            Raylib.DrawText($"Health: {Math.Round(character.Health,0)}", width / 2 - 50, height / 2 + 25, 20, Raylib_cs.Color.Green);
        }
    }
}