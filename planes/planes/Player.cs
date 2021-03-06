﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework.Input.Touch;

namespace planes
{
    class Player : Sprite
    {
        const string assetName = "Images/planes";

        const int w = 68;   //plane width
        const int h = 17;   //plane height

        Vector2 shootButtonPosition = new Vector2(680, 360);
        Vector2 bombButtonPosition = new Vector2(30, 360);

        int maxHealth = 100;
        int maxAmmo = 2800;
        const int maxBombs = 2;

        float reloadTime = 0;

        SoundEffect browning;

        SpriteFont font;
        Texture2D bombTexture;

        Accelerometer accelerometer;
        TouchCollection tc;

        Button shootButton;
        Button bombButton;

        Bar healthBar;
        Bar ammoBar;
        public ScoreHandler score;

        public ExplosionHandler eh = new ExplosionHandler();

        public List<Bullet> bullets = new List<Bullet>();
        public List<Bomb> bombs = new List<Bomb>();
        public List<Powerup> healthPowerups = new List<Powerup>();
        public List<Powerup> ammoPowerups = new List<Powerup>();

        Rectangle[] sources = new Rectangle[]{
            new Rectangle(4,9,w,h),
            new Rectangle(4,41,w,h),
            new Rectangle(4,73,w,h),
            new Rectangle(85,9,w,h),
            new Rectangle(85,41,w,h),
            new Rectangle(85,73,w,h)
        };

        int[] animate = new int[8];
        int currentFrame = 0;
        private bool explosionCreated;

        public Player()
        {
            this.Position = new Vector2(125, 200);
            this.Color = Color.White;
            this.Rotation = 0.0f;
            this.IsAlive = true;
            this.Hitpoints = maxHealth;
            this.Ammo = maxAmmo;


            this.AvailibleBombs = maxBombs;
            healthBar = new Bar(Color.Red, new Vector2(25, 10));
            ammoBar = new Bar(Color.Gold, new Vector2(25, 20));
            score = new ScoreHandler();

            shootButton = new Button(ButtonType.SHOOT, shootButtonPosition);
            bombButton = new Button(ButtonType.BOMB, bombButtonPosition);

            accelerometer = new Accelerometer();
            accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);

            this.Init();
        }


        public int Hitpoints { get; set; }

        public int Ammo { get; set; }

        public int MaxHealth { get { return maxHealth; } private set { this.maxHealth = value; } }

        public int MaxAmmo { get { return maxAmmo; } private set { this.maxAmmo = value; } }

        public int Nation { get; set; }

        public int AvailibleBombs { get; set; }

        public bool Crash { get; set; }

        private bool fireTouch = false;

        private bool currentBombTouch = false;

        private bool oldBombTouch = false;

        private bool currentHealthPowerupTouch = false;

        private bool oldHealthPowerupTouch = false;

        private bool currentAmmoPowerupTouch = false;

        private bool oldAmmoPowerupTouch = false;

        public int W {
            get { return w; }
        } 

        public int H
        {
            get { return h; }
        } 

        public bool IsAlive { get; set; }

        public void LoadContent(ContentManager theContentManager)
        {
            this.contentManager = theContentManager;
            browning = contentManager.Load<SoundEffect>("Sounds/browning");
            healthBar.LoadContent(contentManager);
            ammoBar.LoadContent(contentManager);
            score.LoadContent(contentManager);

            font = contentManager.Load<SpriteFont>("Fonts/font");
            bombTexture = contentManager.Load<Texture2D>("Images/bomb");

            shootButton.LoadContent(theContentManager);
            bombButton.LoadContent(theContentManager);

            base.LoadContent(contentManager, assetName);
            this.Source = sources[0 + this.Nation];
            this.SetAnimation();
        }

        public void Init()
        {
            if (this.Nation == 0)
            {
                this.Ammo = 2800;
                this.Hitpoints = 100;
                this.maxAmmo = 2800;
                this.maxHealth = 100;
            }
            else if (this.Nation == 3)
            {
                this.Ammo = 2300;
                this.Hitpoints = 120;
                this.maxAmmo = 2300;
                this.maxHealth = 120;
            }

            this.explosionCreated = false;

            accelerometer.Start();
        }

        private void SetAnimation()
        {
            for (int i = 0; i < animate.Length / 4; i++)
                animate[i] = 0;
            for (int i = animate.Length / 4; i < animate.Length / 2; i++)
                animate[i] = 1;
            for (int i = animate.Length / 2; i < 3 * animate.Length / 4; i++)
                animate[i] = 2;
            for (int i = 3 * animate.Length / 4; i < animate.Length; i++)
                animate[i] = 1;
        }


        private void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            if (this.IsAlive)
                this.Rotation = e.SensorReading.Acceleration.Y;
        }


        public void Update(GameTimer timer, bool soundMuted)
        {
            fireTouch = false;
            currentBombTouch = false;
            currentHealthPowerupTouch = false;
            currentAmmoPowerupTouch = false;

            shootButton.ButtonPressed = false;
            bombButton.ButtonPressed = false;

            tc = TouchPanel.GetState();
            if (tc.Count > 0)
                foreach (TouchLocation tl in tc)
                {
                    if (tl.Position.X > 650 && tl.Position.Y > 350)
                    {
                        fireTouch = true;
                        shootButton.ButtonPressed = true;
                    } 
                    else if(tl.Position.X < 150 && tl.Position.Y > 350 && !currentBombTouch)
                    {
                        currentBombTouch = true;
                        bombButton.ButtonPressed = true;
                    }
                    else if ((tl.Position.X > 150 && tl.Position.X < 182) && (tl.Position.Y > 438 && tl.Position.Y < 470) && !currentHealthPowerupTouch)
                    {
                        currentHealthPowerupTouch = true;
                    }
                    else if ((tl.Position.X > 618 && tl.Position.X < 650) && (tl.Position.Y > 438 && tl.Position.Y < 470) && !currentAmmoPowerupTouch)
                    {
                        currentAmmoPowerupTouch = true;
                    }
                }

            if (!this.IsAlive && !explosionCreated && !this.Crash)
            {
                eh.CreateExplosion("normal", this.Position, contentManager);
                explosionCreated = true;
                //this.score.SaveScore();
            }
            else if (!this.IsAlive && !explosionCreated && this.Crash)
            {
                eh.CreateExplosion("huge", new Vector2(this.X - 32, this.Y - 64), contentManager);
                explosionCreated = true;
                //this.score.SaveScore();
            }
            else if (this.IsAlive)
            {
                if (this.Y < 0)
                    this.Rotation *= -1f;

                if (fireTouch && reloadTime == 0 && this.Ammo > 0)
                {
                    Vector2 direction = new Vector2((float)Math.Cos((double)(this.Rotation)), (float)Math.Sin((double)(this.Rotation)));
                    direction.Normalize();
                    Bullet newBullet = new Bullet(new Vector2(this.X + 2, this.Y + 3), direction, this.Rotation);
                    newBullet.LoadContent(contentManager);
                    bullets.Add(newBullet);
                    if (!soundMuted)
                        browning.Play(0.5f, 0, 0);
                    this.Ammo -= 1;
                }
                else if (currentBombTouch && !oldBombTouch && this.AvailibleBombs > 0)
                {
                    Bomb newBomb = new Bomb(this.Position);
                    newBomb.LoadContent(contentManager);
                    bombs.Add(newBomb);
                    AvailibleBombs--;
                }

                if (currentHealthPowerupTouch && !oldHealthPowerupTouch && this.healthPowerups.Count > 0)
                {
                    if (this.Hitpoints + 5 < this.MaxHealth)
                        this.Hitpoints += 5;
                    else
                        this.Hitpoints = this.MaxHealth;
                    this.healthPowerups.RemoveAt(healthPowerups.Count - 1);
                }
                if (currentAmmoPowerupTouch && !oldAmmoPowerupTouch && this.ammoPowerups.Count > 0)
                {
                    if (this.Ammo + 50 < this.MaxAmmo)
                        this.Ammo += 50;
                    else
                        this.Ammo = this.MaxAmmo;
                    this.ammoPowerups.RemoveAt(ammoPowerups.Count - 1);
                }

                reloadTime += (float)timer.UpdateInterval.TotalSeconds;
                if (reloadTime > 0.05f)
                    reloadTime = 0;

                this.Y += 5f * this.Rotation;

                healthBar.FilledWidth = this.Hitpoints * healthBar.W / maxHealth;
                ammoBar.FilledWidth = this.Ammo * ammoBar.W / maxAmmo;

                this.Animate();
            }

            foreach (Bullet b in bullets)
            {
                b.Update(timer);
                if (b.Position.X - this.Position.X > 700)
                {
                    bullets.Remove(b);
                    break;
                }
            }

            foreach (Bomb b in bombs)
            {
                b.Update(timer);
                if (b.Position.Y > 445)
                {
                    eh.CreateExplosion("normal", b.Position, contentManager);
                    bombs.Remove(b);
                    break;
                }
            }

            eh.Update(timer, this.IsAlive, soundMuted); //if palyer dies background stops so explosion can't move away, on the other hand when bomb lands explosion has to go off the screen

            oldBombTouch = currentBombTouch;

            oldAmmoPowerupTouch = currentAmmoPowerupTouch;

            oldHealthPowerupTouch = currentHealthPowerupTouch;

            if (this.Hitpoints < 1 || this.Y > 445)
            {
                this.IsAlive = false;

                if (this.Y > 445)
                    this.Crash = true;
            }
        }

        private void Animate()
        {

            Source = sources[animate[currentFrame] + this.Nation];
            currentFrame++;
            if (currentFrame == animate.Length)
                currentFrame = 0;
        }

        //public IEnumerable<Bullet> BulletsIter()
        //{
        //    foreach (Bullet b in bullets)
        //        yield return b;
        //}

        public void Draw(SpriteBatch theSpriteBatch)
        {
            if(healthPowerups.Count > 0)
            {
                healthPowerups[0].Draw(theSpriteBatch, Vector2.Zero, healthPowerups[0].Position, Color.White, 0.0f);
                theSpriteBatch.DrawString(font, string.Format("x {0}", this.healthPowerups.Count), new Vector2(190, 450), Color.Black);
                theSpriteBatch.DrawString(font, string.Format("x {0}", this.healthPowerups.Count), new Vector2(188, 450), Color.Red);
            }

            if(ammoPowerups.Count > 0)
            {
                ammoPowerups[0].Draw(theSpriteBatch, Vector2.Zero, ammoPowerups[0].Position, Color.White, 0.0f);
                theSpriteBatch.DrawString(font, string.Format("x {0}", this.ammoPowerups.Count), new Vector2(592, 450), Color.Black);
                theSpriteBatch.DrawString(font, string.Format("x {0}", this.ammoPowerups.Count), new Vector2(590, 450), Color.Gold);
            }

            shootButton.Draw(theSpriteBatch);
            bombButton.Draw(theSpriteBatch);

            if (IsAlive)
            {
                base.Draw(theSpriteBatch, new Vector2(w / 2, h / 2), this.Position, this.Color, this.Rotation);   
            }

            eh.Draw(theSpriteBatch);

            foreach (Bullet b in bullets)
                b.Draw(theSpriteBatch);
            foreach (Bomb b in bombs)
                b.Draw(theSpriteBatch);

            theSpriteBatch.Draw(bombTexture, new Vector2(118, 13), Color.White);
            theSpriteBatch.DrawString(font, string.Format("x {0}", this.AvailibleBombs), new Vector2(166, 4), Color.Black);
            theSpriteBatch.DrawString(font, string.Format("x {0}", this.AvailibleBombs), new Vector2(164, 4), Color.LightGray);

            healthBar.Draw(theSpriteBatch);
            ammoBar.Draw(theSpriteBatch);
            score.DrawScore(theSpriteBatch);
        }

    }
}
