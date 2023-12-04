using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TANKI_server
{
    internal class Projectile : Tank
    {
        private int SpeedProjectile = 0;
        private int Damage = 0;
        protected int[] PositionProjectile = new int[2];

        public Projectile()
        {
            SpeedProjectile = 50;
            Damage = 50;
            PositionProjectile[0] = 0;
            PositionProjectile[1] = 0;
        }

        public void SetDamage(int NewDamage)
        {
            this.Damage = NewDamage;
        }
        public int GetDamage()
        {
            return this.Damage;
        }
        public void SetSpeedProjectile(int NewSpeedProjectile)
        {
            this.SpeedProjectile = NewSpeedProjectile;
        }
        public int GetSpeedProjectile()
        {
            return this.SpeedProjectile;
        }
        public void SetPositionProjectile(int X, int Y)
        {
            this.PositionProjectile[0] = X;
            this.PositionProjectile[1] = Y;
        }
        public int[] GetPositionProjectile()
        {
            return this.PositionProjectile;
        }
    }
}
