using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    class Board
    {
        /// <summary>
        /// Строка, олицетворяющая фигуры на поле. Пример: "rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR
        /// </summary>
        public string Fen { get; set; }

        Cell[,] figures;//Набор клеток с фигурами

        public List<Figure> whiteFigures = new List<Figure>() { Figure.WhiteKing, Figure.WhiteQueen, Figure.WhiteRook, Figure.WhiteBishop, Figure.WhiteKnight, Figure.WhitePawn };
        public List<Figure> blackFigures = new List<Figure>() { Figure.BlackKing, Figure.BlackQueen, Figure.BlackRook, Figure.BlackBishop, Figure.BlackKnight, Figure.BlackPawn };

        List<FigureMoving> allMoves;

        public string MoveColor { get; set; }

        public Board(string fen)
        {
            this.Fen = fen;
            figures = new Cell[8, 8];
            InitFigures(Fen);
        }

        public Board()
        {

        }

        private void ChangeColor(Board board)
        {
            if (board.MoveColor == "White")
            {
                this.MoveColor = "Black";
                return;
            }
            if (board.MoveColor == "Black")
            {
                this.MoveColor = "White";
                return;
            }
        }

        private void InitFigures(string fen)
        {
            string[] lines = fen.Split('/');

            if (lines.Length != 8)
                throw new ArgumentException("Не корректное поле - более 64 клеток");

            for (int y = 7; y >= 0; y--)//Просмотр сверху вниз (левый вверх (черные фигуры) => правый низ (белые фигуры))
            {
                for (int x = 0; x < 8; x++)
                {
                    figures[x, y] = new Cell(x, y);
                    
                    if(lines[7 - y][x] == '.')
                    {
                        figures[x, y].Figure = Figure.None;
                    }
                    else
                    {
                        figures[x, y].Figure = (Figure)lines[7 - y][x];
                    }

                    if (!whiteFigures.Contains(figures[x, y].Figure) && !blackFigures.Contains(figures[x, y].Figure) && figures[x, y].Figure != Figure.None)
                        throw new ArgumentException("Не существующая фигура");
                }
            }
        }

        /// <summary>
        /// Добавление слеша должно осуществлять только после первого набора, так как 
        /// в начале его нет (Блоки по 8 фигур)
        /// </summary>
        public void UpdateCurrentFen()
        {
            Fen = "";

            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    Fen += (figures[x, y].Figure == Figure.None) ? '.' : (char)figures[x, y].Figure;
                }

                if (y != 0)
                {
                    Fen += "/";
                }
            }
        }

        public Figure GetFigureAt(Cell cell)
        {
            if (cell.OnBoard())
            {
                return figures[cell.X, cell.Y].Figure;
            }

            return Figure.None;
        }

        public Figure GetFigureXY(int x, int y)
        {
            if (new Cell(x, y).OnBoard())
            {
                return figures[x, y].Figure;
            }
            else
            {
                throw new IndexOutOfRangeException("Выход за границы");
            }
        }

        private void SetFigureAt(Cell cell, Figure figure)
        {
            if (cell.OnBoard())
            {
                figures[cell.X, cell.Y].Figure = figure;
            }
        }

        public Board Move(FigureMoving fm)//Получение новой доски (с перемещением), олицетворение процесса хода в игре
        {
            Board nextboard = new Board(Fen);

            nextboard.SetFigureAt(fm.From, Figure.None); //Уборка фигуры из стартовой позиции, осуществляется установкой на данной позиции фигуры "None"
            nextboard.SetFigureAt(fm.To, fm.From.Figure);

            nextboard.UpdateCurrentFen();

            return nextboard;
        }

        /// <summary>
        /// Поиск шаха происходит по принципу, что каждая фигура текущего цвета атаковать вражеского короля
        /// </summary>
        private bool CanBeatKing()
        {
            Cell cellking = FindEnemyKing();

            Moves moves = new Moves(this);
            
            foreach(Cell cell in YieldCellsAndFiguresOfMoveColor())
            {
                FigureMoving fm = new FigureMoving(cell, cellking);

                if (moves.CanMove(fm))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Проверка шаха на текущей доске. Метод определяет ставит ли текущий игрок, который совершает ход или уже совершил его,
        /// но цвет еще не был заменен, шах вражескому королю
        /// </summary>
        public bool IsCheck()
        {
            return this.CanBeatKing();
        }

        /// <summary>
        /// Проверка постановки шаха со стороны врага. На вход требуется ход одного игрока, далее метод определит будет ли
        /// осуществлен шах со стороны врага после данного нашего хода
        /// </summary>
        private bool BeatKingAfter(FigureMoving fm) 
        {
            Board after = Move(fm);//Нужна новая доска, которая примет на себя данный ход, что позволит просмотреть операцию шаха, не ломая данную использующуюся в игре доску
            after.ChangeColor(this);//Смена цвета доски для дальнейшей проверки осуществления шаха вражеским игроком

            return after.CanBeatKing();
        }

        private Cell FindEnemyKing()
        {
            Cell cell;
            Figure figure;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    figure = figures[i, j].Figure;

                    if (MoveColor == "White" && figure == Figure.BlackKing)//Если ходит белый игрок, то ищем черного
                    { 
                        cell = new Cell(i, j, Figure.BlackKing);

                        return cell;
                    }

                    if(MoveColor == "Black" && figure == Figure.WhiteKing)//Если ходит черный игрок, то ищем белого
                    {
                        cell = new Cell(i, j, Figure.WhiteKing);

                        return cell;
                    }
                }
            }

            throw new Exception("Король не может быть найден");
        }

        public string DetectColor(Figure figure)
        {
            foreach (Figure EveryWhiteFigure in whiteFigures)
            {
                if (figure == EveryWhiteFigure)
                {
                    return "White";
                }
            }

            foreach (Figure EveryBlackFigure in blackFigures)
            {
                if (figure == EveryBlackFigure)
                {
                    return "Black";
                }
            }

            return "";
        }

        /// <summary>
        /// Получение всех клеток и фигур текущего цвета (игрока) для дальнейшего получения всевозможных ходов, одним из
        /// которых можно будет воспользоваться
        /// </summary>
        private IEnumerable<Cell> YieldCellsAndFiguresOfMoveColor()
        {
            foreach (Cell cell in Cell.YieldAllCells())
            {
                foreach (Figure figure in whiteFigures)
                {
                    if (GetFigureAt(cell) == figure && MoveColor == "White")
                    {
                        yield return new Cell(cell, figure);
                    }
                }

                foreach(Figure figure in blackFigures)
                {
                    if (GetFigureAt(cell) == figure && MoveColor == "Black")
                    {
                        yield return new Cell(cell, figure);
                    }
                }
            }
        }

        public List<string> GetAllMoves()
        {
            FindAllMoves();

            return GetListMoves();
        }

        private void FindAllMoves()
        {
            allMoves = new List<FigureMoving>();
            
            Moves moves = new Moves(this);

            foreach (Cell from in YieldCellsAndFiguresOfMoveColor())
            {
                foreach (Cell to in Cell.YieldAllCells())//Перебор всех клеток игры, пробуем попасть в каждую клетку доски
                {
                    FigureMoving fm = new FigureMoving(from, to);

                    if (moves.CanMove(fm))
                    {
                        if (!BeatKingAfter(fm))//Для добавления в список разрешенных ходов текущего игрока требуется отсутствие шаха со стороны врага после совершения хода
                        {
                            allMoves.Add(fm);
                        }
                    }
                }
            }
        }
        
        private List<string> GetListMoves()
        {
            List<string> list = new List<string>();

            char figure;

            char fromSymbol;
            int fromNumber;

            char toSymbol;
            int toNumber;

            int startLatinSymbols = 97;

            string resultmove;

            foreach (FigureMoving fm in allMoves)
            {
                figure = (char)fm.From.Figure;

                fromSymbol = (char)(fm.From.X + startLatinSymbols);
                fromNumber = fm.From.Y + 1;

                toSymbol = (char)(fm.To.X + startLatinSymbols);
                toNumber = fm.To.Y + 1;

                resultmove = string.Concat(figure, fromSymbol, fromNumber, toSymbol, toNumber);//Qh5f7

                list.Add(resultmove);
            }

            return list;
        }

        public List<Figure> GetAllFigure()
        {
            List<Figure> AllFigure = new List<Figure>();

            AllFigure.AddRange(blackFigures);
            AllFigure.AddRange(whiteFigures);
            AllFigure.Add(Figure.None);

            return AllFigure;
        }
    }
}
