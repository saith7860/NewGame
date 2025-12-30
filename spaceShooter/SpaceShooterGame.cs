using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Vml.Office;
using GameFramework.Component;
using GameFramework.Core;
using GameFrameWork;
using NAudio.SoundFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        // =================== FIELDS ===================
        List<GameObject> gameObjects = new();
        List<Star> stars = new();

        CollisionSystem collisionSystem = new();
        GameState currentState = GameState.StartMenu;

        RectangleF startButtonRect = new(300, 300, 250, 50);
        RectangleF nextLevelButtonRect = new(350, 350, 300, 60);

        Random rand = new();
        System.Windows.Forms.Timer gameTimer = new();

        float deltaTime = 0.016f;

        Player player;
        Bullet? bullet;
        Enemy enemy;
        Audio audio = new();

        // =================== CONSTRUCTOR ===================
        public SpaceShooterGame()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        // =================== Register Sounds ===============
        private void RegisterSounds()
        {
            AudioTrack bgMusic = new AudioTrack("bg", @"Assets\backMusic.mp3",true,0.5f);
            audio.AddSound(bgMusic);
            AudioTrack shootSound = new AudioTrack("hit", @"Assets\hit.mp3", false, 1f);
            audio.AddSound(shootSound);
        }


        // =================== LOAD ===================
        private void SpaceShooterGame_Load(object sender, EventArgs e)
        {
            InitializePlayer();
            InitializeStars();
            SpawnEnemiesForLevel(10);
            RegisterSounds();
            audio.PlaySound("bg");  
            gameTimer.Interval = 16;
            gameTimer.Tick += gameLoopTimer_Tick;

            gameTimer.Start();
        }

        // =================== GAME LOOP ===================
        private void gameLoopTimer_Tick(object sender, EventArgs e)
        {
            UpdateStars();

            if (currentState != GameState.Playing)
            {
                Invalidate();
                return;
            }

            GameTime gameTime = new(deltaTime);

            HandleInput();
            UpdateGameObjects(gameTime);
            KeepPlayerInsideScreen();
            HandleCollisions();
            CleanupObjects();
            CheckLevelCompletion();
          

            Invalidate();
        }

        // =================== INITIALIZATION ===================
        private void InitializePlayer()
        {
            player = new Player(Properties.Resources.spacePlayer, new PointF(500, 500), 2);
            player.Size = new SizeF(100, 100);
            player.Position = new(
                (ClientSize.Width - player.Size.Width) / 2,
                ClientSize.Height - player.Size.Height - 45
            );

            gameObjects.Add(player);
        }

        private void InitializeStars()
        {
            for (int i = 0; i < 100; i++)
            {
                stars.Add(new Star
                {
                    Position = new(rand.Next(0, ClientSize.Width), rand.Next(0, ClientSize.Height)),
                    Speed = rand.Next(1, 4),
                    Size = rand.Next(1, 3)
                });
            }
        }

        // =================== STARS ===================
        private void UpdateStars()
        {
            foreach (var star in stars)
            {
                star.Position = new(star.Position.X, star.Position.Y + star.Speed);

                if (star.Position.Y > ClientSize.Height)
                    star.Position = new(rand.Next(0, ClientSize.Width), 0);
            }
        }

        // =================== ENEMIES ===================
        private void SpawnEnemiesForLevel(int count)
        {
            for (int i = 0; i < count; i++)
            {
                 enemy = new(
                    Properties.Resources.spaceShooter,
                    new PointF(rand.Next(0, ClientSize.Width - 40), -rand.Next(50, 400))
                );

                int size = rand.Next(30, 80);
                enemy.Size = new(size, size);
                enemy.Velocity = new(0, rand.Next(2 + player.Level, 6 + player.Level * 2));
                enemy.OnDestroyed += () =>
                {
                    //audio.PlaySound("hit");
                };
                gameObjects.Add(enemy);
            }
        }

        private void ResetEnemy(Enemy enemy)
        {
            enemy.Position = new(
                rand.Next(0, (int)(ClientSize.Width - enemy.Size.Width)),
                -enemy.Size.Height
            );
            enemy.Velocity = new(0, rand.Next(2, 6));
        }

        // =================== INPUT ===================
        private void HandleInput()
        {
            if (EZInput.Keyboard.IsKeyPressed(EZInput.Key.Space))
            {
                
                bullet = player.Shoot();
               
                if (bullet != null)
                    gameObjects.Add(bullet);
                   
            }
        }

        // =================== UPDATE OBJECTS ===================
        private void UpdateGameObjects(GameTime gameTime)
        {
            foreach (var obj in gameObjects.ToList())
            {
                obj.Update(gameTime);

                if (obj.Movements.Count > 0)
                    obj.ApplyMovements(gameTime);

                if (obj is Enemy enemy && enemy.Position.Y > ClientSize.Height)
                    ResetEnemy(enemy);
            }
        }

        // =================== PLAYER BOUNDS ===================
        private void KeepPlayerInsideScreen()
        {
            float maxX = ClientSize.Width - player.Size.Width;
            float maxY = ClientSize.Height - player.Size.Height;

            float x = Math.Clamp(player.Position.X, 0, maxX);
            float y = Math.Clamp(player.Position.Y, 0, maxY);

            player.Position = new(x, y);
        }

        // =================== COLLISIONS ===================
        private void HandleCollisions()
        {
            foreach (var obj in gameObjects.ToList())
            {
                if (obj != player && obj.Bounds.IntersectsWith(player.Bounds))
                    player.OnCollision(obj);
            }

            collisionSystem.Check(gameObjects);
        }

        // =================== CLEANUP ===================
        private void CleanupObjects()
        {
            gameObjects = gameObjects.Where(o => o.IsActive).ToList();
        }

        // =================== LEVEL ===================
        private void CheckLevelCompletion()
        {
            if (player.enemiesDestroyed >= player.enemiesToNextLevel)
                currentState = GameState.LevelComplete;
        }

        // =================== MOUSE ===================
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (currentState == GameState.StartMenu &&
                startButtonRect.Contains(e.Location))
            {
                currentState = GameState.Playing;
            }
            else if (currentState == GameState.LevelComplete &&
                     nextLevelButtonRect.Contains(e.Location))
            {
                player.Level++;
                player.enemiesDestroyed = 0;
                SpawnEnemiesForLevel(10);
                currentState = GameState.Playing;
            }
        }

        // =================== DRAW ===================
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

        private void DrawStars(Graphics g)
        {
            foreach (var star in stars)
                g.FillEllipse(Brushes.White, star.Position.X, star.Position.Y, star.Size, star.Size);
        }

        private void DrawUI(Graphics g)
        {
            g.DrawString($"Lives: {player.Lives}", new Font("Arial", 16, FontStyle.Bold), Brushes.White, 10, 10);
            g.DrawString($"Score: {player.Score}", new Font("Arial", 16, FontStyle.Bold), Brushes.White, 10, 40);
            g.DrawString($"Level: {player.Level}", new Font("Arial", 16, FontStyle.Bold), Brushes.White, 10, 70);
        }

        private void DrawStartScreen(Graphics g)
        {
            g.Clear(Color.Black);
            g.DrawString("SPACE SHOOTER", new Font("Arial", 36, FontStyle.Bold), Brushes.White, 170, 200);

            g.FillRectangle(Brushes.DarkBlue, startButtonRect);
            g.DrawRectangle(Pens.White, startButtonRect);
            g.DrawString("START", new Font("Arial", 24, FontStyle.Bold),
                Brushes.White, startButtonRect.X + 40, startButtonRect.Y + 5);
        }

        private void DrawLevelCompleteScreen(Graphics g)
        {
            g.Clear(Color.Black);

            g.DrawString("LEVEL COMPLETE!", new Font("Arial", 36, FontStyle.Bold),
                Brushes.Yellow, 200, 200);

            g.FillRectangle(Brushes.DarkGreen, nextLevelButtonRect);
            g.DrawRectangle(Pens.White, nextLevelButtonRect);

            g.DrawString("NEXT LEVEL", new Font("Arial", 22, FontStyle.Bold),
                Brushes.White, nextLevelButtonRect.X + 20, nextLevelButtonRect.Y + 15);
        }
    }
}
