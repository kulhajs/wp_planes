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
    class PowerupHandler
    {
        public List<Powerup> powerups;
        private Powerup newPowerup;

        public PowerupHandler()
        {
            powerups = new List<Powerup>();
        }

        public void CreatePowerup(Vector2 position, ContentManager theContentManager, string powerupType)
        {
            newPowerup = new Powerup(position, powerupType);
            newPowerup.LoadContent(theContentManager);
            powerups.Add(newPowerup);
        }

        public void Animate(GameTimer timer, Player player)
        {
            foreach (Powerup pu in powerups)
            {
                pu.Animate(timer, player);
                if (pu.X < -16) 
                {
                    powerups.Remove(pu);
                    break;
                }
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Powerup pu in powerups)
                pu.Draw(theSpriteBatch);
        }
    }
}
