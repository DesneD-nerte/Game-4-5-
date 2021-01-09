using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    class Moves
    {
        FigureMoving fm;

        Board board;

        public Moves(Board board)
        {
            this.board = board;
        }

        public bool CanMove(FigureMoving fm)
        {
            UpdateCurrentFigureMoving(fm);

            return AreFiguresEqual() && CanMoveFrom() && CanMoveTo() && CanFigureMove();
        }

        private void UpdateCurrentFigureMoving(FigureMoving fm)
        {
            this.fm = fm;
        }

        private bool AreFiguresEqual()
        {
            return fm.From.Figure == board.GetFigureAt(fm.From);
        }

        private bool CanMoveFrom()
        {
            return fm.From.OnBoard();
        }

        private bool CanMoveTo()
        {
            string colorFrom = board.DetectColor(board.GetFigureAt(fm.From));

            string colorTo = board.DetectColor(board.GetFigureAt(fm.To));

            return fm.To.OnBoard() && colorTo != colorFrom;
        }

        private bool CanFigureMove()
        {
            switch(fm.From.Figure)
            {
                case Figure.BlackKing :
                case Figure.WhiteKing :
                     return CanKingMove();

                case Figure.BlackKnight :
                case Figure.WhiteKnight :
                    return CanKnightMove();

                case Figure.BlackRook :
                case Figure.WhiteRook :
                    return CanRookMove();

                case Figure.BlackBishop :
                case Figure.WhiteBishop :
                    return CanBishopMove();

                case Figure.BlackQueen :
                case Figure.WhiteQueen :
                    return CanRookMove() || CanBishopMove();//Проверка перемещения по вертикали, горизонтали и по диагонали, что охватывает ходы ферзя

                case Figure.BlackPawn :
                case Figure.WhitePawn :
                    return CanPawnMove();

                default: return false;
            }
        }
        
        /// <summary>
        /// Разница между начальным и конечным положением по обоим осям должна быть равна или нулю или единице, так как
        /// король находит на клетку в любую сторону
        /// </summary>
        private bool CanKingMove()
        {
            int absX = Math.Abs(fm.From.X - fm.To.X);//Горизонталь
            int absY = Math.Abs(fm.From.Y - fm.To.Y);//Вертикаль

            return (absX == 0 || absX == 1) && (absY == 0 || absY == 1);
        }

        /// <summary>
        /// Конь способен перемещаться на две единицы по вертикали и на одну единицу по горизонтали и должны
        /// соблюдаться оба условия
        /// </summary>
        private bool CanKnightMove()
        {
            int deltaX = Math.Abs(fm.To.X - fm.From.X);
            int deltaY = Math.Abs(fm.To.Y - fm.From.Y);

            return (deltaX == 1 && deltaY == 2) || (deltaX == 2 && deltaY == 1);
        }

        private bool CanRookMove()
        {
            return IsRookOneCoordinateEquelZero() && CheckOtherFiguresOnTheRookAndBishopLineAttack();
        }

        /// <summary>
        /// Одна из осей должна оставаться такой же, так как ладья ходит либо вертикали либо по горизонтали
        /// </summary>
        private bool IsRookOneCoordinateEquelZero()
        {
            int absX = Math.Abs(fm.To.X - fm.From.X);
            int absY = Math.Abs(fm.To.Y - fm.From.Y);

            return absX == 0 || absY == 0;
        }

        private bool CheckOtherFiguresOnTheRookAndBishopLineAttack()
        {
            int deltaX = Math.Sign(fm.To.X - fm.From.X);
            int deltaY = Math.Sign(fm.To.Y - fm.From.Y);

            int nextX = fm.From.X + deltaX;
            int nextY = fm.From.Y + deltaY;

            while (nextX != fm.To.X || nextY != fm.To.Y)
            {
                if (board.GetFigureXY(nextX, nextY) != Figure.None)
                {
                    return false;
                }

                nextX += deltaX;
                nextY += deltaY;
            }

            return true;
        }

        private bool CanBishopMove()
        {
            return AreBishopBothCoordinatesHaveDifferentStartAndFinishPositions() && CheckOtherFiguresOnTheRookAndBishopLineAttack();
        }

        /// <summary>
        /// Разница между начальным и конечным положением по обоим осям должно совпадать, так как слон ходит только по диагонали
        /// </summary>
        private bool AreBishopBothCoordinatesHaveDifferentStartAndFinishPositions()
        {
            int absX = Math.Abs(fm.To.X - fm.From.X);
            int absY = Math.Abs(fm.To.Y - fm.From.Y);

            return absX == absY;
        }

        private bool CanPawnMove()
        {
            if(IsThereFigureOnToCell())
            {
                return CanPawnAttack();
            }
            else
            {
                return CanPawnMoveSuchWay();
            }
        }

        /// <summary>
        /// Разница между положениями должна равняться единице, так как пешка, атакуя, должна ходить по диагонали на один ход
        /// </summary>
        private bool CanPawnAttack()
        {
            return (Math.Abs(fm.To.X - fm.From.X) == 1) && (Math.Abs(fm.To.Y - fm.From.Y) == 1) ;
        }

        private bool CanPawnMoveSuchWay()
        {
            return MovePawnFromTheStart() || MovePawnNotFromTheStart();
        }

        private bool MovePawnFromTheStart()
        {
            if (fm.From.Y == 1)//1 - Означает ситуацию, когда текущая (белая) пешка на старте
            {
                if (fm.From.X == fm.To.X && (fm.From.Y + 1 == fm.To.Y || (fm.From.Y + 2 == fm.To.Y && board.GetFigureXY(fm.To.X, fm.To.Y - 1) == Figure.None)))
                {
                    return true;
                }
            }
            if (fm.From.Y == 6)//6 - Означает ситуацию, когда текущая (черная) пешка на старте
            {
                if (fm.From.X == fm.To.X && (fm.From.Y - 1 == fm.To.Y || (fm.From.Y - 2 == fm.To.Y && board.GetFigureXY(fm.To.X, fm.To.Y + 1) == Figure.None)))
                {
                    return true;
                }
            }

            return false;
        }

        private bool MovePawnNotFromTheStart()
        {
            if (fm.From.Figure == Figure.WhitePawn)//Белая пешка не на старте
            {
                if (fm.From.X == fm.To.X && (fm.From.Y + 1 == fm.To.Y))
                {
                    return true;
                }
            }
            if (fm.From.Figure == Figure.BlackPawn)//Черная пешка не на старте
            {
                if (fm.From.X == fm.To.X && (fm.From.Y - 1 == fm.To.Y))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsThereFigureOnToCell()
        {
            return board.GetFigureAt(fm.To) != Figure.None;
        }
    }
}
