using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace planes
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;

        Random random = new Random();

        Player player;

        Background background;

        EnemyHandler enemyHandler;
        IntersectionHandler intersectionHandler;
        BuildingHandler buildingHandler;
        PowerupHandler powerupHandler;

        bool soundMuted = true;

        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(/*333333*/ 111111);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            player = new Player();
            background = new Background();
            enemyHandler = new EnemyHandler(player, background);
            intersectionHandler = new IntersectionHandler();
            buildingHandler = new BuildingHandler();
            powerupHandler = new PowerupHandler();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // TODO: use this.content to load your game content here
            player.LoadContent(contentManager);
            enemyHandler.CreateEnemies(contentManager);
            background.LoadContent(contentManager);

            // Start the timer
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            intersectionHandler.HandleBulletIntersections(enemyHandler, player, contentManager);
            intersectionHandler.HandleBombsIntersection(player, buildingHandler, contentManager);

            if (player.IsAlive)
            {
                intersectionHandler.HandlePlanesInterestion(enemyHandler, player);
                intersectionHandler.HandlePowerupIntersections(powerupHandler, player);
                intersectionHandler.HandleBuildingIntersection(player, buildingHandler);

                if (random.Next(7500) == 1337)
                {
                    buildingHandler.CreateBuilding("house", contentManager);
                }
                if(random.Next(7500) == 42)
                    {
                        buildingHandler.CreateBuilding("church", contentManager);
                }
                if(buildingHandler.buildings.Count > 0)
                {
                    buildingHandler.UpdateBuildings(timer, player);
                }
            }


            powerupHandler.Animate(timer, player);

            player.Update(timer, soundMuted);
            enemyHandler.Update(timer, contentManager, powerupHandler, player, soundMuted);
            background.Scroll((float)timer.UpdateInterval.TotalSeconds, player);
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            background.Draw(spriteBatch);
            buildingHandler.DrawBuildings(spriteBatch);
            player.Draw(spriteBatch);
            enemyHandler.DrawEnemies(spriteBatch);
            powerupHandler.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}