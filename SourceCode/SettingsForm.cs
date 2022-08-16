using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace StellarSimulation
{

    /// <summary>
    /// Окно настройки параметров 
    /// </summary>
    public partial class SettingsForm : Form
    {
        private double _tempparam = 0;

        /// <summary>
        /// Создаёт окно параметров
        /// </summary>
        public SettingsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Стоп"
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The raised event</param>
        private void StopButtonClick(Object sender, EventArgs e)
        {
            World.Model.GenerateGravitySystem(SystemType.None);
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Медленные частицы" 
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The raised event</param>
        private void SlowStars_Click(Object sender, EventArgs e)
        {
            World.Model.GenerateGravitySystem(SystemType.SlowStars);
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Быстрые частицы"
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The raised event</param>
        private void FastStars_Click(Object sender, EventArgs e)
        {
            World.Model.GenerateGravitySystem(SystemType.FastStars);
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Массивные частицы"
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The raised event</param>
        private void MassStars_Click(Object sender, EventArgs e)
        {
            World.Model.GenerateGravitySystem(SystemType.MassiveBlackHole);
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Орбитальная система"
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The raised event</param>
        private void OrSys_Click(Object sender, EventArgs e)
        {
            World.Model.GenerateGravitySystem(SystemType.OrbitalSystem);
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Двойная система"
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The raised event</param>
        private void DoubleSys_Click(Object sender, EventArgs e)
        {
            World.Model.GenerateGravitySystem(SystemType.DoubleSystem);
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Планетарная система" 
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The raised event</param>
        private void PlanetSys_Click(Object sender, EventArgs e)
        {
            World.Model.GenerateGravitySystem(SystemType.PlanetarySystem);
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Тест дистрибуции" 
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The raised event</param>
        private void DistribTest_Click(Object sender, EventArgs e)
        {
            World.Model.GenerateGravitySystem(SystemType.DistribTest);
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Гравитационная постоянная"
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The raised event</param>
        private void ChangeGClick(Object sender, EventArgs e)
        {
            if (!Double.TryParse(InputBox.Show("Пожалуйста, введите G", World.G.ToString()), out _tempparam) || _tempparam < 0)
            {
                MessageBox.Show("Вы ввели неверное значение параметра\nG может быть только положительным числом \nБудет использовано значение по умолчанию", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                World.G = _tempparam;
            }

            _tempparam = 0;
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Максимально допустимая скорость" 
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The raised event.</param>
        private void ChangeCClick(Object sender, EventArgs e)
        {
            if (!Double.TryParse(InputBox.Show("Пожалуйста, введите C", World.C.ToString()), out _tempparam) || _tempparam < 0)
            {
                MessageBox.Show("Вы ввели неверное значение параметра\nC может быть только положительным числом \nБудет использовано значение по умолчанию", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                World.C = _tempparam;
            }

            _tempparam = 0;
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Количество частиц"  
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The raised event.</param>
        private void ChangeNClick(Object sender, EventArgs e)
        {
            int n;

            if (!int.TryParse(InputBox.Show("Пожалуйста, введите N", World.Model.BodyAllocNumber.ToString()), out n) || n < 2)
            {
                MessageBox.Show("Вы ввели неверное значение параметра\nN может быть только целым числом >=2 \nБудет использовано значение по умолчанию", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                World.Model.BodyAllocNumber = n;
            }

            n = 0;

        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Пауза / Продолжить"
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The raised event.</param>
        private void PauseClick(Object sender, EventArgs e)
        {
            World.Model.Active ^= true;
            pause.Image = World.Model.Active ? Properties.Resources.PAUSE : Properties.Resources.PLAY;
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Сбросить вид камеры"
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The raised event.</param>
        private void ResetView_Click(Object sender, EventArgs e)
        {
            World.Model.ResetCamera();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Спрятать дерево / Показать дерево"
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The raised event.</param>
        private void ShowBarnesHut_Click(object sender, EventArgs e)
        {
            World.Model.DrawTree ^= true;
            showTree.Image = (World.Model.DrawTree ? StellarSimulation.Properties.Resources.HIDETREE : StellarSimulation.Properties.Resources.SHOWTREE);
        }

        /// <summary>
        /// Завершает приложение при закрытии формы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
