using GameFramework.Component;
using GameFramework.Core;
using EZInput;
using GameFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using GameFramework.Movements;
using GameFrameWork;
using GameFramework.Environment;
using GameFramework.Environment.Behaviours;
using NAudio.Wave;
using System.Windows.Forms;
namespace spaceShooter
{
    


    public partial class SpaceShooter : Form
    {
        List<GameObject> gameObjects = new List<GameObject>();

        System.Windows.Forms.Timer gameTimer;
        float deltaTime = 0.016f;
        public SpaceShooter()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 16;
            gameTimer.Start();
            gameTimer.Tick += TimerEvent_Tick;

        }


        private void Form1_Load(object sender, EventArgs e)
        {

           
        }

        private void TimerEvent_Tick(object sender, EventArgs e)
        {


            Invalidate();
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (var obj in gameObjects)
            {
                obj.Draw(e.Graphics);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {



        }
    }
}
