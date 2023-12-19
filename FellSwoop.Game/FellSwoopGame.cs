using FellSwoop.Game.Models;

namespace FellSwoop.Game
{
    public class FellSwoopGame
    {
        public FellSwoopGame(int width, int height)
        {
            Grid = new Grid(width, height);
        }

        public Grid Grid { get; }

        public IEnumerable<Coordinates> ConnectedNeighbours(Coordinates startPosition)
        {
            var seen = new HashSet<Coordinates>();
            var queue = new Queue<Coordinates>();

            // Add immediate neighbours of start position to queue
            foreach (var neighbour in Grid.Neighbours(startPosition))
            {
                seen.Add(neighbour);
                queue.Enqueue(neighbour);
            }

            // If next item on queue has a value matching that of the start point yield value and add its neighbours to queue 
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (Grid.AtPosition(current) !=
                    Grid.AtPosition(startPosition)) continue;

                yield return current;

                foreach (var neighbourToInvestigate in Grid.Neighbours(current)
                             .Where(neighbour => !seen.Contains(neighbour)))
                {
                    queue.Enqueue(neighbourToInvestigate);
                    seen.Add(neighbourToInvestigate);
                }
            }
        }

        public IEnumerable<ChangingCoordinates> MovementFromColumn(Coordinates startPosition)
        {
            var connectedNeighbours = ConnectedNeighbours(startPosition).ToHashSet();
            var connectedNeighboursByColumn = connectedNeighbours
                .GroupBy(x => x.X)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(c => c.Y).ToList()
                );

            foreach (var x in connectedNeighbours.Select(x => x.X).Distinct().Order())
            {
                var connectedNeighboursSameColumn = new CoordinateCollection(connectedNeighboursByColumn[x]);

                var needsToMove = new CoordinateCollection(connectedNeighboursSameColumn
                    .SelectMany(InColumnAbove)
                    .Where(c => !connectedNeighbours.Contains(c))
                    .Where(c => c != startPosition)
                    .Distinct()
                    .ToArray());

                foreach (var mover in needsToMove)
                {
                    var removedBelow = connectedNeighboursSameColumn.Below(mover).Count();
                    yield return new ChangingCoordinates(mover, new Coordinates(mover.X, mover.Y - removedBelow));
                }

                Console.WriteLine();
            }
        }

        private IEnumerable<Coordinates> InColumnAbove(Coordinates coordinates)
        {
            var x = coordinates.X;

            for (var y = coordinates.Y; y < Grid.Height; y++)
                if (Grid.AtPosition(coordinates.X, y) != CellType.None)
                    yield return new Coordinates(coordinates.X, y);
        }

        private IEnumerable<Coordinates> InColumnBelow(Coordinates coordinates)
        {
            var x = coordinates.X;

            for (var y = 0; y < coordinates.Y; y++)
                if (Grid.AtPosition(coordinates.X, y) != CellType.None)
                    yield return new Coordinates(coordinates.X, y);
        }

        private static ChangingCoordinates ChangingCoordinates(Coordinates coordinates, int distance)
        {
            return new ChangingCoordinates(coordinates, new Coordinates(coordinates.X, coordinates.Y - distance));
        }
    }
}