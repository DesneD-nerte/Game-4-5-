using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GameLogic;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Tests
{
    [TestClass()]
    public class BoardTests//Доска игры 
    {
        public string Fen { get; set; } = "rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR";

        Cell[,] Figures = new Cell[8, 8];//Набор клеток с фигурами

        public List<Figure> WhiteFigures = new List<Figure>() { Figure.WhiteKing, Figure.WhiteQueen, Figure.WhiteRook, Figure.WhiteBishop, Figure.WhiteKnight, Figure.WhitePawn };
        public List<Figure> BlackFigures = new List<Figure>() { Figure.BlackKing, Figure.BlackQueen, Figure.BlackRook, Figure.BlackBishop, Figure.BlackKnight, Figure.BlackPawn };

        #region//Constructor
        //Проверка конструктора, в котором вызывается метод инициализации фигур
        [TestMethod()]
        public void BoardTest()//Проверка правильности заполнения поля фигурами (
        {
            Cell[,] FiguresExpected = new Cell[8, 8]//Ожидаемое положения и фигуры на данных клетках
            {
                { new Cell(0, 0, Figure.WhiteRook),    new Cell(0, 1, Figure.WhitePawn), new Cell(0, 2, Figure.None),  new Cell(0, 3, Figure.None), new Cell(0, 4, Figure.None),  new Cell(0, 5, Figure.None), new Cell(0, 6, Figure.BlackPawn),  new Cell(0, 7, Figure.BlackRook)},
                { new Cell(1, 0, Figure.WhiteKnight),  new Cell(1, 1, Figure.WhitePawn), new Cell(1, 2, Figure.None),  new Cell(1, 3, Figure.None), new Cell(1, 4, Figure.None),  new Cell(1, 5, Figure.None), new Cell(1, 6, Figure.BlackPawn),  new Cell(1, 7, Figure.BlackKnight)},
                { new Cell(2, 0, Figure.WhiteBishop),  new Cell(2, 1, Figure.WhitePawn), new Cell(2, 2, Figure.None),  new Cell(2, 3, Figure.None), new Cell(2, 4, Figure.None),  new Cell(2, 5, Figure.None), new Cell(2, 6, Figure.BlackPawn),  new Cell(2, 7, Figure.BlackBishop)},
                { new Cell(3, 0, Figure.WhiteQueen),   new Cell(3, 1, Figure.WhitePawn), new Cell(3, 2, Figure.None),  new Cell(3, 3, Figure.None), new Cell(3, 4, Figure.None),  new Cell(3, 5, Figure.None), new Cell(3, 6, Figure.BlackPawn),  new Cell(3, 7, Figure.BlackQueen)},
                { new Cell(4, 0, Figure.WhiteKing),    new Cell(4, 1, Figure.WhitePawn), new Cell(4, 2, Figure.None),  new Cell(4, 3, Figure.None), new Cell(4, 4, Figure.None),  new Cell(4, 5, Figure.None), new Cell(4, 6, Figure.BlackPawn),  new Cell(4, 7, Figure.BlackKing)},
                { new Cell(5, 0, Figure.WhiteBishop),  new Cell(5, 1, Figure.WhitePawn), new Cell(5, 2, Figure.None),  new Cell(5, 3, Figure.None), new Cell(5, 4, Figure.None),  new Cell(5, 5, Figure.None), new Cell(5, 6, Figure.BlackPawn),  new Cell(5, 7, Figure.BlackBishop)},
                { new Cell(6, 0, Figure.WhiteKnight),  new Cell(6, 1, Figure.WhitePawn), new Cell(6, 2, Figure.None),  new Cell(6, 3, Figure.None), new Cell(6, 4, Figure.None),  new Cell(6, 5, Figure.None), new Cell(6, 6, Figure.BlackPawn),  new Cell(6, 7, Figure.BlackKnight)},
                { new Cell(7, 0, Figure.WhiteRook),    new Cell(7, 1, Figure.WhitePawn), new Cell(7, 2, Figure.None),  new Cell(7, 3, Figure.None), new Cell(7, 4, Figure.None),  new Cell(7, 5, Figure.None), new Cell(7, 6, Figure.BlackPawn),  new Cell(7, 7, Figure.BlackRook)},
            };

            Board Board = new Board(Fen);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Assert.AreEqual(Board.GetFigureXY(i, j), FiguresExpected[i, j].Figure);
                }
            }
        }

        [TestMethod()]
        public void BoardLess64SymbolsTest()//Неправильная строка ввода, менее 64 символов
        {
            Fen = "rnbr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR";
            Board Board;

            Assert.ThrowsException<IndexOutOfRangeException>(() => Board = new Board(Fen));
        }

        [TestMethod()]
        public void BoardMore64SymbolsTest()//Неправильная строка ввода, более 64 символов
        {
            Fen = "rnbqkbnr/pppppppp/......../......../......../......../......../PPPPPPPP/RNBQKBNR";
            Board Board;

            Assert.ThrowsException<ArgumentException>(() => Board = new Board(Fen));
        }

        [TestMethod()]
        public void BoardIncorrectFiguresTest()//Несуществующие символы фигур
        {
            Fen = "ТЕСТkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR";
            Board Board1;
            Assert.ThrowsException<ArgumentException>(() => Board1 = new Board(Fen));

            Fen = "rnbqkbnr/pppTECTp/......../......../......../......../PPPPPPPP/RNBQKBNR";
            Board Board2;
            Assert.ThrowsException<ArgumentException>(() => Board2 = new Board(Fen));

            Fen = "ТЕСТkbnr/pppppppp/......../......../......../....TECT/PPPPPPPP/RNBQKBNR";
            Board Board3;
            Assert.ThrowsException<ArgumentException>(() => Board3 = new Board(Fen));
        }

        [TestMethod()]
        public void BoardEmptyFenTest()//Пустая строка
        {
            string Fen = "";
            Board Board;

            Assert.ThrowsException<ArgumentException>(() => Board = new Board(Fen));
        }

        [TestMethod()]
        public void BoardNullFenTest()
        {
            string Fen = null;
            Board Board;

            Assert.ThrowsException<NullReferenceException>(() => Board = new Board(Fen));
        }
        #endregion

        #region//NewFenMethod
        [TestMethod()]
        public void NewFenTest()//Построение такой же строки Fen какая и была проинициализирована 
        {
            Board Board = new Board(Fen);

            Board.Fen = "";//Обнуление строки
            Board.UpdateCurrentFen();

            Assert.AreEqual(Fen, Board.Fen);
        }
        #endregion

        #region//GetFigureAtMethod
        [TestMethod()]
        public void GetFigureAtWhiteFigureTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(0, 1, Figure.WhitePawn);

            Assert.AreEqual(FindCell.Figure, Board.GetFigureAt(FindCell));
        }

        [TestMethod()]
        public void GetFigureAtFigureNoneTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(4, 2, Figure.None);

            Assert.AreEqual(FindCell.Figure, Board.GetFigureAt(FindCell));
        }

        [TestMethod()]
        public void GetFigureAtBlackFigureTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(0, 7, Figure.BlackRook);

            Assert.AreEqual(FindCell.Figure, Board.GetFigureAt(FindCell));
        }

        [TestMethod()]
        public void GetFigureAtFigureOutBoundTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(8, 9);

            Assert.AreEqual(Figure.None, Board.GetFigureAt(FindCell));
        }

        [TestMethod()]
        public void GetFigureAtFirstParameterLessZeroTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(-1, 0);

            Assert.AreEqual(Figure.None, Board.GetFigureAt(FindCell));
        }

        [TestMethod()]
        public void GetFigureAtSecondParameterLessZeroTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(0, -1);
            
            Assert.AreEqual(Figure.None, Board.GetFigureAt(FindCell));
        }

        [TestMethod()]
        public void GetFigureAtBothParameterLessZeroTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(-1, -1);

            Assert.AreEqual(Figure.None, Board.GetFigureAt(FindCell));
        }
        #endregion

        #region//GetFigureXYMethod
        [TestMethod()]
        public void GetFigureXYWhiteFigureTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(0, 1, Figure.WhitePawn);

            Assert.AreEqual(FindCell.Figure, Board.GetFigureXY(0, 1));
        }

        [TestMethod()]
        public void GetFigureXYFigureNoneTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(4, 2, Figure.None);

            Assert.AreEqual(FindCell.Figure, Board.GetFigureXY(4, 2));
        }

        [TestMethod()]
        public void GetFigureXYBlackFigureTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(0, 7, Figure.BlackRook);

            Assert.AreEqual(FindCell.Figure, Board.GetFigureXY(0, 7));
        }

        [TestMethod()]
        public void GetFigureXYFigureOutBoundTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(8, 9);

            Assert.ThrowsException<IndexOutOfRangeException>(() => Board.GetFigureXY(8, 9));
        }

        [TestMethod()]
        public void GetFigureXYFirstParameterLessZeroTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(-1, 0);

            Assert.ThrowsException<IndexOutOfRangeException>(() => Board.GetFigureXY(-1, 0));
        }

        [TestMethod()]
        public void GetFigureXYSecondParameterLessZeroTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(0, -1);

            Assert.ThrowsException<IndexOutOfRangeException>(() => Board.GetFigureXY(0, -1));
        }

        [TestMethod()]
        public void GetFigureXYBothParameterLessZeroTest()
        {
            Board Board = new Board(Fen);
            Cell FindCell = new Cell(-1, -1);

            Assert.ThrowsException<IndexOutOfRangeException>(() => Board.GetFigureXY(-1, -1));
        }
        #endregion

        #region//MoveMethod
        [TestMethod()]
        public void MoveWhiteTest()
        {
            Board Board = new Board(Fen);

            FigureMoving Fm = new FigureMoving(Board);
            Fm.GetCellsAndFigures("Pa2a4");

            Board NewBoard = Board.Move(Fm);//Делаем ход
            Board.Fen = "rnbqkbnr/pppppppp/......../......../P......./......../.PPPPPPP/RNBQKBNR";//Поле которое нужно получить

            Assert.AreEqual(NewBoard.Fen, Board.Fen);//Сравниваем поля
        }

        [TestMethod()]
        public void MoveBlackTest()
        {
            Board Board = new Board(Fen);

            FigureMoving Fm = new FigureMoving(Board);
            Fm.GetCellsAndFigures("nb8c6");

            Board NewBoard = Board.Move(Fm);//Делаем ход
            Board.Fen = "r.bqkbnr/pppppppp/..n...../......../......../......../PPPPPPPP/RNBQKBNR";//Поле которое нужно получить

            Assert.AreEqual(NewBoard.Fen, Board.Fen);//Сравниваем поля
        }

        [TestMethod()]
        public void MoveFigureMovingIsNullTest()
        {
            Board Board = new Board(Fen);

            FigureMoving Fm = null;

            Assert.ThrowsException<NullReferenceException>(() => Board.Move(Fm));            
        }

        [TestMethod()]
        public void MoveNegativeFigureMovingTest()
        {
            Board Board = new Board(Fen);

            Cell FromCell = new Cell(-1, -1);
            Cell ToCell = new Cell(-1, -2);

            FigureMoving Fm = new FigureMoving(FromCell, ToCell);

            Assert.AreEqual(Board.Fen, Board.Move(Fm).Fen);//Поле никак не изменилось, хода не был сделан
        }
        #endregion

        #region//IsCheckMethod
        [TestMethod()]
        public void IsCheckWhiteFalseTest()
        {
            Board Board = new Board(Fen);
            Board.MoveColor = "White";

            Assert.IsFalse(Board.IsCheck());//В начальный момент игры белые не могут ударить черного короля
        }

        [TestMethod()]
        public void IsCheckBlackFalseTest()
        {
            Board Board = new Board(Fen);
            Board.MoveColor = "Black";

            Assert.IsFalse(Board.IsCheck());//В начальный момент игры черные не могут ударить черного короля
        }

        [TestMethod()]
        public void IsCheckWhiteTrueTest()
        {
            Board Board = new Board("rnbqkbnr/pppp.ppp/......../......../......../....Q.../PPPPPPPP/RNBQKBNR");
            Board.MoveColor = "White";

            Assert.IsTrue(Board.IsCheck());//В данный момент игры белый ферзь ставит шах черному королю
        }

        [TestMethod()]
        public void IsCheckBlackTrueTest()
        {
            Board Board = new Board("rnbqkbnr/pppppppp/....q.../......../......../......../PPPP.PPP/RNBQKBNR");
            Board.MoveColor = "Black";

            Assert.IsTrue(Board.IsCheck());//В данный момент игры черный ферзь ставит шах белому королю
        }
        #endregion

        #region//DetectColorMethod
        [TestMethod()]
        public void DetectColorTest()//Определение цвета, чтобы не есть своих
        {
            Board Board = new Board(Fen);

            Figure FigureBlackBishop = Figure.BlackBishop;
            Figure FigureWhiteBishop = Figure.WhiteBishop;
            Figure FigureNone = Figure.None;

            Assert.AreEqual("Black", Board.DetectColor(FigureBlackBishop));
            Assert.AreEqual("White", Board.DetectColor(FigureWhiteBishop));
            Assert.AreEqual("", Board.DetectColor(FigureNone));
        }
        #endregion

        #region//GetAllMethod
        [TestMethod()]
        public void GetAllMovesWhiteTest()//Всевозможные ходы белых
        {
            List<string> AllMoves = new List<string>()
            { "Ph2h4", "Ph2h3", "Pg2g4", "Pg2g3", "Pf2f4", "Pf2f3", "Pe2e4", "Pe2e3",
              "Pd2d4", "Pd2d3", "Pc2c4", "Pc2c3", "Pb2b4", "Pb2b3", "Pa2a4", "Pa2a3",
              "Ng1h3", "Ng1f3", "Nb1c3", "Nb1a3"
            };

            Board Board = new Board(Fen);
            Board.MoveColor = "White";

            Assert.AreEqual(AllMoves.Count, Board.GetAllMoves().Count);
            foreach (var item in Board.GetAllMoves())
            {
                Assert.IsTrue(AllMoves.Contains(item));
            }
        }

        [TestMethod()]
        public void GetAllMovesBlackTest()//Всевозможные ходы черных 
        {
            List<string> AllMoves = new List<string>()
            { "pa7a5", "pa7a6", "pb7b5", "pb7b6", "pc7c5", "pc7c6", "pd7d5","pd7d6",
              "pe7e5", "pe7e6", "pf7f5", "pf7f6", "pg7g5", "pg7g6", "ph7h5", "ph7h6",
              "nb8a6", "nb8c6", "ng8f6", "ng8h6"
            };

            Board Board = new Board(Fen);
            Board.MoveColor = "Black";

            Assert.AreEqual(AllMoves.Count, Board.GetAllMoves().Count);
            foreach (var item in Board.GetAllMoves())
            {
                Assert.IsTrue(AllMoves.Contains(item));
            }
        }

        [TestMethod()]
        public void GetAllMovesWhiteCheckBlackTest()//Ходы черных при постановке шаха белыми черным
        {
            List<string> AllMoves = new List<string>()
            { "pg7g6", "ke8e7" };

            Board Board = new Board("rnbqkbnr/pppp..pp/......../....pp.Q/....P.../P......./.PPP.PPP/RNB.KBNR");
            Board.MoveColor = "Black";

            Assert.AreEqual(AllMoves.Count, Board.GetAllMoves().Count);
            foreach (var item in Board.GetAllMoves())
            {
                Assert.IsTrue(AllMoves.Contains(item));
            }
        }

        [TestMethod()]
        public void GetAllMovesBlackCheckWhiteTest()//Ходы белых при постановке шаха черными белым
        {
            List<string> AllMoves = new List<string>()
            { "Nb1d2", "Nb1c3", "Bc1d2", "Qd1d2", "Pb2b4", "Pc2c3"};

            Board Board = new Board("rnb.kbnr/pp.ppppp/......../q.p...../P..P..../......../.PP.PPPP/RNBQKBNR");
            Board.MoveColor = "White";

            Assert.AreEqual(AllMoves.Count, Board.GetAllMoves().Count);
            foreach (var item in Board.GetAllMoves())
            {
                Assert.IsTrue(AllMoves.Contains(item));
            }
        }
        #endregion
    }
}
