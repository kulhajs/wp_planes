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
    class Clouds : Sprite
    {
        const string nightClouds = "Images/Backgrounds/clouds_night";
        const string dayClouds = "Images/Backgrounds/clouds_day";

        float direction = -1.0f;
        float velocity = 100.0f;
        int period = 0;
                
        Texture2D clouds;

        bool textureChanged = true;

        public Clouds()
        {
            this.Position = Vector2.Zero;
            this.Color = Color.White;
            this.Rotation = 0.0f;
        }

        public int Period { get { return this.period; } }

        public void LoadContent(ContentManager theContentManager)
        {
            this.contentManager = theContentManager;
            if (DateTime.Now.Hour >= 18 || DateTime.Now.Hour < 6)
                clouds = contentManager.Load<Texture2D>(nightClouds);
            else
                clouds = contentManager.Load<Texture2D>(dayClouds);
        }
        
        public void Scroll(GameTimer timer, Player p)
        {
            if (p.IsAlive)
                this.X += direction * velocity * (float)timer.UpdateInterval.TotalSeconds;

            #region END_OF_TEXTURE_CHECK
            if (this.X % clouds.Width < 0 && this.X % clouds.Width > -5f && !textureChanged)
            {
                period += 1;
                textureChanged = true;
            }
            if (this.X % clouds.Width < -5f && textureChanged)
                textureChanged = false;
            #endregion

        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(clouds, this.Position + new Vector2((period) * clouds.Width, 0), new Rectangle(0, 0, clouds.Width, clouds.Height), this.Color, this.Rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            theSpriteBatch.Draw(clouds, this.Position + new Vector2((period + 1) * clouds.Width, 0), new Rectangle(0, 0, clouds.Width, clouds.Height), this.Color, this.Rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
    }
}
