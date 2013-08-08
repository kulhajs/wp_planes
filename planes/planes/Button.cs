using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace planes
{
    enum ButtonType
    {
        SHOOT,
        BOMB
    };

    class Button : Sprite
    {
        public bool ButtonPressed { get; set; }

        const int w = 90;
        const int h = 90;

        Rectangle[] shootSources = new Rectangle[]{
            new Rectangle(0,0,w,h),
            new Rectangle(0,h,w,h)
        };

        Rectangle[] bombSources = new Rectangle[]{
            new Rectangle(w,0,w,h),
            new Rectangle(w,h,w,h)
        };

        Rectangle[] sources;

        public Button(ButtonType buttonType, Vector2 position)
        {
            if (buttonType == ButtonType.SHOOT)
                sources = shootSources;
            else if (buttonType == ButtonType.BOMB)
                sources = bombSources;

            this.Position = position;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, "Images/buttons");
            this.Source = sources[0];
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            if(!ButtonPressed)
            {
                this.Source = sources[0];
            }
            else
            {
                this.Source = sources[1];
            }

            base.Draw(theSpriteBatch, Vector2.Zero, this.Position, Color.White, 0);
        }
    }
}
