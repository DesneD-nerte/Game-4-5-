using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    class FigureMoving//Ход фигуры
    {
        Board board;
        public Cell From { get; set; }
        public Cell To { get; set; }

        public FigureMoving(Board board)
        {
            this.board = board;
        }

        public FigureMoving(Cell from, Cell to)
        {
            this.From = from;
            this.To = to;
        }

        public void GetCellsAndFigures(string move)//Получение хода на основе записи, вводимой игроком
        {
            if (IsValidParameter(move))
            {
                FigureMoving fm = GetFigureMovingUsingMove(move);

                if (IsValidFigureMoving(fm))
                {
                    SetupCurrentFigureMoving(fm);
                }
            }
        }

        /// <summary>
        /// Приведение строки хода, которую вводит пользователь к структурированному виду (клетки: "Откуда ход"; "Куда ход";
        /// фигуры на данных клетках.
        /// </summary>
        private FigureMoving GetFigureMovingUsingMove(string move)
        {
            Cell from = new Cell(move.Substring(1, 2));
            from.Figure = (Figure)move[0];

            Cell to = new Cell(move.Substring(3, 2));
            to.Figure = board.GetFigureAt(to);

            FigureMoving fm = new FigureMoving(from, to);

            return fm;
        }

        private bool IsValidParameter(string move)
        {
            if (move == null)
            {
                throw new NullReferenceException("Входящая строка равно null");
            }

            if (move.Length != 5)
            {
                throw new ArgumentException("Входящая строка должна иметь длину в 5 символов");
            }

            return true;
        }

        private bool IsValidFigureMoving(FigureMoving figureMoving)
        {
            if (figureMoving.From.Figure != board.GetFigureAt(figureMoving.From))
            {
                throw new ArgumentException("На выбранной клетке не соответствует написанная фигура");
            }

            if (new Cell(figureMoving.From.X, figureMoving.From.Y).OnBoard() == false)
            {
                throw new ArgumentException("Строка имеет несуществующую координату");
            }

            if (new Cell(figureMoving.To.X, figureMoving.To.Y).OnBoard() == false)
            {
                throw new ArgumentException("Строка имеет несуществующую координату");
            }

            return true;
        }

        private void SetupCurrentFigureMoving(FigureMoving fm)
        {
            this.From = fm.From;
            this.To = fm.To;
        }
    }
}
