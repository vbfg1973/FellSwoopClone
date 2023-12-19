using FellSwoop.Game.Models;

namespace FellSwoop.Game.Tests.Helpers
{
    public static class ChangingCoordinatesHelpers
    {
        public static IEnumerable<ChangingCoordinates> OrderByFrom(
            this IEnumerable<ChangingCoordinates> coordinatesEnumerable)
        {
            return coordinatesEnumerable
                .OrderBy(ch => ch.From.X)
                .ThenBy(ch => ch.From.Y);
        }
    }
}