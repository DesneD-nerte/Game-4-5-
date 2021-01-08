using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameLogic;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();     
        }

        IDisplay display;
        Game game;
        MainWindow window;

        public void Start()
        {
            display = new ScreenDisplay();

            FileService gameFile = new FileService("Board.txt");//имя файла

            gameFile.Fen = gameFile.ReadFile();

            game = new Game(gameFile.Fen, "White", display);//Постройка начальной игры, доски по полученным данным из файла

            window = (MainWindow)display;
            window.game = game;
            window.display = display;

            window.Show();

            if (this != null)
            {
                this.Close();
                game.DisplayGame();
            }
        }

        public void MoveEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                string move = display.ReadLine();
                if (game.GetAllMoves().Contains(move))
                {
                    game.Move(move, game.MoveColor);

                    MovesBox.Clear();

                    game.PrepareNextMove();

                    game.DisplayGame();
                }
                else
                {
                    display.WriteLine("Сделайте ход еще раз");
                }

                MoveEnter.Text = "";
            }
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }
    }
}
