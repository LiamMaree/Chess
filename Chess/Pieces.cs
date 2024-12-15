using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    
    internal abstract class Pieces
    {
        protected bool isWhite;
        protected Vectors position;
        protected Board chessboard;
        protected bool hasMoved;
        protected Texture2D texture;
        
        public Pieces(Vectors position,bool isWhite, Board chessboard, Texture2D texture)
        {
            this.isWhite = isWhite;
            this.position = position;
            this.texture = texture;
            this.chessboard = chessboard;
            
        }
        public abstract Pieces clone(Board cloneBoard);
        
        public bool getisWhite()
        {
            return isWhite;
        }
        public abstract Vectors[] getMoves();
        public Texture2D getTexture()
        {
            return texture;
        }
        public Vectors getPositions()
        {
            return position;
        }
        public void setPositions(Vectors position)
        {
            this.position = position;
        }
        //Handles rook style movement
        protected Vectors[] rookMoves()
        {
            Stack<Vectors> moves = new Stack<Vectors>();
            for (int j = -1; j <= 1; j++)
            {
                for (int i = 1; i <= 7; i++)
                {
                    if (i == 0)
                    {
                        continue; // Skip the rest of the loop when i == 0
                    }


                    // Handle vertical (i, 0)
                    Vectors verticalMove = position.move(0, i * j);
                    if (verticalMove != null && verticalMove.rangeCheck())
                    {
                        var piece = chessboard.getPiece(verticalMove);
                        if (piece == null)
                        {
                            moves.Push(verticalMove);

                        }
                        else
                        {
                            if (piece.getisWhite() != isWhite)
                            {
                                moves.Push(verticalMove);

                            }
                            break; // Stop further moves in this direction
                        }
                    }
                }
            }
            for (int j = -1; j <= 1; j++)
            {
                for (int i = 1; i <= 7; i++)
                {
                    if (i == 0)
                    {
                        continue; // Skip the rest of the loop when i == 0
                    }


                    // Handle horizontal (0, i)
                    Vectors horizontalMove = position.move(i * j, 0);
                    if (horizontalMove != null && horizontalMove.rangeCheck())
                    {
                        var piece = chessboard.getPiece(horizontalMove);
                        if (piece == null)
                        {
                            moves.Push(horizontalMove);

                        }
                        else
                        {
                            if (piece.getisWhite() != isWhite)
                            {
                                moves.Push(horizontalMove);

                            }
                            break; // Stop further moves in this direction
                        }
                    }
                }
            }


            return moves.ToArray();
        }
        //Handles bishop style movement
        protected Vectors[] bishopMoves()
        {
            Stack<Vectors> moves = new Stack<Vectors>();
            for (int j = -1; j <= 1; j++)
            {
                for (int i = 1; i <= 7; i++)
                {
                    if (i == 0)
                        continue; // Skip the loop iteration when i == 0

                    // Handle diagonal (i, i)
                    Vectors move1 = position.move(j * i, -(j * i));
                    if (move1 != null && move1.rangeCheck())
                    {
                        var piece1 = chessboard.getPiece(move1);
                        if (piece1 == null)
                        {
                            moves.Push(move1);
                        }
                        else
                        {
                            if (piece1.getisWhite() != isWhite)
                            {
                                moves.Push(move1);
                            }
                            break; // Stop further moves in this direction
                        }
                    }
                }
            }

            for (int j = -1; j <= 1; j++)
            {
                for (int i = 1; i <= 7; i++)
                {
                    if (i == 0)
                        continue; // Skip the loop iteration when i == 0
                    // Handle diagonal (-i, i)
                    Vectors move2 = position.move(i * j, j * i);
                    if (move2 != null && move2.rangeCheck())
                    {
                        var piece2 = chessboard.getPiece(move2);
                        if (piece2 == null)
                        {
                            moves.Push(move2);
                        }
                        else
                        {
                            if (piece2.getisWhite() != isWhite)
                            {
                                moves.Push(move2);
                            }
                            break; // Stop further moves in this direction
                        }
                    }
                }
            }


            return moves.ToArray();
        }
        public void SetMoved(bool hasmoved)
        {
            this.hasMoved = hasmoved;
        }
        public bool getHasMoved()
        {
            return hasMoved;
        }

    }
    
}
