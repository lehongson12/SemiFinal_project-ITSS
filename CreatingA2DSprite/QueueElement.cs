using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Object
{
    class QueueElement
    {
        public int floor;                   //  So hieu cua tang
        public Boolean flag;                //  Co bao' hieu lenh goi tu tang floor di len hay xuong
        //  Flag = true -> lenh goi muon di len
        //  Flag = false -> lenh goi muon di xuong

        public QueueElement(int theFloor, Boolean theFlag)
        {
            this.floor = theFloor;
            this.flag = theFlag;
        }

        public int getFloor()
        {
            return this.floor;
        }

        public Boolean getFlag()
        {
            return this.flag;
        }

        public void setFloor(int theFloor)
        {
            this.floor = theFloor;
        }

        public void setFlag(Boolean theFlag)
        {
            this.flag = theFlag;
        }

        public Boolean checkEqual(QueueElement obj)
        {
            if ((this.getFlag() == obj.getFlag()) &&
                (this.getFloor() == obj.getFloor())) return true;
            return false;
        }

        public Boolean checkExist(List<QueueElement> list)
        {
            for (int i = 0; i < list.Count; i++)
                if (this.checkEqual(list.ElementAt(i))) return true;
            return false;
        }

        public static void Swap(QueueElement element1, QueueElement element2)
        {
            QueueElement temp = new QueueElement(-1, false);

            temp.setFloor(element1.getFloor());
            temp.setFlag(element1.getFlag());

            element1.setFloor(element2.getFloor());
            element1.setFlag(element2.getFlag());

            element2.setFloor(temp.getFloor());
            element2.setFlag(temp.getFlag());
        }
    }
}
