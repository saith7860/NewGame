using GameFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GameFramework.Movements
{
    internal class ChaseMovement : IMovement
    {

        //target is plsyer
        private GameObject target;
        private float speed;

        public ChaseMovement(GameObject target, float speed)
        {
            this.target = target;
            this.speed = speed;
        }

        public void Move(GameObject obj, GameTime gameTime)
        {

            if (obj.Position.X < target.Position.X)
                obj.Position = new PointF(obj.Position.X + speed, obj.Position.Y);
            else if (obj.Position.X > target.Position.X)
                obj.Position = new PointF(obj.Position.X - speed, obj.Position.Y);


            if (obj.Position.Y < target.Position.Y)
                obj.Position = new PointF(obj.Position.X, obj.Position.Y + speed);
            else if (obj.Position.Y > target.Position.Y)
                obj.Position = new PointF(obj.Position.X, obj.Position.Y - speed);
        }
    }
}


