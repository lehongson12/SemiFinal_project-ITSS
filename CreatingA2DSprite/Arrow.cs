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
using Animation;


namespace Object
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Arrow : Sprite
    {
        public static int UP_ANIMATION = 0;
        public static int DOWN_ANIMATION = 1;
        public static int NO_ANIMATION = 2;

        public int state;

        public Arrow(ContentManager theContentManager, int x, int y, int numberFrame, int framePerSecond, string theAssetName,
                    int initFrame) :
            base(theContentManager, x, y, numberFrame, framePerSecond, theAssetName)
        {
            //this.setCurrentFrame(initFrame);
            this.setState(UP_ANIMATION);
        }

        public int getState()
        {
            return this.state;
        }

        public void setState(int theState)
        {
            this.state = theState;
        }

        public void UpdateFrame(float elapsed)
        {
            switch (this.getState())
            {
                case 0:
                    this.LoopFrameAToFrameB(0, 2);
                    break;
                case 1:
                    this.LoopFrameAToFrameB(3, 5);
                    break;
                case 2:
                    this.setCurrentFrame(6);
                    break;
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            this.DrawFrame(theSpriteBatch, this.getCurrentFrame(), true);
        }
    }
}
