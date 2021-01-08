using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using GameLogic;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Tests
{
    [TestClass()]
    public class CellTests
    {
        #region//Constructors
        //Координаты клетки по названию клетки
        [TestMethod()]
        public void CellRightTest()//Правильное выполнение
        {
            Cell NewCell = new Cell("a2");
            int ExpectedX = 0;
            int ExpectedY = 1;

            Assert.AreEqual(ExpectedX, NewCell.X);
            Assert.AreEqual(ExpectedY, NewCell.Y);
        }

        [TestMethod()]
        public void CellBoundNumberTest()//Граничное значение числа
        {
            Cell NewCell = new Cell("a8");
            int ExpectedX = 0;
            int ExpectedY = 7;

            Assert.AreEqual(ExpectedX, NewCell.X);
            Assert.AreEqual(ExpectedY, NewCell.Y);
        }

        [TestMethod()]
        public void CellBoundSymbolTest()//Граничное значение символа
        {
            Cell NewCell = new Cell("h1");
            int ExpectedX = 7;
            int ExpectedY = 0;

            Assert.AreEqual(ExpectedX, NewCell.X);
            Assert.AreEqual(ExpectedY, NewCell.Y);
        }

        [TestMethod()]
        public void CellBoundNumberAndSymbolTest()//Граничные значения
        {
            Cell NewCell = new Cell("h8");
            int ExpectedX = 7;
            int ExpectedY = 7;

            Assert.AreEqual(ExpectedX, NewCell.X);
            Assert.AreEqual(ExpectedY, NewCell.Y);
        }

        //Координаты клетки за пределами поля
        [TestMethod()]
        public void CellOutNumberTest()//Выход числа за пределы
        {
            Cell NewCell;
            Assert.ThrowsException<ArgumentException>(() => NewCell = new Cell("a9"));
        }

        [TestMethod()]
        public void CellOutSymbolTest()//Выход символа за пределы
        {
            Cell NewCell;
            Assert.ThrowsException<ArgumentException>(() => NewCell = new Cell("i3"));
        }

        [TestMethod()]
        public void CellOutNumberAndSymbolTest()///Выход числа и символа за пределы
        {
            Cell NewCell;
            Assert.ThrowsException<ArgumentException>(() => NewCell = new Cell("i9"));
        }

        [TestMethod()]
        public void CellDoubleSymbol()//Два символа в параметре
        {
            Cell NewCell;
            Assert.ThrowsException<ArgumentException>(() => NewCell = new Cell("aa"));
        }

        [TestMethod()]
        public void CellDoubleNumber()//Два числа в параметре
        {
            Cell NewCell;
            Assert.ThrowsException<ArgumentException>(() => NewCell = new Cell("33"));
        }

        [TestMethod()]
        public void CellThreeParameter()//Длина параметра равна 3
        {
            Cell NewCell;
            Assert.ThrowsException<ArgumentException>(() => NewCell = new Cell("a3b"));
        }

        [TestMethod()]
        public void CellOneParameter()//Длина параметра равна 1
        {
            Cell NewCell;
            Assert.ThrowsException<ArgumentException>(() => NewCell = new Cell("a"));
        }

        [TestMethod()]
        public void CellZeroParameter()//Длина параметра равна 0
        {
            Cell NewCell;
            Assert.ThrowsException<ArgumentException>(() => NewCell = new Cell(""));
        }

        [TestMethod()]
        public void CellNullParameter()//Параметр равен null
        {
            Cell NewCell;
            Assert.ThrowsException<NullReferenceException>(() => NewCell = new Cell(null));
        }
        #endregion

        #region//OnBoardMethod
        //Проверка клеток на доске
        [TestMethod()]
        public void OnBoardTests()//Правильное выполнение
        {
            Cell NewCell = new Cell(2, 4);

            Assert.IsTrue(NewCell.OnBoard());
        }

        [TestMethod()]
        public void OnBoardBoundZeroAndZeroTests()//Граничное значение
        {
            Cell NewCell = new Cell(0, 0);

            Assert.IsTrue(NewCell.OnBoard());
        }

        [TestMethod()]
        public void OnBoardBoundOneAndOneTests()//Граничное значение
        {
            Cell NewCell = new Cell(1, 1);

            Assert.IsTrue(NewCell.OnBoard());
        }

        [TestMethod()]
        public void OnBoardFirstParameterOutTests()//Выход за пределы первого параметра
        {
            Cell NewCell = new Cell(8, 4);

            Assert.IsFalse(NewCell.OnBoard());
        }

        [TestMethod()]
        public void OnBoardSecondParameterOutTests()//Выход за пределы Второго параметра
        {
            Cell NewCell = new Cell(4, 8);

            Assert.IsFalse(NewCell.OnBoard());
        }

        [TestMethod()]
        public void OnBoardBothParameterOutTests()//Выход за пределы обоих параметров
        {
            Cell NewCell = new Cell(8, 8);

            Assert.IsFalse(NewCell.OnBoard());
        }

        [TestMethod()]
        public void OnBoardFirstParameterLessZeroTests()//Отрицательное значение первого параметра
        {
            Cell NewCell = new Cell(-1, 8);

            Assert.IsFalse(NewCell.OnBoard());
        }

        [TestMethod()]
        public void OnBoardSecondParameterLessZeroTests()//Отрицательное значение второго параметра
        {
            Cell NewCell = new Cell(4, -1);

            Assert.IsFalse(NewCell.OnBoard());
        }

        [TestMethod()]
        public void OnBoardBothParameterLessZeroTests()//Отрицательное значение обоих параметров
        {
            Cell NewCell = new Cell(-1, -1);

            Assert.IsFalse(NewCell.OnBoard());
        }
        #endregion
    }
}
