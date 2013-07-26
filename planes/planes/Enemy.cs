using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace planes
{
    class Enemy : Sprite
    {
        const string assetName = "Images/planes";

        const int w = 68;   //plane width
        const int h = 17;   //plane height

        Random random = new Random();

        private const int maxHealth = 10;

        private Vector2 direction = new Vector2(-1, 0);
        private Vector2 velocity = new Vector2(150, 0); //150

        float reloadTime = 0;


        //SoundEffect browning;
        //SoundEffect mg;

        public ExplosionHandler explosionHandler = new ExplosionHandler();

        public List<Bullet> bullets = new List<Bullet>();

        Rectangle[] sources = new Rectangle[]{
            new Rectangle(87,106,w,h),
            new Rectangle(87,138,w,h),
            new Rectangle(87,170,w,h),
            new Rectangle(8,106,w,h),
            new Rectangle(8,138,w,h),
            new Rectangle(8,170,w,h)
        };

        int[] animate = new int[8];
        int currentFrame = 0;

        public bool ExplosionCreated { get; set; }

        public Enemy(Vector2 position)
        {
            this.Color = Color.White;
            this.IsAlive = true;
            this.Position = position;
            this.Hitpoints = maxHealth;
        }

        public int W { get { return w; } }

        public int H { get { return h; } }

        public int Hitpoints { get; set; }

        public bool IsAlive { get; set; }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            //mg = contentManager.Load<SoundEffect>("Sounds/mg");
            base.LoadContent(contentManager, assetName);
            this.SetAnimation();
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

        public void Update(GameTimer timer/*, PowerupHandler pu */, ContentManager theContentManager, Player player, bool soundsMuted)
        {
            this.Position += direction * velocity * (float)timer.UpdateInterval.TotalSeconds;

            if (this.Hitpoints < 1 && !ExplosionCreated)
            {
                //this.GeneratePowerups(pu, theContentManager);

                this.IsAlive = false;
                explosionHandler.CreateExplosion("normal", new Vector2(this.X - 47, this.Y - 87), contentManager);
                this.ExplosionCreated = true;
                //player.score.AddPoints(10);
            }
            
            if (this.IsAlive &&  reloadTime == 0 && random.Next(this.Hitpoints / 2 + 1) == 1)
            {
                Bullet newBullet = new Bullet(new Vector2(this.X - 45, this.Y - 84), direction, this.Rotation);
                newBullet.LoadContent(contentManager);
                bullets.Add(newBullet);
                //if (!soundsMuted)
                //    mg.Play(0.075f, 0, 0);
            }

            reloadTime += (float)timer.UpdateInterval.TotalSeconds;
            if (reloadTime > 0.05f)
                reloadTime = 0;

            foreach (Bullet b in bullets)
            {
                b.Update(timer);
                if (b.Position.X - this.Position.X > 700)
                {
                    bullets.Remove(b);
                    break;
                }
            }

            explosionHandler.Update(timer, player.IsAlive, soundsMuted); //player.IsAlive indicates that all explosions are moving towards the player as long palyer is alive and background is scrolling

            this.Animate(player);
        }

        //private void GeneratePowerups(PowerupHandler pu, ContentManager theContentManager)
        //{
        //    int x = random.Next(0, 7);

        //    if (x == 1)
        //        pu.CreatePowerup(new Vector2(this.X - 50, this.Y - 100), theContentManager, "health");
        //    else if (x == 2)
        //        pu.CreatePowerup(new Vector2(random.Next(810, 900), random.Next(20, 300)), theContentManager, "ten");
        //    else if (x == 3)
        //        pu.CreatePowerup(new Vector2(this.X - 50, this.Y - 100), theContentManager, "ammo");

        //    if (random.Next(30) == 13)
        //        pu.CreatePowerup(new Vector2(random.Next(810, 900), random.Next(20, 300)), theContentManager, "bomb");
        //}

        private void Animate(Player player)
        {
            Source = sources[animate[currentFrame] + player.Nation];
            currentFrame++;
            if (currentFrame == animate.Length)
                currentFrame = 0;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (this.IsAlive)
                base.Draw(theSpriteBatch, new Vector2(texture.Width / 2, texture.Height / 2), this.Position, this.Color, 0.0f);
            
                explosionHandler.Draw(theSpriteBatch);

            foreach (Bullet b in bullets)
                b.Draw(theSpriteBatch);
        }
    }
}
