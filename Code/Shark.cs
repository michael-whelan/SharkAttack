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
using Microsoft.Xna.Framework.Input.Touch;


namespace Shark_Attack
{
    public class Shark : Microsoft.Xna.Framework.GameComponent
    {
        public Shark(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public Texture2D texShark, texShark2, texShark3, texShark4, texShark5;
        int xPos = 100, width = 120, height = 70;
        int yPos = 100;
        Rectangle rec;
        const int speed = 4;
        const int sink = 1;
        int animationTimer;
        int animationNum = 1;
        bool animate = false;
        int slowAnimation;
        int constAnimationTime;
        int baseAnNum = 1;
        public SoundEffect sndSink;
        public SoundEffectInstance sndSinkLoop;
        public SoundEffect sndMove;
        public SoundEffectInstance sndMoveLoop;
        public SoundEffect sndChew;

        public bool Animate
        {
            get { return animate; }
            set { animate = value; }
        }

        public Rectangle Rec
        {
            get { return rec; }
        }

        public void Update(float w, float h,bool up,bool down,bool left,bool right)
        {
            KeyboardState ks = Keyboard.GetState();
                if(left){
                    sndMoveLoop.Play();
                    if (xPos > 0)
                    {
                        xPos -= speed;

                    }
               }
                if (right)
                {
                    sndMoveLoop.Play();
                    if (xPos < w - 50)
                    {
                        xPos += speed;

                    }
                }

                if (down)
                {
                    sndMoveLoop.Play();
                    if (yPos < (int)h - 40)
                    {
                        yPos += speed;
                    }
                }
                if (up)
                {
                    if (yPos > 10)
                    {
                        yPos -= speed;
                    }
                }
                else
                {
                    if (yPos < h - 50)
                    {
                        yPos += sink;
                        sndSinkLoop.Play();
                        sndMoveLoop.Pause();
                    }
                }
            Animation();
        }

        public void Animation()
        {
            //constantAnimation

            slowAnimation++;
            if (slowAnimation > 10)
            {
                if (baseAnNum < 3)
                {
                    baseAnNum++;
                }
                else { baseAnNum = 1; }
                slowAnimation = 0;
            }

            //bite animation
            if (animate == true)
            {
                animationTimer++;
                if (animationTimer < 30)
                {
                    slowAnimation++;
                    if (slowAnimation > 5)
                    {
                        if (animationNum < 3)
                        {
                            animationNum++;
                        }
                        else
                        {
                            animationNum = 1;
                        }
                        slowAnimation = 0;
                    }

                }
                else
                {
                    animationTimer = 0;
                    animate = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.AntiqueWhite;
            rec = new Rectangle(xPos, yPos, width, height);
            if (animate == true)
            {
                if (animationNum == 3)
                {
                    spriteBatch.Draw(texShark3, rec, color);
                }
                else if (animationNum == 2)
                {
                    spriteBatch.Draw(texShark2, rec, color);
                }
                else
                {
                    spriteBatch.Draw(texShark, rec, color);
                }
            }
            else
            {
                if (baseAnNum == 1)
                {
                    spriteBatch.Draw(texShark4, rec, color);
                }
                else if (baseAnNum == 3)
                {
                    spriteBatch.Draw(texShark5, rec, color);
                }
                else
                {
                    spriteBatch.Draw(texShark, rec, color);
                }
            }
        }
    }
}
