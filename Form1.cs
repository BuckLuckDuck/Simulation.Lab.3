using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation_Lab_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        // Массив значение 2^3, для поиска позиций в правилах.
        string[] positions = new string[] { "111", "110", "101", "100", "011", "010", "001", "000" };
        // Число "правил", преобразованное в двоичную форму и разбитое на символьный массив.
        char[] cellRules;
        // Запрет воздействия на ячейки, после нажатия кнопки старт.
        bool start = true; 
        // Переменная указывающая количество используемых строк.
        int rowCounter = 0;

        /// <summary>
        /// Функция преобразования десятичного числа "правил" в двоичную форму.
        /// </summary>
        /// <param name="rule">Десятичное число "правил".</param>
        /// <returns>Массив символов размером 2^3, обозначающий правила по позициям из массива "positions".</returns>
        private char[] acceptRules(int rule)
        {
            char[] result;

            string binaryCode = Convert.ToString(rule, 2);

            int binaryLength = binaryCode.Length;
            if (binaryLength != 8)
            {
                for (int i = 0; i < 8 - binaryLength; i++)
                {
                    binaryCode = "0" + binaryCode;
                }
            }
            
            result = binaryCode.ToCharArray();

            return result;
        }

        /// <summary>
        /// Функция поиска "правила" по найденной комбинации.
        /// </summary>
        /// <param name="xyz">Комбинация.</param>
        /// <returns>Значение комбинации по "правилам."</returns>
        private char calculateLayerCellValue(char[] xyz)
        {
            char result;

            string code = new string(xyz);

            int index = Array.IndexOf(positions, code);

            result = cellRules[index];

            return result;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // При нажатие на кнопку "Create".
            nudColumnsCount.Enabled = false; // Отключается возможность редактирования параметров
            nudRules.Enabled = false;        // количества колонок и указания новых правил.
            btnStart.Enabled = true;         // Появляется возможность нажать кнопку "Start".

            dataGridView.Rows.Clear();       // Очищается вся таблица

            // Удаляются все колонки
            for (int i = 0; i < dataGridView.Columns.Count; i++)                 
            {
                dataGridView.Columns.RemoveAt(0);
            }

            // Добавляется количество колонок, запрошенное пользователем.
            for (int i = 0; i < (int)nudColumnsCount.Value; i++)
            {
                dataGridView.Columns.Add("","");
            }
            
            // Добавляется первичная строка, для пользовательского взаимодействия.
            dataGridView.Rows.Add();

            // Утверждаются правила в массив "cellRules".
            int rule = (int)nudRules.Value;
            cellRules = acceptRules(rule);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // При нажатие на кнопку "Start".
            btnCreate.Enabled = false;  // Отключается возможность повторного создания полей.
            btnStop.Enabled = true;     // Появляется возможность остановить компиляцию программы.
            btnStart.Enabled = false;   // Отключается возможность повторного старта программы.

            start = false;              // Вводится запрет на изменение ячеек пользователем.
            
            rowCounter = 0;
            timer1.Start();             // Компиляция основного кода программы.
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            // При нажатие на кнопку "Stop".
            timer1.Stop();                      // Остановка компиляции.
            btnCreate.Enabled = true;           // Появляется возможность заного задать поле и правила.
            btnStop.Enabled = false;            // Отключается возможность повторного нажатия кнопки.
            start = true;                       // Пользователь снова может взаимодействовать с полями.
            nudColumnsCount.Enabled = true;     // Появляется возможность задать новые параметры поля и правил.
            nudRules.Enabled = true;
        }

        /// <summary>
        /// Возможность покраски ячеек левой и правой кнопками мыши, вне компиляции программы.
        /// </summary>
        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (start)
            switch (e.Button)
            {
                case MouseButtons.Left:
                    dataGridView[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
                    dataGridView.ClearSelection();
                    break;
                case MouseButtons.Right:
                    dataGridView[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
                    dataGridView.ClearSelection();
                    break;
            }
        }

        /// <summary>
        /// Отключение функции выделения ячеек.
        /// </summary>
        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        /// <summary>
        /// Основной процесс моделирования.
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Массив предыдущих значений поколения.
            char[] previousLayer = new char[dataGridView.Columns.Count];

            // Массив будущих значений поколения.
            char[] currentLayer = new char[dataGridView.Columns.Count];

            // Комбинация для вычисления будущего значения ячейки.
            char[] xyz = new char[3];
            
            // Заполнение массива предыдущих значений.
            for (int i = 0; i < previousLayer.Length; i++)
            {
                if (dataGridView[i, rowCounter].Style.BackColor == Color.Red) previousLayer[i] = '1';
                else previousLayer[i] = '0';
            }

            // Добавления новой строки, для нового поколения.
            dataGridView.Rows.Add();
            rowCounter++;

            // Вычисление значений нового поколения на основе предыдущих значений.
            for (int i = 0; i < currentLayer.Length;i++)
            {
                if (i == 0 | i == currentLayer.Length - 1)
                {
                    if (i == 0) xyz[0] = previousLayer[currentLayer.Length - 1];
                    else xyz[0] = previousLayer[i - 1];
                    xyz[1] = previousLayer[i];
                    if (i == 0) xyz[2] = previousLayer[i + 1];
                    else xyz[2] = previousLayer[0];

                    currentLayer[i] = calculateLayerCellValue(xyz);

                    continue;
                }

                xyz[0] = previousLayer[i - 1];
                xyz[1] = previousLayer[i];
                xyz[2] = previousLayer[i + 1];

                currentLayer[i] = calculateLayerCellValue(xyz);
            }

            // Покраска ячеек в новосозданной строке.
            for (int i = 0; i < currentLayer.Length; i++)
            {
                if (currentLayer[i] == '0') dataGridView[i, rowCounter].Style.BackColor = Color.White;
                else dataGridView[i, rowCounter].Style.BackColor = Color.Red;
            }
        }
    }
}