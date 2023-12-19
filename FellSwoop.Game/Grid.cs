using FellSwoop.Game.Abstract;
using FellSwoop.Game.Models;

namespace FellSwoop.Game
{
    public class Grid : IGrid
    {
        private readonly CellType[,] _cells;

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;

            _cells = new CellType[width, height];
            PopulateGrid();
        }

        public int Height { get; }
        public int Width { get; }

        public int Count => _cells.Length;

        public IEnumerable<CellType> Values => GetCellValues();

        public void SetTo(int x, int y, CellType cellType)
        {
            _cells[x, y] = cellType;
        }

        public CellType AtPosition(int x, int y)
        {
            return _cells[x, y];
        }

        public CellType AtPosition(Coordinates coordinates)
        {
            return AtPosition(coordinates.X, coordinates.Y);
        }

        public IEnumerable<Coordinates> PopulatedInColumn(int x)
        {
            for (var y = 0; y < Height; y++)
                if (_cells[x, y] != CellType.None)
                    yield return new Coordinates(x, y);
        }

        public IEnumerable<Coordinates> PopulatedInColumnAbove(Coordinates c)
        {
            return PopulatedInColumn(c.X)
                .Where(coord => coord.Y > c.Y);
        }

        public IEnumerable<Coordinates> Neighbours(Coordinates coordinates)
        {
            for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
            {
                var neighbour = new Coordinates(coordinates.X + x, coordinates.Y + y);

                if (IsInsideGrid(neighbour)) yield return neighbour;
            }
        }

        public bool IsInsideGrid(Coordinates coordinates)
        {
            return coordinates.X >= 0 && coordinates.X < Width && coordinates.Y >= 0 && coordinates.Y < Height;
        }

        public void LoadFile(string path)
        {
            var lines = File.ReadAllLines(path)
                .Select(line => line.Contains('#') ? line.Remove(line.IndexOf('#')) : line)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .Reverse()
                .ToArray();

            var hash = new HashSet<int>(lines.Select(x => x.Length));

            if (hash.Count > 1)
                throw new ArgumentException($"{path} has uneven lines - {string.Join(",", hash.Order())}");

            for (var x = 0; x < lines[0].Length; x++)
            for (var y = 0; y < lines.Length; y++)
                ParseFilePositionAndSet(x, y, lines[y][x]);
        }

        public IEnumerable<Coordinates> GetWholeColumn(Coordinates coordinates)
        {
            for (var y = 0; y < Height; y++)
            {
                var coord = new Coordinates(coordinates.X, y);

                if (IsInsideGrid(coord) && AtPosition(coord) != CellType.None) yield return coord;
            }
        }

        public static bool IsAbove(Coordinates coordinates, Coordinates reference)
        {
            return coordinates.X == reference.X && coordinates.Y > reference.Y;
        }

        public bool IsPopulated(Coordinates coordinates)
        {
            return AtPosition(coordinates) != CellType.None;
        }

        private IEnumerable<CellType> GetCellValues()
        {
            return _cells.Cast<CellType>();
        }

        private void ParseFilePositionAndSet(int x, int y, char c)
        {
            switch (c)
            {
                case 'R':
                    SetTo(x, y, CellType.Red);
                    break;
                case 'G':
                    SetTo(x, y, CellType.Green);
                    break;
                case 'B':
                    SetTo(x, y, CellType.Blue);
                    break;
                default:
                    SetTo(x, y, CellType.None);
                    break;
            }
        }

        private CellType CellTypeAtCoordinates(Coordinates coordinates)
        {
            return AtPosition(coordinates.X, coordinates.Y);
        }

        private void PopulateGrid()
        {
            var rand = new Random();
            var max = Enum.GetValues(typeof(CellType)).Length;

            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                SetTo(x, y, (CellType)rand.Next(1, max));
        }
    }
}