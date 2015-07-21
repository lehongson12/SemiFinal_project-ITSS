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
    public class ElevatorDoor : Sprite
    {
        public static int DOOR_IS_OPENING = 0;
        public static int DOOR_IS_OPENED = 1;
        public static int DOOR_IS_CLOSING = 2;
        public static int DOOR_IS_CLOSED = 3;

        public int state;

        public ElevatorDoor(ContentManager theContentManager, int x, int y, int numberFrame, int framePerSecond, string theAssetName, int state) :
            base (theContentManager, x, y, numberFrame, framePerSecond, theAssetName)
        {
            this.state = state;
        }

        public int getState()
        {
            return this.state;
        }

        public void setState(int theState)
        {
            this.state = theState;
        }

        public void UpdateFrame(float Elapsed)
        {
            switch (this.getState())
            {
                case 0: // DOOR_IS_OPENING
                    this.GotoAndStopFrame(8);
                    if (this.getCurrentFrame() == 8) this.setState(DOOR_IS_OPENED);
                    break;
                case 1: // DOOR_IS_OPENED
                    this.setCurrentFrame(8);
                    break;
                case 2: // DOOR_IS_CLOSING
                    this.GotoAndStopFrameBack(0);
                    if (this.getCurrentFrame() == 0) this.setState(DOOR_IS_CLOSED);
                    break;
                case 3: // DOOR_IS_CLOSED
                    this.setCurrentFrame(0);
                    break;
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            this.DrawFrame(theSpriteBatch, this.getCurrentFrame(), true);
        }
    }
}
