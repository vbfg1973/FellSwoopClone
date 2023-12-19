using System.Collections;
using FellSwoop.Game.Models;

namespace FellSwoop.Game.Tests.Data.MovementData
{
    public class MovementClassData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                "10x20.4-17.3.txt",
                new[]
                {
                    new ChangingCoordinates
                    (
                        new Coordinates(4, 19),
                        new Coordinates(4, 16)
                    )
                }
            };

            yield return new object[]
            {
                "10x20.4-15.6.txt",
                new[]
                {
                    new ChangingCoordinates
                    (
                        new Coordinates(4, 16),
                        new Coordinates(4, 14)
                    ),

                    new ChangingCoordinates
                    (
                        new Coordinates(4, 19),
                        new Coordinates(4, 15)
                    ),

                    new ChangingCoordinates
                    (
                        new Coordinates(5, 19),
                        new Coordinates(5, 16)
                    ),

                    new ChangingCoordinates
                    (
                        new Coordinates(5, 18),
                        new Coordinates(5, 15)
                    )
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}