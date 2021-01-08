using GameLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF
{
    class ScreenDisplay : MainWindow, IDisplay
    {
        public void Clear()
        {
            GridBoard.Children.Clear();
        }

        public void DisplayBoard(Game game)
        {
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri(@$"../../../Спрайты/{game.GetFigureXY(x, y)}.png", UriKind.Relative));

                    ImageBrush myBrush = new ImageBrush();
                    myBrush.ImageSource = image.Source;


                    TextBox textbox = new TextBox();
                    textbox.Background = myBrush;

                    GridBoard.Children.Add(textbox);//Конечное добавление полученного изображения, взятой из папки с игрой
                }
            }
        }

        public void DisplayFinish(Game game)
        {
            Clear();

            game.DisplayGame();

            MessageBox.Show("Мат: Победили " + game.MoveColor, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

            FileService saveGameFile = new FileService("ResultGame.txt");

            saveGameFile.SaveFile(game.MoveColor);
        }

        public void DisplayMoves(Game game)
        {
            List<string> list = game.GetAllMoves();

            foreach (string moves in list)
            {
                MovesBox.Text += moves + "\t";
            }
        }

        public string ReadLine()
        {
            return MoveEnter.Text;
        }

        public void WriteLine(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
