using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TANKI_server
{
    class Tank : Object
    {
        public int TankID;
        public int HP = 100;
        public DateTime TimeSinceDeath = DateTime.MinValue;
        public DateTime LastActionTime;
        //private int Cooldown;
        public int Score = 0;
        //public int MovmentSpeed; 
        //public int Direction;
       //public bool IsAlive;
        public void Fire(Mappy mappy)
        {
            
            if (HP > 0)
            {
                Bullet bullet = new Bullet();
                bullet.OwnerID = TankID;
                bullet.BulletID = mappy.Global_ID_Of_Bullet;
                mappy.Global_ID_Of_Bullet++;
                mappy.Bullets.Add(bullet.BulletID, bullet);
                bullet.Direction = Direction;
                object collision = mappy.TryStep(this);
                if (collision is Tank)
                {
                    mappy.DealDamage((Tank)collision, bullet);
                }
                bullet.X = X;
                bullet.Y = Y;
                /* switch (bullet.Direction)
                 {
                     case 0:
                         bullet.X = X;
                         bullet.Y = Y + 1;
                         break;
                     case 1:
                         bullet.X = X - 1;
                         bullet.Y = Y;
                         break;
                     case 2:
                         bullet.X = X;
                         bullet.Y = Y - 1;
                         break;
                     case 3:
                         bullet.X = X + 1;
                         bullet.Y = Y;
                         break;
                 }*/
            }
            
        }
        
    }
}
