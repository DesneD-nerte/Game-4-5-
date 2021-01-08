using GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole
{
    class ScreenDisplay : IDisplay
    {
        public void Clear()
        {
            Console.Clear();
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void DisplayBoard(Game game)
        {
            string OutText = "   ---------------\n";
            for (int y = 7; y >= 0; y--)
            {
                OutText += y + 1;//Номер столбца, по которому идет работа с осью Y
                OutText += "| "; //Разграничение поля и номера

                for (int x = 0; x < 8; x++)
                {
                    OutText += (char)game.GetFigureXY(x, y) + " ";//Уточнение: если фигура отсутсвует, будет точка.
                }
                OutText += "\n";
            }
            OutText += "   ---------------\n";
            OutText += "   a b c d e f g h \n";//Ось X
            Console.WriteLine(OutText);
        }

        public void DisplayMoves(Game game)
        {
            List<string> list = game.GetAllMoves();
            foreach (string moves in list)
            {
                Console.Write(moves + "\t");
            }
        }

        public void DisplayFinish(Game game)
        {
            Console.WriteLine("Мат: Победили {0}", game.MoveColor);

            FileService SaveGameFile = new FileService("ResultGame.txt");

            SaveGameFile.SaveFile(game.MoveColor);
        }
    }
}
