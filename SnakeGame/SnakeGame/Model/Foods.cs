using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnakeGame.Model {

    class Foods {

        public Foods() {
            FoodPositions = new List<CanvasPosition>();
        }

        public List<CanvasPosition> FoodPositions { get; set; }

        internal void Add(int row, int column, UIElement paint) {
            FoodPositions.Add(new CanvasPosition(row, column, paint));
        }

        /// <summary>
        /// Delete 1 item from the foods.
        /// </summary>
        /// <param name="rowPosition"></param>
        /// <param name="columnPosition"></param>
        /// <returns>Return with the food what was deleted.</returns>
        internal CanvasPosition Remove(int rowPosition, int columnPosition) {

            //If the function doesn't find any item, then return with null, foodToDelete <-- null
            var foodToDelete = FoodPositions.SingleOrDefault(x => x.RowPosition == rowPosition && x.ColumnPosition == columnPosition);
            FoodPositions.Remove(foodToDelete);

            return foodToDelete;
        }

    }
}
