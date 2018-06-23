using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Model {

    class Snake {
        private int v1;
        private int v2;

        public Snake(int rowPosition, int columnPosition) {
            HeadPosition = new ArenaPosition(rowPosition, columnPosition);
            HeadDirection = SnakeHeadDirectionEnum.InPlace;
        }

        public ArenaPosition HeadPosition { get; set; }

        public SnakeHeadDirectionEnum HeadDirection { get; set; }

        //todo: property for snake length 
    }
}
