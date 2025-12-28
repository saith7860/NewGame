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

        bool levelJustStarted = true;
        Random rand=new Random();
        // Game Timer (~60 FPS)
        System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();

        // Delta time for movement calculations
        float deltaTime = 0.016f;

        // Reference to player
        Player player;
        //Enemy enemy;
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
            player = new Player(Properties.Resources.spacePlayer, new PointF(500, 500), 2);
            player.Size = new SizeF(100, 100);
            player.Position = new PointF(
          (1000 - player.Size.Width) / 2, // center horizontally
    800 - player.Size.Height - 45    // bottom with 10px margin
);
            gameObjects.Add(player);


            for (int i = 0; i < 10; i++)
            {
               Enemy enemy = new Enemy(
                   Properties.Resources.spaceShooter,
                   new PointF(rand.Next(0, ClientSize.Width - 40), -rand.Next(50, 400))
               );

                enemy.Velocity = new PointF(0, rand.Next(2, 6)); // slow to fast
                gameObjects.Add(enemy);
            }

            gameTimer.Start();

        }
        //game logic for level incrementation
        private void SpawnEnemiesForLevel(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Enemy enemy = new Enemy(
                    Properties.Resources.spaceShooter,
                    new PointF(
                        rand.Next(0, ClientSize.Width - 40),
                        -rand.Next(50, 400)
                    )
                );

                int size = rand.Next(30, 80);
                enemy.Size = new SizeF(size, size);

                float speed = rand.Next(2 + player.Level, 6 + player.Level * 2);
                enemy.Velocity = new PointF(0, speed);

                gameObjects.Add(enemy);
            }
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
            if (player.enemiesDestroyed >= player.enemiesToNextLevel && levelJustStarted)
            {
               
                player.enemiesDestroyed = 0;

                SpawnEnemiesForLevel(10);

                //levelJustStarted = false;
            }
            foreach (var obj in gameObjects.ToList())
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
                // Keep player inside screen
                if (player != null)
                {
                    float maxX = ClientSize.Width - player.Size.Width;
                    float maxY = ClientSize.Height - player.Size.Height;

                    float x = player.Position.X;
                    float y = player.Position.Y;

                    if (x < 0) x = 0;
                    if (y < 0) y = 0;
                    if (x > maxX) x = maxX;
                    if (y > maxY) y = maxY;

                    player.Position = new PointF(x, y);
                }
              
            }
            

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
            g.DrawString("Level: " + player.Level,
    new Font("Arial", 16, FontStyle.Bold),
    Brushes.White,
    new PointF(10, 70));
            g.DrawString("Level: " + player.enemiesDestroyed,
new Font("Arial", 16, FontStyle.Bold),
Brushes.White,
new PointF(10, 110));
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
