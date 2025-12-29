using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Vml.Office;
using GameFramework.Core;
using GameFrameWork;
using NAudio.SoundFont;
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
        class Star
        {
            public PointF Position;
            public float Speed;
            public int Size;
        }

        // List of all game objects
        List<GameObject> gameObjects = new List<GameObject>();
        List<Star> stars = new List<Star>();

        CollisionSystem collisionSystem = new CollisionSystem();
        GameState currentState = GameState.StartMenu;
        RectangleF startButtonRect = new RectangleF(300, 300, 250, 50);

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

            for (int i = 0; i < 100; i++)
            {
                stars.Add(new Star
                {
                    Position = new PointF(
                        rand.Next(0, ClientSize.Width),
                        rand.Next(0, ClientSize.Height)
                    ),
                    Speed = rand.Next(1, 4),
                    Size = rand.Next(1, 3)
                });
            }

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
            foreach (var star in stars)
            {
                star.Position = new PointF(
                    star.Position.X,
                    star.Position.Y + star.Speed
                );

                if (star.Position.Y > ClientSize.Height)
                {
                    star.Position = new PointF(
                        rand.Next(0, ClientSize.Width),
                        0
                    );
                }
            }

            if (currentState == GameState.StartMenu)
            {
                if (EZInput.Keyboard.IsKeyPressed(EZInput.Key.Enter))
                {
                    currentState = GameState.Playing;
                }

                Invalidate();
                return;
            }
            if (currentState != GameState.Playing)
            {
                Invalidate();
                return;
            }
           

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
        //mouse click event
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (currentState == GameState.StartMenu)
            {
                if (startButtonRect.Contains(e.Location))
                {
                    currentState = GameState.Playing;
                }
            }
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

        }
        private void DrawStartScreen(Graphics g)
        {
            g.Clear(Color.Black);
            g.DrawString("SPACE SHOOTER",
                new Font("Arial", 36, FontStyle.Bold),
                Brushes.White,
                new PointF(170, 200));
            //start button
            g.FillRectangle(Brushes.DarkBlue, startButtonRect);
            g.DrawRectangle(Pens.White, startButtonRect);

            g.DrawString("START",
                new Font("Arial", 24, FontStyle.Bold),
                Brushes.White,
                new PointF(startButtonRect.X + 40, startButtonRect.Y + 2));
        }

        private void DrawLevelCompleteScreen(Graphics g)
        {
            g.Clear(Color.Black);
            g.DrawString("LEVEL COMPLETE!",
                new Font("Arial", 36, FontStyle.Bold),
                Brushes.Yellow,
                new PointF(220, 250));
        }
        //draw stars
        private void DrawStars(Graphics g)
        {
            foreach (var star in stars)
            {
                g.FillEllipse(
                    Brushes.White,
                    star.Position.X,
                    star.Position.Y,
                    star.Size,
                    star.Size
                );
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawStars(e.Graphics);
            switch (currentState)
            {
                case GameState.StartMenu:
                    DrawStartScreen(e.Graphics);
                    break;

                case GameState.Playing:
                    foreach (var obj in gameObjects)
                        obj.Draw(e.Graphics);

                    DrawUI(e.Graphics);
                    break;

                case GameState.LevelComplete:
                    DrawLevelCompleteScreen(e.Graphics);
                    break;
            }
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    foreach (var obj in gameObjects.ToList())
        //    {
        //        obj.Draw(e.Graphics);
        //    }
        //    DrawUI(e.Graphics);
        //}
    }
}
