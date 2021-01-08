using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLogic;
using Moq;
using System;

namespace GameLogic.Tests
{
    [TestClass()]
    public class GameTests
    {
        #region//Constructor
        [TestMethod()]
        public void GameTest()//Прямое выполнение кода, с правильными данными
        {
            Game game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);
            Assert.IsNotNull(game);
        }

        [TestMethod()]
        public void GameParametreStringTest()//Строка имеет неправильный формат
        {
            Game game;
            Assert.ThrowsException<ArgumentException>(() => game = new Game("abs", "White", new Mock<IDisplay>().Object));
            Assert.ThrowsException<NullReferenceException>(() => game = new Game(null, "White", new Mock<IDisplay>().Object));
        }

        [TestMethod()]
        public void GameParametreMoveColorTest()//MoveColor имеет неправильный формат
        {
            Game game;
            Assert.ThrowsException<ArgumentException>(() => game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "What", new Mock<IDisplay>().Object));
            Assert.ThrowsException<NullReferenceException>(() => game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", null, new Mock<IDisplay>().Object));
        }

        [TestMethod()]
        public void GameParametreDisplayTest()//Diplay имеет неправильный формат
        {
            Game game;
            Assert.ThrowsException<NullReferenceException>(() => game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", null));
        }

        [TestMethod()]
        public void GameAllParametresTest()//Параметры равны null
        {
            Game game;
            Assert.ThrowsException<NullReferenceException>(() => game = new Game(null, null, null));//Все параметры = null
            Assert.ThrowsException<NullReferenceException>(() => game = new Game(null, null, null));
        }

        [TestMethod()]
        public void GameTwoParametresTest()//Два параметра равны null
        {
            Game game;
            Assert.ThrowsException<NullReferenceException>(() => game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", null, null));
            Assert.ThrowsException<NullReferenceException>(() => game = new Game(null, "White", null));
            Assert.ThrowsException<NullReferenceException>(() => game = new Game(null, null, new Mock<IDisplay>().Object));
        }
        #endregion

        #region//DisplayGameMethod
        [TestMethod()]
        public void DisplayGameTest()
        {
            Game game = null;
            Assert.ThrowsException<NullReferenceException>(() => game.DisplayGame());
        }
        #endregion

        #region//MoveMethod
        [TestMethod()]
        public void MoveIsRightTest()//Проверка правильного (ожидаемого) хода
        {
            Game game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);
            game.Move("Pa2a4", "White");//Делаем ход

            Game ComparisonGame = new Game("rnbqkbnr/pppppppp/......../......../P......./......../.PPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);
            //Для сравнения того чтобы мы должны были получить, и что получили при работе метода выше)
            Assert.AreEqual(game.Fen, ComparisonGame.Fen);
        }

        [TestMethod()]
        public void MoveCellNoBoardTest()//Проверка хода при отсутствии введенных координат
        {
            Game game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);
            Assert.ThrowsException<ArgumentException>(() => game.Move("Pa2a9", "White"));//Делаем ход);
            Assert.ThrowsException<ArgumentException>(() => game.Move("Paaa4", "White"));//Делаем ход);
        }

        [TestMethod()]
        public void MoveFigureNoBoardTest()//Проверка хода при отсутствии фигуры
        {
            Game game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);
            Assert.ThrowsException<ArgumentException>(() => game.Move("Wa2a4", "White"));//Делаем ход);
        }

        [TestMethod()]
        public void MoveIsRightButMoveColorIsWrongTest()//Проверка правильного (ожидаемого) хода но подходящий под цвет
        {
            Game game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);
            Assert.ThrowsException<ArgumentException>(() => game.Move("pa7a6", "White"));
            Assert.ThrowsException<ArgumentException>(() => game.Move("Pa2a3", "Black"));
        }
        [TestMethod()]
        public void MoveIsWrongTest()//Проверка неправильного хода
        {
            Game game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);
            Assert.ThrowsException<ArgumentException>(() => game.Move("errormove", "White"));
            Assert.ThrowsException<NullReferenceException>(() => game.Move(null, "White"));
        }
        #endregion


        #region//GetFigureXYMethod
        [TestMethod()]
        public void GetFigureXYTest()
        {
            Game game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);

            Figure FirstExpected = Figure.WhiteRook;
            Assert.AreEqual(FirstExpected, game.GetFigureXY(0, 0));

            Figure SecondExpected = Figure.None;
            Assert.AreEqual(SecondExpected, game.GetFigureXY(2, 2));

            Assert.ThrowsException<IndexOutOfRangeException>(() => game.GetFigureXY(-1, -1));
            Assert.ThrowsException<IndexOutOfRangeException>(() => game.GetFigureXY(50, 50));
        }
        #endregion

        #region//GetAllMovesMethod
        [TestMethod()]
        public void GetAllMovesTest()
        {
            Game game = null;
            Assert.ThrowsException<NullReferenceException>(() => game.GetAllMoves());
        }
        #endregion
    }
}
