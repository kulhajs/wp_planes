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
    class Bullet : Sprite
    {
        const string assetName = "Images/bullet";
        Vector2 velocity = new Vector2(750, 750);
        Vector2 direction;

        public Bullet(Vector2 position, Vector2 direction, float rotation)
        {
            this.Position = position;
            this.direction = direction;
            this.Rotation = rotation;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, assetName);
        }

        public void Update(GameTimer timer)
        {
            this.Position += this.direction * velocity * (float)timer.UpdateInterval.TotalSeconds;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch, Vector2.Zero, this.Position, Color.White, this.Rotation);
        }
    }
}
