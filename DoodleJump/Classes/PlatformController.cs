using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleJump.Classes
{
     public static class PlatformController   //с помощью этого класса происходит контроль над платформами : создаем новые очищаем старые  класс статический чтобы им можно бюыло пользоваться без созданий экземпляров класса 
    {
        public static List<Platform> platforms; // статический лист который хранит платформы которые на крате 
        public static List<Bullet> bullets = new List<Bullet>(); // лист пуль 
        public static List<Enemy> enemies = new List<Enemy>(); //лист врагов 
        public static List<Bonus> bonuses = new List<Bonus>(); //бонусы на карте 
        public static int startPlatformPosY = 400; // переменная для спавна платформ на карте 
        public static int score = 0; // переменная ответсвенная за очки 

        public static void AddPlatform(PointF position) //через эту функцию добавляется одна платформа на нашу форму : передаем в нее позицию, а в методе создаем новую платформу и помещаем ее в лист
        {
            Platform platform = new Platform(position);
            platforms.Add(platform);
        }

        public static void CreateBullet(PointF pos) //генерация пули и добавление ее в лист  используя позицию
        {
            var bullet = new Bullet(pos);
            bullets.Add(bullet);
        }

        public static void GenerateStartSequence() // фунцкия которая при старте игры создает определенное количество платформ на карту 
        {
            Random r = new Random();
            for(int i = 0; i < 10; i++) // через цикл генерируем координаты по х и у 
            {
                int x = r.Next(0, 270);
                int y = r.Next(50, 60);
                startPlatformPosY -= y; // вычитая из начального значения сгененрированный у тем самым смещая генерацию платформы по у тем самым с каждой итерацией платформа будет подниматься все выше
                PointF position = new PointF(x, startPlatformPosY); // затем создаем позицию и платформу с этими позициями 
                Platform platform = new Platform(position);
                platforms.Add(platform);  // и помещаем в лист
            }
        }

        public static void GenerateRandomPlatform()  // функция  генерации платформы в разных позициях делает все то же что и функция выше только без цикла 
        {
            ClearPlatforms();
            Random r = new Random();
            int x = r.Next(0, 270);
            PointF position = new PointF(x, startPlatformPosY);
            Platform platform = new Platform(position);
            platforms.Add(platform);

            var c = r.Next(1, 3);

            switch (c) //выбор генерация монстра или бонуса на платформе чтобы не спавггились вдвоем 
            {
                case 1:
                    c = r.Next(1, 10);
                    if (c == 1)
                    {
                        CreateEnemy(platform);
                    }
                    break;
                case 2: //генерация бонуса на платформе 
                    c = r.Next(1, 10);
                    if (c == 1)
                    {
                        CreateBonus(platform);

                    }
                    break;
            }

           

            
        }

        public static void CreateBonus(Platform platform) //создание бонуса со случаным типом 
        {
            Random r = new Random();
            var bonusType = r.Next(1,3);

            switch (bonusType)
            { //бонус генен=рируется на платформе как и враги
                case 1:
                    var bonus = new Bonus(new PointF(platform.transform.position.X + (platform.sizeX / 2) - 7, platform.transform.position.Y - 15), bonusType);
                    bonuses.Add(bonus);
                    break;
                case 2:
                    bonus = new Bonus(new PointF(platform.transform.position.X + (platform.sizeX / 2) - 15, platform.transform.position.Y - 30), bonusType);
                    bonuses.Add(bonus);
                    break;
            }
        }

        public static void CreateEnemy(Platform platform) // генерация врага принимаю позицию 
        {
            Random r = new Random();
            var enemyType = r.Next(1, 4);

            switch (enemyType)
            {
                case 1: //для пупрощения генерации в функцию эними используем трансформ платформы на которой он будет находится
                    var enemy = new Enemy(new PointF(platform.transform.position.X + (platform.sizeX / 2) - 20, platform.transform.position.Y - 40),enemyType);
                    enemies.Add(enemy);
                    break;
                case 2:
                    enemy = new Enemy(new PointF(platform.transform.position.X + (platform.sizeX / 2) - 35, platform.transform.position.Y - 50), enemyType);
                    enemies.Add(enemy);
                    break;
                case 3:
                    enemy = new Enemy(new PointF(platform.transform.position.X + (platform.sizeX / 2) -35, platform.transform.position.Y - 60), enemyType);
                    enemies.Add(enemy);
                    break;

            }

            
            
        }

        public static void RemoveEnemy(int i) //удаление монтсра
        {
            enemies.RemoveAt(i);
        }

        public static void RemoveBullet(int i) //удаление пуль с карты 
        {
            bullets.RemoveAt(i);
        }

        public static void ClearPlatforms() //функция очищения платформы, врага, бонуса если она находится далеко от игрока 
        { 
            for(int i = 0; i < platforms.Count; i++)
            {
                if (platforms[i].transform.position.Y >= 700)
                    platforms.RemoveAt(i);
            }
            for (int i = 0; i < bonuses.Count; i++)
            {
                if (bonuses[i].physics.transform.position.Y >= 700)
                    bonuses.RemoveAt(i);
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].physics.transform.position.Y >= 700)
                    enemies.RemoveAt(i);
            }
        }
    }
}
