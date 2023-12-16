using FluentAssertions;

namespace FellSwoop.Game.Tests
{
    public class GameGridTests
    {
        [Theory]
        [InlineData(10, 20)]
        [InlineData(100, 200)]
        public void Game_Dimensions_Are_Correct(int width, int height)
        {
            var game = new FellSwoopGame(width, height);

            // Correct number cells
            game
                .Width
                .Should()
                .Be(width);

            game
                .Height
                .Should()
                .Be(height);
        }

        [Theory]
        [InlineData(10, 20, 0, 0, 4)]
        [InlineData(10, 20, 9, 19, 4)]
        [InlineData(10, 20, 5, 0, 6)]
        [InlineData(10, 20, 5, 19, 6)]
        [InlineData(10, 20, 5, 5, 9)]
        [InlineData(10, 20, 5, 10, 9)]
        public void Neighbour_Count_Is_Correct(int width, int height, int x, int y, int expected)
        {
            var game = new FellSwoopGame(width, height);

            var coordinates = new Coordinates(x, y);

            game.GridNeighbours(coordinates)
                .Should()
                .HaveCount(expected);
        }


        [Theory]
        [InlineData(10, 20)]
        [InlineData(100, 200)]
        public void Coordinate_Is_Inside_Grid(int width, int height)
        {
            var game = new FellSwoopGame(width, height);

            for (var x = 0; x < game.Width; x++)
            {
                for (var y = 0; y < game.Height; y++)
                {
                    game
                        .IsInsideGrid(new Coordinates(x, y))
                        .Should()
                        .BeTrue();
                }
            }
        }

        [Theory]
        [InlineData(10, 20)]
        [InlineData(100, 200)]
        public void Grid_Full_Connected_Neighbour_Sweep(int width, int height)
        {
            var game = new FellSwoopGame(width, height);
            var type = FellSwoopGame.CellType.Blue;
            var otherType = FellSwoopGame.CellType.Red;

            for (var x = 0; x < game.Width; x++)
            {
                for (var y = 0; y < game.Height; y++)
                {
                    game.SetGrid(x, y, type);
                }
            }

            var coords = new Coordinates(5, 5);

            game
                .ConnectedNeighboursOfSameTypeAs(coords)
                .Should()
                .HaveCount(width * height);

            game.SetGrid(coords.X, coords.Y, otherType);

            game
                .ConnectedNeighboursOfSameTypeAs(coords)
                .Should()
                .HaveCount(1);
        }

        [Theory]
        [InlineData("10x20.5-5.3.txt")]
        [InlineData("10x20.5-5.5.txt")]
        public void Grid_Connected_Neighbours_Count_Correct_From_Files(string fileName)
        {
            var pathElements = new[] { "Resources", "Grids", fileName };

            var testDetails = fileName.Split(".").ToArray();
            var size = testDetails[0].Split("x").Select(x => Convert.ToInt32(x)).ToArray();

            var game = new FellSwoopGame(size[0], size[1]);
            game.LoadFile(Path.Combine(pathElements));

            game.Width.Should().Be(size[0]);
            game.Height.Should().Be(size[1]);

            var coords = testDetails[1].Split("-").Select(x => Convert.ToInt32(x)).ToArray();
            game.ConnectedNeighboursOfSameTypeAs(new Coordinates(coords[0], coords[1]))
                .Should()
                .HaveCount(Convert.ToInt32(testDetails[2]));
        }

        [Theory]
        [InlineData(10, 20)]
        [InlineData(100, 200)]
        public void Coordinate_Is_Outside_Grid(int width, int height)
        {
            var game = new FellSwoopGame(width, height);

            game.IsInsideGrid(new Coordinates(width, 0)).Should().BeFalse();

            game.IsInsideGrid(new Coordinates(0, height)).Should().BeFalse();

            game.IsInsideGrid(new Coordinates(width, height)).Should().BeFalse();
        }

        [Theory]
        [InlineData(10, 20)]
        [InlineData(100, 200)]
        public void Game_Array_Is_Generated_Correctly(int width, int height)
        {
            var game = new FellSwoopGame(width, height);

            // Correct number cells
            game
                .Grid
                .Length
                .Should()
                .Be(width * height);
        }

        [Theory]
        [InlineData(10, 20)]
        [InlineData(100, 200)]
        public void Game_Array_Type_Distribution_Is_Adequate(int width, int height)
        {
            var game = new FellSwoopGame(width, height);

            var typeCounts = Flatten(game.Grid)
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());

            // All types are in grid
            typeCounts
                .Values
                .Should()
                .HaveCount(Enum.GetValues(typeof(FellSwoopGame.CellType)).Length - 1);

            // Grid cells more than a quarter for each of the three types
            typeCounts
                .Values
                .All(x => x / Convert.ToDouble(width * height) >= 0.25)
                .Should()
                .BeTrue();
        }

        private IEnumerable<T> Flatten<T>(T[,] map)
        {
            for (var row = 0; row < map.GetLength(0); row++)
            for (var col = 0; col < map.GetLength(1); col++)
                yield return map[row, col];
        }
    }
}