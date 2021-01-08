using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Tests
{
    [TestClass()]
    public class MovesTests
    {
        string Fen = "rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR";//Инициализация поля//а то надоедает писать

        #region//MainCases       
        [TestMethod()]
        public void CanMoveWrongSideBlackTest()//Неправильный выбор фигуры (на a2 стоит не "p", а "P"
        {
            Board Board = new Board(Fen);
            Moves Moves = new Moves(Board);

            Cell From = new Cell(0, 1, Figure.BlackPawn);//Ход (pa2a4)
            Cell To = new Cell(0, 3, Figure.None);

            Assert.IsFalse(Moves.CanMove(new FigureMoving(From, To)));
        }

        [TestMethod()]
        public void CanMoveWrongSideWhiteTest()//Неправильный выбор фигуры (на a7 стоит не "P", а "p"
        {
            Board Board = new Board(Fen);
            Moves moves = new Moves(Board);

            Cell From = new Cell(0, 6, Figure.WhitePawn);//Ход (Pa7a5)
            Cell To = new Cell(0, 4, Figure.None);

            Assert.IsFalse(moves.CanMove(new FigureMoving(From, To)));
        }

        [TestMethod()]
        public void CanMoveNoFigureTest()//Отсутствие фигуры на клетке игре (Figure.none)
        {
            Board Board = new Board(Fen);
            Moves moves = new Moves(Board);

            Cell From = new Cell(0, 1, Figure.None);//Ход (.a2a4)
            Cell To = new Cell(0, 3, Figure.None);

            Assert.IsFalse(moves.CanMove(new FigureMoving(From, To)));
        }
        #endregion

        #region//MovesPawn
        [TestMethod()]
        public void CanMoveWrongMoveTest()//Невозможное перемещение пешки
        {
            Board Board = new Board(Fen);
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Pa2a5");
            Assert.IsFalse(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMoveRightMoveWhiteOneStepTest()//Правильное перемещение белых 
        {
            Board Board = new Board(Fen);
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Pa2a3");//На 1 ход вперед
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Pa2a4");//На 2 хода вперед
            Assert.IsTrue(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMoveRightMoveBlackOneStepTest()//Правильное перещение черных
        {
            Board Board = new Board(Fen);
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("pa7a6");//На 1 ход вперед
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("pa7a5");//На 2 хода вперед
            Assert.IsTrue(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMoveBeatFigureTest()//Атака фигуры 
        {
            Board Board = new Board("rnbqkbnr/pppppppp/......../p.p...../.P....../......../PPPPPPPP/RNBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Pb4a5");//Налево
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Pb4c5");//Направо
            Assert.IsTrue(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMoveBeatFriendlyFigureTest()//Атака дружественных фигур
        {
            Board Board = new Board("rnbqkbnr/pppppppp/......../R.Q...../.P....../......../PPPPPPPP/RNBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Pb4a5");//Налево
            Assert.IsFalse(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Pb4c5");//Направо
            Assert.IsFalse(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMovePawnJumpOverFigureTest()//Попытка пешки перепрыгнуть через фигуру, если она стоит на старте
        {
            Board Board = new Board("rnbqkbnr/pppppppp/P......./......../......../p......./PPPPPPPP/RNBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Pa2a4");//Белыми
            Assert.IsFalse(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("pa7a5");//Черными
            Assert.IsFalse(Moves.CanMove(Fm));
        }
        #endregion

        #region//MovesRook
        [TestMethod()]
        public void CanMoveRookJumpOverFigureTest()//Попытка ладьи перепрыгнуть через фигуры
        {
            Board Board = new Board("rnbqkbnr/pppppppp/......../p......./R....p../......../PPPPPPPP/.NBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Ra4a6");//По вертикали
            Assert.IsFalse(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Ra4g4");//По горизонтали
            Assert.IsFalse(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMoveRookBeatFriendlyFigureTest()//Попытка ладьи атаковать дружеские фигуры
        {
            Board Board = new Board(Fen);
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Ra1a2");
            Assert.IsFalse(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMoveRookRightMoveTest()//Перемещение ладьи
        {
            Board Board = new Board("rnbqkbnr/pppppppp/......../......../......../R......./.PPPPPPP/.NBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Ra3a4");//По вертикали
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Ra3b3");//По горизонтали
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Ra3b4");//По диагонали
            Assert.IsFalse(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMoveRookBeatFigureTest()//Атака ладьи
        {
            Board Board = new Board("rnbqkbnr/pppppppp/......../......../......../R....p../.PPPPPPP/.NBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Ra3a7");//По вертикали
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Ra3f3");//По горизонтали
            Assert.IsTrue(Moves.CanMove(Fm));
        }
        #endregion

        #region//MovesKnight
        [TestMethod()]
        public void CanMoveKnightMoveTest()//Перемещение коня
        {
            Board Board = new Board("rnbqkbnr/......../......../......../..N...../......../......../RNBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            foreach (var fm in AllKnightMoves(Fm))
            {
                Assert.IsTrue(Moves.CanMove(fm));
            }
        }

        [TestMethod()]
        public void CanMoveKnightBeatTeamFigureTest()//Попытка коня атаковать дружеские фигуры
        {
            Board Board = new Board("rnbqkbnr/pppppppp/PPPPPPPP/PPPPPPPP/PPNPPPPP/PPPPPPPP/PPPPPPPP/RNBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            foreach (var fm in AllKnightMoves(Fm))
            {
                Assert.IsFalse(Moves.CanMove(fm));
            }
        }

        [TestMethod()]
        public void CanMoveKnightBeatFigureTest()//Атака конем вражеских фигур
        {
            Board Board = new Board("rnbqkbnr/pppppppp/pppppppp/pppppppp/ppNppppp/pppppppp/pppppppp/RNBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            foreach (var fm in AllKnightMoves(Fm))
            {
                Assert.IsTrue(Moves.CanMove(fm));
            }
        }
        private static IEnumerable<FigureMoving> AllKnightMoves(FigureMoving fm)
        {
            fm.GetCellsAndFigures("Nc4a5");//Влево-Вверх
            yield return fm;

            fm.GetCellsAndFigures("Nc4a3");//Влево-Вниз
            yield return fm;

            fm.GetCellsAndFigures("Nc4b6");//Вверх-Влево
            yield return fm;

            fm.GetCellsAndFigures("Nc4d6");//Вверх-Вправо
            yield return fm;

            fm.GetCellsAndFigures("Nc4e5");//Вправо-Вверх
            yield return fm;

            fm.GetCellsAndFigures("Nc4e3");//Вправо-Вниз
            yield return fm;

            fm.GetCellsAndFigures("Nc4b2");//Вниз-Влево
            yield return fm;

            fm.GetCellsAndFigures("Nc4d2");//Вниз-Вправо
            yield return fm;
        }
        #endregion

        #region//MovesBishop
        [TestMethod()]
        public void CanMoveBishopBeatFigureTest()//Атака вражеских фигур
        {
            Board Board = new Board("rnbqkbnr/......../......../pppppppp/...B..../pppppppp/......../RNBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Bd4c5");//Вверх-влево по диагонали
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Bd4e5");//Вверх-вправо по диагонали
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Bd4c3");//Вниз-влево по диагонали
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Bd4e5");//Вниз-вправо по диагонали
            Assert.IsTrue(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMoveBishopMoveTest()//Перемещение слона
        {
            Board Board = new Board("rnbqkbnr/......../......../......../...B..../......../......../RNBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            foreach (var fm in AllBishopMoves(Fm))
            {
                Assert.IsTrue(Moves.CanMove(fm));
            }
        }

        [TestMethod()]
        public void CanMoveBishopJumpOverFigureTest()//Попытка слона перепрыгнуть через фигуры
        {
            Board Board = new Board("rnbqkbnr/......../......../..p.p.../...B..../..P.P.../......../RNBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            foreach (var fm in AllBishopMoves(Fm))
            {
                Assert.IsFalse(Moves.CanMove(fm));
            }
        }
        private static IEnumerable<FigureMoving> AllBishopMoves(FigureMoving fm)
        {
            fm.GetCellsAndFigures("Bd4b6");//Влево-Вверх
            yield return fm;

            fm.GetCellsAndFigures("Bd4f6");//Влево-Вниз
            yield return fm;

            fm.GetCellsAndFigures("Bd4b2");//Вверх-Влево
            yield return fm;

            fm.GetCellsAndFigures("Bd4f6");//Вверх-Вправо
            yield return fm;
        }
        #endregion

        #region//MovesQueen
        /// <summary>
        /// Ферзь принимает себя в качестве ладьи, то есть перенимает его возможность ходить по вертикали и горизонтали,
        /// если ход невозможен, принимает себя за слона, то есть проверяется возможность ходить по диагонали,
        /// оба эти способа покрывают возможность ходить на 1 клетку во все стороны вокруг ферзя
        /// </summary>
        [TestMethod()]
        public void CanMoveLikeRookJumpOverFigureTest()//Попытка ферзя перепрыгнуть через фигуры, выполняя ход как ладья
        {
            Board Board = new Board("rnbqkbnr/pppppppp/......../p......./Q....p../......../PPPPPPPP/RNB.KBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Qa4a6");//По вертикали
            Assert.IsFalse(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Qa4g4");//По горизонтали
            Assert.IsFalse(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMoveLikeRookBeatFriendlyFigureTest()//Попытка ферзя атаковать дружеские фигуры, выполняя ход как ладья
        {
            Board Board = new Board(Fen);
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Qd1d2");
            Assert.IsFalse(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMoveLikeRookRightMoveTest()//Перемещение ферзя, выполняя ход как ладья
        {
            Board Board = new Board("rnbqkbnr/pppppppp/......../......../......../Q......./.PPPPPPP/RNB.KBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Qa3a4");//По вертикали
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Qa3b3");//По горизонтали
            Assert.IsTrue(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMoveLikeRookBeatFigureTest()//Атака вражеских фигур ферзем, выполняя ход как ладья
        {
            Board Board = new Board("rnbqkbnr/pppppppp/......../......../......../Q....p../.PPPPPPP/RNB.KBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Qa3a7");//По вертикали
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Qa3f3");//По горизонтали
            Assert.IsTrue(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMoveLikeBishopBeatFigureTest()//Атака вражеских фигур ферзем, выполняя ход как слон
        {
            Board Board = new Board("rnbqkbnr/......../......../pppppppp/...Q..../pppppppp/......../RNBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Qd4c5");//Вверх-влево по диагонали
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Qd4e5");//Вверх-вправо по диагонали
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Qd4c3");//Вниз-влево по диагонали
            Assert.IsTrue(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Qd4e5");//Вниз-вправо по диагонали
            Assert.IsTrue(Moves.CanMove(Fm));
        }

        [TestMethod()]
        public void CanMoveLikeBishopMoveTest()//Перемещение ферзя, выполняя ход как слон
        {
            Board Board = new Board("rnbqkbnr/......../......../......../...Q..../......../......../RNBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            foreach (var fm in AllBishopMovesForQueen(Fm))
            {
                Assert.IsTrue(Moves.CanMove(fm));
            }
        }

        [TestMethod()]
        public void CanMoveLikeBishopJumpOverFigureTest()//Попытка ферзя перепрыгнуть через фигуры, выполняя ход как слон
        {
            Board Board = new Board("rnbqkbnr/......../......../..p.p.../...Q..../..P.P.../......../RNBQKBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            foreach (var fm in AllBishopMovesForQueen(Fm))
            {
                Assert.IsFalse(Moves.CanMove(fm));
            }
        }
        private static IEnumerable<FigureMoving> AllBishopMovesForQueen(FigureMoving fm)
        {
            fm.GetCellsAndFigures("Qd4b6");//Влево-Вверх
            yield return fm;

            fm.GetCellsAndFigures("Qd4f6");//Влево-Вниз
            yield return fm;

            fm.GetCellsAndFigures("Qd4b2");//Вверх-Влево
            yield return fm;

            fm.GetCellsAndFigures("Qd4f6");//Вверх-Вправо
            yield return fm;
        }

        #endregion

        #region//MovesKing
        [TestMethod()]
        public void CanMoveKingWrongMoveTest()//Возможные перемещения короля
        {
            Board Board = new Board("rnbqkbnr/pppppppp/......../....K.../......../......../PPPPPPPP/RNBQPBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            foreach (var fm in AllKingMoves(Fm))
            {
                Assert.IsTrue(Moves.CanMove(fm));
            }
        }

        [TestMethod()]
        public void CanMoveKingBeatFigureTest()//Атака фигуры 
        {
            Board Board = new Board("rnbqkbnr/pppppppp/pppppppp/ppppKppp/pppppppp/pppppppp/PPPPPPPP/RNBQPBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            foreach (var fm in AllKingMoves(Fm))
            {
                Assert.IsTrue(Moves.CanMove(fm));
            }
        }
        private static IEnumerable<FigureMoving> AllKingMoves(FigureMoving fm)
        {
            fm.GetCellsAndFigures("Ke5e6");
            yield return fm;

            fm.GetCellsAndFigures("Ke5f6");
            yield return fm;

            fm.GetCellsAndFigures("Ke5f5");
            yield return fm;

            fm.GetCellsAndFigures("Ke5f4");
            yield return fm;

            fm.GetCellsAndFigures("Ke5e4");
            yield return fm;

            fm.GetCellsAndFigures("Ke5d4");
            yield return fm;

            fm.GetCellsAndFigures("Ke5d5");
            yield return fm;

            fm.GetCellsAndFigures("Ke5d6");
            yield return fm;
        }
        [TestMethod()]
        public void CanMoveKingBeatFriendlyFigureTest()//Атака дружественных фигур
        {
            Board Board = new Board("rnbqkbnr/pppppppp/PPPPPPPP/PPPPKPPP/PPPPPPPP/PPPPPPPP/PPPPPPPP/RNBQPBNR");
            Moves Moves = new Moves(Board);

            FigureMoving Fm = new FigureMoving(Board);

            Fm.GetCellsAndFigures("Ke5e6");
            Assert.IsFalse(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Ke5f6");
            Assert.IsFalse(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Ke5f5");
            Assert.IsFalse(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Ke5f4");
            Assert.IsFalse(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Ke5e4");
            Assert.IsFalse(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Ke5d4");
            Assert.IsFalse(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Ke5d5");
            Assert.IsFalse(Moves.CanMove(Fm));

            Fm.GetCellsAndFigures("Ke5d6");
            Assert.IsFalse(Moves.CanMove(Fm));
        }
        #endregion
    }
}
