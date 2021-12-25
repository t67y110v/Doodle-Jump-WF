using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleJump.Classes
{
    public class Player //  класс отвечающий за игрока 
    {
        public Physics physics; // привязка физики к персонажу 
        public Image sprite;  // изображение персонажа

        public Player()
        {
            sprite = Properties.Resources.man2; // из ресурсов грузим картинку главного персонажа 
            physics = new Physics(new PointF(100, 350), new Size(40, 40)); // новый экземпляр с позицией и размером
        }

        public void DrawSprite(Graphics g) //отрисовка героя в форме 
        {
            g.DrawImage(sprite, physics.transform.position.X, physics.transform.position.Y, physics.transform.size.Width, physics.transform.size.Height);
        }
    }
}
