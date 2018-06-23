using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnakeGame.Model {
    /// <summary>
    /// It contains the logic of game
    /// </summary>
    class Arena {
        private MainWindow View;

        public Arena(MainWindow view) {
            this.View = view;
            View.GamePlayTextBlockBorder.Visibility = System.Windows.Visibility.Visible;
        }

        internal void KeyDown(KeyEventArgs e) {
            switch (e.Key) {
                case Key.Left:
                case Key.Up:
                case Key.Right:
                case Key.Down:
                    View.GamePlayTextBlockBorder.Visibility = System.Windows.Visibility.Hidden;
                    View.NumberOfMealsTextBlock.Visibility = System.Windows.Visibility.Visible;
                    View.ArenaGrid.Visibility = System.Windows.Visibility.Visible;
                    Console.WriteLine(e.Key);
                    break;
            }
        }
    }
}
