using System.Linq.Expressions;

namespace Chess
{
    public class ChessEngine
    {
        private Field[,] _board;
        private Coordinates _cords;
        private List<string> _validFields;
        public string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H" };
        private bool PlayerWhiteTurn = true;
        public ChessEngine()
        {
            _cords = new Coordinates();
            _board = new Field[8, 8];
            FillBoardWithFields();
            FillBoardWithFigures();
            DrawBoard();
        }

        private void FillBoardWithFields()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _board[i, j] = new Field();
                }
            }
        }

        private void FillBoardWithFigures()
        {
            for (int i = 0; i < 8; i++)
            {
                _board[i, 1].Figure = new Figure(FigureType.Pawn, "W");
                _board[i, 6].Figure = new Figure(FigureType.Pawn, "B");
            }
            _board[0, 0].Figure = new Figure(FigureType.Rook, "W");
            _board[1, 0].Figure = new Figure(FigureType.Knight, "W");
            _board[2, 0].Figure = new Figure(FigureType.Bishop, "W");
            _board[3, 0].Figure = new Figure(FigureType.Queen, "W");
            _board[4, 0].Figure = new Figure(FigureType.King, "W");
            _board[5, 0].Figure = new Figure(FigureType.Bishop, "W");
            _board[6, 0].Figure = new Figure(FigureType.Knight, "W");
            _board[7, 0].Figure = new Figure(FigureType.Rook, "W");

            _board[0, 7].Figure = new Figure(FigureType.Rook, "B");
            _board[1, 7].Figure = new Figure(FigureType.Knight, "B");
            _board[2, 7].Figure = new Figure(FigureType.Bishop, "B");
            _board[3, 7].Figure = new Figure(FigureType.Queen, "B");
            _board[4, 7].Figure = new Figure(FigureType.King, "B");
            _board[5, 7].Figure = new Figure(FigureType.Bishop, "B");
            _board[6, 7].Figure = new Figure(FigureType.Knight, "B");
            _board[7, 7].Figure = new Figure(FigureType.Rook, "B");

        }

        private void DrawBoard()
        {

            while (true)
            {

                int size = 8;

                const string top = " ---------------------------------";

                for (int y = 0; y < size; y++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" {0}", top);
                    Console.Write("{0} ", size - y);
                    for (int x = 0; x < size; x++)
                    {
                        Console.Write("|");
                        var fieldContent = _board[x, y].Figure != null ? GetFigureName(_board[x, y].Figure.FigureType) : "   ";
                        var isFigure = _board[x, y].Figure == null;
                        bool color = !isFigure ? _board[x, y].Figure.Color.Equals("W") : false;
                        if (color)
                        {
                            DrawInGreen(false, fieldContent);
                        }
                        else
                        {
                            DrawInBlue(false, fieldContent);
                        }

                    }
                    Console.WriteLine("|");
                }
                Console.WriteLine(" {0}", top);

                Console.Write("   ");
                for (int i = 0; i < size; i++)
                {
                    Console.Write(" {0}  ", letters[i]);
                }

                Move();
                Console.Clear();
            }
        }

        private void DrawInBlue(bool inLine, string content)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (inLine)
            {
                Console.WriteLine(content);
            }
            else
            {
                Console.Write(content);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void DrawInGreen(bool inLine, string content)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (inLine)
            {
                Console.WriteLine(content);
            }
            else
            {
                Console.Write(content);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private string GetFigureName(FigureType figure)
        {
            switch (figure)
            {
                case FigureType.Bishop:
                    return " Bi";
                    break;
                case FigureType.Rook:
                    return " Ro";
                    break;
                case FigureType.Pawn:
                    return " Pa";
                    break;
                case FigureType.King:
                    return " Ki";
                    break;
                case FigureType.Queen:
                    return " Qu";
                    break;
                case FigureType.Knight:
                    return " Kn";
                    break;
            }

            return "  ";
        }

        private Field[,] GetBoard()
        {
            return _board;
        }

        private void Move()
        {

            try
            {
                Console.WriteLine("");
                Console.Write("Old position: ");
                var oldCord = Console.ReadLine();
                var xOld = _cords.GetBoardIndexByCoordinates(oldCord[0].ToString());
                var yOld = ((Int32.Parse(oldCord[1].ToString()) - 8) * -1);

                ShowValidMovesForPawn(xOld, yOld);
                if (_validFields.Count > 0)
                {

                    Console.WriteLine("");
                    Console.Write("New position: ");
                    var newCord = Console.ReadLine();
                    string letter = newCord[0].ToString();
                    letter = letter.ToUpper();
                    newCord = letter + newCord[1];
                    if (_validFields.Contains(newCord))
                    {
                        var xNew = _cords.GetBoardIndexByCoordinates(newCord[0].ToString());
                        var yNew = ((Int32.Parse(newCord[1].ToString()) - 8) * -1);

                        if (CheckIfNewFieldIsValid(oldCord, newCord, xNew, yNew, xOld, yOld))
                        {
                            _board[xNew, yNew].Figure = _board[xOld, yOld].Figure;
                            if (yNew == 7 || yNew == 0)
                            {
                                ChangePawnToNewFigureIfAtTheEnd(xNew, yNew);
                            }
                            _board[xOld, yOld].Figure = null;
                            PlayerWhiteTurn = !PlayerWhiteTurn;
                        }
                    }
                }

                else
                {
                    Console.WriteLine("Ungültige Eingabe, bitte erneut versuchen");
                    Console.WriteLine("");
                    Move();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ungültige Eingabe, bitte erneut versuchen");
                Console.WriteLine("");
                Move();
            }

        }

        private void ShowValidMovesForBishop(int xOld, int yOld)
        {
            _validFields.Clear();


        }

        private int AddOrSub(string sign, int a, int b)
        {
            switch (sign)
            {
                case "+":
                    return a + b;
                    break;
                case "-":
                    return a - b;
                    break;
            }

            return 0;
        }

        private string GetFieldInFront(int yOld, string color)
        {
            if (color == "B") return ((yOld - 9) * -1).ToString();
            return (7 - yOld).ToString();
        }
        private void ShowValidMovesForPawn(int xOld, int yOld)
        {
            _validFields = new();
            _validFields.Clear();
            var sign = _board[xOld, yOld].Figure.Color == "B" ? "-" : "+";
            var color = _board[xOld, yOld].Figure.Color;

            GetFirstTwoFieldsForPawn(xOld, yOld, _validFields, sign, color);

            var figureToHitIsDifferentColor2 =
                xOld < 7 && _board[xOld + 1, AddOrSub(sign, yOld, 1)]?.Figure?.Color != _board[xOld, yOld]?.Figure?.Color;

            if (figureToHitIsDifferentColor2 && _board[xOld + 1, AddOrSub(sign, yOld, 1)]?.Figure != null)
            {
                var fieldCord1 = color == "B" ? ((yOld - 9) * -1).ToString() : (7 - yOld).ToString();
                _validFields.Add(letters[xOld + 1] + fieldCord1);
            }
            var figureToHitIsDifferentColor =
                xOld > 0 && _board[xOld - 1, AddOrSub(sign, yOld, 1)]?.Figure?.Color != _board[xOld, yOld]?.Figure?.Color;

            if (figureToHitIsDifferentColor && _board[xOld - 1, AddOrSub(sign, yOld, 1)]?.Figure != null)
            {
                var fieldCord1 = color == "B" ? ((yOld - 9) * -1).ToString() : (7 - yOld).ToString();
                _validFields.Add(letters[xOld - 1] + fieldCord1);
            }
            foreach (var validField in _validFields)
            {
                Console.WriteLine(validField);
            }
        }

        private void GetFirstTwoFieldsForPawn(int xOld, int yOld, List<string> validFields, string sign, string color)
        {
            if ((yOld == 6 && color == "B") || (yOld == 1 && color == "W"))
            {
                var fieldCord = AddOrSub(sign, yOld, color == "B" ? 10 : 14).ToString();
                validFields.Add(letters[xOld] + fieldCord[1]);
            }

            if (_board[xOld, AddOrSub(sign, yOld, 1)].Figure == null)
            {
                var fieldCord1 = GetFieldInFront(yOld, color);
                validFields.Add(letters[xOld] + fieldCord1);
            }
        }

        private bool CheckIfNewFieldIsValid(string oldCord, string newCord, int xNew, int yNew, int xOld, int yOld)
        {
            var isValid = !oldCord.Equals(newCord) && _board[xNew, yNew].Figure?.Color != _board[xOld, yOld]?.Figure?.Color;
            isValid = isValid && _board[xOld, yOld].Figure.FigureType == FigureType.Pawn &&
                      _board[xNew, yNew]?.Figure?.Color != _board[xOld, yOld].Figure.Color;
            return isValid;
        }

        private void ChangePawnToNewFigureIfAtTheEnd(int xNew, int yNew)
        {
            var isPawn = _board[xNew, yNew].Figure?.FigureType == FigureType.Pawn;
            if (yNew != 0 && yNew != 7 && !isPawn) return;
            var color = _board[xNew, yNew].Figure?.Color;
            if (isPawn && (color.Equals("B") || color.Equals("W")))
            {
                Console.WriteLine("Choose new Figure: ");
                Console.WriteLine("1: Rook");
                Console.WriteLine("2: Knight");
                Console.WriteLine("3: Bishop");
                Console.WriteLine("4: Queen");
                var input = Int32.Parse(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        _board[xNew, yNew].Figure.FigureType = FigureType.Rook;
                        break;
                    case 2:
                        _board[xNew, yNew].Figure.FigureType = FigureType.Knight;
                        break;
                    case 3:
                        _board[xNew, yNew].Figure.FigureType = FigureType.Bishop;
                        break;
                    case 4:
                        _board[xNew, yNew].Figure.FigureType = FigureType.Queen;
                        break;
                }
            }
        }
    }
}












