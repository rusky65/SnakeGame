using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnakeGame.Model {

    class CanvasPosition : ArenaPosition {
        private UIElement paint;
        public UIElement Paint { get; set; }

        public CanvasPosition(int rowPosition, int columnPosition, UIElement paint) : base(rowPosition, columnPosition) {
            this.Paint = paint;
        }
    }
}
