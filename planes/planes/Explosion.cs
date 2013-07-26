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
    class Explosion : Sprite
    {

        const int w = 64;
        const int h = 64;

        public Vector2 origin;
        
        #region SOURCE RECTANGLES
        Rectangle[] sources = new Rectangle[]{
            new Rectangle(0, 0, w, h),
            new Rectangle(w, 0, w, h),
            new Rectangle(2*w, 0, w, h),
            new Rectangle(3*w, 0, w, h), 
            new Rectangle(4*w, 0, w, h), 
            new Rectangle(5*w, 0, w, h),
            new Rectangle(6*w, 0, w, h), 
            new Rectangle(7*w, 0, w, h), 
            new Rectangle(0, h, w, h),
            new Rectangle(w, h, w, h),
            new Rectangle(2*w, h, w, h),
            new Rectangle(3*w, h, w, h), 
            new Rectangle(4*w, h, w, h), 
            new Rectangle(5*w, h, w, h),
            new Rectangle(6*w, h, w, h), 
            new Rectangle(7*w, h, w, h), 
            new Rectangle(0, 2 * h, w, h),
            new Rectangle(w, 2 * h, w, h),
            new Rectangle(2*w, 2 * h, w, h),
            new Rectangle(3*w, 2 * h, w, h), 
            new Rectangle(4*w, 2 * h, w, h), 
            new Rectangle(5*w, 2 * h, w, h),
            new Rectangle(6*w, 2 * h, w, h), 
            new Rectangle(7*w, 2 * h, w, h), 
            new Rectangle(0, 3 * h, w, h),
            new Rectangle(w, 3 * h, w, h),
            new Rectangle(2*w, 3 * h, w, h),
            new Rectangle(3*w, 3 * h, w, h), 
            new Rectangle(4*w, 3 * h, w, h), 
            new Rectangle(5*w, 3 * h, w, h),
            new Rectangle(6*w, 3 * h, w, h), 
            new Rectangle(7*w, 3 * h, w, h), 
            new Rectangle(0, 4 * h, w, h),
            new Rectangle(w, 4 * h, w, h),
            new Rectangle(2*w, 4 * h, w, h),
            new Rectangle(3*w, 4 * h, w, h), 
            new Rectangle(4*w, 4 * h, w, h), 
            new Rectangle(5*w, 4 * h, w, h),
            new Rectangle(6*w, 4 * h, w, h), 
            new Rectangle(7*w, 4 * h, w, h), 
            new Rectangle(0, 5 * h, w, h),
            new Rectangle(w, 5 * h, w, h),
            new Rectangle(2*w, 5 * h, w, h),
            new Rectangle(3*w, 5 * h, w, h), 
            new Rectangle(4*w, 5 * h, w, h), 
            new Rectangle(5*w, 5 * h, w, h),
            new Rectangle(6*w, 5 * h, w, h), 
            new Rectangle(7*w, 5 * h, w, h), 
        };
        #endregion

        int currentFrame = 0;

        private SoundEffect explosion;
        private float explosionVolume;

        private string explostionType;
        
        public Explosion(string type, Vector2 origin, Vector2 position)
        {

            this.explostionType = type;

            if (explostionType == "small")
            {
                this.Scale = 0.2f;
                this.explosionVolume = 0.5f;
            }
            else if (explostionType == "normal")
            {
                this.Scale = 1.0f;
                this.explosionVolume = 0.75f;
            }
            else if (explostionType == "huge")
            {
                this.Scale = 2.0f;
                this.explosionVolume = 1.0f;
            }
            else
                throw new Exception("unknown explosion type!");

            this.ShouldDraw = true;
            this.origin = origin;
            this.Position = position;
        }

        public bool ShouldDraw { get; set; }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, "Images/explosion");
            this.Source = new Rectangle(0, 0, w, h);
            explosion = contentManager.Load<SoundEffect>("Sounds/explosion");
        }

        public void Explode(bool soundMuted)
        {
            if (currentFrame < sources.Length && ShouldDraw)
            {
                Source = sources[currentFrame];

                if (explostionType == "small")
                    currentFrame += 2;
                else if (explostionType == "normal")
                    currentFrame++;
                else if (explostionType == "huge")
                    currentFrame++;
            }
            else
            {
                currentFrame = 0;
                this.ShouldDraw = false;
            }

            if (!soundMuted && currentFrame == 2)
                explosion.Play(explosionVolume, 0, 0);
        }


        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (this.ShouldDraw)
                base.Draw(theSpriteBatch, origin, Position, Color.White, 0.0f);
        }

    }
}
