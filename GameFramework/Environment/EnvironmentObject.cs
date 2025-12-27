using GameFramework.Interfaces;
using GameFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Environment
{
    public class EnvironmentObject:GameObject
    {
        public string Name { get; set; }=string.Empty;
        public IEnvironmentObject behaviour=null!;
        public EnvironmentObject(string name,IEnvironmentObject behaviour,PointF postion,SizeF size,Image? sprite=null)
        {
            Name = name;
            Position = postion;
            Size = size;
            Sprite = sprite;
            this.behaviour = behaviour;
            HasPhysics = false;
            Velocity=PointF.Empty;
            IsRigidBody=behaviour.IsRigidBody;
        }


    }
}
