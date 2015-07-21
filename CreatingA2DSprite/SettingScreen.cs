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
using CreatingA2DSprite;

namespace Screen
{
    class SettingScreen
    {
        private Sprite settingBanner;
        public  Sprite soundCheck;
        private Sprite bgCheck;
        private Sprite backButton;
        
        
        public static Rectangle soundOn;
        public static Rectangle soundOff;
        public Rectangle background1;
        public Rectangle background2;
        public Rectangle background3;

        public SettingScreen(ContentManager theContentManager)
        {
            settingBanner = new Sprite(theContentManager, -15, 0, 1, 15, "Object_start/banner1");
            soundCheck = new Sprite(theContentManager, 0, 0, 1, 15, "Object_start/ok");
            bgCheck = new Sprite(theContentManager, 0, 0, 1, 15, "Object_start/ok");
            backButton = new Sprite(theContentManager, Building.CELL_WIDTH, 12 * Building.CELL_HEIGHT, 2, 15, "Object_start/back");

            settingBanner.setCurrentFrame(0);
            settingBanner.setWidthDraw(15 * Building.CELL_WIDTH + 25);
            settingBanner.setHeightDraw(15 * Building.CELL_HEIGHT + Building.CELL_HEIGHT / 2);

            backButton.setCurrentFrame(0);
            backButton.setWidthDraw(2 * Building.CELL_WIDTH);
            backButton.setHeightDraw(Building.CELL_HEIGHT);

            soundCheck.setCurrentFrame(0);
            bgCheck.setCurrentFrame(0);
            /* Tao cac hinh chu nhat nua */
            soundOn = new Rectangle (6 * Building.CELL_WIDTH - 33  , 7 * Building.CELL_HEIGHT - 27, 30, 44);
            soundOff = new Rectangle(8 * Building.CELL_WIDTH - 5 , 7 * Building.CELL_HEIGHT - 27, 30, 44);
        }

        public void Render(Point mousePoint, MouseState mouseState)
        {
            if (backButton.getBound().Contains(mousePoint))
            {
                backButton.setCurrentFrame(1);
                backButton.setWidthDraw(4 * Building.CELL_WIDTH);
                backButton.setHeightDraw(Building.CELL_HEIGHT + 30);
                backButton.setX(0);
                if (mouseState.LeftButton == ButtonState.Pressed) Game1.screen = 0;
            }
            else
            {
                backButton.setCurrentFrame(0);
                backButton.setWidthDraw(2 * Building.CELL_WIDTH);
                backButton.setHeightDraw(Building.CELL_HEIGHT);
                backButton.setX(Building.CELL_WIDTH);
            }
            if (soundOn.Contains(mousePoint) && mouseState.LeftButton == ButtonState.Pressed)
                Game1.Music_on = 1;
            if (soundOff.Contains(mousePoint) && mouseState.LeftButton == ButtonState.Pressed)
                Game1.Music_on = 0;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            settingBanner.DrawFrame(theSpriteBatch, settingBanner.getCurrentFrame(), false);
            backButton.DrawFrame(theSpriteBatch, backButton.getCurrentFrame(), false);
            soundCheck.DrawFrame(theSpriteBatch, soundCheck.getCurrentFrame(), false);
            bgCheck.DrawFrame(theSpriteBatch, bgCheck.getCurrentFrame(), false);
            if (Game1.screen == 2)
            {
                if (Game1.Music_on == 1)
                    theSpriteBatch.Draw(soundCheck.mSpriteTexture, soundOn, Color.Violet);
                if (Game1.Music_on == 0)
                    theSpriteBatch.Draw(soundCheck.mSpriteTexture, soundOff, Color.Violet);
            }
        }
    }
}
