using System.Drawing;
namespace GameFrameWork
{

    public class Bullet : GameObject
    {
        public GameObject Owner { get; private set; }
        // Bullets set a default velocity in the constructor - a simple example of behavior initialization.
        public Bullet(GameObject owner,PointF startPosition)
        {
            Owner=owner;
            Position = startPosition;
            Size = new SizeF(6, 10);
            Velocity = new PointF(0, -8);
        }

        /// Bullets use the default movement logic (base.Update) and deactivate when off-screen.
        /// Consider extending with continous collision detection (CCD) to avoid tunnelling at high speeds.
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Position.Y + Size.Height < 0)
                IsActive = false;
        }

        /// Simple visual representation for bullets (polymorphism example).
        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Red, Bounds);
        }
      


        /// On collision bullets deactivate when hitting an enemy.
        /// Keep collision reaction encapsulated in the object class.
        public override void OnCollision(GameObject other)
        {
            if (other is Enemy)
                IsActive = false;
                
        }
    }
}