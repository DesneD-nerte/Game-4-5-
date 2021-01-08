using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Game
    {
        readonly IDisplay display;
        internal Board Board { get; set; }
        public string Fen => Board.Fen;
        public string MoveColor { get; set; }
        public int GameState { get; set; }//Состояние игры (0 - игра; 1 - шах; 2 - мат) 

        enum State
        {
            GameProcess,
            Check,
            CheckMate
        }

        public Game(string fen, string movecolor, IDisplay display)
        {
            if(IsValidMainArguments(fen, movecolor, display))
            {
                this.display = display;

                Board = new Board(fen);

                MoveColor = movecolor;

                Board.MoveColor = movecolor;
            }
        }

        private bool IsValidMainArguments(string fen, string movecolor, IDisplay display)
        {
            return IsValidMoveColorAndDisplay(movecolor, display) && IsValidFen(fen);
        }

        private bool IsValidMoveColorAndDisplay(string movecolor, IDisplay display)
        {
            if (movecolor == null)
            {
                throw new NullReferenceException();
            }

            if (display == null)
            {
                throw new NullReferenceException();
            }

            if (movecolor != "White" && movecolor != "Black")
            {
                throw new ArgumentException("Некорректный цвет, разрешены только белый или черный");
            }

            return true;
        }

        private bool IsValidFen(string fen)
        {
            if (fen.Length != 71)
            {
                throw new ArgumentException("Строка имела неправильную длину");
            }

            Board checkBoard = new Board();

            int separator = 0;

            foreach (var symbol in fen)
            {
                if (separator % 8 == 0 && symbol == '/')
                {
                    continue;
                }

                if (!checkBoard.whiteFigures.Contains((Figure)symbol) && !checkBoard.blackFigures.Contains((Figure)symbol) && (Figure)symbol != Figure.None)
                {
                    throw new ArgumentException("Строка содержит некорректные данные");
                }

                separator++;
            }

            return true;
        }

        public void DisplayGame()
        {
            display.DisplayBoard(this);
            display.DisplayMoves(this);
        }


        /// <summary>
        /// По началу меняется цвет игры, и только потом проверяется наличие мата в игре, так как для данной проверки
        /// необходимо чтобы цвет совпадал, так как если поставлен мат, нужно проверить имеются ли ходы конкретно у текущего игрока
        /// </summary>
        public void PrepareNextMove()
        {
            display.Clear();

            if (DetermineTheCheck() == true)
            {
                DisplayState();
            }

            ChangeColor();

            if (DetermineTheCheckMate() == true)
            {
                DisplayState();
            }
        }

        /// <summary>
        /// В игре 3 состояния: 0 - процесс игры; 1 - шах; 2 - мат 
        /// При наличии мата в игре обратно возвращается предыдущий цвет, так как перед проверкой на существование мата
        /// он был изменен на тот, который был под атакой, чтобы проверить все его возможные ходы
        /// </summary>
        private void DisplayState()
        {
            if (this.GameState == 2)
            {
                ChangeColor();

                display.DisplayFinish(this);
                Environment.Exit(0);
            }

            if (this.GameState == 1)
            {
                display.WriteLine("Шах");
            }
        }

        private bool DetermineTheCheck()
        {
            if (Board.IsCheck() == true)
            {
                this.GameState = 1;
                return true;
            }

            this.GameState = 0;
            return false;
        }

        private bool DetermineTheCheckMate()
        {
            if (Board.GetAllMoves().Count == 0)
            {
                this.GameState = 2;
                return true;
            }

            this.GameState = 0;
            return false;
        }

        public void Move(string move, string movecolor)
        {
            if (IsValidMoveArguments(move, movecolor))
            {
                FigureMoving fm = new FigureMoving(this.Board);
                fm.GetCellsAndFigures(move);

                Board nextBoard = Board.Move(fm);
                nextBoard.MoveColor = movecolor;

                this.Board = nextBoard;
            }
        }

        private bool IsValidMoveArguments(string move, string movecolor)
        {
            if (!Board.whiteFigures.Contains((Figure)move[0]) && !Board.blackFigures.Contains((Figure)move[0]))
            {
                throw new ArgumentException("Фигура отсутствует на поле");
            }

            if (Board.whiteFigures.Contains((Figure)move[0]) && movecolor != "White")
            {
                throw new ArgumentException("Ход не соответствует данному цвету");
            }

            if (Board.blackFigures.Contains((Figure)move[0]) && movecolor != "Black")
            {
                throw new ArgumentException("Ход не соответствует данному цвету");
            }

            return true;
        }

        private void ChangeColor()
        {
            if (this.MoveColor == "White")
            {
                this.MoveColor = "Black";
                Board.MoveColor = this.MoveColor;

                return;
            }

            if (this.MoveColor == "Black")
            {
                this.MoveColor = "White";
                Board.MoveColor = this.MoveColor;

                return;
            }
            
            throw new ArgumentException();
        }

        public Figure GetFigureXY(int x, int y)
        {
           return this.Board.GetFigureXY(x, y);
        }

        public List<string> GetAllMoves()
        {
            return this.Board.GetAllMoves();
        }
    }
}
