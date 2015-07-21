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
    public class Elevator : Sprite
    {
        public static int ELEVATOR_IS_GOING_UP = 0;
        public static int ELEVATOR_IS_GOING_DOWN = 1;
        public static int ELEVATOR_IS_WAITING = 2;

        public static int ELEVATOR_WIDTH = 3 * Building.CELL_WIDTH;
        public static int ELEVATOR_HEIGHT = 4 * Building.CELL_HEIGHT;

        public static int MAX = 3;

        public int maxPeople;                               // So nguoi toi da thang may co the cho duoc
        public int currentNumberOfPeople;                   // So nguoi hien tai co trong thang may
        public int state;                                   // Trang thai cua thang may ( 1 trong 4 trang thai static )
        public int beforeState;
        public int speed;                                   // Toc do di cua thang may
        public int currentFloor;                            // Tang hien tai cua thang may
        public Boolean isGoUpNext;                          // Tiep theo thang may se di len hay di xuong


        public Elevator(ContentManager theContentManager, int x, int y, int numberFrame, int framePerSecond, string theAssetName,
                        int maxPeople, int maxWeight, int speed) :
            base(theContentManager, x, y, numberFrame, framePerSecond, theAssetName)
        {
            this.maxPeople = maxPeople;
            this.speed = speed;
            this.currentFloor = 10;
            this.state = ELEVATOR_IS_WAITING;
            this.beforeState = ELEVATOR_IS_WAITING;
        }

        public int getBeforeState()
        {
            return this.beforeState;
        }

        public int getSpeed()
        {
            return this.speed;
        }

        public int getState()
        {
            return this.state;
        }

        public int getCurrentFloor()
        {
            return this.currentFloor;
        }

        public int getMaxPeople()
        {
            return this.maxPeople;
        }

        public void setBeforeState(int theState)
        {
            this.beforeState = theState;
        }

        public void setMaxPeople(int theMaxPeople)
        {
            this.maxPeople = theMaxPeople;
        }

        public void setCurrentFloor(int theCurrentFloor)
        {
            this.currentFloor = theCurrentFloor;
        }

        public void setState(int theState)
        {
            this.state = theState;
        }

        public void setSpeed(int theSpeed)
        {
            this.speed = theSpeed;
        }

        public void setGoUpNext(Boolean theGoUpNext)
        {
            this.isGoUpNext = theGoUpNext;
        }

        /**************************************/
        public void GoUp()
        {
            if (this.state != ELEVATOR_IS_GOING_UP) this.state = ELEVATOR_IS_GOING_UP;
            this.setY(this.y -= this.speed);
        }

        public void GoDown()
        {
            if (this.state != ELEVATOR_IS_GOING_DOWN) this.state = ELEVATOR_IS_GOING_DOWN;
            this.setY(this.y += this.speed);
        }

        public void Wait()
        {

        }

        public void IncNumOfPeople()
        {
            if (this.currentNumberOfPeople >= this.maxPeople) this.Warning("");
            else this.currentNumberOfPeople++;
        }

        public void DecNumOfPeople()
        {
            this.currentNumberOfPeople--;
        }


        public void Warning(string warning)
        {
            
        }

        public Boolean AcceptPeople(People people)
        {
            if (this.getCurrentFloor() == people.getStartFloor() &&
                this.getState() == ELEVATOR_IS_WAITING) return true;
            return false;
        }

        public void Render()
        {
            if (this.getState() == ELEVATOR_IS_GOING_UP) this.GoUp();
            if (this.getState() == ELEVATOR_IS_GOING_DOWN) this.GoDown();
        }
    }
}
