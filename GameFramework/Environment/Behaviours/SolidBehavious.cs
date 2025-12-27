using GameFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Environment.Behaviours
{
    public class SolidBehavious:IEnvironmentObject
    {
       public bool IsRigidBody { get; set; }=true;
    }
}
