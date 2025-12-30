using System.Drawing;
namespace GameFrameWork
{

    public class Enemy : GameObject
    {
        private float fireCooldown = 0.25f; // seconds between shots
        private float fireTimer = 0f;
        // Optional movement behavior: demonstrates composition and allows testable movement logic.
        public IMovement? Movement { get; set; }

        // Default enemy velocity is set in constructor to give basic movement out-of-the-box.
        public event Action? OnDestroyed;
        public bool isDestroyed {  get; set; }=false;

        public Enemy(Image Sprite,PointF startPos)
        {
            this.Sprite = Sprite;
            Position = startPos;
            Velocity = new PointF(0, 1);
            Size = new SizeF(40, 40);
        }

        /// Update will call movement behavior (if any) and then apply base update to move by velocity.
        public override void Update(GameTime gameTime)
        {
            Movement?.Move(this, gameTime); // movement must be called
            base.Update(gameTime);
            //if (Position.Y > 800)   // adjust to form height
            //IsActive= false;
            //Random rand = new Random();

            //Position = new PointF(
            //    rand.Next(0, 1000 - (int)Size.Width), // random X
            //    -Size.Height                          // above screen
            //);

            //Velocity = new PointF(0, rand.Next(50, 200));
        }

        /// Custom draw: demonstrates polymorphism (override base draw to provide enemy visuals).
        public override void Draw(Graphics g)
        {
            base.Draw(g);
            //g.FillRectangle(Brushes.Red, Bounds);
        }
        //enemy shoot method

        /// On collision, enemy deactivates when hit by bullets (encapsulation of reaction logic inside the entity).
        public override void OnCollision(GameObject other)
        {
            if (other is Bullet)
                IsActive = false;
            isDestroyed = true;
            OnDestroyed?.Invoke();
            if (other is Bullet && other is GameObject)
            {

            }
        }
    }
}