using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    
    internal class Pawn : Pieces
    {
       
        public Pawn(Vectors position, bool isWhite, Board chessboard, Texture2D texture) : base(position, isWhite, chessboard,texture)
        {
            hasMoved = false;
        }
        public override Pieces clone(Board cloneBoard)
        {
            Pawn temp = new Pawn(position, isWhite, cloneBoard, texture);
            if(this.hasMoved == true)
            {
                temp.SetMoved(true);
                
            }
            return temp;
        }

        public override Vectors[] getMoves()
        {
            int direction;
            Stack<Vectors> moves = new Stack<Vectors>();
            if (isWhite == true)
            {
                direction = 1;

            }
            else
            {
                direction = -1;
            }
            Vectors move1 = position.move(1, direction);
            if (move1 != null && move1.rangeCheck())
                if (chessboard.getPiece(move1) != null)
                {
                    if (chessboard.getPiece(move1).getisWhite() != isWhite)
                    {
                        moves.Push(move1);
                    }
                }
            Vectors move2 = position.move(-1, direction);
            if (move2 != null && move2.rangeCheck())
                if (chessboard.getPiece(move2) != null)
                {
                    if (chessboard.getPiece(move2).getisWhite() != isWhite)
                    {
                        moves.Push(move2);
                    }
                }
            Vectors move3 = position.move(0, direction);         
            if (move3 != null && move3.rangeCheck())
            {
                if (chessboard.getPiece(move3) == null)
                {
                    moves.Push(move3);
                }
            }
            if (!hasMoved)
            {
                Vectors move4 = position.move(0, 2 * direction);
                if(move4 != null && move4.rangeCheck())
                {
                    if (chessboard.getPiece(move4) == null && chessboard.getPiece(move3) == null)
                    {
                        moves.Push(move4);
                    }
                }
                
            }
            
            return moves.ToArray();
        }
        
        public void promotion()
        {
            int direction;
            if (isWhite == true)
            {
                direction = 7;
            }
            else direction = 0;
            if(position.getY() == direction)
            {
                chessboard.promotePiece(this);
            }
        }
    } 
}
