using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Figure
    {
        public string Color { get; set; }
        public FigureType FigureType { get; set; }
        public Figure(FigureType figure, string color)
        {
            FigureType = figure;
            Color = color;
        }


    }
}
