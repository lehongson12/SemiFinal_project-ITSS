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
    class TutorialScreen
    {
        private Sprite tutorialBanner;
        private Sprite backButton;

        public TutorialScreen(ContentManager theContentManager)
        {
            tutorialBanner = new Sprite(theContentManager, -15, 0, 1, 15, "Object_start/banner2");
            backButton = new Sprite(theContentManager, Building.CELL_WIDTH, 12 * Building.CELL_HEIGHT, 2, 15, "Object_start/back");

            tutorialBanner.setWidthDraw(15 * Building.CELL_WIDTH + 25);
            tutorialBanner.setHeightDraw(15 * Building.CELL_WIDTH + Building.CELL_WIDTH / 2);
            tutorialBanner.setCurrentFrame(0);

            backButton.setWidthDraw(2 * Building.CELL_WIDTH);
            backButton.setHeightDraw(Building.CELL_HEIGHT);
            backButton.setCurrentFrame(0);
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
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            tutorialBanner.DrawFrame(theSpriteBatch, tutorialBanner.getCurrentFrame(), false);
            backButton.DrawFrame(theSpriteBatch, backButton.getCurrentFrame(), false);
        }
    }
}
