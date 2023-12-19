using System.Collections;
using FellSwoop.Game.Models;

namespace FellSwoop.Game.Tests.Data.CoordinateCollectionData
{
    public class CollectionIsBelowClassData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new[]
                {
                    new Coordinates(1, 1),
                    new Coordinates(1, 2),
                    new Coordinates(1, 3),
                },
                new Coordinates(1, 4),
                3
            };

            yield return new object[]
            {
                new[]
                {
                    new Coordinates(1, 2),
                    new Coordinates(1, 3),
                    new Coordinates(1, 4),
                },
                new Coordinates(1, 1),
                0
            };

            yield return new object[]
            {
                new[]
                {
                    new Coordinates(1, 0),
                    new Coordinates(1, 1),
                    new Coordinates(1, 2),
                    new Coordinates(1, 4),
                    new Coordinates(1, 5),
                    new Coordinates(1, 6),
                    new Coordinates(1, 7),
                },
                new Coordinates(1, 3),
                3
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}