using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLogic;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GameLogic.Tests
{
    [TestClass()]
    public class FileServiceTests
    {
        #region//ReadFileMethod
        [TestMethod()]
        public void ReadFileTest()
        {
            FileService file = new FileService("Такого файла нет");
            file.Fen = file.ReadFile();

            Assert.AreEqual("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR", file.Fen);
            //Значит что текущий FileService создал файл, и получил игровую строку с этого файла
        }

        [TestMethod()]
        public void ReadFileNoDirectionTest()
        {
            FileService file = new FileService(null);
            Assert.ThrowsException<DirectoryNotFoundException>(() => file.ReadFile());
        }
        #endregion

        #region//SaveFileMethod
        [TestMethod()]
        public void SaveFileNoDirectionTest()
        {
            FileService file = new FileService(null);
            Assert.ThrowsException<DirectoryNotFoundException>(() => file.SaveFile("White"));
        }

        [TestMethod()]
        public void SaveFileNoColorTest()
        {
            FileService file = new FileService("Board.txt");
            Assert.ThrowsException<ArgumentException>(() => file.SaveFile(null));
        }
        #endregion
    }
}