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
    class Powerup : Sprite
    {

        const int w = 16;
        const int h = 16;

        int currentFrame = 0;

        private string assetName;

        private bool smallest = true;
        private bool biggest = false;


        Rectangle[] sources = new Rectangle[]{
            new Rectangle(0, 0, w, h),
            new Rectangle(w, 0, w, h),
            new Rectangle(2 * w, 0, w, h),
            new Rectangle(3 * w, 0, w, h)
        };

        Vector2 direction = new Vector2(-1, 0);
        Vector2 velocity = new Vector2(100, 0);

        int[] animate = new int[18];

        public Powerup(Vector2 position, string powerupType)
        {
            this.Position = position;
            assetName = powerupType;
        }

        public string Type { get { return this.assetName; } }

        public int W { get { return w; } }

        public int H { get { return h; } }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, "Images/" + assetName);
            if (this.assetName != "bomb")
            {
                Source = sources[0];
                this.InitializeAnimation();
            }
            else
                Scale = 0.4f;
        }

        private void InitializeAnimation()
        {
            for (int i = 0; i < animate.Length / 6; i++)
                animate[i] = 0;
            for (int i = animate.Length / 6; i < animate.Length / 3; i++)
                animate[i] = 1;
            for (int i = animate.Length / 3; i < animate.Length / 2; i++)
                animate[i] = 2;
            for (int i = animate.Length / 3; i < animate.Length * 2 / 3; i++)
                animate[i] = 3;
            for (int i = animate.Length * 2 / 3; i < animate.Length * 5 / 6; i++)
                animate[i] = 2;
            for (int i = animate.Length * 5 / 6; i < animate.Length; i++)
                animate[i] = 1;
        }

        public void Animate(GameTimer timer, Player player)
        {
            if (player.IsAlive)
                this.Position += direction * velocity * (float)timer.UpdateInterval.TotalSeconds;

            if (this.assetName != "bomb")
            {
                if (currentFrame < animate.Length)
                {
                    Source = sources[animate[currentFrame]];
                    currentFrame++;
                }
                else
                    currentFrame = 0;
            }
            else
            {
                if (this.Scale < 0.75f && smallest)
                    this.Scale += 0.6f * (float)timer.UpdateInterval.TotalSeconds;
                else
                {
                    smallest = false;
                    biggest = true;
                }
                if (this.Scale > 0.4f && biggest)
                    this.Scale -= 0.6f * (float)timer.UpdateInterval.TotalSeconds;
                else
                {
                    smallest = true;
                    biggest = false;
                }
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch, Vector2.Zero, this.Position, Color.White, 0.0f);
        }
    }
}
