using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tic_tac_toe
{
    class Program
    {
//        Написать класс для игры в крестики-нолики: поле 3x3, два игрока по очереди ставят крестик и нолик.
//      Игра должна проверять корректность действий:
//      Не давать крестику или нолику сходить не в свой ход
//      Не давать сходить в занятую клетку
//      Не давать сделать ход после завершения игры
//      Игра должна иметь минимальный необходимый интерфейс и не давать сломать свои данные снаружи.
        static void Main(string[] args)
        {
            Field toe = new Field();

            toe.PrintField();
            Console.WriteLine();

            bool playerOne = true; //X
            bool playerTwo = false; //Y

            toe.DoStep(playerOne, 0, 0);
            //X _ _
            //_ _ _
            //_ _ _
            toe.DoStep(playerTwo, 1, 1);
            //X _ _
            //_ O _
            //_ _ _
            toe.DoStep(playerOne, 2, 2);
            //X _ _
            //_ O _
            //_ _ X
            toe.DoStep(playerTwo, 2, 0);
            //X _ _
            //_ O _
            //O _ X
            toe.DoStep(playerOne, 0, 2);
            //X _ X
            //_ O _
            //O _ X
            toe.DoStep(playerTwo, 1, 2);
            //X _ X
            //_ O O
            //O _ X
            toe.DoStep(playerOne, 0, 1);
            //X X X
            //_ O O
            //O _ X
            //toe.PrintField();


            Console.ReadLine();
        }
    }
}
