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
    class Background : Sprite
    {
        const string night = "Images/Backgrounds/background_night";
        const string day = "Images/Backgrounds/background_day";

        float direction = -1.0f;
        float velocity = 100.0f;
        int period = 0;

        //Song backgroundSong;
        SpriteFont font; 

        public Texture2D background;

        bool textureChanged = true;
    
        public Background()
        {
            this.Position = Vector2.Zero;
            this.Color = Color.White;
            this.Rotation = 0.0f;
        }

        public int Period { get { return this.period; } }

        public void LoadContent(ContentManager theContentManager)
        {
            this.contentManager = theContentManager;
            if(DateTime.Now.Hour >= 18 || DateTime.Now.Hour < 6)
                background = contentManager.Load<Texture2D>(night);
            else
                background = contentManager.Load<Texture2D>(day);
            
           // backgroundSong = contentManager.Load<Song>("Sounds/backgroundMusic");
            font = contentManager.Load<SpriteFont>("Fonts/font");
        }

        //public void PlaySong()
        //{
        //    MediaPlayer.IsRepeating = true;
        //    MediaPlayer.Volume = 0.1f;
        //    MediaPlayer.Play(backgroundSong);
        //}

        //public void StopSong()
        //{
        //    MediaPlayer.Stop();
        //}

        //public void PauseSong()
        //{
        //    MediaPlayer.Pause();
        //}

        //public void ResumeSong()
        //{
        //    MediaPlayer.Resume();
        //}

        public void Scroll(GameTimer timer,  Player p )
        {
            if (p.IsAlive)
                this.X += direction * velocity * (float)timer.UpdateInterval.TotalSeconds;

            #region END_OF_TEXTURE_CHECK
            if (this.X % background.Width < 0 && this.X % background.Width > -5f && !textureChanged)
            {
                period += 1;
                textureChanged = true;
            }
            if (this.X % background.Width < -5f && textureChanged)
                textureChanged = false;
            #endregion

        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(background, this.Position + new Vector2((period) * background.Width, 0), new Rectangle(0, 0, background.Width, background.Height), this.Color, this.Rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            theSpriteBatch.Draw(background, this.Position + new Vector2((period + 1) * background.Width, 0), new Rectangle(0, 0, background.Width, background.Height), this.Color, this.Rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            theSpriteBatch.DrawString(font, string.Format("Level: {0}", this.period), new Vector2(402 - 55, 30), Color.Black);
            theSpriteBatch.DrawString(font, string.Format("Level: {0}", this.period), new Vector2(400 - 55, 30), Color.LightGray);
        }
    }
}
