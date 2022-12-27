using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Coordinates
    {
        public int GetBoardIndexByCoordinates(string cords)
        {
            switch (cords)
            {
                case "A":
                    return 0;
                    break;
                case "B":
                    return 1;
                    break;
                case "C":
                    return 2;
                    break;
                case "D":
                    return 3;
                    break;
                case "E":
                    return 4;
                    break;
                case "F":
                    return 5;
                    break;
                case "G":
                    return 6;
                    break;
                case "H":
                    return 7;
                    break;
            }

            return -1;
        }
    }
}
