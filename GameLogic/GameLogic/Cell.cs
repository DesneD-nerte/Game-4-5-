using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    class Cell //Клетка
    {
        public int X { get; set; }//Координата X
        public int Y { get; set; }//Координата Y

        public Figure Figure { get; set; } = Figure.None;//Фигура на данной клетке

        public Cell(int x, int y, Figure figure = Figure.None)
        {
            this.X = x;
            this.Y = y;
            this.Figure = figure;
        }

        /// <summary>
        ///  Конструктор позволяющий в качестве параметра принимать другую клетку и фигуру, что облегчает инициализацию
        ///  клеток в определенных случаях
        /// </summary>
        public Cell(Cell cell, Figure figure) : this(cell.X, cell.Y, figure)
        {

        }

        /// <summary>
        /// Конструктор для облегчения нахождения положения клетки на основе всеобщей записи клетки (a1, c5, h8)
        /// </summary>
        public Cell(string e2)
        {
            if (e2.Length == 2 && e2[0] >= 'a' && e2[0] <= 'h' && e2[1] >= '1' && e2[1] <= '8')
            {
                X = e2[0] - 'a';
                Y = e2[1] - '1';
            }
            else
            {
                throw new ArgumentException("Выход за поле");
            }
        }

        public bool OnBoard()
        {
            return X >= 0 && X < 8 &&
                   Y >= 0 && Y < 8;
        }

        public static IEnumerable<Cell> YieldAllCells()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                   yield return new Cell(x, y);
                }
            }
        }
    }
}
