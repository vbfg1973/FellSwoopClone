using FellSwoop.Game.Models;
using FellSwoop.Game.Tests.Data.CoordinateCollectionData;
using FluentAssertions;

namespace FellSwoop.Game.Tests
{
    public class CoordinateCollectionTests
    {
        [Theory]
        [ClassData(typeof(CollectionIsAboveClassData))]
        public void CountCollection_Above_Comparison_Point(Coordinates[] collection, Coordinates c, int expectedCount)
        {
            var coordCollection = new CoordinateCollection(collection);

            coordCollection.Above(c).Should().HaveCount(expectedCount);
        }
        
        [Theory]
        [ClassData(typeof(CollectionIsBelowClassData))]
        public void CountCollection_Below_Comparison_Point(Coordinates[] collection, Coordinates comparison, int expectedCount)
        {
            var coordCollection = new CoordinateCollection(collection);

            coordCollection.Below(comparison).Should().HaveCount(expectedCount);
        }
    }
}