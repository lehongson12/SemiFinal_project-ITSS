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
using CreatingA2DSprite;

namespace Object
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    /// 

    public class Building
    {
        public static int CELL_WIDTH = 60;
        public static int CELL_HEIGHT = 60;
        public static int ELEVATOR_POSITION_X = 6;
        public static Vector2 position = new Vector2();
        public static int start = 0, pause = 0, exit = 0, random_people = 0;
        public static int START = 0;
        public static int PAUSE = 0;
        public static int EXIT = 0;
        public static int CREATE_RANDOM = 1;
        public static int CREATE = 0;
        public static int PEOPLE_TYPE = 0;
        public static int PEOPLE_WANT_FLOOR = 0;
        public static int PEOPLE_START_FLOOR = 0;

        public int numberFloor;                         // Số tầng của tòa nhà

        public List<People> peopleList;                 // Danh sách người trong mô phỏng
        public List<Arrow> arrowList;                   // Danh sách các mũi tên biểu thị trạng thái thang máy đang đi lên hay đi xuống
        public List<ElevatorDoor> doorList;             // Danh sách cửa thang máy ở mô phỏng, mỗi tầng có 1 cửa thang máy
        public Elevator elevator;                       // Thang máy
        public Sprite backGround;                       // Background
        public int[,] waitArray;                        // 
                                                        // waitArray[i,j]: ( i = 0->numberFloor-1, j = 0, 1)
                                                        // Số người chờ thang máy ở tâng i+1, mặt đang quay về trái( j = 0) hoặc phải ( j = 1)

        public int height;                              // Chiều cao tòa nhà ( tính bằng CELL)
        public int width;                               // Chiều rộng tòa nhà ( tính bằng CELL)
        public int time;                                // Thời gian để tạo người (trong trường hợp random create people)

        private Random random = new Random();
        private int before = 0;
        private int doorFlag = 0;                           // Cờ đánh dấu xem cửa có được mở hay ko
        private int numberOfPeople = 0;
      
        private List<QueueElement> queue;                   // Queue ưu tiên lưu trữ các lệnh gọi
        public Form1 Console_system;                               // Form thể hiện thông tin của mô phỏng
        DateTime now;                                       // Thời gian hiện tại
        

        public Building(ContentManager theContentManager, int numberFloor)
        {
            this.numberFloor = numberFloor;
            this.peopleList = new List<People>();
            this.arrowList = new List<Arrow>();
            this.doorList = new List<ElevatorDoor>();
            this.height = 5 * this.numberFloor + 1;
            this.width = 15;
            this.time = 0;
            this.queue = new List<QueueElement>();
            this.waitArray = new int[this.numberFloor, 2];
            Console_system = new Form1();

            for (int i = 0; i < this.numberFloor; i++)
            {
                this.waitArray[i, 0] = 0;
                this.waitArray[i, 1] = 0;
            }

            CreateBackGround(theContentManager);
            CreateDoor(theContentManager);
            CreateArrows(theContentManager);
            CreateElevator(theContentManager);
            /********************************/

            this.peopleList.Add(CreatePeople(theContentManager));
        }

        /* In ra tất cả các thông tin trong queue */
        public void PrintQueue()
        {
            System.Console.WriteLine("------------------------");
            System.Console.WriteLine("elevator.currentFloor: " + elevator.getCurrentFloor());
            System.Console.Write("elevator.state: ");
            if (elevator.getState() == Elevator.ELEVATOR_IS_GOING_DOWN) System.Console.WriteLine("Down");
            else
                if (elevator.getState() == Elevator.ELEVATOR_IS_GOING_UP) System.Console.WriteLine("Up");
                else System.Console.WriteLine("Waiting");
            System.Console.Write("elevator.beforeState: ");
            if (elevator.getBeforeState() == Elevator.ELEVATOR_IS_GOING_DOWN) System.Console.WriteLine("Down");
            else
                if (elevator.getBeforeState() == Elevator.ELEVATOR_IS_GOING_UP) System.Console.WriteLine("Up");
                else System.Console.WriteLine("Waiting");
            for (int i = 0; i < queue.Count; i++)
            {
                System.Console.WriteLine(i + 1 + " : Floor = " + queue.ElementAt(i).getFloor() + "\tFlag = " + queue.ElementAt(i).getFlag());
            }
            System.Console.WriteLine("------------------------");
        }

        public void Init()
        {
            for (int i = 0; i < peopleList.Count; i++) peopleList.RemoveAt(i);
            for (int i = 0; i < queue.Count; i++) queue.RemoveAt(i);
            elevator.setCurrentFloor(this.numberFloor);
            EXIT = 0;
            PAUSE = 0;
            START = 0;
        }

        /* Tạo 1 người ngẫu nhiên*/
        public People CreatePeople(ContentManager theContentManager)
        {
            int randomIsLeft;
            int numberFrame;
            String theAssetName;
            People people;

            randomIsLeft = random.Next();
            if (CREATE_RANDOM == 1)
            {
                PEOPLE_TYPE = random.Next();
                PEOPLE_START_FLOOR = random.Next(1, 10);
                do { PEOPLE_WANT_FLOOR = random.Next(1, 10); } while (PEOPLE_WANT_FLOOR == PEOPLE_START_FLOOR);
            }

            theAssetName = "Object_play/human" + PEOPLE_TYPE % 3;
            numberFrame = PEOPLE_TYPE % 3 == 0 ? 27 : (PEOPLE_TYPE % 3 == 1 ? 15 : 27);

            people = new People(theContentManager, 0, 0, numberFrame, 15, theAssetName, 1, 1, PEOPLE_START_FLOOR, PEOPLE_WANT_FLOOR, 10, 0);

            people.setY(((this.numberFloor - people.getStartFloor()) * 5 + 2) * CELL_HEIGHT + CELL_HEIGHT / 3);
            people.setIsLeft(randomIsLeft % 2 == 0 ? false : true);
            people.setX(randomIsLeft % 2 == 0 ? CELL_WIDTH / 2 : this.width * CELL_WIDTH - CELL_WIDTH / 2);
            people.setState(People.PEOPLE_IS_GOING);
            people.setWidthDraw(1 * CELL_WIDTH);
            people.setHeightDraw(3 * CELL_HEIGHT);
            people.setOD(++numberOfPeople);

            this.Console_system.Infor0.Text = Building.CREATE_RANDOM == 1 ? "Create Random" : "Customize";
            this.Console_system.Infor1.Text = "Start Floor: " + Building.PEOPLE_START_FLOOR;
            this.Console_system.Infor2.Text = "Want Floor: " + Building.PEOPLE_WANT_FLOOR;

            return people;
        }

        /* Tạo backGournd*/
        public void CreateBackGround(ContentManager theContentManager)
        {
            this.backGround = new Sprite(theContentManager, 0, -1 * CELL_HEIGHT, 1, 15, "Object_play/background");
            this.backGround.setWidthDraw(15 * CELL_WIDTH);
            this.backGround.setHeightDraw(52 * CELL_HEIGHT);
        }

        /* Tạo các mũi tên thông báo chiều di chuyển của thang máy*/
        public void CreateArrows(ContentManager theContentManager)
        {
            Arrow arrow;

            for (int i = 1; i <= this.numberFloor; i++)
            {
                arrow = new Arrow(theContentManager, ELEVATOR_POSITION_X * CELL_WIDTH - CELL_WIDTH * 2 / 3, (3 + (this.numberFloor - i) * 5) * CELL_HEIGHT, 7, 0, "Object_play/arrow", 6);
                arrow.setWidthDraw(CELL_WIDTH / 3);
                arrow.setHeightDraw(CELL_HEIGHT / 2);

                this.arrowList.Add(arrow);
            }
        }

        /* Tạo các cửa ở trên thang máy tại mỗi tầng*/
        public void CreateDoor(ContentManager theContentManager)
        {
            ElevatorDoor elevatorDoor;

            for (int i = 1; i <= this.numberFloor; i++)
            {
                elevatorDoor = new ElevatorDoor(theContentManager,
                    ELEVATOR_POSITION_X * CELL_WIDTH, (1 + (this.numberFloor - i) * 5) * CELL_HEIGHT, 9, 15, "Object_play/elevatorDoor", ElevatorDoor.DOOR_IS_CLOSED);
                elevatorDoor.setWidthDraw(3 * CELL_WIDTH);
                elevatorDoor.setHeightDraw(4 * CELL_HEIGHT);
                this.doorList.Add(elevatorDoor);
            }
        }

        /* Tạo thang máy*/
        public void CreateElevator(ContentManager theContentManager)
        {
            this.elevator = new Elevator(theContentManager, ELEVATOR_POSITION_X * CELL_WIDTH, CELL_HEIGHT, 1, 15, "Object_play/elevator", 20, 2000, 10);
            this.elevator.setWidthDraw(3 * CELL_WIDTH);
            this.elevator.setHeightDraw(4 * CELL_HEIGHT);
        }

        /**********************************************************/
        public void Render(ContentManager theContentManager, float elapsed , SpriteFont font , SpriteBatch thespriteBatch)
        {
            position.X = elevator.getX();
            position.Y = elevator.getY();
           
            if (START == 1 && PAUSE == 0)
            {
                RenderElevator(elapsed);
                if (CheckToCloseDoor())
                {
                    doorList.ElementAt(elevator.getCurrentFloor() - 1).setState(ElevatorDoor.DOOR_IS_CLOSING);
                    this.before = 1;
                }

                if (doorList.ElementAt(elevator.getCurrentFloor() - 1).getState() == ElevatorDoor.DOOR_IS_CLOSED)
                {
                    if (this.before == 1)
                        this.queue.RemoveAt(queue.Count - 1);
                    this.before = 2;
                }

                RenderDoorList(elapsed);
                RenderArrowList(elapsed);
                RenderPeople(elapsed);
                RemovePeople();
                this.time = (++this.time) % 200;
                if (CREATE_RANDOM == 1)
                    if (this.time == 0) this.peopleList.Add(CreatePeople(theContentManager));
                    else ;
                else if (CREATE == 1)
                {
                    CREATE = 0;
                    this.peopleList.Add(CreatePeople(theContentManager));
                }
            }

            now = DateTime.Now;
            Console_system.Display_Infomation(peopleList.Count,
                elevator.currentNumberOfPeople,
                elevator.getCurrentFloor(),
                elevator.getState(),
                elevator.getSpeed(), now);  // Update thông tin lên Form
        }

        /* Phương thức xử lí trạng thái của thang máy */
        public void RenderElevator(float elapsed)
        {
            /* Nếu trong queue ko có lệnh gọi nào thì thang máy ở trạng thái chờ */
            if (queue.Count == 0)
            {
                elevator.setState(Elevator.ELEVATOR_IS_WAITING);
                elevator.setBeforeState(Elevator.ELEVATOR_IS_WAITING);
                this.before = 0;
            }
            else
            {
                this.elevator.setCurrentFloor(this.numberFloor - (this.elevator.getY() / CELL_HEIGHT - 1) / 5);
                if (this.elevator.getBeforeState() == Elevator.ELEVATOR_IS_GOING_UP) SortQueueWhenElevatorGoUp();
                else
                    if (this.elevator.getBeforeState() == Elevator.ELEVATOR_IS_GOING_DOWN) SortQueueWhenElevatorGoDown();

                /*  Nếu tầng hiện tại của than máy nhỏ hơn tầng của lệnh gọi đầu tiên ở đầu ra của queue */
                if (elevator.getY() < GetYFromFloor(queue.ElementAt(queue.Count - 1).getFloor()))
                {
                    elevator.setBeforeState(Elevator.ELEVATOR_IS_GOING_DOWN);
                    elevator.setState(Elevator.ELEVATOR_IS_GOING_DOWN);               //  -> Thang máy chuyển sang trạng thái đi lên
                    doorFlag = 0;
                }
                else
                    /* Nếu tầng hiện tại của thang máy lớn hơn tầng của lệnh gọi đầu tiên ở đầu ra của queue */
                    if (elevator.getY() > GetYFromFloor(queue.ElementAt(queue.Count - 1).getFloor()))
                    {
                        elevator.setBeforeState(Elevator.ELEVATOR_IS_GOING_UP);
                        elevator.setState(Elevator.ELEVATOR_IS_GOING_UP);         //  -> Thang máy chuyển trạng thái sang đi xuống
                        doorFlag = 0;
                    }
                    else
                    {
                        if (elevator.getBeforeState() == Elevator.ELEVATOR_IS_GOING_UP)
                            if (queue.ElementAt(queue.Count - 1).getFlag() == false)
                                elevator.setBeforeState(Elevator.ELEVATOR_IS_GOING_DOWN);
                            else ;
                        else
                            if (elevator.getBeforeState() == Elevator.ELEVATOR_IS_GOING_DOWN)
                                if (queue.ElementAt(queue.Count - 1).getFlag() == true)
                                    elevator.setBeforeState(Elevator.ELEVATOR_IS_GOING_UP);
                                else ;
                            else
                                elevator.setBeforeState((queue.ElementAt(queue.Count - 1).getFlag() == true) ? Elevator.ELEVATOR_IS_GOING_UP : Elevator.ELEVATOR_IS_GOING_DOWN);

                        /* Trường hợp tầng của thang máy trùng với tầng của lệnh gọi đầu tiên ở đầu ra của queue */
                        if (elevator.getState() != Elevator.ELEVATOR_IS_WAITING)
                            elevator.setState(Elevator.ELEVATOR_IS_WAITING);        //  -> Thang máy chuyển sang trạng thái chờ

                        if (doorFlag == 0)
                        {
                            doorList.ElementAt(elevator.getCurrentFloor() - 1).setState(ElevatorDoor.DOOR_IS_OPENING);  //  -> Bắt đầu mở cửa của tầng mà thang máy đứng
                            doorFlag = 1;
                        }

                        if (doorList.ElementAt(elevator.getCurrentFloor() - 1).getState() == ElevatorDoor.DOOR_IS_OPENED)
                            if (queue.Count >= 2)
                                if (queue.ElementAt(queue.Count - 1).getFloor() == queue.ElementAt(queue.Count - 2).getFloor())
                                    queue.RemoveAt(queue.Count - 1);
                    }
            }

            elevator.Render();
        }

        /* Xác định trạng thái của people có trong mô phỏng*/
        public void RenderPeople(float elapsed)
        {
            foreach (People people in peopleList) Render1People(people, elapsed);
        }

        /* Xác định trạng thái của 1 đối  tượng people */
        public void Render1People(People people, float elapsed)
        {
            int t;
            Boolean theFlag;
            QueueElement element;

            if (people.getIsLeft()) t = 0;
            else t = 1;

            theFlag = people.wantGoUp();                        

            if (people.getFlag() == 0)                          //  Nếu ở tầng đang đứng, people chưa được va chạm với thang máy lần nào
            {
                if (people.CheckCollides(this.doorList.ElementAt(people.getStartFloor() - 1)))      //  nếu people và chạm với thang máy 
                {
                    people.setFlag(1);                                                              //  Thay đổi cờ -> đánh dấu people đã va chạm 1 lần với thang máy 
                    people.setWaitNumber(this.waitArray[people.getStartFloor() - 1, t]);
                    people.setWaitPos((people.getIsLeft() == true) ?
                        (ELEVATOR_POSITION_X + 3 + people.getWaitNumber()) * CELL_WIDTH :
                        (ELEVATOR_POSITION_X - 1 - people.getWaitNumber()) * CELL_WIDTH);           //  Xác định vị trí điểm chờ của people
                    this.waitArray[people.getStartFloor() - 1, t]++;                                
                    /* Thêm lệnh gọi vào queue nếu lệnh gọi chưa tồn tại*/
                    element = new QueueElement(people.getStartFloor(), theFlag);
                    if (!element.checkExist(queue)) queue.Insert(0, element);
                }
            }

            if (people.getFlag() == 1)                          // Nếu people đã va chạm 1 lần với thang máy và đang trên đường đi đến điểm chờ
                if (AcceptComeIn(people)) people.setFlag(2);                                        // Nếu people được phép vào thang máy -> thay đổi cờ -> đang chờ thang máy phục vụ
                else                                            // Ko được phép vào thang máy
                {
                    if (people.getX() != people.getWaitPos())                                       // people chưa đi đến vị trí xếp hàng chờ của mình -> đi đến vị trí xếp hàng chờ
                    {
                        if (people.getState() != People.PEOPLE_IS_GOING) 
                            people.setState(People.PEOPLE_IS_GOING);
                        people.GotoPoint(people.getWaitPos(), elevator);
                    }
                    else                                                                            // people đã đi đến điểm chờ
                    {
                        people.setIsLeft(people.getX() < elevator.getX() ? false : true);       
                        people.setState(People.PEOPLE_IS_WAITING);                                  // Set trạng thái đứng chờ
                        people.setFlag(2);                                                          // Thay đổi cờ -> đang chờ thang máy phục vụ
                    }
                }

            if (people.getFlag() == 2)                          //  Nếu people đang chờ thang máy phục vụ
                if (AcceptComeIn(people))                               //  Nếu người được phép vào trong thang máy
                {
                    elevator.IncNumOfPeople();                                      // Tăng số người có trong thang máy
                    people.setState(People.PEOPLE_IS_COMING_IN);                    // Set trạng thái đi vào trong
                    people.setFlag(3);                                              // Set cờ đang sử dụng thang máy
                    people.aConst = (!people.getIsLeft()) ?
                        random.Next(elevator.getX() - people.getX() + CELL_WIDTH, elevator.getX() + elevator.getWidthDraw() - people.getX() - CELL_WIDTH) / (people.numberFrame / 3) :
                        random.Next(people.getX() - elevator.getX() - elevator.getWidthDraw() + CELL_WIDTH, people.getX() - elevator.getX() - CELL_WIDTH) / (people.numberFrame / 3);
                    this.waitArray[people.getStartFloor() - 1, t]--;    
                    this.UpdateWaitNumber(people.getStartFloor(), people.getIsLeft());  
                }

            if (people.getFlag() == 3)                             //  Nếu people đang hoặc đã sử dụng thang máy
            {
                if (people.getState() == People.PEOPLE_IS_IN_ELEVATOR)  //  People ở trong thang máy
                {
                    /* people cùng tiến lên hoặc cùng xuống với thang máy*/
                    if (elevator.getState() == Elevator.ELEVATOR_IS_GOING_UP) people.setY(people.getY() - elevator.getSpeed());
                    else
                        if (elevator.getState() == Elevator.ELEVATOR_IS_GOING_DOWN) people.setY(people.getY() + elevator.getSpeed());

                    if (AcceptGoOut(people))                                //  people được phép ra khỏi thang máy
                    {
                        elevator.DecNumOfPeople();
                        people.setState(People.PEOPLE_IS_GOING_OUT);        //  people bắt đầu chuyển sang thang thái PEOPLE_IS_GOING_OUT
                    }

                    element = new QueueElement(people.getWantFloor(), theFlag);
                    if (!element.checkExist(queue)) queue.Insert(0, element);
                }
            }
            people.UpdateFrame(elapsed);
            people.Render();
        }

        /* Thay đổi trạng thái của door trong door List */
        public void RenderDoorList(float elapsed)
        {
            foreach (ElevatorDoor door in doorList)
                door.UpdateFrame(elapsed);
        }
        
        /* Thay đổi trạng thái của arror trong arrow List*/
        public void RenderArrowList(float elapsed)
        {
            int theFlag;
            if (elevator.getState() == Elevator.ELEVATOR_IS_WAITING) theFlag = Arrow.NO_ANIMATION;
            else
                if (elevator.getState() == Elevator.ELEVATOR_IS_GOING_UP) theFlag = Arrow.UP_ANIMATION;
                else theFlag = Arrow.DOWN_ANIMATION;

            foreach (Arrow arrow in arrowList)
            {
                arrow.setState(theFlag);
                arrow.UpdateFrame(elapsed);
            }
        }

        /* Update lại vị trí đứng chờ của các people đang đứng ở tầng theFloor và quay mặt về phía theLeft*/
        public void UpdateWaitNumber(int theFloor, Boolean theLeft)
        {
            int number = 0;
            foreach (People people in peopleList)
                if (people.getStartFloor() == theFloor &&
                    people.getIsLeft() == theLeft &&
                    (people.getState() == People.PEOPLE_IS_GOING ||
                    people.getState() == People.PEOPLE_IS_WAITING))
                    people.setWaitNumber(++number);
        }

        /* Check xem cửa ở tầng mà thang máy đang dừng đang ở trạng thái mở đã được phép đóng hay chưa
         * Cửa ở tầng thang máy đang ở được phép đóng khi 
         *      + Tất cả những people được phép vào thang máy đều đã ở trạng thái PEOPLE_IN_ELEVATOR
         *      + Tất cả những people được phép ra khỏi thang máy đều đã ở trạng thái PEOPLE_IS_GOING
         * true -> được phép đóng
         * false -> chưa được phép đóng
         */
        public Boolean CheckToCloseDoor()
        {
            foreach (People people in peopleList)
                if ((AcceptComeIn(people) && people.getState() != People.PEOPLE_IS_IN_ELEVATOR) ||
                    (AcceptGoOut(people) && people.getState() != People.PEOPLE_IS_GOING))
                    return false;

            if (doorList.ElementAt(elevator.getCurrentFloor() - 1).getState() != ElevatorDoor.DOOR_IS_OPENED)
                return false;

            return true;
        }

        /* Kiểm tra xem people có được phép vào trong thang máy hay ko
         * true -> có
         * false -> ko
         */
        public Boolean AcceptComeIn(People people)
        {
            if (people.getStartFloor() == elevator.getCurrentFloor() &&                                           //  Tầng hiện tại của thang máy bằng tầng của people đang đứng
                elevator.getState() == Elevator.ELEVATOR_IS_WAITING &&                                            //  Thang máy đang ở trạng thái chờ 
                doorList.ElementAt(elevator.getCurrentFloor() - 1).getState() == ElevatorDoor.DOOR_IS_OPENED)     //  Cửa thang máy ở tầng mà thang máy đang dừng đang ở trạng thái mở
            {
                if (queue.Count < 2) return true;
                
                if ((people.wantGoUp() == true && elevator.getBeforeState() != Elevator.ELEVATOR_IS_GOING_DOWN) ||
                    (people.wantGoUp() == false && elevator.getBeforeState() != Elevator.ELEVATOR_IS_GOING_UP))
                    return true;
            }

            return false;
        }

        /* Loại bỏ đi tất cả những người đã ko còn ở trong màn hình*/
        public void RemovePeople()
        {
            for (int i = 0; i < peopleList.Count; i++)
            {
                if (peopleList.ElementAt(i).getX() < -CELL_WIDTH ||
                    peopleList.ElementAt(i).getX() > 16 * CELL_WIDTH)
                    peopleList.RemoveAt(i);
            }
        }

        /* Kiểm tra xem people có được phép ra khỏi thang máy hay ko
         * true -> có
         * false -> ko
         */
        public Boolean AcceptGoOut(People people)
        {
            if (people.getWantFloor() == elevator.getCurrentFloor() &&                                                 //  Tâng hiện tại của thang máy là là tầng của people muốn ra
                people.getState() == People.PEOPLE_IS_IN_ELEVATOR &&                                                
                elevator.getState() == Elevator.ELEVATOR_IS_WAITING &&                                              //  Thang máy đang ở trạng thái dừng
                doorList.ElementAt(elevator.getCurrentFloor() - 1).getState() == ElevatorDoor.DOOR_IS_OPENED)      //  Cửa thang máy ở tầng mà thang máy đang dừng đang ở trạng thái mở
                return true;
            return false;
        }

        /* Trả lại giá trị Y của đỉnh trái trên của tầng theFloor */
        public int GetYFromFloor(int theFloor)
        {
            return ((this.numberFloor - theFloor) * 5 + 1) * CELL_HEIGHT;
        }

        /* Vẽ background, thang máy, người và các đối tượng khác*/
        public void Draw(SpriteBatch theSpriteBatch, SpriteFont font)
        {
            this.elevator.DrawFrame(theSpriteBatch, 0, true);                               //  Vẽ thang máy
            this.backGround.DrawFrame(theSpriteBatch, 0, false);                             //  Vẽ backGround
            foreach (People people in peopleList)
                if (people.getState() == People.PEOPLE_IS_IN_ELEVATOR)
                {
                    theSpriteBatch.DrawString(font, people.getOD() + "", new Vector2(people.getX() + CELL_WIDTH / 2, people.getY() - 10 ), Color.Red);
                    people.Draw(theSpriteBatch);                                            //  Vẽ các đối tượng người trong thang máy
                }

            foreach (Arrow arrow in arrowList) arrow.Draw(theSpriteBatch);                  //  Vẽ mũi tên
            foreach (ElevatorDoor door in doorList) door.Draw(theSpriteBatch);              //  Vẽ cửa thang máy tại các tầng
            foreach (People people in peopleList)
                if (people.getState() != People.PEOPLE_IS_IN_ELEVATOR)
                {
                    theSpriteBatch.DrawString(font, people.getOD() + "", new Vector2(people.getX() + CELL_WIDTH / 2, people.getY() - 20), Color.Red);
                    people.Draw(theSpriteBatch);                                            //  Vẽ các đối tượng người ko ở trong thang máy
                }
        }

        /* Sap xep queue theo uu tien khi thang may dang di xuong */
        public void SortQueueWhenElevatorGoDown()
        {
            int i = 0;
            int j = queue.Count - 1;
            int n;

            /* Sap xep cac phan tu trong queue thanh 2 phan
             *  1. Cac thanh phan gan dau ra se co chieu goi cung chieu voi chieu di chuyen va tang co nguoi goi do < tang cua thang may
             *  2. Cac thanh phan con lai duoc sap xep theo thu tu nho nh
             */
            while (i <= j)
            {
                //this.Infor();
                //  Tim tang dau tien tu trai sang co gia tri < tang hien tai cua thang may va` co lenh goi cung chieu di xuong cua thang may 
                while ((queue.ElementAt(i).flag != false) || queue.ElementAt(i).floor > elevator.getCurrentFloor())
                {
                    i++;
                    if (i == queue.Count) break;
                }

                //  Tim tang dau tien tu phai sang co gai tri > tang hien tai cua thang may hoac co' lenh goi nguoc chieu di xuong cua thang may
                while ((queue.ElementAt(j).flag == false) && queue.ElementAt(j).floor <= elevator.getCurrentFloor())
                {
                    j--;
                    if (j == -1) break;
                }


                if (i > j) break;
                /* Swap i <-> j */
                QueueElement.Swap(queue.ElementAt(i), queue.ElementAt(j));
                /****************/
                i++;
                j--;

            }

            n = i;
            /* Sap xep cac tang < tang hien tai cua thang may va co lenh goi cung chieu voi chieu di xuong cua thang may trong queue theo thu tu tang dan */
            for (i = n; i < queue.Count - 1; i++)
                for (j = i + 1; j < queue.Count; j++)
                    if (queue.ElementAt(i).getFloor() > queue.ElementAt(j).getFloor())
                        QueueElement.Swap(queue.ElementAt(i), queue.ElementAt(j));

            /* Tim tang nho nhat co lenh goi nguoc chieu voi chieu di xuong cua thang may */
            j = 0;
            for (i = 1; i < n; i++)
                if (queue.ElementAt(i).getFlag() == true && queue.ElementAt(i).getFloor() < queue.ElementAt(j).getFloor())
                    j = i;
            /* Neu ko co tang nao co lenh goi nguoc chieu voi chieu di xuong cua thang may -> return */

            if (queue.Count != 1)
                if (queue.ElementAt(j).getFlag() == false) return;
                else QueueElement.Swap(queue.ElementAt(j), queue.ElementAt(n - 1));
        }

        /* Sap xep queue theo uu tien khi thang may dang di len */
        public void SortQueueWhenElevatorGoUp()
        {
            int i = 0;
            int j = queue.Count - 1;
            int n;

            /*  Sap xep Queue thanh 2 phan:
             *  1.  Phan gan dau ra nhat se co cac phan tu co lenh goi cung chieu len voi chieu thang may va co tang >= thang may
             *  2.  
             */
            while (i <= j)
            {
                //  Tim tang dau tien tu trai sang trong queue co gia tri lon hon tang hien tai cua thang may va co lenh goi cung chieu voi thang may
                while (queue.ElementAt(i).getFlag() != true || queue.ElementAt(i).getFloor() < elevator.getCurrentFloor())
                {
                    i++;
                    if (i == queue.Count) break;
                }
                //  Tim tang dau tien tu phai sang trong queue co gia tri lon hon tang hien tai cua thang may hoac co lenh goi nguoc chieu voi thang may
                while (queue.ElementAt(j).getFlag() == true && queue.ElementAt(j).getFloor() >= elevator.getCurrentFloor())
                {
                    j--;
                    if (j == -1) break;
                }
                if (i > j) break;
                /* Swap i <-> j */
                QueueElement.Swap(queue.ElementAt(i), queue.ElementAt(j));
                /***************/
                i++;
                j--;
            }

            /* Sap xeo cac tang lon hon tang hien tai cua thang may va co lenh goi cung chieu voi chieu di len cua thang may theo thu tu giam dan gan` dau ra*/
            n = i;
            for (i = n; i < queue.Count - 1; i++)
                for (j = i + 1; j < queue.Count; j++)
                    if (queue.ElementAt(i).getFloor() < queue.ElementAt(j).getFloor())
                        QueueElement.Swap(queue.ElementAt(i), queue.ElementAt(j));
            /* Tim tang lon nhat co lenh goi nguoc chieu voi chieu cua thang may */
            j = 0;
            for (i = 1; i < n; i++)
                if (queue.ElementAt(i).getFlag() == false && queue.ElementAt(i).getFloor() > queue.ElementAt(j).getFloor())
                    j = i;
            //  Neu khong co tang nao goi nguoc chieu voi chieu di cua thang may -> return */
            if (queue.Count != 1)
                if (queue.ElementAt(j).getFlag() == true) return;
                else QueueElement.Swap(queue.ElementAt(j), queue.ElementAt(n - 1));
        }
    }
}
