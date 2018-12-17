using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProgramSedziowski
{
    /// <summary>
    /// Interaction logic for HistoryRenderer.xaml
    /// </summary>
    public partial class HistoryRenderer : UserControl
    {
        #region Commands
        public ICommand NextStepCommand { get; }
        public ICommand PreviousStepCommand { get; }
        #endregion

        public static readonly DependencyProperty CurrentHistoryGameProperty =
             DependencyProperty.Register(nameof(CurrentHistoryGame),
             typeof(Model.Game),
             typeof(HistoryRenderer),
             new UIPropertyMetadata(default(Model.Game), CurrentHistoryGameChanged));
        public Model.Game CurrentHistoryGame
        {
            get { return (Model.Game)GetValue(CurrentHistoryGameProperty); }
            set { SetValue(CurrentHistoryGameProperty, value); }
        }

        private static Grid mainGrid;
        private static int currentMoveNumber = 0;

        public HistoryRenderer()
        {
            NextStepCommand = new CommandHandler(() => { NextStep(); });
            PreviousStepCommand = new CommandHandler(() => { PreviousStep(); });
            InitializeComponent();
        }

        private static void CurrentHistoryGameChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var context = e.NewValue as Model.Game;
            if (context != null)
            {
                ((HistoryRenderer)sender).canvas.Children.Clear();
                double cellSize = 1000.0 / context.boardSize;
                var grid = new Grid();
                grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                grid.VerticalAlignment = VerticalAlignment.Stretch;
                for (int i = 0; i < context.boardSize; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(cellSize, GridUnitType.Star) });
                }

                for (int i = 0; i < context.boardSize; i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(cellSize, GridUnitType.Star) });
                }

                for (int x = 0; x < context.boardSize; x++)
                {
                    for (int y = 0; y < context.boardSize; y++)
                    {
                        var cellBorder = new Border();
                        cellBorder.Background = Brushes.White;
                        cellBorder.BorderBrush = Brushes.Black;
                        cellBorder.BorderThickness = new Thickness(1);
                        cellBorder.Name = $"cell_{x}_{y}";
                        cellBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
                        cellBorder.VerticalAlignment = VerticalAlignment.Stretch;
                        grid.Children.Add(cellBorder);
                        Grid.SetRow(cellBorder, y);
                        Grid.SetColumn(cellBorder, x);
                    }
                }

                ((HistoryRenderer)sender).canvas.Children.Add(grid);
                mainGrid = grid;
                ((HistoryRenderer)sender).InitializeBoard();
            }

        }

        public void InitializeBoard()
        {
            currentMoveNumber = 0;
            // load startPoints
            if (CurrentHistoryGame.moves.Count > 0)
            {
                while (currentMoveNumber < CurrentHistoryGame.moves.Count && CurrentHistoryGame.moves[currentMoveNumber].gamerId == 255)
                {
                    var currentPoint = CurrentHistoryGame.moves[currentMoveNumber].point;
                    ((Border)(mainGrid.Children.FindElement($"cell_{currentPoint.x}_{currentPoint.y}"))).Background = GamerIdToBrush(255);
                    currentMoveNumber++;
                }
            }
        }

        private void NextStep()
        {
            if (CurrentHistoryGame != null && CurrentHistoryGame.moves.Count > 0 && currentMoveNumber < CurrentHistoryGame.moves.Count)
            {
                var currentMove = CurrentHistoryGame.moves[currentMoveNumber];
                var drawingCell = ((Border)(mainGrid.Children.FindElement($"cell_{currentMove.point.x}_{currentMove.point.y}")));
                if (drawingCell != null)
                {
                    drawingCell.Background = GamerIdToBrush(currentMove.gamerId);
                    drawingCell.ToolTip = GetToolTip(GetGamerName(currentMove.gamerId), currentMove.time);
                }
                else
                {
                    Debug.WriteLine("Coś nie halo!");
                }

                currentMoveNumber++;
            }
        }

        private void PreviousStep()
        {
            if (CurrentHistoryGame != null && currentMoveNumber > 0)
            {
                currentMoveNumber--;
                var currentMove = CurrentHistoryGame.moves[currentMoveNumber];
                var drawingCell = ((Border)(mainGrid.Children.FindElement($"cell_{currentMove.point.x}_{currentMove.point.y}")));
                drawingCell.Background = GamerIdToBrush(0);
                drawingCell.ToolTip = null;
            }
        }

        private static Brush GamerIdToBrush(int id)
        {
            switch (id)
            {
                case 0:
                    return Brushes.White;
                case 1:
                    return new LinearGradientBrush(Color.FromArgb(255, 255, 0, 0), Color.FromArgb(255, 180, 0, 0), 90);
                case 2:
                    return new LinearGradientBrush(Color.FromArgb(255, 0, 0, 255), Color.FromArgb(255, 0, 0, 180), 90);
                case 255:
                    return new LinearGradientBrush(Color.FromArgb(255, 255, 255, 0), Color.FromArgb(255, 180, 180, 0), 90);
                default:
                    return null;
            }
        }

        private static object GetToolTip(string gamerName, DateTime time)
        {
            var res = new StackPanel();
            var title = new TextBlock();
            title.FontWeight = FontWeights.Bold;
            title.Text = gamerName;

            var date = new TextBlock();
            date.Text = time.ToString("H:m:s.FFFFFFF");

            res.Children.Add(title);
            res.Children.Add(date);
            return res;
        }
        private string GetGamerName(int gamerId)
        {
            switch (gamerId)
            {
                case 0:
                    return "Puste";
                case 1:
                    return CurrentHistoryGame.gamer1.Name;
                case 2:
                    return CurrentHistoryGame.gamer2.Name;
                case 255:
                    return "Program sędziowski";
                default:
                    return "Nieznany gracz";
            }
        }
    }

    public static class VisualHelperClass
    {
        public static FrameworkElement FindElement(this UIElementCollection elements, string name)
        {
            foreach (var elem in elements)
            {
                if (((FrameworkElement)elem).Name == name)
                {
                    return (FrameworkElement)elem;
                }
            }
            return null;
        }
    }
}
