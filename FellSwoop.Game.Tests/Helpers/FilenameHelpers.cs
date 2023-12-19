using FellSwoop.Game.Models;

namespace FellSwoop.Game.Tests.Helpers
{
    public static class FilenameHelpers
    {
        public static GridTestParameters GridTestParametersFromFilename(string fileName)
        {
            var testDetails = fileName.Split(".").ToArray();
            var size = testDetails[0].Split("x").Select(x => Convert.ToInt32(x)).ToArray();
            var coords = testDetails[1].Split("-").Select(x => Convert.ToInt32(x)).ToArray();
            var connectedNeighbourCount = Convert.ToInt32(testDetails[2]);

            var tp = new GridTestParameters(size[0], size[1], coords[0], coords[1], connectedNeighbourCount);

            return tp;
        }
    }

    public static class ChangingCoordinatesHelpers
    {
        public static IEnumerable<ChangingCoordinates> OrderByFrom(this IEnumerable<ChangingCoordinates> coordinatesEnumerable)
        {
            return coordinatesEnumerable
                .OrderBy(ch => ch.From.X)
                .ThenBy(ch => ch.From.Y);
        } 
    }
}