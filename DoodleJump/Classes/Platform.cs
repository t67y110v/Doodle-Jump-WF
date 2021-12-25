using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleJump.Classes
{
    public class Platform  //класс отвечающий за платформы в игре 
    {
        Image sprite; //картинка которую будет отрисовывать 
        public Transform transform; // пеерменная трансформ сохраняющая размер и позицию 
        public int sizeX; //переменные х и у для того чтобы задать размеры платформы 
        public int sizeY;
        public bool isTouchedByPlayer;//переменная проверки было ли касание платформы игроком

        public Platform(PointF pos)
        {
            sprite = Properties.Resources.platform;// в конструктор передаем позицию , берем спрайт из ресурсов  указываем размеры платформы 60 на 12 
            sizeX = 60;
            sizeY = 12;
            transform = new Transform(pos, new Size(sizeX, sizeY)); //создаем новый экземпляр класса трансформ в который передаем необходимые поараметры
            isTouchedByPlayer = false;
        }

        public void DrawSprite(Graphics g)
        {
            g.DrawImage(sprite, transform.position.X, transform.position.Y, transform.size.Width, transform.size.Height);
            //фунцкия отрисвоки текущей платформы , из графики используем функцию drawimage передаем туда картинку позици и размер нашей платформы 
        }

    }
}
