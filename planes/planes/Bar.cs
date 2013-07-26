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
    class Bar : Sprite
    {
        const int w = 75;
        const int h = 8;

        Texture2D frame;
        Texture2D fill;

        Color color;

        public int FilledWidth { get; set; }
        public int W { get { return w; } }

        public Bar(Color color, Vector2 position)
        {
            this.Position = position;
            this.FilledWidth = w;
            this.color = color;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            frame = contentManager.Load<Texture2D>("Images/healthbar");
            fill = contentManager.Load<Texture2D>("Images/healthbar");
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(fill, new Rectangle((int)X, (int)Y, this.FilledWidth, h), new Rectangle(0, h, w, h), this.color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            theSpriteBatch.Draw(frame, new Rectangle((int)X, (int)Y, w, h), new Rectangle(0, 0, w, h), this.color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        }
    }
}
