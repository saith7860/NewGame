using GameFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    internal class VerticalMovement: IMovement
    {
        private float topBound;
        private float bottomBound;
        private float speed;
        public VerticalMovement(float topBound, float bottomBound, float speed)
        {
            this.topBound = topBound;
            this.bottomBound = bottomBound;
            this.speed = speed;
        }
        public void Move(GameObject obj, GameTime gameTime)
        {
            obj.Position = new PointF(obj.Position.X, obj.Position.Y + speed);
            if (obj.Position.Y < topBound)
            {
                obj.Position = new PointF(obj.Position.X, topBound);
                speed = Math.Abs(speed); // Move down
            }
            else if (obj.Position.Y > bottomBound)
            {
                obj.Position = new PointF(obj.Position.X, bottomBound);
                speed = -Math.Abs(speed); // Move up
            }
        }
    }
}
