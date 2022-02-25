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

        private void btnStart_Click(object sender, EventArgs e)
        {

            dataGridView.Rows.Clear();                      
            int count = dataGridView.Columns.Count;
            for (int i = 0; i < count; i++)                 
            {
                dataGridView.Columns.RemoveAt(0);
            }
            int columnsCount = (int)nudColumnsCount.Value;
            for (int i = 0; i < columnsCount; i++)
            {
                dataGridView.Columns.Add("","");
            }
            
            dataGridView.Rows.Add();
            int rule = (int)nudRules.Value;
            cellRules = acceptRules(rule);
        }

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
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

        }
    }
}
