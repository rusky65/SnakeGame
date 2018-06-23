using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SnakeGame.Model {
    /// <summary>
    /// It contains the logic of game
    /// </summary>
    class Arena {
        private bool isStarted;
        private DispatcherTimer pendulum;
        private Snake snake;
        private MainWindow View;
        private DispatcherTimer gameClock;
        private TimeSpan playTime;

        public Arena(MainWindow view) {
            this.View = view;
            View.GamePlayTextBlockBorder.Visibility = System.Windows.Visibility.Visible;

            snake = new Snake(10, 10);

            pendulum = new DispatcherTimer(TimeSpan.FromMilliseconds(500),DispatcherPriority.Normal, ItsTimeToDisplay, Application.Current.Dispatcher);

            gameClock = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, ClockTick, Application.Current.Dispatcher);
            gameClock.Stop();

            isStarted = false;

        }

        private void ClockTick(object sender, EventArgs e) {
            playTime = playTime + TimeSpan.FromSeconds(1);
            View.LabelPlayTime.Content = $"{playTime.Minutes:00}:{playTime.Seconds:00}";
        }

        private void ItsTimeToDisplay(object sender, EventArgs e) {

            if (!isStarted) {
                return;
            }

            var currentPosition = new ArenaPosition(snake.HeadPosition.RowPosition, snake.HeadPosition.ColumnPosition);

            // clac the head next position
            switch (snake.HeadDirection) {
                case SnakeHeadDirectionEnum.Up:
                    snake.HeadPosition.RowPosition = snake.HeadPosition.RowPosition - 1;
                    break;
                case SnakeHeadDirectionEnum.Down:
                    snake.HeadPosition.RowPosition = snake.HeadPosition.RowPosition + 1;
                    break;
                case SnakeHeadDirectionEnum.Left:
                    snake.HeadPosition.ColumnPosition = snake.HeadPosition.ColumnPosition - 1;
                    break;
                case SnakeHeadDirectionEnum.Right:
                    snake.HeadPosition.ColumnPosition = snake.HeadPosition.ColumnPosition + 1;
                    break;
                case SnakeHeadDirectionEnum.InPlace:
                    break;
                default:
                    break;
            }

            // show the new position
            var cell = View.ArenaGrid.Children[snake.HeadPosition.RowPosition * 20 + snake.HeadPosition.ColumnPosition];
            var image = (FontAwesome.WPF.ImageAwesome)cell;
            image.Icon = FontAwesome.WPF.FontAwesomeIcon.Circle;

            // hide the current position
            cell = View.ArenaGrid.Children[currentPosition.RowPosition * 20 + currentPosition.ColumnPosition];
            image = (FontAwesome.WPF.ImageAwesome)cell;
            image.Icon = FontAwesome.WPF.FontAwesomeIcon.SquareOutline;
        }

        internal void KeyDown(KeyEventArgs e) {
            switch (e.Key) {
                case Key.Left:
                case Key.Up:
                case Key.Right:
                case Key.Down:
                    if (!isStarted) {
                        StartNewGame();
                    }

                    switch (e.Key) {
                        case Key.Left:
                            snake.HeadDirection = SnakeHeadDirectionEnum.Left;
                            break;
                        case Key.Up:
                            snake.HeadDirection = SnakeHeadDirectionEnum.Up;
                            break;
                        case Key.Right:
                            snake.HeadDirection = SnakeHeadDirectionEnum.Right;
                            break;
                        case Key.Down:
                            snake.HeadDirection = SnakeHeadDirectionEnum.Down;
                            break;
                    }
                    Console.WriteLine(e.Key);
                    break;
            }
        }

        private void StartNewGame() {
            View.GamePlayTextBlockBorder.Visibility = System.Windows.Visibility.Hidden;
            View.NumberOfMealsTextBlock.Visibility = System.Windows.Visibility.Visible;
            View.ArenaGrid.Visibility = System.Windows.Visibility.Visible;
            isStarted = true;
            gameClock.Start();
        }
    }
}
