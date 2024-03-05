using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private bool[,] grid;
        private bool[,] nextGrid;
        private bool isRunning;

        public Form1()
        {
            InitializeComponent();
            InitializeGrid();

            timer = new Timer();
            timer.Interval = 500;  // Set the refresh interval to 0.5 seconds
            timer.Tick += Timer1_Tick;

            grid = new bool[20, 20]; // Initialize the grid
            nextGrid = new bool[20, 20]; // Initialize the grid of the next state
        }

        private void InitializeGrid()
        {
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.BackColor = Color.Black;

            //add rows and columns
            for (int i = 0; i < 20; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            }

            // Add grid cells and register click events
            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 20; col++)
                {
                    Panel panel = new Panel();
                    panel.Dock = DockStyle.Fill;
                    panel.Margin = new Padding(0);
                    panel.BackColor = Color.White;
                    panel.Click += Panel_Click; // add click event

                    tableLayoutPanel1.Controls.Add(panel, col, row);
                }
            }
        }
        private void Panel_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                Panel panel = sender as Panel;
                if (panel != null)
                {
                    int row = tableLayoutPanel1.GetPositionFromControl(panel).Row;
                    int col = tableLayoutPanel1.GetPositionFromControl(panel).Column;
                    grid[row, col] = !grid[row, col];
                    panel.BackColor = grid[row, col] ? Color.Black : Color.White;
                }
            }
        }
        private void ApplyCustomRule()
        {
            //            rules:
            //            If a cell is alive and there are 2 living cells around it,
            //            or there is 1 living cell around it, it will remain alive at
            //            the next moment.
            //            Otherwise, the cell will die at the next moment.
            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 20; col++)
                {
                    int aliveNeighbors = CountAliveNeighbors(row, col);

                    if ((grid[row, col] && aliveNeighbors == 2) || aliveNeighbors == 1)
                    {
                        nextGrid[row, col] = true;
                    }
                    else
                    {
                        nextGrid[row, col] = false;
                    }
                }
            }
        }
        private int CountAliveNeighbors(int row, int col)
        {
            int count = 0;
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < 20 && j >= 0 && j < 20 && !(i == row && j == col))
                    {
                        if (grid[i, j])
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }
        private void UpdateGrid()
        {
            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 20; col++)
                {
                    grid[row, col] = nextGrid[row, col];
                    UpdatePanelColor(row, col);
                }
            }
        }

        private void UpdatePanelColor(int row, int col)
        {
            Panel panel = tableLayoutPanel1.GetControlFromPosition(col, row) as Panel;
            if (panel != null)
            {
                panel.BackColor = grid[row, col] ? Color.Black : Color.White;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            ApplyCustomRule(); //reply of rule
            UpdateGrid();
        }
        private void PauseButton_Click_1(object sender, EventArgs e)
        {
            isRunning = false;
            timer.Stop();
        }

        private void StartButton_Click_1(object sender, EventArgs e)
        {
            isRunning = true;
            timer.Start();
        }
    }
}
