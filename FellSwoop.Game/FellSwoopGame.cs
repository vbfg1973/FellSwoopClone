namespace FellSwoop.Game
{
    public record Coordinates(int X, int Y)
    {
    }


    public interface IFellSwoopGame
    {
        int Width { get; init; }
        int Height { get; init; }
        FellSwoopGame.CellType[,] Grid { get; init; }
        IEnumerable<Coordinates> ConnectedNeighboursOfSameTypeAs(Coordinates coordinatesToInvestigate);
        IEnumerable<Coordinates> GridNeighbours(Coordinates coordinates);
        bool IsInsideGrid(Coordinates coordinates);
        void SetGrid(int x, int y, FellSwoopGame.CellType setTo);
        void LoadFile(string path);
    }

    public class FellSwoopGame : IFellSwoopGame
    {
        public enum CellType
        {
            None = 0,
            Red = 1,
            Green = 2,
            Blue = 3
        }

        public FellSwoopGame(int width, int height)
        {
            Width = width;
            Height = height;

            Grid = new CellType[Width, Height];

            PopulateGrid();
        }

        public int Width { get; init; }
        public int Height { get; init; }

        public CellType[,] Grid { get; init; }

        public IEnumerable<Coordinates> ConnectedNeighboursOfSameTypeAs(Coordinates coordinatesToInvestigate)
        {
            var seen = new HashSet<Coordinates>();
            var queue = new Queue<Coordinates>();

            // Add immediate neighbours to queue
            foreach (var neighbour in GridNeighbours(coordinatesToInvestigate))
            {
                seen.Add(neighbour);
                queue.Enqueue(neighbour);
            }

            // If next item on queue matches start point yield value and add its neighbours to queue 
            while (queue.Count > 0)
            {
                var currentInvestigation = queue.Dequeue();
                if (CellTypeAtCoordinates(currentInvestigation) !=
                    CellTypeAtCoordinates(coordinatesToInvestigate)) continue;

                yield return currentInvestigation;

                foreach (var neighbourToInvestigate in GridNeighbours(currentInvestigation)
                             .Where(neighbour => !seen.Contains(neighbour)))
                {
                    queue.Enqueue(neighbourToInvestigate);
                    seen.Add(neighbourToInvestigate);
                }
            }
        }

        public IEnumerable<Coordinates> GridNeighbours(Coordinates coordinates)
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

        public void SetGrid(int x, int y, CellType setTo)
        {
            Grid[x, y] = setTo;
        }

        public void LoadFile(string path)
        {
            var lines = File.ReadAllLines(path).Select(x => x.Trim()).Reverse().ToArray();
            var hash = new HashSet<int>(lines.Select(x => x.Length));
            if (hash.Count > 1)
                throw new ArgumentException($"{path} has uneven lines - {string.Join(",", hash.Order())}");

            for (var x = 0; x < lines[0].Length; x++)
            {
                for (var y = 0; y < lines.Length; y++)
                {
                    ParseFilePositionAndSet(x, y, lines[y][x]);
                }
            }
        }

        private void ParseFilePositionAndSet(int x, int y, char c)
        {
            switch (c)
            {
                case 'R':
                    SetGrid(x, y, CellType.Red);
                    break;
                case 'G':
                    SetGrid(x, y, CellType.Green);
                    break;
                case 'B':
                    SetGrid(x, y, CellType.Blue);
                    break;
                default:
                    SetGrid(x, y, CellType.None);
                    break;
            }
        }

        private CellType CellTypeAtCoordinates(Coordinates coordinates)
        {
            return Grid[coordinates.X, coordinates.Y];
        }

        private void PopulateGrid()
        {
            var rand = new Random();
            var max = Enum.GetValues(typeof(CellType)).Length;

            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                SetGrid(x, y, (CellType)rand.Next(1, max));
        }
    }
}