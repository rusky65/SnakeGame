using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGame.Model {
    /// <summary>
    /// It contains the logic of game
    /// </summary>
    class Arena {
        private bool isStarted;
        private bool isReStarted;
        private DispatcherTimer pendulum;
        private DispatcherTimer gameClock;
        private TimeSpan playTime;
        private Snake snake;
        private MainWindow View;
        private int RowCount;
        private int ColumnCount;
        private Random Random;
        private Foods foods;
        private int foodsHaveEatenCount;

        public Arena(MainWindow view) {
            this.View = view;
            // Setting the size of arena. Getting the size from the grid.
            RowCount = View.ArenaGrid.RowDefinitions.Count;
            ColumnCount = View.ArenaGrid.ColumnDefinitions.Count;

            StartingState();

        }

        private void StartPendulum() {

            if (pendulum != null && pendulum.IsEnabled) {
                pendulum.Stop();
            }

            var interval = 1500 / snake.Length;

            // A pendulum for doing some task for snake to go ahead and etc. .
            pendulum = new DispatcherTimer(TimeSpan.FromMilliseconds(interval), DispatcherPriority.Normal, ItsTimeToDisplay, Application.Current.Dispatcher);

            // Clock to displaying the time of game.
            gameClock = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, ClockTick, Application.Current.Dispatcher);
            gameClock.Stop();

        }

        // Display the time of game.
        private void ClockTick(object sender, EventArgs e) {
            playTime = playTime + TimeSpan.FromSeconds(1);
            View.LabelPlayTime.Content = $"{playTime.Minutes:00}:{playTime.Seconds:00}";
        }

        // Resolves soma task for snake to go ahead.
        private void ItsTimeToDisplay(object sender, EventArgs e) {

            if (!isStarted || isReStarted) {
                return;
            }

            // clac the head next position
            switch (snake.HeadDirection) {
                case SnakeHeadDirectionEnum.Up:
                    snake.HeadPosition.RowPosition -= 1;
                    break;
                case SnakeHeadDirectionEnum.Down:
                    snake.HeadPosition.RowPosition += 1;
                    break;
                case SnakeHeadDirectionEnum.Left:
                    snake.HeadPosition.ColumnPosition -= 1;
                    break;
                case SnakeHeadDirectionEnum.Right:
                    snake.HeadPosition.ColumnPosition += 1;
                    break;
                case SnakeHeadDirectionEnum.InPlace:
                    break;
                default:
                    break;
            }

            //check if the snake crash into the wall
            if (snake.HeadPosition.RowPosition < 0 || snake.HeadPosition.RowPosition > RowCount - 1 ||
                snake.HeadPosition.ColumnPosition < 0 || snake.HeadPosition.ColumnPosition > ColumnCount - 1) {
                EndOfGame();
                return;
            }

            //check if the snake crash into itself
            if (snake.Tail.Any(x=>x.RowPosition == snake.HeadPosition.RowPosition && x.ColumnPosition == snake.HeadPosition.ColumnPosition)) {
                EndOfGame();
                return;
            }

            //check if the snake aet a food
            //if (foods.FoodPositions.Any(x=>x.RowPosition == snake.HeadPosition.RowPosition && x.ColumnPosition == snake.HeadPosition.ColumnPosition)) {

            var foodToDelete = foods.Remove(snake.HeadPosition.RowPosition, snake.HeadPosition.ColumnPosition);
            if (foodToDelete != null) {

                EraseFromCanvas(foodToDelete.Paint);

                foodsHaveEatenCount += 1;
                View.NumberOfMealsTextBlock.Text = foodsHaveEatenCount.ToString();

                snake.Length += 1;
                GetNewFood();
            }

            var paintHead = ShowSnakeHead(snake.HeadPosition.RowPosition, snake.HeadPosition.ColumnPosition);
            //we have to delete the old head position befor we save the new head position.
            EraseFromCanvas(snake.HeadPosition.Paint);
            //we save the new head position into the HeadPosition.
            snake.HeadPosition.Paint = paintHead;

            // head of snake will change to neck !
            var neckPaint = ShowSnakeNeck(snake.HeadPositionOld.RowPosition, snake.HeadPositionOld.ColumnPosition);
            //var neckPaint = ShowSnakeNeck(snake.HeadPosition.RowPositionOld, snake.HeadPosition.ColumnPositionOld);
            //var neckPaint = ShowSnakeNeck(neck.RowPosition, neck.ColumnPosition);

            snake.Tail.Add(new CanvasPosition(snake.HeadPositionOld.RowPosition, snake.HeadPositionOld.ColumnPosition, neckPaint));
            //snake.Tail.Add(new CanvasPosition(snake.HeadPosition.RowPositionOld, snake.HeadPosition.ColumnPositionOld, neckPaint));
            //snake.Tail.Add(new CanvasPosition(neck.RowPosition, neck.ColumnPosition, neckPaint));

            if (snake.Tail.Count < snake.Length) {
            } else {

                var end = snake.Tail[0];

                ShowEmptyArenaPosition(end.RowPosition, end.ColumnPosition, end.Paint );

                snake.Tail.RemoveAt(0);
            }
        }

        // 4 in 1 function with VisibleElementTypesEnum class.
        private void PaintOnGrid(int rowPosition, int columnPosition, VisibleElementTypesEnum VisibleType) {
            var image = GetImage(rowPosition, columnPosition);

            switch (VisibleType) {
                case VisibleElementTypesEnum.SnakeHead:
                    image.Icon = FontAwesome.WPF.FontAwesomeIcon.Circle;
                    image.Foreground = Brushes.Black;
                    image.Opacity = 1;
                    break;
                case VisibleElementTypesEnum.SnakeNeck:
                    image.Icon = FontAwesome.WPF.FontAwesomeIcon.Circle;
                    image.Foreground = Brushes.Gray;
                    image.Opacity = 1;
                    break;
                case VisibleElementTypesEnum.Food:
                    image.Icon = FontAwesome.WPF.FontAwesomeIcon.Apple;
                    image.Foreground = Brushes.Red;
                    image.Opacity = 1;
                    break;
                case VisibleElementTypesEnum.EmptyArenaPosition:
                    image.Icon = FontAwesome.WPF.FontAwesomeIcon.SquareOutline;
                    //image.Foreground = Brushes.Black;
                    image.Opacity = 0;
                    break;
                default:
                    break;
            }

        }

        private UIElement ShowNewFoods(int rowPosition, int columnPosition) {
            // show the food on new position

            //Paint on grid
            PaintOnGrid(rowPosition, columnPosition, VisibleElementTypesEnum.Food);

            //Paint on canvas
            var paint = PaintOnCanvas(rowPosition, columnPosition, VisibleElementTypesEnum.Food);

            //send back the paint to delete
            return paint;
        }

        private UIElement ShowSnakeHead(int rowPosition, int columnPosition) {
            // show the head of snake new position
            PaintOnGrid(rowPosition, columnPosition, VisibleElementTypesEnum.SnakeHead);
            //Paint on canvas
            var paint = PaintOnCanvas(rowPosition, columnPosition, VisibleElementTypesEnum.SnakeHead);

            //send back the paint to delete
            return paint;
        }

        private UIElement ShowSnakeNeck(int rowPosition, int columnPosition) {
            // show the neck of snake new position
            PaintOnGrid(rowPosition, columnPosition, VisibleElementTypesEnum.SnakeNeck);
            //Paint on canvas
            var paint = PaintOnCanvas(rowPosition, columnPosition, VisibleElementTypesEnum.SnakeNeck);

            //send back the paint to delete
            return paint;
        }

        private void ShowEmptyArenaPosition(int rowPosition, int columnPosition, UIElement paint) {
            // hide the end of snake
            PaintOnGrid(rowPosition, columnPosition, VisibleElementTypesEnum.EmptyArenaPosition);
            EraseFromCanvas(paint);

            //Paint on canvas
            //var paint = PaintOnCanvas(rowPosition, columnPosition, VisibleElementTypesEnum.EmptyArenaPosition);

            //send back the paint to delete
            //return paint;
        }

        /// <summary>
        /// It paints an element on the canvas.
        /// </summary>
        /// <param name="rowPosition"></param>
        /// <param name="columnPosition"></param>
        /// <returns>The painted item what has to be deleted.</returns>
        private UIElement PaintOnCanvas(int rowPosition, int columnPosition, VisibleElementTypesEnum VisibleType) {
            var paint = new Ellipse();

            paint.Height = View.ArenaCanvas.ActualHeight / RowCount;
            paint.Width = View.ArenaCanvas.ActualWidth / ColumnCount;

            switch (VisibleType) {
                case VisibleElementTypesEnum.SnakeHead:
                    paint.Fill = Brushes.Black;
                    break;
                case VisibleElementTypesEnum.SnakeNeck:
                    paint.Fill = Brushes.Gray;
                    break;
                case VisibleElementTypesEnum.Food:
                    paint.Fill = Brushes.Red;
                    break;
                // We don't need this snipet anymore. We delete the canvas.
                //case VisibleElementTypesEnum.EmptyArenaPosition:
                //    paint.Fill = Brushes.Aquamarine;
                //    break;
                default:
                    break;
            }

            Canvas.SetTop(paint, rowPosition * paint.Height);
            Canvas.SetLeft(paint, columnPosition * paint.Width);

            View.ArenaCanvas.Children.Add(paint);

            return paint;
        }

        /// <summary>
        /// The painted item to delete.
        /// </summary>
        /// <param name="paint"></param>
        private void EraseFromCanvas(UIElement paint) {
            View.ArenaCanvas.Children.Remove(paint);

        }

        private FontAwesome.WPF.ImageAwesome GetImage(int rowPosition, int columnPosition) {
            // Get the image at the position of row and column
            var cell = View.ArenaGrid.Children[rowPosition * 20 + columnPosition];
            var image = (FontAwesome.WPF.ImageAwesome)cell;
            return image;
        }

        internal void KeyDown(KeyEventArgs e) {
            Console.WriteLine(e.Key);

            switch (e.Key) {
                case Key.Multiply:
                    if (!isStarted && !isReStarted) {
                        isReStarted = true;
                        ClearTheScreen();
                        StartingState();
                    }
                    break;
                case Key.Left:
                case Key.Up:
                case Key.Right:
                case Key.Down:
                    if (!isStarted && isReStarted) {
                        StartNewGame();
                    }

                    if (isStarted) {
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
                    }

                    //Console.WriteLine(e.Key);
                    break;
            }
        }

        private void StartNewGame() {
            View.GamePlayTextBlockBorder.Visibility = System.Windows.Visibility.Hidden;
            View.NumberOfMealsTextBlock.Visibility = System.Windows.Visibility.Visible;
            View.ArenaGrid.Visibility = System.Windows.Visibility.Visible;
            isStarted = true;
            isReStarted = false;
            gameClock.Start();

            GetNewFood();
        }

        private void EndOfGame() {
            Console.WriteLine("End of the Game");
            pendulum.Stop();
            gameClock.Stop();

            isStarted = false;

            View.LabelPlayTime.Content = $"{playTime.Minutes:00}:{playTime.Seconds:00}";
            View.LabelPlayTimeEnd.Content = $"{playTime.Minutes:00}:{playTime.Seconds:00}";
            View.NumberOfMealsLabel.Content = foodsHaveEatenCount.ToString();
            View.GameEndResultsTextBlockBorder.Visibility = Visibility.Visible;

            //            StartingState();
        }

        private void StartingState() {
            View.GameEndResultsTextBlockBorder.Visibility = Visibility.Hidden;
            View.GamePlayTextBlockBorder.Visibility = System.Windows.Visibility.Visible;
            snake = new Snake(10, 10);


            Random = new Random();
            foods = new Foods();

            foodsHaveEatenCount = 0;
            isReStarted = true;
            StartPendulum();
        }

        private void ClearTheScreen() {
            //Clear the tail
            for (int i = 0; i < snake.Tail.Count; i++) {
                CanvasPosition tailPosition = snake.Tail[i];
                ShowEmptyArenaPosition(tailPosition.RowPosition, tailPosition.ColumnPosition, tailPosition.Paint);
            }

            //Clear the head
            ShowEmptyArenaPosition(snake.HeadPositionOld.RowPosition, snake.HeadPositionOld.ColumnPosition, snake.HeadPosition.Paint);

            //Clear the food(s)
            for (int i = 0; i < foods.FoodPositions.Count; i++) {
                CanvasPosition foodPosition = foods.FoodPositions[i];
                ShowEmptyArenaPosition(foodPosition.RowPosition, foodPosition.ColumnPosition, foodPosition.Paint);
            }

            // Reset the displays
            playTime = TimeSpan.Zero;
            View.LabelPlayTime.Content = $"{playTime.Minutes:00}:{playTime.Seconds:00}";
            View.NumberOfMealsTextBlock.Text = "0";

            // Set the snake to null
            snake = null;
        }

        /// <summary>
        /// Create a new food and show it.
        /// </summary>
        private void GetNewFood() {
            var row = Random.Next(0, RowCount - 1);
            var column = Random.Next(0, ColumnCount - 1);
            while (snake.HeadPosition.RowPosition == row && snake.HeadPosition.ColumnPosition == column
                || snake.Tail.Any(x => x.RowPosition == row && x.ColumnPosition == column)) {
                row = Random.Next(0, RowCount - 1);
                column = Random.Next(0, ColumnCount - 1);
            }

            //Displaying the new food
            var paint = ShowNewFoods(row, column);

            foods.Add(row, column, paint);

        }
    }
}
