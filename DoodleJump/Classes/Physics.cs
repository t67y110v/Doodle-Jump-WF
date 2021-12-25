using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoodleJump.Classes
{
    public class Physics  // класс через котрый реализуется физика : прыжок персонажа и его столкновение с платформой на крате 
    {
        public Transform transform; // размер и позиция
        float gravity; //гравитация для реализации прыжка 
        float a; // ускорение 

        public float dx;  // для передвижения персонажа вправо влево 
        bool usedBonus = false;

        public Physics(PointF position, Size size)
        {
            transform = new Transform(position, size);
            gravity = 0;
            a = 0.4f;
            dx = 0;
        }

        public void ApplyPhysics() // фунция для вызова расчета физики 
        {
            CalculatePhysics();
        }

        public void CalculatePhysics() // функция для осущесмтвления передвежжений влево вправл  и прыжка 
        {
            if (dx != 0) // если дх не равен нулю то двигаем персонажа по дх 
            {
                transform.position.X += dx;
            }
            if(transform.position.Y < 700) // если позиция по y меньше 700 то увеличиваем позицию на переменную гравити а гравити увеличиваем на ускорение 
            {
                transform.position.Y += gravity;
                gravity += a;

                if (gravity > -25 && usedBonus)
                {
                    PlatformController.GenerateRandomPlatform();
                    PlatformController.startPlatformPosY = -200;
                    PlatformController.GenerateStartSequence();
                    PlatformController.startPlatformPosY = 0;
                    usedBonus = false;
                }

                Collide();
            }
        }

        public bool StandartCollidePlayerWithObjects(bool forMonsters,bool forBonuses) //функция отвечающая за коллизюъ с монстрами и бонусами 
        {
            if (forMonsters) //если монстры
            {
                for (int i = 0; i < PlatformController.enemies.Count; i++)
                {
                    var enemy = PlatformController.enemies[i];
                    PointF delta = new PointF(); // высчитываем растояние между центрами наших обьктов 
                    delta.X = (transform.position.X + transform.size.Width / 2) - (enemy.physics.transform.position.X + enemy.physics.transform.size.Width / 2);
                    delta.Y = (transform.position.Y + transform.size.Height / 2) - (enemy.physics.transform.position.Y + enemy.physics.transform.size.Height / 2);
                    if (Math.Abs(delta.X) <= transform.size.Width / 2 + enemy.physics.transform.size.Width / 2)
                    { // если расстояние между серединами меньше чем  сумма их длинн и ширин означает что они находятся в столкновении 
                        if (Math.Abs(delta.Y) <= transform.size.Height / 2 + enemy.physics.transform.size.Height / 2)
                        {
                            if (!usedBonus)
                                return true;
                        }
                    }
                }
            }
            if (forBonuses) //если бонусы аналогично как и с монстрами
            {
                for (int i = 0; i < PlatformController.bonuses.Count; i++)
                {
                    var bonus = PlatformController.bonuses[i];
                    PointF delta = new PointF();
                    delta.X = (transform.position.X + transform.size.Width / 2) - (bonus.physics.transform.position.X + bonus.physics.transform.size.Width / 2);
                    delta.Y = (transform.position.Y + transform.size.Height / 2) - (bonus.physics.transform.position.Y + bonus.physics.transform.size.Height / 2);
                    if (Math.Abs(delta.X) <= transform.size.Width / 2 + bonus.physics.transform.size.Width / 2)
                    {
                        if (Math.Abs(delta.Y) <= transform.size.Height / 2 + bonus.physics.transform.size.Height / 2)
                        {
                            if (bonus.type == 1 && !usedBonus)
                            {
                                usedBonus = true;
                                AddForce(-30);
                            }
                            if (bonus.type == 2 && !usedBonus)
                            {
                                usedBonus = true;
                                AddForce(-60);
                            }

                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool StandartCollide()
        {
            for(int i = 0; i < PlatformController.bullets.Count; i++)
            {
                var bullet = PlatformController.bullets[i];
                PointF delta = new PointF();
                delta.X = (transform.position.X + transform.size.Width / 2) - (bullet.physics.transform.position.X + bullet.physics.transform.size.Width / 2);
                delta.Y = (transform.position.Y + transform.size.Height / 2) - (bullet.physics.transform.position.Y + bullet.physics.transform.size.Height / 2);
                if (Math.Abs(delta.X) <= transform.size.Width / 2 + bullet.physics.transform.size.Width / 2)
                {
                    if (Math.Abs(delta.Y) <= transform.size.Height / 2 + bullet.physics.transform.size.Height / 2)
                    {
                        PlatformController.RemoveBullet(i);
                        return true;
                    }
                }
            }
            return false;
        }

        public void Collide() //в данной функции проходим по всем платформам  в списке 
        {
            for(int i = 0; i < PlatformController.platforms.Count; i++)
            {
                var platform = PlatformController.platforms[i]; // локальная переменная для хранения итой платформы 
                if (transform.position.X+transform.size.Width/2 >= platform.transform.position.X && transform.position.X + transform.size.Width/2 <= platform.transform.position.X + platform.transform.size.Width)
                //условие если середина игрока по х находится в пределах платформы от х до  х + ее размер 
                {
                    if (transform.position.Y+transform.size.Height >= platform.transform.position.Y && transform.position.Y + transform.size.Height <= platform.transform.position.Y + platform.transform.size.Height)
                    //дополняем улсовие по у если позиция по у + размер игрока больше чем позиция лплтформы а так же позиция по у + ее высота меньше или равно позиции по у платформы + размер этой лпатфорсмы
                    {
                        if (gravity > 0) //если персонаж приолете на плптформу сверху 
                        {
                            AddForce();
                            if (!platform.isTouchedByPlayer) // если до этой платформы игрок еще не косался то увеличиваем очки на 20 и создаем новую платформу 
                            {
                                PlatformController.score += 20;
                                PlatformController.GenerateRandomPlatform();
                                platform.isTouchedByPlayer = true;
                            }
                        }
                    }
                }
            }
        }

        public void AddForce(int force = -10) // осуществление прыжка нашего персонажа 
        {
            gravity = force;
        }
    }
}
