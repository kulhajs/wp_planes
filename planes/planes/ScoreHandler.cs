using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    class ScoreHandler
    {
        private int score;
        private int oldScore;
        private SpriteFont font;
        private FileStream fs;
        private StreamWriter sw;
        private StreamReader sr;
        public List<int> highScores;
        private const string fileName = "score";
        private bool scoreChanged;

        public ScoreHandler()
        {
            this.score = 0;
            scoreChanged = false;
            //highScores = new List<int>();
            //if (!File.Exists(fileName))
            //{
            //    using (fs = File.Create(fileName)) { }

            //    using (sw = new StreamWriter(fileName))
            //    {
            //        for (int i = 0; i < 5; i++)
            //        {
            //            sw.WriteLine(0);
            //        }
            //    }
            //}
            //this.LoadScores();
        }

        //private void LoadScores()
        //{
        //    using (sr = new StreamReader(fileName))
        //    {
        //        string line;
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            highScores.Add(Convert.ToInt32(line)); ;
        //        }
        //    }
        //}

        public void LoadContent(ContentManager theContentManager)
        {
            font = theContentManager.Load<SpriteFont>("Fonts/font");
        }

        public int Points { get { return this.score; } }

        //public void SaveScore()
        //{

        //    for (int i = 0; i < highScores.Count ; i++)
        //    {
        //        if (this.Points > highScores[i])
        //        {
        //            highScores[i] = this.Points;
        //            scoreChanged = true;
        //        }
        //        if (scoreChanged)
        //            break;
        //    }
        //    highScores.Sort();
        //    highScores.Reverse();

        //    while (highScores.Count > 5)
        //        highScores.RemoveAt(highScores.Count - 1);

        //    if (oldScore < this.score)
        //    {
        //        using (sw = new StreamWriter(fileName))
        //        {
        //            for (int i = 0; i < highScores.Count; i++)
        //            {
        //                sw.WriteLine(highScores[i]);
        //            }
        //        }
        //    }
        //}

        public void AddPoints(int value)
        {
            this.score += value;
        }

        public void DrawScore(SpriteBatch theSpriteBacth)
        {
            theSpriteBacth.DrawString(font, string.Format("Score: {0}", this.score), new Vector2(602, 4), Color.Black);
            theSpriteBacth.DrawString(font, string.Format("Score: {0}", this.score), new Vector2(600, 4), Color.LightGray);
        }
    }
}
