using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class Bullet : Object
    {
        public int OwnerID;
        public int BulletID;
        // public int BulletDirection;
        public const int Damage = 20;
    }

}
