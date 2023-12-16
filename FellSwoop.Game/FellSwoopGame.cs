namespace FellSwoop.Game
{
    public record Coordinates(int X, int Y)
    {
    }

    public interface IFellSwoopGame
    {
        int Width { get; }
        int Height { get; }
        FellSwoopGame.CellType[,] Grid { get; }
        bool IsInsideGrid(Coordinates coordinates);
        IEnumerable<Coordinates> ConnectedNeighboursOfSameTypeAs(Coordinates coordinatesToInvestigate);
        IEnumerable<Coordinates> GridNeighbours(Coordinates coordinates);
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

            foreach (var neighbour in GridNeighbours(coordinatesToInvestigate))
            {
                seen.Add(neighbour);
                queue.Enqueue(neighbour);
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (CellTypeAtCoordinates(current) != CellTypeAtCoordinates(coordinatesToInvestigate)) continue;

                yield return current;

                foreach (var neighbour in GridNeighbours(current).Where(n => !seen.Contains(n)))
                {
                    queue.Enqueue(neighbour);
                    seen.Add(neighbour);
                }
            }
        }

        public void SetGrid(int x, int y, CellType setTo)
        {
            Grid[x, y] = setTo;
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

        private CellType CellTypeAtCoordinates(Coordinates coordinates)
        {
            return Grid[coordinates.X, coordinates.Y];
        }

        public bool IsInsideGrid(Coordinates coordinates)
        {
            return coordinates.X >= 0 && coordinates.X < Width && coordinates.Y >= 0 && coordinates.Y < Height;
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