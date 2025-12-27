using System.Drawing;
namespace GameFrameWork
{
    public class Player : GameObject
    {
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

        /// Update the player: delegate movement to the Movement strategy (if provided) and then apply base update.
        /// Shows the Strategy pattern (movement behavior varies independently from Player class).
        public override void Update(GameTime gameTime)
        {
            Movement?.Move(this, gameTime);
            base.Update(gameTime);
        }

        /// Draw uses base implementation; override if player needs custom visuals.
   
        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }

        /// Collision reaction for the player. Demonstrates single responsibility: domain reaction is handled here.
        public override void OnCollision(GameObject other)
        {
            if (other is Enemy)
            { 
                Lives --;
                ResetPosition();
                if (Lives<=0)
                {
                   //gameOver logic 
                }
            }


            if (other is PowerUp)
                Lives ++;
        }
        public void ResetPosition()
        {
            this.Position=new PointF(0,0);
        }
    }

}