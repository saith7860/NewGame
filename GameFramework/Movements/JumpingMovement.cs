using GameFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace GameFramework.Movements
{
    public class JumpingMovement : IMovement
    {

        private float jumpForce;
        private float gravity;
        private float groundY;
        private float verticalVelocity = 0;
        private bool isJumping = false;

        public JumpingMovement(float jumpForce, float gravity, float groundY)
        {
            this.jumpForce = jumpForce;
            this.gravity = gravity;
            this.groundY = groundY;
            
        }

        public void Jump()
        {
            if (!isJumping)
            {
                verticalVelocity = -jumpForce;
                isJumping = true;
            }
        }

        public void Move(GameObject obj, GameTime gameTime)
        {
            if (isJumping)
            {
                verticalVelocity += gravity * gameTime.DeltaTime;


                obj.Position = new PointF(
                    obj.Position.X,
                    obj.Position.Y + verticalVelocity * gameTime.DeltaTime
                );


                if (obj.Position.Y >= groundY)
                {
                    obj.Position = new PointF(obj.Position.X, groundY);
                    verticalVelocity = 0;
                    isJumping = false;
                }
            }
        }
    }
}


