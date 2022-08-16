using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using Lattice;

namespace StellarSimulation
{

    /// <summary>
    /// Окно симуляции
    /// </summary>
    class SimulationForm : Form
    {

        /// <summary>
        /// Количество миллисекунд между обновлением кадров 
        /// </summary>
        private const int PaintInt = 33;

        /// <summary>
        /// Погрешность измерения FPS (кадров в секунду)
        /// </summary>
        private const double DrawFpsEasing = 0.2;

        /// <summary>
        /// Максимальное количество кадров в секунду 
        /// </summary>
        private const double MaxFps = 999.9;

        /// <summary>
        /// Расстояние от правой границы окна, служащее для вывода на экран текста информации 
        /// </summary>
        private const int InformationW = 270;

        /// <summary>
        /// Выбранная модель симуляции 
        /// </summary>
        private World _selectedWorld = World.Model;

        /// <summary>
        /// Текущее расположение мыши 
        /// </summary>
        private Point _mousePosition = new Point();

        /// <summary>
        /// Указывает - нажата ли кнопка мыши
        /// </summary>
        private Boolean _mouseBtn = false;

        /// <summary>
        /// Вспомогательный компонент, служащий для вычисления FPS (кадров в секунду)
        /// </summary>
        private Stopwatch _stpwtch = new Stopwatch();

        /// <summary>
        /// Выводимый FPS (кадры в секунду) 
        /// </summary>
        private double _framPerSecond = 0;

        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SimulationForm());
        }

        /// <summary>
        /// Создаёт главное окно и отображает панель настроек симуляции
        /// </summary>
        public SimulationForm()
        {

            // Инициализирует свойства окна и поведение мыши
            InitializeComponent();
            InitializeMouseEvents();

            // Инициализация отрисовки
            Paint += Draw;

            new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    Invalidate();
                    Thread.Sleep(PaintInt);
                }
            }))
            {
                IsBackground = true
            }.Start();

            // Показ окна настроек
            new SettingsForm().Show();
        }

        /// <summary>
        /// Инициализирует свойства окна 
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulationForm));
            this.SuspendLayout();

            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(711, 559);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Window";
            this.ShowIcon = false;
            this.Text = "Симуляция";
            this.Load += new System.EventHandler(this.Window_Load);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Инициализирует поведение мыши 
        /// </summary>
        private void InitializeMouseEvents()
        {

            MouseDown += (sender, e) =>
            {
                _mouseBtn = true;
            };
            MouseUp += (sender, e) =>
            {
                _mouseBtn = false;
            };

            MouseMove += (sender, e) =>
            {
                int deltaX = e.X - _mousePosition.X;
                int deltaY = e.Y - _mousePosition.Y;
                _mousePosition = e.Location;

                if (_mouseBtn)
                    RotationHelper.MouseDrag(_selectedWorld.Turn, deltaX, deltaY);
            };

            MouseWheel += (sender, e) =>
            {
                _selectedWorld.MoveCamera(e.Delta); ;
            };
        }

        /// <summary>
        /// Отрисовывает симуляцию 
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void Draw(Object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Отрисовка "мира" 
                g.TranslateTransform(ClientSize.Width / 2, ClientSize.Height / 2);
                _selectedWorld.Draw(g);
                g.ResetTransform();

                // Вывод информационного текста на экран 
                using (Font font = new Font("Arial", 11))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(50, Color.Yellow)))
                {
                    int x = Width - InformationW;

                    g.DrawString(String.Format("{0,-13}{1:#0.0}", "Кадров в секунду: "    , _selectedWorld.FramePerSecond), font, brush, x, 10);
                    //g.DrawString(String.Format("{0,-13}{1:#0.0}", "Прошло лет (тыс): "    , _selectedWorld.Frames*15.6), font, brush, x, 26);
                    g.DrawString(String.Format("{0,-13}{1}", "Количество тел: "           , _selectedWorld.BodyNumber), font, brush, x, 42);
                    g.DrawString(String.Format("{0,-13}{1:e2}", "Полная масса системы: "  , _selectedWorld.TotalWeight), font, brush, x, 58);
                    g.DrawString(String.Format("{0,-13}{1}", "Кадров с начала симуляции: ", _selectedWorld.Frames), font, brush, x, 74);

                    g.DrawString("Загрузка прошла успешно", font, brush, x, Height - 60);
                }

                // Обновление значения кадров в секунду
                _stpwtch.Stop();
                _framPerSecond += (1000.0 / _stpwtch.Elapsed.TotalMilliseconds - _framPerSecond) * DrawFpsEasing;
                _framPerSecond = Math.Min(_framPerSecond, MaxFps);
                _stpwtch.Reset();
                _stpwtch.Start();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Обрабатывает первоначальную загрузку окна симуляции
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void Window_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("Загрузка прошла без сбоев", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
