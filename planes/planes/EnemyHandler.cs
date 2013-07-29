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
    class EnemyHandler
    {

        Random random = new Random();

        private List<Enemy> readyEnemies;
        private List<Enemy> visibleEnemies;
        private Player player;
        private Background background;

        private float elapsedTime = 0;

        private const int maxHealth = 10;

        private int enemiesInLevel;

        public bool EnemiesCreated { get; set; }

        public EnemyHandler(Player p, Background b)
        {
            readyEnemies = new List<Enemy>();
            visibleEnemies = new List<Enemy>();
            player = p;
            background = b;
        }

        public void Update(GameTimer timer, ContentManager theContentManager, PowerupHandler pu, Player player, bool soundMuted)
        {
            enemiesInLevel = (background.Period + 1) * 3;

            elapsedTime += (float)timer.UpdateInterval.TotalSeconds;
            if (elapsedTime > 45f / enemiesInLevel && readyEnemies.Count > 0) //45f = time to 4000px to pass (current background width = 2000px)
            {
                elapsedTime = 0;
                readyEnemies[0].Position = new Vector2(player.Position.X + random.Next(760, 810), random.Next(110, 450)); 
                visibleEnemies.Add(readyEnemies[0]);
                readyEnemies.Remove(readyEnemies[0]);
            }

            foreach (Enemy e in visibleEnemies)
            {
                if (e.Position.X < player.Position.X - 50)
                {
                    if (e.IsAlive && player.IsAlive)
                        player.score.AddPoints(-5);
                    e.IsAlive = true;
                    e.Hitpoints = maxHealth;
                    e.ExplosionCreated = false;
                    e.explosionHandler.explosions.Clear();
                    readyEnemies.Add(e);
                    visibleEnemies.Remove(e);
                    break;
                }
                e.Update(timer, pu, theContentManager, player, soundMuted);
            }
        }

        public bool CreateEnemies(ContentManager theContentManager)
        {
            Enemy newEnemy;
            for (int i = 0; i < 10; i++)
            {
                newEnemy = new Enemy(Vector2.Zero);
                newEnemy.LoadContent(theContentManager);
                readyEnemies.Add(newEnemy);
            }
            this.EnemiesCreated = true;

            return true;
        }

        public IEnumerable<Enemy> EnemiesIter()
        {
            foreach (Enemy e in visibleEnemies)
                yield return e;
        }

        public void DrawEnemies(SpriteBatch theSpriteBatch)
        {
            foreach (Enemy e in visibleEnemies)
                e.Draw(theSpriteBatch);
        }
    }
}
