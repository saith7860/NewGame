using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Vml.Office;
using GameFrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spaceShooter
{
    public partial class SpaceShooterGame : Form
    {
        // List of all game objects
        List<GameObject> gameObjects = new List<GameObject>();
        CollisionSystem collisionSystem = new CollisionSystem();
        Random rand;
        // Game Timer (~60 FPS)
        System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();

        // Delta time for movement calculations
        float deltaTime = 0.016f;

        // Reference to player
        Player player;
        Enemy spaceEnemy;
        Bullet? bullet;
        public SpaceShooterGame()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Prevent flickering

            // Setup game loop timer
            gameTimer.Interval = 16; // ~60 FPS
            gameTimer.Tick += gameLoopTimer_Tick;
        }

        private void SpaceShooterGame_Load(object sender, EventArgs e)
        {
            player = new Player(Properties.Resources.spacePlayer,new PointF(500,500),2);
            player.Size = new SizeF(100, 100);
            player.Position = new PointF(
          (1000 - player.Size.Width) / 2, // center horizontally
    800 - player.Size.Height - 45    // bottom with 10px margin
);
            gameObjects.Add(player);
            rand = new Random();

            for (int i = 0; i < 10; i++)
            {
                Enemy enemy = new Enemy(
                    Properties.Resources.spaceShooter,
                    new PointF(rand.Next(0, ClientSize.Width - 40), -rand.Next(50, 400))
                );

                enemy.Velocity = new PointF(0, rand.Next(2, 6)); // slow to fast
                gameObjects.Add(enemy);
            }
            //for(int i=0;i<10;i++)
            //{
            //    Enemy enemy= new Enemy(Properties.Resources.spaceShooter,new PointF(100 + i * 120,120));
            //    gameObjects.Add(enemy);
            //}   
            //spaceEnemy = new Enemy(Properties.Resources.spaceShooter,new PointF(100,150));
            //gameObjects.Add(spaceEnemy);
            gameTimer.Start();

        }

        private void gameLoopTimer_Tick(object sender, EventArgs e)
        {
                GameTime gameTime = new GameTime(deltaTime);
          
                if (EZInput.Keyboard.IsKeyPressed(EZInput.Key.Space))
            {
                 bullet = player.Shoot();
                if (bullet != null)
                    gameObjects.Add(bullet);
            }
            foreach(var obj in gameObjects.ToList())
            {
                obj.Update(gameTime);
                if (obj is Enemy enemy)
                {


                    if (enemy.Position.Y > ClientSize.Height)
                    {
                        enemy.Position = new PointF(
                            rand.Next(0, (int)(ClientSize.Width - enemy.Size.Width)),
                            -enemy.Size.Height
                        );

                        enemy.Velocity = new PointF(0, rand.Next(2, 6));
                    }
                }
                if (obj.Movements.Count > 0)
                   obj.ApplyMovements(gameTime);
            
            }
            // 1️⃣ Update all game objects
            //foreach (var obj in gameObjects.ToList())
            //{
            //    if (obj is Enemy enemy)
            //    {
            //        enemy.Shoot();
            //        if (bullet!=null)
            //        {
            //            gameObjects.Add(bullet);
            //        }
            //    }
            //    obj.Update(gameTime);

            //    // Apply multiple movements if available
            //    if (obj.Movements.Count > 0)
            //        obj.ApplyMovements(gameTime);
            //}

            // 2️⃣ Collision handling (for now just player vs enemies/power-ups)
            foreach (var obj in gameObjects.ToList())
            {
              
               if (obj != player && obj.Bounds.IntersectsWith(player.Bounds))
                {
                    player.OnCollision(obj);
                }
            }
            collisionSystem.Check(gameObjects);

            gameObjects = gameObjects.Where(o => o.IsActive).ToList();



            // 3️⃣ Redraw the screen
            Invalidate();

        }
        //show UI
        private void DrawUI(Graphics g)
        {
            // Draw Lives
            g.DrawString("Lives: " + player.Lives,
                         new Font("Arial", 16, FontStyle.Bold),
                         Brushes.White,
                         new PointF(10, 10));

            // Draw Score
            g.DrawString("Score: " + player.Score,
                         new Font("Arial", 16, FontStyle.Bold),
                         Brushes.White,
                         new PointF(10, 40));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (var obj in gameObjects.ToList())
            {
                obj.Draw(e.Graphics);
            }
            DrawUI(e.Graphics);
        }
    }
}
