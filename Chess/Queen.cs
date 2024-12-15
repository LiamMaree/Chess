using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    
    internal class Queen : Pieces
    {
        public Queen(Vectors position, bool isWhite, Board chessboard, Texture2D texture) : base(position, isWhite, chessboard, texture)
        {
            hasMoved = false;
        }

        public override Pieces clone(Board cloneBoard)
        {
            Queen temp = new Queen(position, isWhite, cloneBoard, texture);
            if (this.hasMoved == true)
            {
                temp.SetMoved(true);

            }
            return temp;
        }

        
        public override Vectors[] getMoves()
        {
            return rookMoves().Concat(bishopMoves()).ToArray();
        }

            
       
    }
}
