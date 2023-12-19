using FellSwoop.Game.Models;
using FellSwoop.Game.Tests.Data.MovementData;
using FellSwoop.Game.Tests.Helpers;
using FluentAssertions;

namespace FellSwoop.Game.Tests
{
    public class ConnectedNeighbourTests
    {
        [Theory]
        [InlineData("10x20.4-17.3.txt")]
        [InlineData("10x20.5-5.5.txt")]
        public void Grid_Connected_Neighbours_Count_Correct_From_Files(string fileName)
        {
            var tp = FilenameHelpers.GridTestParametersFromFilename(fileName);

            var game = new FellSwoopGame(tp.Width, tp.Height);
            game.Grid.LoadFile(GetPath(fileName));

            game.Grid.Width.Should().Be(tp.Width);
            game.Grid.Height.Should().Be(tp.Height);

            game.ConnectedNeighbours(new Coordinates(tp.X, tp.Y))
                .Should()
                .HaveCount(tp.ConnectedNeighbourCount);
        }

        [Theory]
        [ClassData(typeof(MovementClassData))]
        public void Grid_Connected_Neighbours_Movement(string fileName,
            IEnumerable<ChangingCoordinates> expectedMovement)
        {
            var tp = FilenameHelpers.GridTestParametersFromFilename(fileName);

            var game = new FellSwoopGame(tp.Width, tp.Height);
            game.Grid.LoadFile(GetPath(fileName));

            game.Grid.Width.Should().Be(tp.Width);
            game.Grid.Height.Should().Be(tp.Height);

            var movementList = game.MovementFromColumn(new Coordinates(tp.X, tp.Y)).ToList();

            movementList.OrderByFrom().Should().Equal(expectedMovement.OrderByFrom());
        }

        [Theory]
        [InlineData(10, 20)]
        [InlineData(100, 200)]
        public void Grid_Full_Connected_Neighbour_Sweep(int width, int height)
        {
            var game = new FellSwoopGame(width, height);
            const CellType type = CellType.Blue;
            const CellType otherType = CellType.Red;

            for (var x = 0; x < game.Grid.Width; x++)
            for (var y = 0; y < game.Grid.Height; y++)
                game.Grid.SetTo(x, y, type);

            var coords = new Coordinates(5, 5);

            game
                .ConnectedNeighbours(coords)
                .Should()
                .HaveCount(width * height);

            game.Grid.SetTo(coords.X, coords.Y, otherType);

            game
                .ConnectedNeighbours(coords)
                .Should()
                .HaveCount(1);
        }

        private static string GetPath(string fileName)
        {
            var pathElements = new[] { "Resources", "Grids", fileName };

            return Path.Combine(pathElements);
        }
    }
}