using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Animation;

namespace Object
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Randomize.Select();
            Human1.Enabled = false;
            Human2.Enabled = false;
            Human3.Enabled = false;
            StartFloor.Enabled = false;
            WantFloor.Enabled = false;
            CreateButton.Enabled = false;
            Randomize.Checked = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        public void Display_Infomation( int numberPeople , int currentPeople , int currentFloor , int eleState , int theSpeed , DateTime now)
        {
            TotalPeople.Clear(); ElevatorCurrentFloor.Clear(); PeopleInElevator.Clear(); State.Clear(); Speed.Clear();
            TotalPeople.Text = numberPeople.ToString();
            PeopleInElevator.Text = currentPeople.ToString();
            ElevatorCurrentFloor.Text = currentFloor.ToString();
            State.Text = (eleState == 0) ? "UP" : (eleState == 1 ? "DOWN" : "WAITING");
            Speed.Text = theSpeed.ToString();
            label17.Text = now.ToString();
        }
       
        private void Randomize_Click(object sender, EventArgs e)
        {
            Building.CREATE_RANDOM = 1;
            Human1.Enabled = false;
            Human2.Enabled = false;
            Human3.Enabled = false;
            StartFloor.Enabled = false;
            WantFloor.Enabled = false;
            CreateButton.Enabled = false;
            Customize.Checked = false;
        }

        private void Customize_Click(object sender, EventArgs e)
        {
            Building.CREATE_RANDOM = 0;
            Human1.Enabled = true;
            Human2.Enabled = true;
            Human3.Enabled = true;
            StartFloor.Enabled = true;
            WantFloor.Enabled = true;
            CreateButton.Enabled = true;
            Randomize.Checked = false;
        }

        private void StartFloor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Building.START = 1;
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            Building.PAUSE = 1;
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            Building.PAUSE = 0;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Building.EXIT = 1;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            if (StartFloor.SelectedIndex != WantFloor.SelectedIndex)
            {
                Building.CREATE = 1;
                Building.PEOPLE_START_FLOOR = StartFloor.SelectedIndex + 1;
                Building.PEOPLE_WANT_FLOOR = WantFloor.SelectedIndex + 1;
            }
            else
            {
                Infor0.Text = "Error: WantFloor must be difference StartFloor!";
                Infor1.Text = "";
                Infor2.Text = "";
            }
        }

        private void Human1_Click(object sender, EventArgs e)
        {
            Building.PEOPLE_TYPE = 0;
        }

        private void Human2_Click(object sender, EventArgs e)
        {
            Building.PEOPLE_TYPE = 1;
        }

        private void Human3_Click(object sender, EventArgs e)
        {
            Building.PEOPLE_TYPE = 2;
        }

        private void Infor1_Click(object sender, EventArgs e)
        {

        }

        private void Message_Click(object sender, EventArgs e)
        {

        }

        private void Infor0_Click(object sender, EventArgs e)
        {

        }

        private void Infor2_Click(object sender, EventArgs e)
        {

        }
    }
}
