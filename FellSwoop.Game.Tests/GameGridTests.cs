using FellSwoop.Game.Models;
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
            var grid = new Grid(width, height);

            // Correct number cells
            grid
                .Width
                .Should()
                .Be(width);

            grid
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
            var grid = new Grid(width, height);

            var coordinates = new Coordinates(x, y);

            grid.Neighbours(coordinates)
                .Should()
                .HaveCount(expected);
        }


        [Theory]
        [InlineData(10, 20)]
        [InlineData(100, 200)]
        public void Coordinate_Is_Inside_Grid(int width, int height)
        {
            var grid = new Grid(width, height);

            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
                grid
                    .IsInsideGrid(new Coordinates(x, y))
                    .Should()
                    .BeTrue();
        }

        [Theory]
        [InlineData(10, 20)]
        [InlineData(100, 200)]
        public void Coordinate_On_Boundary_Is_Outside_Grid(int width, int height)
        {
            var grid = new Grid(width, height);

            grid.IsInsideGrid(new Coordinates(width, 0)).Should().BeFalse();

            grid.IsInsideGrid(new Coordinates(0, height)).Should().BeFalse();

            grid.IsInsideGrid(new Coordinates(width, height)).Should().BeFalse();
        }

        [Theory]
        [InlineData(10, 20)]
        [InlineData(100, 200)]
        public void Game_Array_Is_Generated_Correctly(int width, int height)
        {
            var grid = new Grid(width, height);

            // Correct number cells
            grid
                .Count
                .Should()
                .Be(width * height);
        }

        [Theory]
        [InlineData(10, 20)]
        [InlineData(100, 200)]
        public void Game_Array_Type_Distribution_Is_Adequate(int width, int height)
        {
            var game = new Grid(width, height);

            var typeCounts = game
                .Values
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());

            // All types are in grid
            typeCounts
                .Values
                .Should()
                .HaveCount(Enum.GetValues(typeof(CellType)).Length - 1);

            // Grid cells more than a quarter for each of the three types
            typeCounts
                .Values
                .All(x => x / Convert.ToDouble(width * height) >= 0.25)
                .Should()
                .BeTrue();
        }
    }
}