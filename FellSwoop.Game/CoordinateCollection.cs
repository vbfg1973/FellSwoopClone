using FellSwoop.Game.Models;

namespace FellSwoop.Game
{
    public class CoordinateCollection : HashSet<Coordinates>
    {
        public CoordinateCollection(Coordinates[] coordinates) : base(coordinates)
        {
            ValidateCoordinates(coordinates);
        }
        
        public CoordinateCollection(List<Coordinates> coordinates) : base(coordinates)
        {
            ValidateCoordinates(coordinates);
        }

        private static void ValidateCoordinates(IEnumerable<Coordinates> coordinates)
        {
            if (coordinates.Select(x => x.X).Distinct().Count() > 1)
            {
                throw new ArgumentException("Too many different columns in collection data");
            }
        }

        public IEnumerable<Coordinates> Above(Coordinates coordinates)
        {
            return this.Where(z => z.Y > coordinates.Y);
        }
        
        public IEnumerable<Coordinates> Below(Coordinates coordinates)
        {
            return this.Where(z => z.Y < coordinates.Y);
        }
    }
}