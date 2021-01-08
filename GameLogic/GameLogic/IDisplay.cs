using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public interface IDisplay
    {
        string ReadLine();
        void WriteLine(string message);
        void Clear();
        void DisplayBoard(Game game);
        void DisplayMoves(Game game);
        void DisplayFinish(Game game);
    }
}
