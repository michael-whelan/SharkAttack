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

    public class Fish : Microsoft.Xna.Framework.GameComponent
    {
        public Fish(Game game)
            : base(game)
        {
            width = 50;
            height = 20;
        }

        public Texture2D texFish, texFish2, texFish3;
        int xPos, yPos, width, height;
        Rectangle rec;
        static Random rand = new Random();
        int speed;
        int timer, timerMax;
        bool swim = false;
        int topSpeed = 4;
        int animateNum = 1;
        int slowAnimation;


        public int Speed
        {
            get { return speed; }
        }

        public int X
        {
            get { return xPos; }
            set { xPos = value; }
        }

        public int TopSpeed
        {
            get { return topSpeed; }
            set { topSpeed = value; }
        }

        public Rectangle Rec
        {
            get { return rec; }
        }

        public void Initialize(float w, float h)
        {
            swim = false;
            timer = 0;
            timerMax = rand.Next(30, 1000);
            yPos = rand.Next(10, (int)h - 20);
            xPos = (int)w + 100;
            speed = rand.Next(1, topSpeed);
        }

        public void Update()
        {
            timer++;

            if (timer >= timerMax)
            {
                swim = true;
            }
            if (swim == true)
            {
                xPos -= speed;
            }

            //animate Fish
            slowAnimation++;
            if (slowAnimation > 20 - speed)
            {
                if (animateNum < 3)
                {
                    animateNum++;
                }
                else { animateNum = 1; }
                slowAnimation = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            rec = new Rectangle(xPos, yPos, width, height);
            if (animateNum == 3)
            {
                spriteBatch.Draw(texFish2, rec, Color.AntiqueWhite);
            }

            else if (animateNum == 2)
            {
                spriteBatch.Draw(texFish, rec, Color.AntiqueWhite);
            }
            else if (animateNum == 1)
            {
                spriteBatch.Draw(texFish3, rec, Color.AntiqueWhite);
            }
        }
    }
}