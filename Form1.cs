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

        // 111 110 101 100 011 010 001 000
        string[] positions = new string[] { "111", "110", "101", "100", "011", "010", "001", "000" };
        char[] cellRules;
        bool start = true; /// Запрет воздействия на ячейки, после нажатия кнопки старт.
        int rowCounter = 0;
        int colCounter = 0;

        private char[] acceptRules(int rule)
        {
            char[] result;

            string binaryCode = Convert.ToString(rule, 2);
            
            if (binaryCode.Length != 8)
            {
                for (int i = 0; i < 8 - binaryCode.Length; i++)
                {
                    binaryCode = "0" + binaryCode;
                }
            }
            
            result = binaryCode.ToCharArray();

            return result;
        }

        private char calculateLayerCellValue(char[] xyz)
        {
            char result;

            string code = new string(xyz);

            int index = Array.IndexOf(xyz, positions);

            result = cellRules[index];

            return result;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            nudColumnsCount.Enabled = false;
            nudRules.Enabled = false;

            dataGridView.Rows.Clear();                      

            for (int i = 0; i < dataGridView.Columns.Count; i++)                 
            {
                dataGridView.Columns.RemoveAt(0);
            }

            for (int i = 0; i < (int)nudColumnsCount.Value; i++)
            {
                dataGridView.Columns.Add("","");
            }
            
            dataGridView.Rows.Add();

            int rule = (int)nudRules.Value;
            cellRules = acceptRules(rule);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnCreate.Enabled = false;
            
            start = false;

            timer1.Start();
        }

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

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            char[] previousLayer = new char[dataGridView.Columns.Count];
            char[] currentLayer = new char[dataGridView.Columns.Count];
            char[] xyz = new char[3];
            
            rowCounter = 0;
            colCounter = 0;

            for (int i = 0; i < previousLayer.Length; i++)
            {
                if (dataGridView[i, rowCounter].Style.BackColor == Color.Red) previousLayer[i] = '1';
                else previousLayer[i] = '0';
            }

            dataGridView.Rows.Add();

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


        }
    }
}
