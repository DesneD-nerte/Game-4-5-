using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class FileService
    {
        public string Fen { get; set; }//Строка, олицетворяющая игровое поле
        public string NameFile { get; set; }//Название файла

        public FileService(string nameFile)
        {
            this.NameFile = nameFile;
        }

        public string ReadFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + NameFile;
            string fen = string.Empty;

            if (!File.Exists(path))
            {
                CreateFile(path);
                fen = ReadFenFromFile(path);
            }
            else
            {
                fen = ReadFenFromFile(path);
            }

            if (IsValidFen(fen) == false || IsValidCountOfKings(fen) == false)
            {
                CreateFile(path);
                fen = ReadFenFromFile(path);
            }

            return fen;
        }


        private bool IsValidFen(string fen)
        {
            if (fen == null)
            {
                return false;
            }
            else
            {
                string[] parts = fen.Split(new char[] { '/' });//Разбитие на массив подстрок

                if (parts.Length != 8)
                {
                    return false;
                }

                foreach (string stroka in parts)//Анализ по каждой подстроке
                {
                    if (stroka.Length != 8)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsValidCountOfKings(string fen)
        {
            List<char> kings = new List<char>();

            if(AreThereKings(fen, kings))
            {
                if(CountOfKingsEqualsTwo(kings))
                {
                    return true;
                }
            }

            return false;
        }

        private bool AreThereKings(string fen, List<char> kings)
        {
            Board board = new Board();

            List<Figure> allFigure = board.GetAllFigure();

            if(FindKings(fen, kings, allFigure))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private bool FindKings (string fen, List<char> kings, List<Figure> allFigure)
        {
            foreach (char symbol in fen)
            {
                if (symbol == (char)Figure.WhiteKing || symbol == (char)Figure.BlackKing)
                {
                    kings.Add(symbol);
                }

                if (symbol != '/' && !allFigure.Contains((Figure)symbol))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Игра должна содержать только конкретно 2 короля, причем разных цветов
        /// </summary>
        private bool CountOfKingsEqualsTwo(List<char> kings)
        {
            if (kings.Count != 2 || kings[0] == kings[1])
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private string ReadFenFromFile(string path)
        {
            string fen = string.Empty;

            using (StreamReader stream = new StreamReader(path))
                fen = stream.ReadLine();

            return fen;
        }

        private void CreateFile(string path)
        {
            using (FileStream CurrentFile = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter streamwrite = new StreamWriter(CurrentFile))
                {
                    streamwrite.WriteLine("rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR");
                }
            }
        }

        public void SaveFile(string moveColor)
        {
            if (moveColor == null)
            {
                throw new ArgumentException();
            }

            string path = AppDomain.CurrentDomain.BaseDirectory + this.NameFile;

            using (FileStream CurrentFile = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter streamwrite = new StreamWriter(CurrentFile))
                {
                    streamwrite.WriteLine(moveColor);
                }
            }
        }
    }
}
