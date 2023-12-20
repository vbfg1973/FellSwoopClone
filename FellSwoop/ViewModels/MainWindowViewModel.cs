using System.Collections.Generic;
using System.Linq;
using FellSwoop.Game;
using FellSwoop.Game.Models;

namespace FellSwoop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly FellSwoopGame _game;

        public MainWindowViewModel()
        {
            var w = 40;
            var h = 40;
            _game = new FellSwoopGame(w, h);

            Cells = new List<List<CellType>>();

            for (var x = 0; x < w; x++)
            {
                Cells.Add(new List<CellType>());
                for (var y = 0; y < h; y++)
                {
                    var lastList = Cells.Last();
                    lastList.Add(_game.Grid.AtPosition(x, y));
                }
            }
        }

        public List<List<CellType>> Cells { get; }

        private void LoadCellsFromGame()
        {
            for (var x = 0; x < _game.Grid.Width; x++)
            for (var y = 0; y < _game.Grid.Height; y++)
                Cells[x][y] = _game.Grid.AtPosition(x, y);
        }
    }
}