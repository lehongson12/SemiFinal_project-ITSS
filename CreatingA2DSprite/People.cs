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
    public class People : Sprite 
    {
        /* Cac trang thai cuar nguoi*/
        public static int PEOPLE_IS_GOING = 0;          
        public static int PEOPLE_IS_COMING_IN = 1;
        public static int PEOPLE_IS_GOING_OUT = 2;
        public static int PEOPLE_IS_WAITING = 3;
        public static int PEOPLE_IS_IN_ELEVATOR = 4;

        public Boolean isLeft;              // true -> dang quay mat ve ben trai; false -> dang quay mat ve ben phai
        public int type;                    // 0 -> nguoi lon; 1 -> tre con
        public int sex;                     // 0 -> nam; 1 -> nu; 2 -> gay; 3 -> les
        public int weight;                  // trong luong cua nguoi
        public int startFloor;            // Tang ma nguoi dang dung
        public int wantFloor;               // Tang ma nguoi muon di den
        public int state;                   // Trang thai cua nguoi ( 1 trong 4 trang thai static )
        public int speed;                   // Toc do di
        public int waitNumber;              // Số thứ tự chờ đợi 
        public int waitPos;                 // Vị trí chờ
        public int OD;                      // Số thứ tự của người trong mô phỏng
        public int flag;                    // * flag = 0 -> chua va cham voi thang may lan nao
                                            // * flag = 1 -> đã va chạm với thang máy 1 lần và đi đến điểm chờ
                                            // * flag = 2 -> đang ở điểm chờ, chờ được thang máy phục vụ
                                            // * flag = 3 -> đang hoặc đã sử dụng thang máy

        public int countOut = 0;
        public int countIn = 0;
        public int aConst;              //  1 trong cac gia tri 7 -> 15

        public People(ContentManager theContentManager, int x, int y, int numberFrame, int framePerSecond, string theAssetName, 
                        int type, int sex, int startFloor, int wantFloor, int speed, int initFrame) :
            base(theContentManager, x, y, numberFrame, framePerSecond, theAssetName)
        {
            this.setType(type);
            this.setSex(sex);
            this.setStartFloor(startFloor);
            this.setWantFloor(wantFloor);
            this.setCurrentFrame(initFrame);
            this.setSpeed(speed);
            this.setFlag(0);
            this.setWaitNumber(-1);
        }

        /********************************************/
        /* Get các thuộc tính*/
        public int getFlag()
        {
            return this.flag;
        }

        public int getType()
        {
            return this.type;
        }

        public int getState()
        {
            return this.state;
        }

        public Boolean getIsLeft()
        {
            return this.isLeft;
        }

        public int getStartFloor()
        {
            return this.startFloor;
        }

        public int getWantFloor()
        {
            return this.wantFloor;
        }

        public int getSex()
        {
            return this.sex;
        }

        public int getSpeed()
        {
            return this.speed;
        }

        public Boolean wantGoUp()
        {
            return this.getStartFloor() < this.getWantFloor() ? true : false;
        }

        public int getWaitNumber()
        {
            return this.waitNumber;
        }

        public int getWaitPos()
        {
            return this.waitPos;
        }

        public int getOD()
        {
            return this.OD;
        }

        /***************************************/
        /* Set các thuộc tính*/
        public void setFlag(int theFlag)
        {
            this.flag = theFlag;
        }

        public void setType(int theType)
        {
            this.type = theType;
        }

        public void setState(int theState)
        {
            this.state = theState;
        }


        public void setStartFloor(int theStartFloor)
        {
            this.startFloor = theStartFloor;
        }


        public void setWantFloor(int theWantFloor)
        {
            this.wantFloor = theWantFloor;
        }

        public void setIsLeft(Boolean isLeft)
        {
            this.isLeft = isLeft;
        }

        public void setSex(int theSex)
        {
            this.sex = theSex;
        }

        public void setSpeed(int theSpeed)
        {
            this.speed = theSpeed;
        }

        public void setWaitNumber(int theWaitNumber)
        {
            this.waitNumber = theWaitNumber;
        }

        public void setWaitPos(int theWaitPos)
        {
            this.waitPos = theWaitPos;
        }

        public void setOD(int theOD)
        {
            this.OD = theOD;
        }

        /********************************************************/
        /********************************************************/
        public void GoLeft()
        {
            if (!this.getIsLeft()) this.setIsLeft(true);
            this.setX(this.getX() - this.speed);
        }

        public void GoRight()
        {
            if (this.getIsLeft()) this.setIsLeft(false);
            this.setX(this.getX() + this.speed);
        }

        public void ComeIn()
        {
            if (this.getIsLeft()) this.setX(this.getX() - aConst);
            else this.setX(this.getX() + aConst);
        }

        public void GoOut()
        {/*
            if (this.getIsLeft()) this.setX(this.getX() - aConst);
            else this.setX(this.getX() + aConst);
          */
        }

        public Boolean IsNearElevator(Elevator elevator)
        {
            if (this.getX() <= elevator.getX() - Elevator.MAX * Building.CELL_WIDTH) return false;
            if (this.getX() >= elevator.getX() + elevator.getWidthDraw() + Elevator.MAX * Building.CELL_WIDTH) return false;
            return true;
        }

        public void UpdateFrame(float elapsed)
        {
            if (this.getState() == PEOPLE_IS_GOING) this.LoopFrameAToFrameB(0, this.numberFrame / 3 - 1);
            if (this.getState() == PEOPLE_IS_WAITING) this.setCurrentFrame(0);
            if (this.getState() == PEOPLE_IS_COMING_IN) 
                if (this.getCurrentFrame() < this.numberFrame / 3) this.setCurrentFrame(this.numberFrame / 3);
                else
                    if (this.getCurrentFrame() < this.numberFrame * 2 / 3) this.setCurrentFrame(this.currentFrame + 1);
                    else
                        if (this.getCurrentFrame() == this.numberFrame * 2 / 3)
                        {
                            //this.setCurrentFrame(36);
                            this.setState(PEOPLE_IS_IN_ELEVATOR);
                        }

            if (this.getState() == PEOPLE_IS_GOING_OUT)
                if (this.getCurrentFrame() >= this.numberFrame * 2 / 3 && this.getCurrentFrame() < this.numberFrame) this.setCurrentFrame(this.getCurrentFrame() + 1);
                else
                    if (this.getCurrentFrame() == this.numberFrame)
                    {
                        this.setCurrentFrame(0);
                        this.setState(PEOPLE_IS_GOING);
                    }

            if (this.getState() == PEOPLE_IS_IN_ELEVATOR)
                this.setCurrentFrame(this.numberFrame * 2 / 3);
        }

        public void GotoPoint(int DesX, Elevator elevator)
        {
            if (this.getX() < DesX) this.setIsLeft(false);
            else
                if (this.getX() > DesX) this.setIsLeft(true);
                else
                {
                    this.setState(PEOPLE_IS_WAITING);
                    if (this.getX() < elevator.getX()) this.setIsLeft(false);
                    else this.setIsLeft(true);
                }
        }

        public void Render()
        {
            if (this.getState() == PEOPLE_IS_GOING)
            {
                if (this.getIsLeft()) this.GoLeft();
                else this.GoRight();
                this.setWidthDraw(Building.CELL_WIDTH);
                this.setHeightDraw(Building.CELL_HEIGHT * 3);
            }

            if (this.getState() == PEOPLE_IS_COMING_IN)
            {
                if (this.getCurrentFrame() != 36)
                {
                    this.ComeIn();
                    this.setWidthDraw(this.getWidthDraw() - 1);
                    this.setHeightDraw(this.getHeightDraw() - 5);
                }
            }

            if (this.getState() == PEOPLE_IS_GOING_OUT)
            {
                if (this.getCurrentFrame() != 0)
                {
                    this.GoOut();
                    this.setWidthDraw(this.getWidthDraw() + 1);
                    this.setHeightDraw(this.getHeightDraw() + 5);
                }
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            this.DrawFrame(theSpriteBatch, this.getCurrentFrame(), this.getIsLeft());
        }

        public void Infor()
        {
            System.Console.WriteLine("X : " + this.getX() + "\tY : " + this.getY());
        }
    }
}
