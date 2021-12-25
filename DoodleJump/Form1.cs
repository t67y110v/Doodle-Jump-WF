using DoodleJump.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DoodleJump
{
    public partial class Form1 : Form
    {
        Player player;  //переменные игрока и таймера 
        Timer timer1;
        public Form1()
        {
            InitializeComponent();
            Init();
            timer1 = new Timer();
            timer1.Interval = 15; //интервал таймера 
            timer1.Tick += new EventHandler(Update); //привязка событий на тик 
            timer1.Start();
            this.KeyDown += new KeyEventHandler(OnKeyboardPressed); //привязка обработчиков 
            this.KeyUp += new KeyEventHandler(OnKeyboardUp);
            this.BackgroundImage = Properties.Resources.back; // задний фон 
            this.Height = 600; //размеры
            this.Width = 330;
            this.Paint += new PaintEventHandler(OnRepaint); //отрисовка
        }

        public void Init() //вся инициализация классов 
        {
            PlatformController.platforms = new System.Collections.Generic.List<Platform>(); //создаем лист платформ и добавляем одну стартовую платформу прямо по д персонажем 
            PlatformController.AddPlatform(new System.Drawing.PointF(100, 400));
            PlatformController.startPlatformPosY = 400;
            PlatformController.score = 0;
            PlatformController.GenerateStartSequence();
            PlatformController.bullets.Clear();
            PlatformController.bonuses.Clear();
            PlatformController.enemies.Clear();
            player = new Player();
        }
//обработчики 
        private void OnKeyboardUp(object sender,KeyEventArgs e) // первый вызывается при отпускании клавишь 
        {
            player.physics.dx = 0; //обнуляем дх чтобы персонаж остановился 
            player.sprite = Properties.Resources.man2;
            switch (e.KeyCode.ToString())
            {
                case "Space": //выстрел из середины нашего персонажа
                    PlatformController.CreateBullet(new PointF(player.physics.transform.position.X + player.physics.transform.size.Width / 2, player.physics.transform.position.Y));
                    break;
            }
        }

        private void OnKeyboardPressed(object sender,KeyEventArgs e) //второй  смотрит какая кнопка нажата и в соответсвии с нажатой кнопкой присваиваем переменной дх значения которые будут двигать персонажа вправо влево 
        {
            switch (e.KeyCode.ToString())
            {
                case "Right":
                    player.physics.dx = 6;
                    break;
                case "Left":
                    player.physics.dx = -6;
                    break;
                case "Space": //нажатие на пробел для генерации пули 
                    player.sprite = Properties.Resources.man_shooting; //для этого меняем анимацию основоного персонажа на анимацию стрельбы 

                    break;
            }
        }


        private void Update(object sender,EventArgs e) //рассчитывание физики и всех функция для игры 
        {
            this.Text = "Your score in this fun game -  " + PlatformController.score;

            if ( (player.physics.transform.position.Y >= PlatformController.platforms[0].transform.position.Y + 200) || player.physics.StandartCollidePlayerWithObjects(true,false))
                Init(); //условие поражения  когда позиция игрока по у меньше позиции самой нижней платформы 

            player.physics.StandartCollidePlayerWithObjects(false, true);// столкновение с бонусами 

            if (PlatformController.bullets.Count > 0) //двигаем пули используя мув ап
            {
                for (int i = 0; i < PlatformController.bullets.Count; i++)
                {
                    if (Math.Abs(PlatformController.bullets[i].physics.transform.position.Y - player.physics.transform.position.Y) > 500)
                    {
                        PlatformController.RemoveBullet(i);
                        continue;
                    }
                    PlatformController.bullets[i].MoveUp();
                }
            }
            if (PlatformController.enemies.Count > 0) //проверка на столконовение 
            {
                for (int i = 0; i < PlatformController.enemies.Count; i++)
                {
                    if (PlatformController.enemies[i].physics.StandartCollide())
                    {
                        PlatformController.RemoveEnemy(i); //если столкнулись удаляем обьект
                        break;
                    }
                }
            }

            player.physics.ApplyPhysics();
            FollowPlayer();

            Invalidate();
        }

        public void FollowPlayer() //функция заменяющая камеру  тоесть мир бует двигаться за персонажем 
        { //двигаем  не окно а игрока и платформы на нем 
            int offset = 400 - (int)player.physics.transform.position.Y;
            player.physics.transform.position.Y += offset;
            for(int i = 0; i < PlatformController.platforms.Count; i++)
            {
                var platform = PlatformController.platforms[i];
                platform.transform.position.Y += offset;
            }
            for (int i = 0; i < PlatformController.bullets.Count; i++)
            {
                var bullet = PlatformController.bullets[i];
                bullet.physics.transform.position.Y += offset;
            }
            for (int i = 0; i < PlatformController.enemies.Count; i++)
            {
                var enemy = PlatformController.enemies[i];
                enemy.physics.transform.position.Y += offset;
            }
            for (int i = 0; i < PlatformController.bonuses.Count; i++)
            {
                var bonus = PlatformController.bonuses[i];
                bonus.physics.transform.position.Y += offset;
            }
        }

        private void OnRepaint(object sender, PaintEventArgs e) // обработчик на событии перерисовки 
        { //если в списке платформ что то есть то отрисовыывем обьект в цикле и так для пуль бонусов и монстров
            Graphics g = e.Graphics;
            if (PlatformController.platforms.Count > 0)
            {
                for (int i = 0; i < PlatformController.platforms.Count; i++)
                    PlatformController.platforms[i].DrawSprite(g);
            }
            if (PlatformController.bullets.Count > 0)
            {
                for (int i = 0; i < PlatformController.bullets.Count; i++)
                    PlatformController.bullets[i].DrawSprite(g);
            }
            if (PlatformController.enemies.Count > 0)
            {
                for (int i = 0; i < PlatformController.enemies.Count; i++)
                    PlatformController.enemies[i].DrawSprite(g);
            }
            if (PlatformController.bonuses.Count > 0)
            {
                for (int i = 0; i < PlatformController.bonuses.Count; i++)
                    PlatformController.bonuses[i].DrawSprite(g);
            }
            player.DrawSprite(g);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
