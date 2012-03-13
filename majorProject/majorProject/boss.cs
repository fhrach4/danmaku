using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

//XNA imports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace majorProject
{
    class Boss : Enemy
    {
        private List<Shot> shotTypes = new List<Shot>();

        private int action = 0;
        private int tarx;
        private int tary;

        // timers
        private Stopwatch startTimer = new Stopwatch();
        private Stopwatch shotTimer = new Stopwatch();
        private Stopwatch phaseTimer = new Stopwatch();
        private Stopwatch burstTimer = new Stopwatch();


        public Boss(int xPos, int yPos, int health, int tarx, int tary)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.health = health;
            this.tarx = tarx;
            this.tary = tary;
            this.moveSpeed = 5;

            this.hitBox.X = xPos;
            this.hitBox.Y = yPos;
        }

        public Boss(Texture2D sprite, List<Shot> shotTypes, int spriteWidth, int spriteHeight, int xPos, int yPos, int health,
            int tarx, int tary, Constants constants)
        {
            // Sprite variables
            this.sprite = sprite;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            
            // Shot variables
            this.shotTypes = shotTypes;

            // Other
            this.xPos = xPos;
            this.yPos = yPos;
            this.health = health;

            this.tarx = tarx;
            this.tary = tary;

            this.hitBox.X = -100;
            this.hitBox.Y = -50;
        }

        public override void update(Player human, ref EnemyShot[] shotList)
        {
            base.update(human, ref shotList);

            bool complete = false;

            base.update(human, ref shotList);

            if (action == 0)
            {
                //action++;
                if (base.moveTo(400, 100))
                {
                    action++;
                }
            }
            else if (action == 1)
            {
                if (phase1(ref shotList))
                {
                    action++;
                }
            }
            else if (action == 2)
            {
                if (phase2(ref shotList))
                {
                    action--;
                }
            }

            //Console.WriteLine("Boss Health: " + Convert.ToString(health));
        }

        private bool phase1(ref EnemyShot[] shotList)
        {
            if (!shotTimer.IsRunning)
            {
                shotTimer.Start();
            }

            if (!phaseTimer.IsRunning)
            {
                phaseTimer.Start();
            }
            
            if (phaseTimer.ElapsedMilliseconds >= 10000)
            {
                shotTimer.Stop();
                phaseTimer.Reset();
                phaseTimer.Stop();
                return true;
            }

            else
            {
                if (shotTimer.ElapsedMilliseconds >= 500)
                {
                    // slow shots
                    EnemyShot shot1 = new EnemyShot(shotSprite, 10, 45, constants.ENEMY_SHOT_MAXSPEED, 
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot2 = new EnemyShot(shotSprite, 10, 90, constants.ENEMY_SHOT_MAXSPEED,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot3 = new EnemyShot(shotSprite, 10, 135, constants.ENEMY_SHOT_MAXSPEED,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);

                    // faster shots
                    EnemyShot shot4 = new EnemyShot(shotSprite, 10, 105, constants.ENEMY_SHOT_MAXSPEED * 2,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot5 = new EnemyShot(shotSprite, 10, 75, constants.ENEMY_SHOT_MAXSPEED * 2,
                       xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);

                    EnemyShot[] temp = new EnemyShot[5] { shot1, shot2, shot3, shot4, shot5};

                    foreach (EnemyShot shot in temp)
                    {
                        for (int i = 0; i < shotList.Length; i++)
                        {
                            if (shotList[i] == null)
                            {
                                shotList[i] = shot;
                                break;
                            }
                        }
                    }
                    shotTimer.Reset();
                }
                return false;
            }
        }

        private bool phase2(ref EnemyShot[] shotList)
        {
            if (!shotTimer.IsRunning)
            {
                shotTimer.Start();
            }

            if (!phaseTimer.IsRunning)
            {
                phaseTimer.Start();
            }

            if (!burstTimer.IsRunning)
            {
                burstTimer.Start();
            }

            if (phaseTimer.ElapsedMilliseconds >= 10000)
            {
                shotTimer.Stop();
                burstTimer.Stop();
                phaseTimer.Reset();
                phaseTimer.Stop();
                return true;
            }

            else
            {
                if (shotTimer.ElapsedMilliseconds >= 1000)
                {
                    // burst 1
                    EnemyShot shot1 = new EnemyShot(shotSprite, 13, 70, constants.ENEMY_SHOT_MAXSPEED * 2,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot2 = new EnemyShot(shotSprite, 13, 80, constants.ENEMY_SHOT_MAXSPEED * 2,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot3 = new EnemyShot(shotSprite, 13, 90, constants.ENEMY_SHOT_MAXSPEED * 2,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot4 = new EnemyShot(shotSprite, 13, 100, constants.ENEMY_SHOT_MAXSPEED * 2,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot5 = new EnemyShot(shotSprite, 13, 110, constants.ENEMY_SHOT_MAXSPEED * 2,
                       xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);

                    EnemyShot[] temp = new EnemyShot[5] { shot1, shot2, shot3, shot4, shot5 };

                    foreach (EnemyShot shot in temp)
                    {
                        for (int i = 0; i < shotList.Length; i++)
                        {
                            if (shotList[i] == null)
                            {
                                shotList[i] = shot;
                                break;
                            }
                        }
                    }
                    shotTimer.Reset();
                }

                if (burstTimer.ElapsedMilliseconds >= 2000)
                {
                    // burst 2
                    EnemyShot shot1 = new EnemyShot(shotSprite, 13, 5, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot2 = new EnemyShot(shotSprite, 13, 15, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot3 = new EnemyShot(shotSprite, 13, 25, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot4 = new EnemyShot(shotSprite, 13, 35, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot5 = new EnemyShot(shotSprite, 13, 45, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot6 = new EnemyShot(shotSprite, 13, 55, constants.ENEMY_SHOT_MAXSPEED * 3,
                       xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot7 = new EnemyShot(shotSprite, 13, 65, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot8 = new EnemyShot(shotSprite, 13, 75, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot9 = new EnemyShot(shotSprite, 13, 85, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot10 = new EnemyShot(shotSprite, 13, 95, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot11 = new EnemyShot(shotSprite, 13, 105, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot12 = new EnemyShot(shotSprite, 13, 115, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot13 = new EnemyShot(shotSprite, 13, 125, constants.ENEMY_SHOT_MAXSPEED * 3,
                       xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot14 = new EnemyShot(shotSprite, 13, 135, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot15 = new EnemyShot(shotSprite, 13, 145, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot16 = new EnemyShot(shotSprite, 13, 155, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot17 = new EnemyShot(shotSprite, 13, 165, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot18 = new EnemyShot(shotSprite, 13, 175, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot19 = new EnemyShot(shotSprite, 13, 185, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot20 = new EnemyShot(shotSprite, 13, 195, constants.ENEMY_SHOT_MAXSPEED * 3,
                       xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot21 = new EnemyShot(shotSprite, 13, 205, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot22 = new EnemyShot(shotSprite, 13, 215, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot23 = new EnemyShot(shotSprite, 13, 225, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot24 = new EnemyShot(shotSprite, 13, 235, constants.ENEMY_SHOT_MAXSPEED * 3,
                       xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot25 = new EnemyShot(shotSprite, 13, 245, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot26 = new EnemyShot(shotSprite, 13, 255, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot27 = new EnemyShot(shotSprite, 13, 265, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot28 = new EnemyShot(shotSprite, 13, 275, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot29 = new EnemyShot(shotSprite, 13, 285, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot30 = new EnemyShot(shotSprite, 13, 295, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot31 = new EnemyShot(shotSprite, 13, 305, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot32 = new EnemyShot(shotSprite, 13, 315, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot33 = new EnemyShot(shotSprite, 13, 325, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot34 = new EnemyShot(shotSprite, 13, 335, constants.ENEMY_SHOT_MAXSPEED * 3,
                       xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot35 = new EnemyShot(shotSprite, 13, 345, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);
                    EnemyShot shot36 = new EnemyShot(shotSprite, 13, 355, constants.ENEMY_SHOT_MAXSPEED * 3,
                        xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_WIDTH);

                  

                    EnemyShot[] temp = new EnemyShot[36] { shot1, shot2, shot3, shot4, shot5, shot6, shot7, shot8, shot9, shot10,
                    shot11, shot12, shot13, shot14, shot15, shot16, shot17, shot18, shot19, shot20, shot21, shot22, shot23, shot24,
                    shot25, shot26, shot27, shot28, shot29, shot30, shot31, shot32, shot33, shot34, shot35, shot36};

                    foreach (EnemyShot shot in temp)
                    {
                        for (int i = 0; i < shotList.Length; i++)
                        {
                            if (shotList[i] == null)
                            {
                                shotList[i] = shot;
                                break;
                            }
                        }
                    }
                    burstTimer.Reset();
                }

                return false;
            }


        }

        public override void die(Expolsion explosion, SpriteBatch batch)
        {
            base.die(explosion, batch);
            xPos = -100;
            yPos = -100;
        }
    }
}
