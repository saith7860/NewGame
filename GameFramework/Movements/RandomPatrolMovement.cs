using GameFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GameFramework
{
    internal class RandomPatrolMovement : IMovement
    {


        private float speedX;
        private float speedY;

        private float minX, maxX, minY, maxY;

        private Random rand = new Random();
        private float changeTime = 0;
        private float changeInterval = 60;

        public RandomPatrolMovement(float minX, float maxX, float minY, float maxY, float speed)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;

            speedX = speed;
            speedY = speed;
        }

        public void Move(GameObject obj, GameTime gameTime)
        {

            obj.Position = new PointF(
                obj.Position.X + speedX,
                obj.Position.Y + speedY
            );

            changeTime++;


            if (changeTime >= changeInterval)
            {
                speedX = rand.Next(-2, 3);
                speedY = rand.Next(-2, 3);
                changeTime = 0;
            }


            if (obj.Position.X < minX || obj.Position.X > maxX)
                speedX = -speedX;

            if (obj.Position.Y < minY || obj.Position.Y > maxY)
                speedY = -speedY;
        }
    }
}


