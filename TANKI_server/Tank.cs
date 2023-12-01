using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TANKI_server
{
    internal class Tank
    {
        private int Health = 0;
        private int Speed = 0;
        private int[] Position = new int[2];
        private int AngleOfRotation = 0;

        public Tank()
        {
            Health = 100;
            Speed = 10;
            Position[0] = 0;
            Position[1] = 0;
            AngleOfRotation = 0;
        }

        public void SetHealth(int NewHealth)
        {
            this.Health = NewHealth;
        }
        public int GetHealth()
        {
            return this.Health;
        }
        public void SetSpeed(int NewSpeed)
        {
            this.Speed = NewSpeed;
        }
        public int GetSpeed()
        {
            return this.Speed;
        }
        public void SetAngleOfRotation(int NewAngleOfRotation)
        {
            this.AngleOfRotation = NewAngleOfRotation;
        }
        public int GetAngleOfRotation()
        {
            return this.AngleOfRotation;
        }
        public void SetPosition(int X, int Y)
        {
            this.Position[0] = X;
            this.Position[1] = Y;
        }
        public int[] GetPosition()
        {
            return this.Position;
        }
    }
}
