using System;
using System.Collections.Generic;
using System.Drawing;
using Raylib_cs;

namespace main
{
    internal class Program
    {
        static float pointReq = 10f;
        public static int width = 1000;
        public static int height = 1000;

        static void Main(string[] args)
        {
            var kd = 1f;
            var title = "Game";
            var character = new Character();
            var center = new Point(width / 2, height / 2);
            var enemies = new List<Enemy>();
            var spawnTimer = 0;
            bool isPaused = false;
            Raylib.InitWindow(width, height, title);
            Raylib.SetTargetFPS(60);

            float cPointReq = pointReq;
            float cSpeedIncrease = 0f;

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
                            character.Kills++;
                            break;
                        }
                    }

                    if (character.Kills > cPointReq)
                    {
                        cPointReq += (((cPointReq * 0.25f)) * kd) + 10;
                        kd += 0.025f;
                        cSpeedIncrease = (float)Math.Log(5 / ((double)character.Kills / 100 + 1f));
                        character.Speed += cSpeedIncrease;
                    }

                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Raylib_cs.Color.Black);

                    DisplayInfo(character);

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
                pointReq = cPointReq;
            }

            Raylib.CloseWindow();
        }

        static void DisplayInfo(Character character)
        {
            Raylib.DrawText($"Score: {character.Score:00000}", 10, 10, 20, Raylib_cs.Color.RayWhite);
            Raylib.DrawText($"Kills: {character.Kills}", 10, 35, 20, Raylib_cs.Color.Red);
            Raylib.DrawText($"Kills until next upgrade: {(int)pointReq - character.Kills}", 10, 60, 20, Raylib_cs.Color.Gold);
            Raylib.DrawText($"Additional speed: +{Math.Round(character.Speed - 5, 1)}", 10, 85, 20, Raylib_cs.Color.SkyBlue);
        }

        static void DisplayPauseInfo(Character character)
        {
            Raylib.DrawText("PAUSED", width / 2 - 50, height / 2 - 100, 40, Raylib_cs.Color.RayWhite);
            Raylib.DrawText($"Score: {character.Score}", width / 2 - 50, height / 2 - 50, 20, Raylib_cs.Color.RayWhite);
            Raylib.DrawText($"Kills: {character.Kills}", width / 2 - 50, height / 2 - 25, 20, Raylib_cs.Color.Red);
            Raylib.DrawText($"Speed: {Math.Round(character.Speed, 1)}", width / 2 - 50, height / 2, 20, Raylib_cs.Color.SkyBlue);
            Raylib.DrawText($"Health: {character.Health}", width / 2 - 50, height / 2 + 25, 20, Raylib_cs.Color.Green);
        }
    }
}