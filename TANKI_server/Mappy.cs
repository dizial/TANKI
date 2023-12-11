using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TANKI_server
{
    class Mappy // класс хранит объекты, их положение и базовые взаимодействия
    {
        public int Global_ID_Of_Bullet=0;
        public List<Obstacle> Obstacles = new List<Obstacle>();
       public Dictionary<int, Tank> Tanks = new Dictionary<int, Tank>();
        // public List<Tank> Tanks = new List<Tank>();
       public Dictionary<int, Bullet> Bullets = new Dictionary<int, Bullet>();
        //public Dictionary<int, int> Scores = new Dictionary<int, int>();
        //public List<Bullet> Bullets = new List<Bullet>();
        public List<Object> SpawnPoints = new List<Object>();
        readonly private string MapAdress = "MapStartConfig.txt";
		public void CreateTank(int ID)
        {
            Tank tank = new Tank();
            tank.LastActionTime = DateTime.Now;
            int index = Tanks.Count % SpawnPoints.Count;
            tank.X = tank.TargetX = SpawnPoints[index].X;
            tank.Y = tank.TargetY = SpawnPoints[index].Y;
            tank.Direction = tank.TargetDirection = 0;
            tank.TankID = ID;
            Tanks.Add(ID, tank);
           
        }
        public void DeleteTank(int ID)
        {
            Tanks.Remove(ID);
            
        }
        string FormatCoords(string type, Object obj, int ID)
        {
            //MessageBox.Show(":" + obj.Direction);
            return type + ID + ":" + obj.X + "x" + obj.Y + "x" + obj.Direction + ";";
        }

        public string GetMapData()
        {
            string tmp = "";
            foreach (var tank in Tanks)
                if (tank.Value != null && tank.Value.HP > 0)
                tmp += FormatCoords("t", tank.Value, tank.Key);
            foreach (var bullet in Bullets)
                tmp += FormatCoords("b", bullet.Value, bullet.Key);
            return tmp;
        }
        public string GetScore(int ID)
        {
            string tmp = "";
            tmp += ("s" + ID + ":" + Tanks[ID].Score+";");
            return tmp;
        }
        public string GetHPOfClient(int ID)
        {
            string tmp = "";
            tmp += ("h" + ID + ":" + Tanks[ID].HP);
            return tmp;
        }
        public string GetMapAndID(int ID)
        {
            string tmp;
            StreamReader sr = new StreamReader(MapAdress);
            return tmp = ID + "\n"+ sr.ReadToEnd();
        }
        public Tank GetTankByID(int ID)
        {
            // if (Tanks[ID] != null)
            try { return Tanks[ID]; }
            catch { return null; }
        }
        public Bullet GetBulletByID(int ID)
        {
            return Bullets[ID];
        }
        public bool LoadFromFile ()
        {
            String line;
            
            try
            {
                StreamReader sr = new StreamReader(MapAdress); // серверу надо прописать путь где будет хранится карта
               for(int LineIndex = 0; LineIndex <= 10; LineIndex++)
               { 
                    line = sr.ReadLine();
                    for (int i = 0; i <= 10; i++)
                    {
                        if (line[i] == '1')
                        {
                            Obstacle obstacle = new Obstacle();
                            Obstacles.Add(obstacle);
                            obstacle.X = i;
                            obstacle.Y = LineIndex;
                        }
                        else if (line[i] == '*')
                        {
                            Object spawnpoints = new Object();
                            SpawnPoints.Add(spawnpoints);

                            spawnpoints.X = i;
                            spawnpoints.Y = LineIndex;

                        }
                    }
               }
                return true;
            }
            catch 
            {
                return false;
            }
           
        }
        public Object TryStep(Object obj) // проверка доступности шага в выбранном направлении 
        {
            int offsetX = 0, offsetY = 0;
            switch (obj.Direction)
            {
                case 0:
                    offsetX = 0;
                    offsetY = 1;
                    break;

                case 1:
                    offsetX = -1;
                    offsetY = 0;
                    break;

                case 2:
                    offsetX = 0;
                    offsetY = -1;
                    break;

                case 3:
                    offsetX = 1;
                    offsetY = 0;
                    break;
            }

            for (int i = 0; i < Obstacles.Count; i++)
            {
                if (obj.X + offsetX == Obstacles[i].X && obj.Y + offsetY == Obstacles[i].Y) return Obstacles[i];
            }
            foreach (var tank in Tanks)
            {
                if (tank.Value == null) continue;
                if (tank.Value.HP > 0)
                if (obj.X + offsetX == tank.Value.X && obj.Y + offsetY == tank.Value.Y) return tank.Value;
            }
            foreach (var bullet in Bullets)
            {
                if (obj.X + offsetX == bullet.Value.X && obj.Y + offsetY == bullet.Value.Y) return bullet.Value;
            }
            return null;
        }
      
        public void Move(Object obj)
        {
            if (obj == null) return;
            if (obj is Tank && ((Tank)obj).HP <= 0) return;
            obj.TargetX = obj.X;
            obj.TargetY = obj.Y;
            switch (obj.Direction)
            {
                case 0:
                    obj.TargetY++;
                    break;
                case 1:
                    obj.TargetX--;
                    break;
                case 2:
                    obj.TargetY--;
                    break; 
                case 3:
                    obj.TargetX++;
                    break;
            }
        }
        public void SetPosition(Object obj)
        {
            if (obj == null) return;
            obj.X = obj.TargetX;
            obj.Y = obj.TargetY;
            
        }
        public void SetUpTankDirection(Object obj)
        {
            if (obj == null) return;
            Console.WriteLine(obj.Direction +";"+ obj.TargetDirection);
            if (obj.TargetDirection != obj.Direction)
            {
                obj.TargetX = obj.X;
                obj.TargetY = obj.Y;
            }
            obj.Direction = obj.TargetDirection;
            Console.WriteLine("=====");
            Console.WriteLine(obj.Direction + ";" + obj.TargetDirection);
        }
        /*  public void TurnRight(Object obj)
          {                                             // значение направлений
              obj.Direction = (obj.Direction + 1) % 4; //      2
          }                                            //1             3       
                                                       //      0
          public void TurnLeft(Object obj)
          {
             // MessageBox.Show("Left:" + obj.Direction);
              obj.Direction = (4 + obj.Direction - 1) % 4;
            //  MessageBox.Show("finally:" + obj.Direction);
         }
        public void TurnBackward(Object obj)
        {
            obj.Direction = (obj.Direction + 2) % 4;
        } */
        public void WorldStep()
        {
           
            var tanksToRemove = Tanks.ToArray();
            foreach (var tank in tanksToRemove)
            {
                if (tank.Value == null) continue;
                if ((DateTime.Now - tank.Value.TimeSinceDeath).TotalSeconds > 5)
                {
                    tank.Value.HP = 100;
                    tank.Value.X = tank.Value.TargetX = SpawnPoints[tank.Key%SpawnPoints.Count].X;
                    tank.Value.Y = tank.Value.TargetY = SpawnPoints[tank.Key % SpawnPoints.Count].Y;
                    tank.Value.TimeSinceDeath = DateTime.MaxValue;
                }
              
                if ((DateTime.Now - tank.Value.LastActionTime).TotalMinutes > 1)
                {
                    Tanks.Remove(tank.Key);

                }
                if (tank.Value.TargetX != tank.Value.X || tank.Value.TargetY != tank.Value.Y || tank.Value.Direction != tank.Value.TargetDirection)
                {
                    //  MessageBox.Show(""+ tank.Value.Direction);
                    SetUpTankDirection(tank.Value);
                    Object obj = TryStep(tank.Value);
                    // if(obj != null)
                    // MessageBox.Show("" + tank.Value.Direction + " " + obj.GetType().Name+ " "+ obj.X+ " "+ obj.Y);
                    //  if (mappy.TryStep(mappy.GetTankByID(ID)) == null)
                    if (obj == null)
                    {
                        //  MessageBox.Show("X:"+ obj.X + "Y:" + obj.Y + "TargetX:" + obj.TargetX + "TargetY:" + obj.TargetY);
                        SetPosition(tank.Value);
                    }
                    else 
                    {
                        tank.Value.TargetX = tank.Value.X;
                        tank.Value.TargetY = tank.Value.Y;
                    }
                }
                

            }
            UpdateBullets();

        }
        public void DealDamage(Tank tank, Bullet bullet)
        {
            if (tank == null) return;
            tank.HP -= Bullet.Damage;
            if (tank.HP <= 0)
            {
                tank.Score -= 50;
				if (tank.Score <= 0)
					tank.Score = 0;
				Tanks[bullet.OwnerID].Score += 100;
                tank.TimeSinceDeath = DateTime.Now;
            }
        }
       public void UpdateBullets()// метод для обработки попадания пули
       {
            var itemsToRemove = Bullets.ToArray();
           foreach (var obj in itemsToRemove)
           {
                Object collided = TryStep(obj.Value);
                if (collided != null)
                {
                    if (collided is Tank)
                    {
                        DealDamage((Tank)collided, obj.Value);

                    }
                    else if (collided is Bullet)
                    {
                        Bullet bullet = (Bullet)collided;
                        Bullets.Remove(bullet.BulletID);
                    }
                    Bullets.Remove(obj.Key);
                }
                else
                {
                    switch (obj.Value.Direction)
                    {
                        case 0:
                            obj.Value.Y++;
                            break;
                        case 1:
                            obj.Value.X--;
                            break;
                        case 2:
                            obj.Value.Y--;
                            break;
                        case 3:
                            obj.Value.X++;
                            break;
                    }
                }
           }
       }


    }
}
