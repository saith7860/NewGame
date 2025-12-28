using System.Drawing;
using System.Numerics;
namespace GameFrameWork
{
    public class Player : GameObject
    {
        private float fireCooldown = 0.25f; // seconds between shots
        private float fireTimer = 0f;
       
        public Player(Image Sprite,PointF startPos,float speed)
        {
            this.Sprite = Sprite;
            this.Position = startPos;
            this.Movement=new KeyboardMovement(speed);
            this.Size=new SizeF(Sprite.Width,Sprite.Height);
        }
        // Movement strategy: demonstrates composition over inheritance.
        // Different movement behaviors can be injected (KeyboardMovement, PatrolMovement, etc.).
        public IMovement? Movement { get; set; }

        // Domain state
        public int Lives { get; set; } = 3;
        public int Score { get; set; } = 0;
        //add score method
        public void AddScore(int points)
        {
            Score += points;
        }
        /// Update the player: delegate movement to the Movement strategy (if provided) and then apply base update.
        /// Shows the Strategy pattern (movement behavior varies independently from Player class).
        public override void Update(GameTime gameTime)
        {
            fireTimer += gameTime.DeltaTime;
            Movement?.Move(this, gameTime);
            base.Update(gameTime);
        }

        /// Draw uses base implementation; override if player needs custom visuals.
   
        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }
        //bullet shoot
        public Bullet? Shoot()
        {

            if (fireTimer < fireCooldown)
                return null;

            fireTimer = 0f;
            PointF bulletPos = new PointF(
         Position.X + Size.Width / 2 - 3,
         Position.Y
     );

            return new Bullet(this, bulletPos,new PointF(0,-8));
        }
        /// Collision reaction for the player. Demonstrates single responsibility: domain reaction is handled here.
        public override void OnCollision(GameObject other)
        {
            if (other is Enemy)
            {
                if (Lives!=0)
                {
                    Lives--;
                }
                
                
                ResetPosition();
                if (Lives<=0)
                {
                    IsActive = false;   
                   //gameOver logic 
                }
            }


            if (other is PowerUp)
                Lives ++;
        }
        public void ResetPosition()
        {
            this.Position= new PointF(
          (1000 - 100) / 2, // center horizontally
    (800 - 100) - 45    // bottom with 10px margin
);
        }
    }

}