using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FellSwoop.Views
{
    public partial class ButtonGrid : UserControl
    {
        public ButtonGrid()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var grid = new Grid();

            var rows = 50;
            var cols = 50;

            var rowDefinitions = string.Join(",", Enumerable.Range(1, rows).Select(x => "*"));
            var colDefinitions = string.Join(",", Enumerable.Range(1, cols).Select(x => "*"));

            grid.RowDefinitions = new RowDefinitions(rowDefinitions);
            grid.ColumnDefinitions = new ColumnDefinitions(colDefinitions);

            for (var x = 0; x < cols; x++)
            for (var y = 0; y < rows; y++)
            {
                var button = new Button
                {
                    [Grid.ColumnProperty] = x,
                    [Grid.RowProperty] = y,
                };

                button.Click += button_Click;

                grid.Children.Add(button);
            }

            Content = grid;
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            var row = Grid.GetRow(b);
            var col = Grid.GetColumn(b);

            Console.WriteLine($"{col} - {row}");
        }
    }
}