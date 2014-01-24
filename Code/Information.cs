using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;


namespace Shark_Attack
{

    public class Information : Microsoft.Xna.Framework.GameComponent
    {
        public Information(Game game)
            : base(game)
        {
        }

        int score;
        int missed;
        bool gameOver = false;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public int Missed
        {
            get { return missed; }
            set { missed = value; }
        }

        public bool EndGame
        {
            get { return gameOver; }
            set { gameOver = value; }
        }

        public void GameOver(SpriteBatch spritebatch, SpriteFont spriteFont)
        {
            gameOver = true;
        }

        public void WriteToFile(StreamWriter outFile, string playerName)
        {
            //saves data
            outFile.Write("Name: " + playerName + ",");
            outFile.Write("Score: " + (int)score + ".");
          //  outFile.Write("" + score);
            outFile.WriteLine();
        }

        public void DrawName(SpriteBatch spritebatch, SpriteFont spriteFont, string st)
        {
            Vector2 stringPos = new Vector2(20, 100);
            string output = st;
            spritebatch.DrawString(spriteFont, output, stringPos, Color.Black);
        }

        public void DrawLeadBd(SpriteBatch spritebatch, SpriteFont spriteFont,int st)
        {
            Vector2 stringPos = new Vector2(520, 100);
            string output = Convert.ToString( st);
            spritebatch.DrawString(spriteFont, output, stringPos, Color.Black);
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont spriteFont)
        {
            if (gameOver == false)
            {
                Vector2 stringPos = new Vector2(10, 10);
                string output = "Score: " + score + "\n" + "Missed: " + missed;
                spritebatch.DrawString(spriteFont, output, stringPos, Color.Black);
            }
            else
            {
                Vector2 stringPos = new Vector2(100, 100);
                string output = "Score: " + score;
                spritebatch.DrawString(spriteFont, output, stringPos, Color.Black);
            }
        }
    }
}