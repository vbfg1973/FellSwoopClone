namespace FellSwoop.Game
{
    public class FellSwoopGame
    {
        public FellSwoopGame(int width, int height)
        {
            Grid = new Grid(width, height);
        }

        public Grid Grid { get; set; }

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

        private static ChangingCoordinates ChangingCoordinates(Coordinates coordinates, int distance)
        {
            return new ChangingCoordinates(coordinates, new Coordinates(coordinates.X, coordinates.Y - distance));
        }
    }
}