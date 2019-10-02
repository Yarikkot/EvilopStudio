using System;
using System.Collections.Generic;
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

namespace MazeCreator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Test.Maze maze;
        public MainWindow()
        {
            InitializeComponent();
            maze = new Test.Maze(10);
            maze.doneEvent += Maze_doneEvent;    
            maze.FillMaze();
            maze.BuildMazeAsync(RandomStart:true,delay:60);
            But.IsEnabled = false;
            DataContext = maze.CellsInList;
            IC.Width = maze.MazeMatrix.GetLength(0) * 15;
            IC.Height = maze.MazeMatrix.GetLength(1) * 15;


        }

        private void Maze_doneEvent(object sender, EventArgs e)
        {
            But.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            But.IsEnabled = false;
            maze.ResetMaze();
            maze.BuildMazeAsync(RandomStart: true);
        }
    }
}
