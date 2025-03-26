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
            int availablePoints = 0;

            character.EnemyKilled += (sender, e) =>
            {
                var value = (int)((20 - e.EnemySize) * 2);
                character.Score += value;
                character.Kills++;

                if (character.Score > cPointReq)
                {
                    availablePoints += 1;
                    cPointReq += (cPointReq * 0.25f) + 100;
                    kd += 0.025f;
                    character.Regeneration += Math.Log(1) * Math.Exp(-kd);
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
                            character.Health -= enemy.EnemyRect.Width * 0.1f;
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

                    DisplayInfo(character, cPointReq, availablePoints);

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
                    if (Raylib.IsKeyPressed(KeyboardKey.One) && availablePoints > 0)
                    {
                        character.Speed += 0.5f;
                        availablePoints--;
                    }
                    if (Raylib.IsKeyPressed(KeyboardKey.Two) && availablePoints > 0)
                    {
                        character.Health += 10;
                        availablePoints--;
                    }
                    if (Raylib.IsKeyPressed(KeyboardKey.Three) && availablePoints > 0)
                    {
                        character.Regeneration += 0.1;
                        availablePoints--;
                    }

                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Raylib_cs.Color.Black);
                    DisplayPauseInfo(character, availablePoints);
                    Raylib.EndDrawing();
                }
            }

            Raylib.CloseWindow();
        }

        protected static void DisplayInfo(Character character, float cPointReq, int availablePoints)
        {
            Raylib.DrawText($"Score: {character.Score:00000}", 10, 10, 20, Raylib_cs.Color.RayWhite);
            Raylib.DrawText($"Kills: {character.Kills}", 10, 35, 20, Raylib_cs.Color.Red);
            Raylib.DrawText($"Points until next upgrade: {(int)cPointReq - character.Score}", 10, 60, 20, Raylib_cs.Color.Gold);
            Raylib.DrawText($"Available Points: {availablePoints}", 10, 85, 20, Raylib_cs.Color.SkyBlue);
        }

        protected static void DisplayPauseInfo(Character character, int availablePoints)
        {
            Raylib.DrawText("PAUSED", width / 2 - 50, height / 2 - 150, 40, Raylib_cs.Color.RayWhite);
            Raylib.DrawText($"Score: {character.Score}", width / 2 - 50, height / 2 - 100, 20, Raylib_cs.Color.RayWhite);
            Raylib.DrawText($"Kills: {character.Kills}", width / 2 - 50, height / 2 - 75, 20, Raylib_cs.Color.Red);
            Raylib.DrawText($"Available Points: {availablePoints}", width / 2 - 50, height / 2 - 50, 20, Raylib_cs.Color.Gold);

            Raylib.DrawText("Press 1 to increase Speed (+0.5)", width / 2 - 150, height / 2, 20, Raylib_cs.Color.SkyBlue);
            Raylib.DrawText($"Current Speed: {Math.Round(character.Speed, 1)}", width / 2 - 50, height / 2 + 25, 20, Raylib_cs.Color.SkyBlue);

            Raylib.DrawText("Press 2 to increase Health (+10)", width / 2 - 150, height / 2 + 50, 20, Raylib_cs.Color.Green);
            Raylib.DrawText($"Current Health: {Math.Round(character.Health, 0)}", width / 2 - 50, height / 2 + 75, 20, Raylib_cs.Color.Green);

            Raylib.DrawText("Press 3 to increase Regeneration (+0.1)", width / 2 - 150, height / 2 + 100, 20, Raylib_cs.Color.DarkPurple);
            Raylib.DrawText($"Current Regeneration: {character.Regeneration:0.00}", width / 2 - 50, height / 2 + 125, 20, Raylib_cs.Color.DarkPurple);
        }
    }
}