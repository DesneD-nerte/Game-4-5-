using System;
using GameLogic;

namespace ChessConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IDisplay display = new ScreenDisplay();

            FileService gameFile = new FileService("Board.txt");

            gameFile.Fen = gameFile.ReadFile();

            Game game = new Game(gameFile.Fen, "White", display);//Начальная игра формируется из доски по полученным данным из файла

            while (true)
            {
                game.DisplayGame();

                while (true)
                {
                    string move = display.ReadLine();

                    if (game.GetAllMoves().Contains(move))
                    {
                        game.Move(move, game.MoveColor);
                        break;
                    }

                    display.WriteLine("Сделайте ход еще раз");
                }

                game.PrepareNextMove();
            }
        }
    }
}
