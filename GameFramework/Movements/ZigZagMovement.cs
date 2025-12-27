using GameFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GameFramework
{
    internal class ZigZagMovement : IMovement
    {


        private float leftBound;
        private float rightBound;
        private float topBound;
        private float bottomBound;

        private float speedX;
        private float speedY;

        public ZigZagMovement(float left, float right, float top, float bottom, float speedX, float speedY)
        {
            this.leftBound = left;
            this.rightBound = right;
            this.topBound = top;
            this.bottomBound = bottom;
            this.speedX = speedX;
            this.speedY = speedY;
        }

        public void Move(GameObject obj, GameTime gameTime)
        {

            obj.Position = new PointF(
                obj.Position.X + speedX * gameTime.DeltaTime,
                obj.Position.Y + speedY * gameTime.DeltaTime
            );

            if (obj.Position.X < leftBound)
            {
                obj.Position = new PointF(leftBound, obj.Position.Y);
                speedX = Math.Abs(speedX);
            }
            else if (obj.Position.X > rightBound)
            {
                obj.Position = new PointF(rightBound, obj.Position.Y);
                speedX = -Math.Abs(speedX);
            }

            if (obj.Position.Y < topBound)
            {
                obj.Position = new PointF(obj.Position.X, topBound);
                speedY = Math.Abs(speedY);
            }
            else if (obj.Position.Y > bottomBound)
            {
                obj.Position = new PointF(obj.Position.X, bottomBound);
                speedY = -Math.Abs(speedY);
            }
        }
    }
}





