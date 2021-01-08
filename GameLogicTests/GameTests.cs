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
        public void GameTest()//������ ���������� ����, � ����������� �������
        {
            Game game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);
            Assert.IsNotNull(game);
        }

        [TestMethod()]
        public void GameParametreStringTest()//������ ����� ������������ ������
        {
            Game game;
            Assert.ThrowsException<ArgumentException>(() => game = new Game("abs", "White", new Mock<IDisplay>().Object));
            Assert.ThrowsException<NullReferenceException>(() => game = new Game(null, "White", new Mock<IDisplay>().Object));
        }

        [TestMethod()]
        public void GameParametreMoveColorTest()//MoveColor ����� ������������ ������
        {
            Game game;
            Assert.ThrowsException<ArgumentException>(() => game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "What", new Mock<IDisplay>().Object));
            Assert.ThrowsException<NullReferenceException>(() => game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", null, new Mock<IDisplay>().Object));
        }

        [TestMethod()]
        public void GameParametreDisplayTest()//Diplay ����� ������������ ������
        {
            Game game;
            Assert.ThrowsException<NullReferenceException>(() => game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", null));
        }

        [TestMethod()]
        public void GameAllParametresTest()//��������� ����� null
        {
            Game game;
            Assert.ThrowsException<NullReferenceException>(() => game = new Game(null, null, null));//��� ��������� = null
            Assert.ThrowsException<NullReferenceException>(() => game = new Game(null, null, null));
        }

        [TestMethod()]
        public void GameTwoParametresTest()//��� ��������� ����� null
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
        public void MoveIsRightTest()//�������� ����������� (����������) ����
        {
            Game game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);
            game.Move("Pa2a4", "White");//������ ���

            Game ComparisonGame = new Game("rnbqkbnr/pppppppp/......../......../P......./......../.PPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);
            //��� ��������� ���� ����� �� ������ ���� ��������, � ��� �������� ��� ������ ������ ����)
            Assert.AreEqual(game.Fen, ComparisonGame.Fen);
        }

        [TestMethod()]
        public void MoveCellNoBoardTest()//�������� ���� ��� ���������� ��������� ���������
        {
            Game game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);
            Assert.ThrowsException<ArgumentException>(() => game.Move("Pa2a9", "White"));//������ ���);
            Assert.ThrowsException<ArgumentException>(() => game.Move("Paaa4", "White"));//������ ���);
        }

        [TestMethod()]
        public void MoveFigureNoBoardTest()//�������� ���� ��� ���������� ������
        {
            Game game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);
            Assert.ThrowsException<ArgumentException>(() => game.Move("Wa2a4", "White"));//������ ���);
        }

        [TestMethod()]
        public void MoveIsRightButMoveColorIsWrongTest()//�������� ����������� (����������) ���� �� ���������� ��� ����
        {
            Game game = new Game("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", "White", new Mock<IDisplay>().Object);
            Assert.ThrowsException<ArgumentException>(() => game.Move("pa7a6", "White"));
            Assert.ThrowsException<ArgumentException>(() => game.Move("Pa2a3", "Black"));
        }
        [TestMethod()]
        public void MoveIsWrongTest()//�������� ������������� ����
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
