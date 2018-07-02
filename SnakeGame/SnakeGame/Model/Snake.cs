using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Model {

    class Snake {

        public Snake(int rowPosition, int columnPosition) {
            HeadPosition = new CanvasPosition(rowPosition, columnPosition, null);
            HeadPositionOld = new CanvasPosition(rowPosition, columnPosition, null);
            HeadDirection = SnakeHeadDirectionEnum.InPlace;
            Length = 6;
            Tail = new List<CanvasPosition>();
            HeadPosition.ArenaPositionChanged += HeadPositionChanged;
        }

        public CanvasPosition HeadPosition { get; set; }
        public CanvasPosition HeadPositionOld { get; set; }

        public SnakeHeadDirectionEnum HeadDirection { get; set; }

        //Property for snake length
        public List<CanvasPosition> Tail { get; set; }

        public int Length { get; set; }

        public void HeadPositionChanged(object sender, ArenaPositionChangedEventArgs e) {
            HeadPositionOld.RowPosition = e.RowPositionOld;
            HeadPositionOld.ColumnPosition = e.ColumnPositionOld;
        }
    }
}
