using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    
    internal class King : Pieces
    {
        
        public King(Vectors position, bool isWhite, Board chessboard, Texture2D texture) : base(position, isWhite, chessboard, texture)
        {
            hasMoved = false;
        }
        public override Pieces clone(Board cloneBoard)
        {
            King temp = new King(position, isWhite, cloneBoard, texture);
            if (this.hasMoved == true)
            {
                temp.SetMoved(true);

            }
            return temp;
        }
        public override Vectors[] getMoves()
        {
            Stack<Vectors> moves = new Stack<Vectors>();
            
            for(int dX = -1; dX <= 1; dX++)
            {
                for(int dY = -1; dY <= 1; dY++)
                {
                    if (dX == 0 && dY == 0)
                    {
                        continue;
                    }
                    Vectors kingMove = position.move(dX, dY);
                    if (kingMove != null && kingMove.rangeCheck())
                    {
                        if (chessboard.getPiece(kingMove) == null)
                        {
                            if (chessboard.isTileSafe(kingMove, isWhite))
                            {
                                moves.Push(kingMove);
                            }
                        }
                        else
                        {
                            if(chessboard.getPiece(kingMove).getisWhite() != isWhite)
                            {
                                if (chessboard.isTileSafe(kingMove, isWhite))
                                {
                                    moves.Push(kingMove);
                                }
                            }
                        }
                    }
                }
            }
            castlingMove(moves);
            return moves.ToArray();
        }
        private void castlingMove(Stack<Vectors> moves)
        {
            if(hasMoved == false)
            {
                if(chessboard.getChessboard()[position.getX() + 3,position.getY()].getHasMoved() == false)
                {
                    if (chessboard.getChessboard()[position.getX() + 2, position.getY()] == null)
                    {
                        if (chessboard.getChessboard()[position.getX() + 1, position.getY()] == null)
                        {
                            moves.Push(position.move(2, 0));
                        }
                    }
                }
                if (chessboard.getChessboard()[position.getX() - 4, position.getY()].getHasMoved() == false)
                {
                    if (chessboard.getChessboard()[position.getX() -3, position.getY()] == null)
                    {
                        if (chessboard.getChessboard()[position.getX() -2, position.getY()] == null)
                        {
                            if (chessboard.getChessboard()[position.getX() - 1, position.getY()] == null)
                            {
                                moves.Push(position.move(-2, 0));
                            }
                        }
                    }
                }

            }
        }
        
    }
}
