using System;
using System.Windows.Forms;

namespace StellarSimulation
{

    /// <summary>
    /// Диалоговое окно, служащее для введения параметров
    /// </summary>
    partial class InputBox : Form
    {

        /// <summary>
        /// Инициализация InputBox. 
        /// </summary>
        public InputBox()
        {
            InitializeComponent();
            Button button = new Button();
            button.Click += (sender, e) =>
            {
                Close();
            };
            CancelButton = button;
        }

        /// <summary>
        /// Вызывается при отпускании кнопки ОК. Закрывает диалоговое окно 
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event.</param>
        void ButtonClick(Object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Показывает InputBox с заданным сообщением и текстом по умолчанию в поле ввода 
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="defaultInputText">Текст по умолчанию</param>
        /// <returns>Флаг выхода из InputBox.</returns>
        public DialogResult ShowDialog(String message, String defaultInputText)
        {
            promptLabel.Text = message;
            responseBox.Text = defaultInputText;
            CenterToScreen();
            return ShowDialog();
        }

        /// <summary>
        /// Инициализирует и открывает новый Input Box с заданным сообщением
        /// и текстом по умолчанию в поле ввода 
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="defaultInputText">Текст по умолчанию</param>
        /// <returns>Введённое значение, иначе - значение по умолчанию</returns>
        public static String Show(String message, String defaultInputText = "")
        {
            using (InputBox box = new InputBox())
            {
                if (box.ShowDialog(message, defaultInputText) == DialogResult.OK)
                    return box.responseBox.Text;
                return defaultInputText;
            }
        }
    }
}