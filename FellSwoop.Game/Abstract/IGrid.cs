using FellSwoop.Game.Models;

namespace FellSwoop.Game.Abstract
{
    public interface IGrid
    {
        int Height { get; }
        int Width { get; }
        int Count { get; }
        public IEnumerable<CellType> Values { get; }
        void SetTo(int x, int y, CellType cellType);
        CellType AtPosition(int x, int y);
        CellType AtPosition(Coordinates coordinates);
        IEnumerable<Coordinates> PopulatedInColumn(int x);
        IEnumerable<Coordinates> PopulatedInColumnAbove(Coordinates c);
        IEnumerable<Coordinates> Neighbours(Coordinates coordinates);
        bool IsInsideGrid(Coordinates coordinates);
        void LoadFile(string path);
    }
}