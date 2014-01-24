using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Xml.Serialization;
using System.IO.IsolatedStorage;

namespace Shark_Attack
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        Texture2D background, back1, back2, back3, back4, back5, back6, back7,leaderBack;
        Rectangle backRec;
        SpriteBatch spriteBatch;
        Shark shark;
        Fish[] fish;
        Menu menu;
        Information info;
        SpriteFont spriteFont;
        int backTexNum = 1;
        enum gameState { menu, gamePlay,contactPage,leaderBoard } // holds the game state
        gameState theGameState = gameState.menu;
        bool pause = false;
        int timer;
        bool gameOver = false;
        DisplayOrientation ori;
        Texture2D upTex, downTex, leftTex, rightTex;
        Rectangle upRec, downRec, leftRec, rightRec;
        bool up = false, down = false, left = false, right = false;
        String currentFile = "SaveFileSharkAttack.txt";
        String currentName = "SaveNameSharkAttack.txt";
        string playerName = "";
        Texture2D contactImage;
        Rectangle contRec;
        string highScore = "0";
        string name = "N/A";
        int lastHighScore;
        bool newHigh = false;
        Texture2D newHighTex;
        Rectangle newHighRec; int timerH;
        Rectangle highOrig;//stores x ,y ,w,h of highscore draw position

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            fish = new Fish[10];
            for (int i = 0; i < 10; i++)
            {
                fish[i] = new Fish(this);
            }
            info = new Information(this);
            menu = new Menu(this);
            shark = new Shark(this);

            //set resolution
            DisplayOrientation ori = DisplayOrientation.LandscapeRight;
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 400;
            graphics.PreferredBackBufferWidth = 666;

        }

        protected override void Initialize()
        {
           
            for (int i = 0; i < 10; i++)
            {
                fish[i].Initialize(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            }
            highOrig = new Rectangle(200,150,500,30);
            newHighRec = highOrig;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            shark.sndChew = this.Content.Load<SoundEffect>("chew");

            shark.sndSink = this.Content.Load<SoundEffect>("SharkSink");
            shark.sndSinkLoop = shark.sndSink.CreateInstance();
            shark.sndSinkLoop.IsLooped = true;

            shark.sndMove = this.Content.Load<SoundEffect>("sharkMove");
            shark.sndMoveLoop = shark.sndMove.CreateInstance();
            shark.sndMoveLoop.IsLooped = true;

            shark.texShark = this.Content.Load<Texture2D>("Shark/shark");
            shark.texShark2 = this.Content.Load<Texture2D>("Shark/shark2");
            shark.texShark3 = this.Content.Load<Texture2D>("Shark/shark3");
            shark.texShark4 = this.Content.Load<Texture2D>("Shark/shark4");
            shark.texShark5 = this.Content.Load<Texture2D>("Shark/shark5");

            upTex = this.Content.Load<Texture2D>("Up");
            downTex = this.Content.Load<Texture2D>("Down");
            leftTex = this.Content.Load<Texture2D>("Left");
            rightTex = this.Content.Load<Texture2D>("Right");

            newHighTex = this.Content.Load<Texture2D>("newScore");

            for (int i = 0; i < 10; i++)
            {
                fish[i].texFish = this.Content.Load<Texture2D>("fish");
                fish[i].texFish2 = this.Content.Load<Texture2D>("fish2");
                fish[i].texFish3 = this.Content.Load<Texture2D>("fish3");
            }
            spriteFont = this.Content.Load<SpriteFont>("SpriteFont1");
            menu.playOff = this.Content.Load<Texture2D>("PlayOff");
            menu.playOn = this.Content.Load<Texture2D>("Play");
            menu.quitOff = this.Content.Load<Texture2D>("QuitOff");
            menu.quitOn = this.Content.Load<Texture2D>("Quit");
            menu.resumeOn = this.Content.Load<Texture2D>("resume");
            menu.resumeOff = this.Content.Load<Texture2D>("resumeOff");
            menu.contact = this.Content.Load<Texture2D>("contact");
            menu.lead = this.Content.Load<Texture2D>("leaderboard");

            contactImage = this.Content.Load<Texture2D>("Background/contactpic");
            leaderBack = this.Content.Load<Texture2D>("Background/leaderboard");
            back1 = this.Content.Load<Texture2D>("Background/back1");
            back2 = this.Content.Load<Texture2D>("Background/back2");
            back3 = this.Content.Load<Texture2D>("Background/back3");
            back4 = this.Content.Load<Texture2D>("Background/back4");
            back5 = this.Content.Load<Texture2D>("Background/back5");
            back6 = this.Content.Load<Texture2D>("Background/back6");
            back7 = this.Content.Load<Texture2D>("Background/back7");

        }

        protected override void UnloadContent()
        {

        }

        public void Pause()
        {
            if (timer >= 10)
            {
                if (!pause)
                {
                    pause = true;
                }
                else if (pause)
                {
                    pause = false;
                }
                timer = 0;
            }
        }

        public void CheckHighScore() 
        {
            if (info.Score > lastHighScore)
            {
                newHigh = true;
            }

            if (newHigh)
            {
                timerH++;

                if (timerH > 5)
                {
                    newHighRec.X-=4;
                    newHighRec.Y-=4;
                   
                }
            }
            else
            { newHighRec = highOrig; }

            if (newHighRec.Y < -100)
            { newHigh = false; }
        }

        protected override void Update(GameTime gameTime)
        {
            timer++;//for pause
            KeyboardState ks = Keyboard.GetState();
            if (theGameState == gameState.gamePlay)
            {
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                { Pause(); }
                if (!pause)
                {
                    CheckHighScore();
                    UpdateButtons();
                    shark.Update(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, up, down, left, right);
                    for (int i = 0; i < 10; i++)
                    {
                        fish[i].Update();

                        //off screen
                        if (fish[i].X <= -30)
                        {
                            info.Missed++;
                            fish[i].Initialize(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
                        }
                    }
                }
                else if (pause)
                {
                    menu.Update(pause);
                }
                if (info.Missed >= 3)
                {
                    EndGame();
                }
                else
                {
                    info.EndGame = false;
                }
                Collision();
                UpdateDifficulty();

            }
            else if (theGameState == gameState.menu)
            {
                menu.Update(pause);
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                { this.Exit(); }
            }

               
            UpdateGameState(ks);
            base.Update(gameTime);
        }

        private void OnEndShowKeyboardInput(IAsyncResult result)
        {
 
                playerName = (string)Guide.EndShowKeyboardInput(result);
    
        }

        public void UpdateGameState(KeyboardState ks)
        {
            if (theGameState == gameState.menu)
            {
                timer++;
                foreach (TouchLocation tl in TouchPanel.GetState())
                {
                    if (menu.playRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                    {
                        timerH = 0;
                       // theGameState = gameState.g;
                        Guide.BeginShowKeyboardInput(PlayerIndex.One, "You Win", "Insert your name", "", new AsyncCallback(OnEndShowKeyboardInput), null, false);
                       theGameState = gameState.gamePlay;
                       gameOver = false;
                        info.Score = 0;
                        info.Missed = 0;
                        backTexNum = 1;
                        for (int i = 0; i < 10; i++)
                        {
                            fish[i].Initialize(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
                        }
                        lastHighScore = loadScore();
                    }
                          
                    else if (menu.contactRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                     {
                         if (timer >= 20)
                         {
                        timer = 0;
                            theGameState = gameState.contactPage;
                            
                        }
                    }
                    else if (menu.leadRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                    {
                        if (timer >= 20)
                        {
                           
                            timer = 0;
                            theGameState = gameState.leaderBoard;
                        }
                    }
                    else if (menu.quitRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                    {
                        if (timer >= 10)
                        {
                            this.Exit();
                            timer = 0;
                        }
                    }   
                }
                pause = false;
            }

            if (theGameState == gameState.gamePlay)
            {
                if (gameOver) 
                {
                    saveScore(info.Score);
                }

                if (pause)
                {
                    foreach (TouchLocation tl in TouchPanel.GetState())
                    {
                        timer = 0;
                       
                        if (menu.playRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                        {
                            pause = false;
                        }
                        else if (menu.quitRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                        {
                            theGameState = gameState.menu;
                        }
                    }
                }
            }


            if (theGameState == gameState.contactPage || theGameState == gameState.leaderBoard)
            {
                timer = 0;
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
                    theGameState = gameState.menu;
                }
            }
        }

        public void UpdateButtons()
        {
            foreach (TouchLocation tl in TouchPanel.GetState())
            {
                if (tl.State == TouchLocationState.Released)
                {
                    up = false; down = false; left = false; right = false;
                }
                else
                {
                    if (upRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                    { up = true; }

                    else if (downRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                    { down = true; }

                    if (leftRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                    { left = true; }

                    else if (rightRec.Contains((int)tl.Position.X, (int)tl.Position.Y))
                    { right = true; }
                }
            }
        }

        public void UpdateDifficulty()
        {
            for (int i = 0; i < 10; i++)
            {
                if (info.Score >= 800 && info.Score < 1600)
                {
                    backTexNum = 2;
                    fish[i].TopSpeed = 5;
                }
                if (info.Score >= 1600 && info.Score < 4000)
                {
                    backTexNum = 3;
                    fish[i].TopSpeed = 6;
                }
                if (info.Score >= 4000 && info.Score < 6000)
                {
                    backTexNum = 4;
                    fish[i].TopSpeed = 7;
                }
                if (info.Score >= 6000 && info.Score < 10000)
                {
                    backTexNum = 5;
                    fish[i].TopSpeed = 8;
                }
                if (info.Score >= 10000 && info.Score < 12000)
                {
                    backTexNum = 6;
                    fish[i].TopSpeed = 9;
                }
                if (info.Score >= 12000)
                {
                    backTexNum = 7;
                    fish[i].TopSpeed = 10;
                }
            }
        }

        public void Collision()
        {
            for (int i = 0; i < 10; i++)
            {
                if (shark.Rec.Intersects(fish[i].Rec))
                {
                    
                    shark.sndChew.Play();
                    shark.Animate = true;
                    info.Score += (fish[i].Speed * 10);
                    fish[i].Initialize(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
                }
            }
        }

        public void EndGame()
        {
            pause = true;
            gameOver = true;
            info.EndGame = true;
        }

        private void saveScore(int score)
        {
            if (score > Convert.ToInt16(highScore))
            {
                highScore = score.ToString();
                IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication(); // grab the storage
                FileStream stream = store.OpenFile(currentFile, FileMode.Create); // Open a file in Create mode
                FileStream stream2 = store.OpenFile(currentName, FileMode.Create); // Open a file in Create mode
                BinaryWriter writer = new BinaryWriter(stream);
                BinaryWriter writer2 = new BinaryWriter(stream2);
                writer.Write(highScore);
                if (playerName == null) { playerName = "N/A"; }
                writer2.Write(playerName);
                writer.Close();
                writer2.Close();
            }
        }
 
        private int loadScore()
        {
          //Called before Save whenever game ends
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

            if (store.FileExists("SaveFileSharkAttack.txt") && store.FileExists("SaveNameSharkAttack.txt")) // Check if file exists
            {
                IsolatedStorageFileStream save = new IsolatedStorageFileStream("SaveFileSharkAttack.txt", FileMode.Open, store);
                IsolatedStorageFileStream save2 = new IsolatedStorageFileStream("SaveNameSharkAttack.txt", FileMode.Open, store);
                BinaryReader reader = new BinaryReader(save);
                BinaryReader reader2 = new BinaryReader(save2);
                save.Position = 0;
                save2.Position = 0;
                highScore = reader.ReadString();
                name = reader2.ReadString();

                reader.Close();
                reader2.Close();
            }
            else { highScore = "0"; name = "Michael :)"; }
            return Convert.ToInt16(highScore);
        
        }

        public void DrawBackground()
        {
            if (backTexNum == 1)
            {
                spriteBatch.Draw(back1, backRec, Color.AntiqueWhite);
            }
            else if (backTexNum == 2)
            {
                spriteBatch.Draw(back2, backRec, Color.AntiqueWhite);
            }
            else if (backTexNum == 3)
            {
                spriteBatch.Draw(back3, backRec, Color.AntiqueWhite);
            }
            else if (backTexNum == 4)
            {
                spriteBatch.Draw(back4, backRec, Color.AntiqueWhite);
            }
            else if (backTexNum == 5)
            {
                spriteBatch.Draw(back5, backRec, Color.AntiqueWhite);
            }
            else if (backTexNum == 6)
            {
                spriteBatch.Draw(back6, backRec, Color.AntiqueWhite);
            }
            else
            {
                spriteBatch.Draw(back7, backRec, Color.AntiqueWhite);
            }

        }

        public void SetRecs()
        {
            backRec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            upRec = new Rectangle(60, GraphicsDevice.Viewport.Height - 50,50,50);
            downRec = new Rectangle(0, GraphicsDevice.Viewport.Height - 50, 50, 50);
            leftRec = new Rectangle(GraphicsDevice.Viewport.Width - 110, GraphicsDevice.Viewport.Height - 50, 50, 50);
            rightRec = new Rectangle(GraphicsDevice.Viewport.Width - 50, GraphicsDevice.Viewport.Height - 50, 50, 50);
        }

        protected override void Draw(GameTime gameTime)
        {  
            SetRecs();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (theGameState == gameState.gamePlay)
            {
                DrawBackground();

                shark.Draw(spriteBatch);
                // blood.Draw(gameTime,spriteBatch,Color.AntiqueWhite);
                for (int i = 0; i < 10; i++)
                {
                    fish[i].Draw(spriteBatch);
                }

                if (pause && !gameOver)
                {
                    menu.Draw(spriteBatch);
                }
                else if (gameOver)
                {
                    menu.GameOver(spriteBatch);
                }
                spriteBatch.Draw(upTex, upRec, Color.AntiqueWhite);
                spriteBatch.Draw(downTex, downRec, Color.AntiqueWhite);
                spriteBatch.Draw(leftTex, leftRec, Color.AntiqueWhite);
                spriteBatch.Draw(rightTex, rightRec, Color.AntiqueWhite);

                info.Draw(spriteBatch, spriteFont);
                newHighRec = new Rectangle(newHighRec.X, newHighRec.Y, (10 * newHighRec.X)/5, (7 * newHighRec.Y)/5);
                if (newHigh)
                {
                    spriteBatch.Draw(newHighTex, newHighRec, Color.AntiqueWhite);
                }
            }
            else if (theGameState == gameState.menu)
            {
                spriteBatch.Draw(back1, backRec, Color.AntiqueWhite);
                menu.Draw(spriteBatch);
            }
            else if (theGameState == gameState.contactPage)
            {
                spriteBatch.Draw(contactImage, backRec, Color.AntiqueWhite);
            }
            else if (theGameState == gameState.leaderBoard)
            {
                spriteBatch.Draw(leaderBack, backRec, Color.AntiqueWhite);
                info.DrawLeadBd(spriteBatch, spriteFont, loadScore());
                info.DrawName(spriteBatch, spriteFont, name);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
