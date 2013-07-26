using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace planes
{
    class Building : Sprite
    {

        const int church_w = 83;
        const int church_h = 79;

        const int house_w = 78;
        const int house_h = 64;

        public Building(string type)
        {
            this.Type = type;
            if (this.Type == "church")
            {
                this.origin = new Vector2(0, church_h);
                this.W = church_w;
                this.H = church_h;
            }
            else if (this.Type == "house")
            {
                this.origin = new Vector2(0, house_h);
                this.W = house_w;
                this.H = house_h;
            }

            this.Damaged = false;
        }

        public string Type { get; set; }

        public bool Damaged { get; set; }

        public int W { get; set; }
        public int H { get; set; }

        private Vector2 origin;

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, string.Format("Images/Buildings/{0}", this.Type));
            if (this.Type == "church")
                Source = new Rectangle(church_w, 0, church_w, church_h);
            else if (this.Type == "house")
                Source = new Rectangle(house_w, 0, house_w, house_h);
        }

        public void Update(GameTimer timer, Player player)
        {
            if (player.IsAlive)
                this.Position.X -= 100.0f * (float)timer.UpdateInterval.TotalSeconds; //100.0f = background velocity
            if (this.Damaged)
                this.Source = new Rectangle(0, 0, this.W, this.H);
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (DateTime.Now.Hour >= 18 || DateTime.Now.Hour < 6)
                base.Draw(theSpriteBatch, this.origin, this.Position, Color.Gray, 0.0f);
            else
                base.Draw(theSpriteBatch, this.origin, this.Position, Color.LightGray, 0.0f);
        }
    }
}
