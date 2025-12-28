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

        // Game Timer (~60 FPS)
        System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();

        // Delta time for movement calculations
        float deltaTime = 0.016f;

        // Reference to player
        Player player;

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
            player = new Player(Properties.Resources.spacePlayer,new PointF(500,500),5);
            player.Size = new SizeF(100, 100);
            player.Position = new PointF(
          (1000 - player.Size.Width) / 2, // center horizontally
    800 - player.Size.Height - 45    // bottom with 10px margin
);
            gameObjects.Add(player);
            Enemy spaceEnemy= new Enemy(Properties.Resources.spaceShooter,new PointF(100,150));
            gameObjects.Add(spaceEnemy);
            gameTimer.Start();

        }

        private void gameLoopTimer_Tick(object sender, EventArgs e)
        {
            GameTime gameTime = new GameTime(deltaTime);
            if (EZInput.Keyboard.IsKeyPressed(EZInput.Key.Space))
            {
                Bullet bullet = player.Shoot();
                gameObjects.Add(bullet);
            }

            // 1️⃣ Update all game objects
            foreach (var obj in gameObjects)
            {
                obj.Update(gameTime);

                // Apply multiple movements if available
                if (obj.Movements.Count > 0)
                    obj.ApplyMovements(gameTime);
            }

            // 2️⃣ Collision handling (for now just player vs enemies/power-ups)
            foreach (var obj in gameObjects)
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
        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (var obj in gameObjects)
            {
                obj.Draw(e.Graphics);
            }
        }
    }
}
