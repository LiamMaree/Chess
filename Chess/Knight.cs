using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    
    internal class Knight : Pieces
    {
        public Knight(Vectors position, bool isWhite, Board chessboard, Texture2D texture) : base(position, isWhite, chessboard, texture)
        {
            hasMoved = false;

        }

        public override Pieces clone(Board cloneBoard)
        {
            Knight temp = new Knight(position, isWhite, cloneBoard, texture);
            if (this.hasMoved == true)
            {
                temp.SetMoved(true);

            }
            return temp;
        }
        public override Vectors[] getMoves()
        {
            Stack<Vectors> moves = new Stack<Vectors>();
            for (int i = -1; i <= 1; i++)
            {
                if (i == 0)
                    continue; // Skip the loop iteration when i == 0

                // Define reusable variables to avoid redundant calls
                Vectors move1 = position.move(2 * i, i);
                Vectors move2 = position.move(2 * (-i), i);
                Vectors move3 = position.move(i, 2 * i);
                Vectors move4 = position.move(i, 2 * (-i));

                // Check and push each move if valid
                if (move1 != null && move1.rangeCheck())
                {
                    var piece1 = chessboard.getPiece(move1);
                    if (piece1 == null || piece1.getisWhite() != isWhite)
                    {
                        moves.Push(move1);
                    }
                }

                if (move2 != null && move2.rangeCheck())
                {
                    var piece2 = chessboard.getPiece(move2);
                    if (piece2 == null || piece2.getisWhite() != isWhite)
                    {
                        moves.Push(move2);
                    }
                }

                if (move3 != null && move3.rangeCheck())
                {
                    var piece3 = chessboard.getPiece(move3);
                    if (piece3 == null || piece3.getisWhite() != isWhite)
                    {
                        moves.Push(move3);
                    }
                }

                if (move4 != null && move4.rangeCheck())
                {
                    var piece4 = chessboard.getPiece(move4);
                    if (piece4 == null || piece4.getisWhite() != isWhite)
                    {
                        moves.Push(move4);
                    }
                }
            }

            return moves.ToArray();
        }
    }
}
