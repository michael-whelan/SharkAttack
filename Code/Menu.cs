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


namespace Shark_Attack
{

    public class Menu : Microsoft.Xna.Framework.GameComponent
    {
        public Menu(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public Texture2D playOn, playOff, quitOn, quitOff, resumeOn, resumeOff,contact,lead;
        public Rectangle playRec, quitRec,contactRec,leadRec;
        int highLight = 1;
        const int play = 1, quit = 2;
        bool _pause = false;

        public int HighLight
        {
            get { return highLight; }
            set { highLight = value; }
        }

        public Rectangle PlayRec
        {
            get { return playRec; }
            set { playRec = value; }
        }

        public void Update(bool pause)
        {
            KeyboardState ks = Keyboard.GetState();
            _pause = pause;

            if (ks.IsKeyDown(Keys.Up))
            {
                highLight = 1;
            }
            else if (ks.IsKeyDown(Keys.Down))
            {
                highLight = 2;
            }

        }

        public void GameOver(SpriteBatch spriteBatch)
        {
            highLight = 2;
            spriteBatch.Draw(quitOn, quitRec, Color.AntiqueWhite);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            playRec = new Rectangle(100, 100, 100, 50);
            quitRec = new Rectangle(100, 180, 100, 50);
            contactRec = new Rectangle(100,260,100,50);
            leadRec = new Rectangle(500, 320, 130, 60);

                if (!_pause)
                {
                    spriteBatch.Draw(playOn, playRec, Color.AntiqueWhite);
                    spriteBatch.Draw(quitOn, quitRec, Color.AntiqueWhite);
                    spriteBatch.Draw(contact, contactRec, Color.AntiqueWhite);
                    spriteBatch.Draw(lead, leadRec, Color.AntiqueWhite);
                }
                else
                {
                    spriteBatch.Draw(resumeOn, playRec, Color.AntiqueWhite);
                    spriteBatch.Draw(quitOn, quitRec, Color.AntiqueWhite);
                }
        }
    }
}

