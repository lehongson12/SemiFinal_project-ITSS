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
    class StartScreen
    {
        public Sprite menu;
        public Sprite startButton;
        public Sprite settingButton;
        public Sprite tutorialButton;
        public Sprite aboutButton;
        public Sprite exitButton;
        private int speed = 10;

        public StartScreen(ContentManager theContentManager)
        {
            menu = new Sprite(theContentManager, -15, 0, 1, 15, "Object_start/Menu_banner");
            startButton = new Sprite(theContentManager, 5 * Building.CELL_WIDTH + 30, 17 * Building.CELL_HEIGHT, 2, 15, "Object_start/start");
            settingButton = new Sprite(theContentManager, 5 * Building.CELL_WIDTH + 30, 17 * Building.CELL_HEIGHT, 2, 15, "Object_start/setting");
            tutorialButton = new Sprite(theContentManager, 5 * Building.CELL_WIDTH + 30, 17 * Building.CELL_HEIGHT, 2, 15, "Object_start/tutorial");
            aboutButton = new Sprite(theContentManager, 5 * Building.CELL_WIDTH + 30, 17 * Building.CELL_HEIGHT, 2, 15, "Object_start/about");
            exitButton = new Sprite(theContentManager, 5 * Building.CELL_WIDTH + 30, 17 * Building.CELL_HEIGHT, 2, 15, "Object_start/exit");

            menu.setCurrentFrame(0);
            startButton.setCurrentFrame(0);
            settingButton.setCurrentFrame(0);
            tutorialButton.setCurrentFrame(0);
            aboutButton.setCurrentFrame(0);
            exitButton.setCurrentFrame(0);

            menu.setWidthDraw(15 * Building.CELL_WIDTH + 25);
            menu.setHeightDraw(15 * Building.CELL_HEIGHT + Building.CELL_HEIGHT / 2);
            startButton.setWidthDraw(4 * Building.CELL_WIDTH);
            startButton.setHeightDraw(Building.CELL_HEIGHT);
            settingButton.setWidthDraw(4 * Building.CELL_WIDTH);
            settingButton.setHeightDraw(Building.CELL_HEIGHT);
            tutorialButton.setWidthDraw(4 * Building.CELL_WIDTH);
            tutorialButton.setHeightDraw(Building.CELL_HEIGHT);
            aboutButton.setWidthDraw(4 * Building.CELL_WIDTH);
            aboutButton.setHeightDraw(Building.CELL_HEIGHT);
            exitButton.setWidthDraw(4 * Building.CELL_WIDTH);
            exitButton.setHeightDraw(Building.CELL_HEIGHT);
        }

        public Boolean Appear()
        {
            if (startButton.getY() > 4 * Building.CELL_HEIGHT) { startButton.setY(startButton.getY() - this.speed); return false; }
            if (settingButton.getY() > 6 * Building.CELL_HEIGHT) { settingButton.setY(settingButton.getY() - this.speed); return false; }
            if (tutorialButton.getY() > 8 * Building.CELL_HEIGHT) { tutorialButton.setY(tutorialButton.getY() - this.speed); return false; }
            if (aboutButton.getY() > 10 * Building.CELL_HEIGHT) { aboutButton.setY(aboutButton.getY() - this.speed); return false; }
            if (exitButton.getY() > 12 * Building.CELL_HEIGHT) { exitButton.setY(exitButton.getY() - this.speed); return false; }
            return true;
        }

        public void Reset()
        {
            startButton.setY(17 * Building.CELL_HEIGHT);
            settingButton.setY(17 * Building.CELL_HEIGHT);
            tutorialButton.setY(17 * Building.CELL_HEIGHT);
            aboutButton.setY(17 * Building.CELL_HEIGHT);
            exitButton.setY(17 * Building.CELL_HEIGHT);
        }

        public void Render(Point mousePoint, MouseState mouseState, Sprite button, int screen)
        {
            if (button.getBound().Contains(mousePoint))
            {
                button.setCurrentFrame(1);
                button.setWidthDraw(6 * Building.CELL_WIDTH);
                button.setHeightDraw(Building.CELL_HEIGHT + 30);
                button.setX(4 * Building.CELL_WIDTH + 30);
                if (mouseState.LeftButton == ButtonState.Pressed) { Game1.screen = screen; MediaPlayer.Resume(); }
            }
            else
            {
                button.setCurrentFrame(0);
                button.setWidthDraw(4 * Building.CELL_WIDTH);
                button.setHeightDraw(Building.CELL_HEIGHT);
                button.setX(5 * Building.CELL_WIDTH + 30);
            }
        }

        public void Render(Point mousePoint, MouseState mouseState)
        {
            Render(mousePoint, mouseState, startButton, 1);
            Render(mousePoint, mouseState, settingButton, 2);
            Render(mousePoint, mouseState, tutorialButton, 3);
            Render(mousePoint, mouseState, aboutButton, 4);
            Render(mousePoint, mouseState, exitButton, 5);
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            menu.DrawFrame(theSpriteBatch, menu.getCurrentFrame(), false);
            startButton.DrawFrame(theSpriteBatch, startButton.getCurrentFrame(), false);
            settingButton.DrawFrame(theSpriteBatch, settingButton.getCurrentFrame(), false);
            tutorialButton.DrawFrame(theSpriteBatch, tutorialButton.getCurrentFrame(), false);
            aboutButton.DrawFrame(theSpriteBatch, aboutButton.getCurrentFrame(), false);
            exitButton.DrawFrame(theSpriteBatch, exitButton.getCurrentFrame(), false);
        }
    }
}
