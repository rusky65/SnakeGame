using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Model {

    class ArenaPosition {

        public ArenaPosition(int rowPosition, int columnPosition) {
            RowPosition = rowPosition;
            ColumnPosition = columnPosition;
        }

        private int rowPosition;
        private int columnPosition;

        public int RowPosition {
            get {
                return rowPosition;
            }

            set {
                int rowPositionOld = rowPosition;
                rowPosition = value;
                OnArenaPositionChanged(new ArenaPositionChangedEventArgs(rowPosition, columnPosition, rowPositionOld, columnPosition));
            }
        }

        public int ColumnPosition {
            get {
                return columnPosition;
            }

            set {
                int columnPositionOld = columnPosition;
                columnPosition = value;
                OnArenaPositionChanged(new ArenaPositionChangedEventArgs(rowPosition, columnPosition, rowPosition, columnPositionOld));
            }
        }

        public delegate void ArenaPositionChangedHandler(object sender, ArenaPositionChangedEventArgs e);
        public event ArenaPositionChangedHandler ArenaPositionChanged;
        protected virtual void OnArenaPositionChanged(ArenaPositionChangedEventArgs e) {
            if (ArenaPositionChanged != null) {
                ArenaPositionChanged(this, e);
            }
        }

    }

    class ArenaPositionChangedEventArgs : EventArgs {
        public readonly int RowPositionNew;
        public readonly int ColumnPositionNew;
        public readonly int RowPositionOld;
        public readonly int ColumnPositionOld;

        public ArenaPositionChangedEventArgs(int newRowPosition, int newColumnPosition, int oldRrowPosition, int oldColumnPosition) {
            RowPositionNew = newRowPosition;
            ColumnPositionNew = newColumnPosition;
            RowPositionOld = oldRrowPosition;
            ColumnPositionOld = oldColumnPosition;
        }
    }
}
