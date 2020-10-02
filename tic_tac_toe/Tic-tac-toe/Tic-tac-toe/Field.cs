using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tic_tac_toe
{
    class Field
    {
        private string[,] field = new string[3, 3] { { "_", "_", "_" }, { "_", "_", "_" }, { "_", "_", "_" } };
        private bool lastPlayer;
        private int countcells;
        public Field()
        {
            lastPlayer = false;
            countcells = 0;
        }

        public void PrintField()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(field[i, j]);
                }
                Console.WriteLine();
            }
        }

        public void DoStep(bool player, int x, int y)
        {
            if(lastPlayer != player && countcells != 9)
            {
                if (field[x, y] == "_")
                {
                    if (countcells != 9)
                    {
                        switch (player)
                        {
                            case true:
                                field[x, y] = "X";
                                countcells++;
                                if (CheckWinners() == true)
                                {
                                    countcells = 9;
                                    Console.WriteLine("Х is winner! End of the Game.");
                                    PrintField();
                                }
                                break;
                            case false:
                                field[x, y] = "O";
                                countcells++;
                                if (CheckWinners() == true)
                                {
                                    countcells = 9;
                                    Console.WriteLine("Y is winner! End of the Game.");
                                    PrintField();
                                }
                                break;
                        }
                        lastPlayer = player;
                        if (countcells == 9 && CheckWinners() != true)
                        {
                            Console.WriteLine("The Field is full. End of the Game.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The Field is full. End of the Game.");
                    }
                }
                else
                {
                    Console.WriteLine("Can't do that step, choose another place");
                }
            }
            else if(countcells == 9)
            {
                Console.WriteLine("Game is over.");
            }
            else
            {
                Console.WriteLine("Not your step");
            }
        }

        public bool CheckWinners()
        {
            for(int i=0; i<3; i++)
            {
                if (field[i, 0] == field[i, 1] && field[i, 0] == field[0, 2] && field[i, 0] != "_")
                {
                    return true;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (field[0, i] == field[1, i] && field[0, i] == field[2, i] && field[0, i] != "_")
                {
                    return true;
                }
            }
            if (field[0, 0] == field[1, 1] && field[0, 0] == field[2, 2] && field[1, 1] != "_")
            {
                return true;
            }
            else if (field[0, 2] == field[1, 1] && field[0, 2] == field[2, 0] && field[1, 1] != "_")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
