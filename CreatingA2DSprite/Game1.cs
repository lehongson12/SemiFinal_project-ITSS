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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Animation;
using Object;
using Screen;

namespace CreatingA2DSprite
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {   
                            /************* MAIN COMPONENT **********/
        GraphicsDeviceManager graphics;                                   // Game component //
        SpriteBatch spriteBatch;
        Building building;
        SpriteFont font;                                       // font to display text //
        Input input;                                      // Getsignal from keyboard //
                            /************* CAMERA *****************/

        Camera camera;                                                    // Camera follows Elevator's Sprite //
        public Vector2 Camera_pos;                                 // Camera's Position //
        public float Camera_zoom;                                // Zoom   //
        public int Cam_left_right;                             // Change camera's position to left or right     //

        private StartScreen startScreen;
        private SettingScreen settingScreen;
        private TutorialScreen tutorialScreen;
        private AboutScreen aboutScreen;
                            /************  MOUSE *******************/

        MouseState Mouse_st;                                   // Determine mouse click or not    //
        Point Mouse_pt;                                   // Determine mouse's position      //

                            /************  MUSIC *******************/
        public List<Song> Song_list;
        public static int Music_on, current_song;
        private int before;
        
        public static int screen = 0;   // 0 -> menu
                                    // 1 -> start
                                    // 2 -> setting
                                    // 3 -> tutorial
                                    // 4 -> about
                                    // 5 -> exit
                            /************ GAME'S STATE *************/

  
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 50);
            this.graphics.PreferredBackBufferWidth = 15 * Building.CELL_WIDTH;
            this.graphics.PreferredBackBufferHeight = 15 * Building.CELL_HEIGHT;
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

     
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
                        
                        // MAIN COMPONENT //
            font = Content.Load<SpriteFont>("font");
            building = new Building(this.Content, 10);
            input = new Input();
            camera = new Camera(GraphicsDevice.Viewport);

            /* Screen */
            startScreen = new StartScreen(this.Content);
            settingScreen = new SettingScreen(this.Content);
            tutorialScreen = new TutorialScreen(this.Content);
            aboutScreen = new AboutScreen(this.Content);

            screen = 0;

            /* Camera */
            Camera_zoom = 0.5f;
            Cam_left_right = 0;
            Camera_pos = new Vector2(7 * Building.CELL_WIDTH, 8 * Building.CELL_HEIGHT);
            
            /* Mouse */
            Mouse_st = new MouseState();
            Mouse_st = Mouse.GetState();
            Mouse_pt = new Point(Mouse_st.X, Mouse_st.Y);
           
            /* Music */
            this.Song_list = new List<Song>();

            Song_list.Add(Content.Load<Song>("Music/gangnam"));
            Song_list.Add(Content.Load<Song>("Music/euro"));
            Song_list.Add(Content.Load<Song>("Music/world_cup"));
            Song_list.Add(Content.Load<Song>("Music/world_cup_2"));
            Song_list.Add(Content.Load<Song>("Music/camera"));
            Music_on = 1;
            current_song = 0;
            
            MediaPlayer.Play(Song_list.ElementAt(4));
            
            base.Initialize();
           
        }
         
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
       
        
        protected override void Update(GameTime gameTime)
        {
          
            float elapsed;
            elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            /* Update Camera */
            if (screen == 1) Camera_pos = Building.position;
            else Camera_pos = new Vector2(0, 0);
            camera.Update(gameTime, this, Camera_pos, Camera_zoom, Cam_left_right);
                                    
            /* Update input and mouse*/
            Mouse_st = Mouse.GetState();
            Mouse_pt.X = Mouse_st.X;
            Mouse_pt.Y = Mouse_st.Y;

            input.Update();
                                 
            /* Xử lí tín hiệu từ bàn phím */
            /* Camera */
            if (input.Release(Keys.Left) && screen == 1)   
                if (Cam_left_right > 0) Cam_left_right = 0;
                else Cam_left_right -= 10;
            if (input.Release(Keys.Right) && screen == 1)
                if (Cam_left_right < 0) Cam_left_right = 0;
                else Cam_left_right += 10;
            if (input.Release(Keys.Z) && screen == 1) Camera_zoom += 0.05f;
            if (input.Release(Keys.W) && screen == 1)
            {
                Camera_zoom -= 0.05f;
                if (Cam_left_right < 0) Camera_zoom -= 0.05f;
            }
            /* Music */
            if (input.Release(Keys.S)) MediaPlayer.Pause();
            if (input.Release(Keys.R)) MediaPlayer.Resume();
            if (input.Release(Keys.U)) MediaPlayer.Volume += 0.05f;
            if (input.Release(Keys.L)) MediaPlayer.Volume -= 0.05f;
            if (input.Release(Keys.M))
                if (Music_on == 0) Music_on = 1;
                else Music_on = 0;
            if (input.Release(Keys.C) && screen == 1) MediaPlayer.Play(Song_list.ElementAt((++current_song) % 3));
            if (Music_on == 1) MediaPlayer.IsMuted = false;
            else MediaPlayer.IsMuted = true;

            if (input.Release(Keys.E) && screen == 0) this.Exit();
            if (input.Release(Keys.P) && Building.PAUSE == 0) Building.PAUSE = 1;

            if (Building.EXIT == 1) screen = 0;

            /* Screen */
            switch (screen)
            {
                case 0:
                    building.Console_system.Visible = false;
                    if (this.before == 1) MediaPlayer.Play(Song_list.ElementAt(0));
                    before = 0;
                    if (!startScreen.Appear()) this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 5);
                    else
                    {
                        building.Init();
                        this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 50);
                        startScreen.Render(Mouse_pt, Mouse_st);
                    }   
                    break;
                case 1:
                    startScreen.Reset();
                    building.Render(this.Content, elapsed, font, spriteBatch);
                    building.Console_system.Visible = true;
                    if (this.before == 0)
                    {
                        MediaPlayer.Play(Song_list.ElementAt(current_song));
                        this.before = 1;
                    }
                    else MediaPlayer.Resume();
                    break;
                case 2:
                    before = 0;
                    building.Init();
                    startScreen.Reset();
                    settingScreen.Render(Mouse_pt, Mouse_st);
                    break;
                case 3:

                    before = 0;
                    building.Init();
                    startScreen.Reset();
                    tutorialScreen.Render(Mouse_pt, Mouse_st);
                    break;
                case 4:
                    before = 0;
                    building.Init();
                    startScreen.Reset();
                    aboutScreen.Render(Mouse_pt, Mouse_st);
                    break;
                case 5:
                    this.Exit();
                    break;
            }
            
            base.Update(gameTime);
        }

       
        protected override void Draw(GameTime gameTime)
        {
          
            graphics.GraphicsDevice.Clear(Color.YellowGreen);
           
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                              null,
                              null,
                              null,
                              null,
                              camera.transform);

            switch (screen)
            {
                case 0: 
                    startScreen.Draw(spriteBatch);
                    break;
                case 1:
                    building.Draw(spriteBatch, font);
                    break;
                case 2:
                    settingScreen.Draw(spriteBatch);
                    break;
                case 3:
                    tutorialScreen.Draw(spriteBatch);
                    break;
                case 4:
                    aboutScreen.Draw(spriteBatch);
                    break;
                case 5:
                    startScreen.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
