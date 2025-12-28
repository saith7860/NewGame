using System.Drawing;
namespace GameFrameWork
{

    public class Enemy : GameObject
    {
        // Optional movement behavior: demonstrates composition and allows testable movement logic.
        public IMovement? Movement { get; set; }

        // Default enemy velocity is set in constructor to give basic movement out-of-the-box.
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
            if (Position.Y > 800)   // adjust to form height
                IsActive = false;
        }

        /// Custom draw: demonstrates polymorphism (override base draw to provide enemy visuals).
        public override void Draw(Graphics g)
        {
            base.Draw(g);
            //g.FillRectangle(Brushes.Red, Bounds);
        }

        /// On collision, enemy deactivates when hit by bullets (encapsulation of reaction logic inside the entity).
        public override void OnCollision(GameObject other)
        {
            if (other is Bullet)
                IsActive = false;
                
            if (other is Bullet && other is GameObject)
            {

            }
        }
    }
}