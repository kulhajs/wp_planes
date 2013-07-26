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
    class Bomb : Sprite
    {

        private float wRotation = 1.0f;

        const int w = 42; //42 original width
        const int h = 14;  //14 original height

        Vector2 gravity = new Vector2(-1, 1);
        private Vector2 velocity = new Vector2(100, 300);

        Random random = new Random();

        public Bomb(Vector2 position)
        {
            this.Position = position;
        }

        public void LoadContent(ContentManager theContentmanager)
        {
            contentManager = theContentmanager;
            base.LoadContent(contentManager, "Images/bomb"); 
            Scale = 0.4f;
        }

        public void Update(GameTimer timer)
        {
            this.Position += gravity * velocity * (float)timer.UpdateInterval.TotalSeconds;

            if (this.Rotation < wRotation)
                this.Rotation += 0.02f;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch, Vector2.Zero, this.Position, Color.White, this.Rotation);
        }

    }
}
