using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using GameLogic;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Tests
{
    [TestClass()]
    public class FigureMovingTests
    {
        string Fen = "rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR";

        #region//GetCellAndFigureMethod
        //Получение клетки "От" и клетки "Куда" и их фигуры
        [TestMethod()]
        public void GetCellAndFigureWhiteTest()//Правильное получение белой фигуры
        {
            Board Board = new Board(Fen);

            FigureMoving Fm = new FigureMoving(Board);
            FigureMoving FmExpected = Fm;

            Cell ExpectedCellX = new Cell(0, 1, Figure.WhitePawn);//Ожидаем пешку на a2
            Cell ExpectedCellY = new Cell(0, 3, Figure.None);//Не ожидаем фигуру на a4

            FmExpected.From = ExpectedCellX;
            FmExpected.To = ExpectedCellY;

            Fm.GetCellsAndFigures("Pa2a4");
            Assert.AreEqual(FmExpected, Fm);
        }

        [TestMethod()]
        public void GetCellAndFigureBlackTest()//Правильное получение черной фигуры
        {
            Board Board = new Board(Fen);

            FigureMoving Fm = new FigureMoving(Board);
            FigureMoving FmExpected = Fm;

            Cell ExpectedCellX = new Cell(0, 6, Figure.BlackPawn);//Ожидаем черную пешку на a7
            Cell ExpectedCellY = new Cell(0, 4, Figure.None);//Не ожидаем фигуру на a5

            FmExpected.From = ExpectedCellX;
            FmExpected.To = ExpectedCellY;

            Fm.GetCellsAndFigures("pa7a5");
            Assert.AreEqual(FmExpected, Fm);
        }

        [TestMethod()]
        public void GetCellAndFigureWrongFigureTest()//Неправильная фигура на начальной клетке
        {
            Board Board = new Board(Fen);

            FigureMoving Fm= new FigureMoving(Board);

            Assert.ThrowsException<ArgumentException>(() => Fm.GetCellsAndFigures("ra1a2"));
        }

        [TestMethod()]
        public void GetCellAndFigureNoFigureTest()//Отсутствие фигуры в параметре, параметр меньше 5 символов
        {
            Board Board = new Board(Fen);

            FigureMoving Fm = new FigureMoving(Board);

            Assert.ThrowsException<ArgumentException>(() => Fm.GetCellsAndFigures("a1a2"));
        }

        [TestMethod()]
        public void GetCellAndFigureWrongXTest()//Отсутствие координаты X на поле
        {
            Board Board = new Board(Fen);

            FigureMoving Fm = new FigureMoving(Board);

            Assert.ThrowsException<ArgumentException>(() => Fm.GetCellsAndFigures("Nb1a9"));
        }

        [TestMethod()]
        public void GetCellAndFigureWrongYTest()//Отсутствие координаты Y на поле
        {
            Board Board = new Board(Fen);

            FigureMoving Fm = new FigureMoving(Board);

            Assert.ThrowsException<ArgumentException>(() => Fm.GetCellsAndFigures("nb9a1"));
        }

        [TestMethod()]
        public void GetCellAndFigureWrongYAndXTest()//Отсутствие координаты X и Y на поле
        {
            Board Board = new Board(Fen);

            FigureMoving Fm = new FigureMoving(Board);

            Assert.ThrowsException<ArgumentException>(() => Fm.GetCellsAndFigures("nb9a9"));
        }

        [TestMethod()]
        public void GetCellAndFigureSymbolsMoreThan5Test()//Параметр имеет больше 5 символов
        {
            Board Board = new Board(Fen);

            FigureMoving Fm = new FigureMoving(Board);

            Assert.ThrowsException<ArgumentException>(() => Fm.GetCellsAndFigures("nb-1a2"));
        }

        [TestMethod()]
        public void GetCellAndFigureNoFigureAndNegativeCoordinateXTest()//Отсутствие фигуры и отрицательная координата, но зато 5 символов
        {
            Board Board = new Board(Fen);

            FigureMoving Fm = new FigureMoving(Board);

            Assert.ThrowsException<ArgumentException>(() => Fm.GetCellsAndFigures("b-1a2"));
        }

        [TestMethod()]
        public void GetCellAndFigureNoFigureAndNegativeCoordinateYTest()//Отсутствие фигуры и отрицательная координата, но зато 5 символов
        {
            Board Board = new Board(Fen);

            FigureMoving Fm = new FigureMoving(Board);

            Assert.ThrowsException<ArgumentException>(() => Fm.GetCellsAndFigures("b1a-2"));
        }
        #endregion
    }
}
