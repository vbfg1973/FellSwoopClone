namespace FellSwoop.Game.Tests.Helpers
{
    public record TestParameters(int Width, int Height, int X, int Y, int ConnectedNeighbourCount);

    public static class FilenameHelpers
    {
        public static TestParameters TestParamsFromFilename(string fileName)
        {
            var testDetails = fileName.Split(".").ToArray();
            var size = testDetails[0].Split("x").Select(x => Convert.ToInt32(x)).ToArray();
            var coords = testDetails[1].Split("-").Select(x => Convert.ToInt32(x)).ToArray();
            var connectedNeighbourCount = Convert.ToInt32(testDetails[2]);
            var tp = new TestParameters(size[0], size[1], coords[0], coords[1], connectedNeighbourCount);
            return tp;
        }
    }
}