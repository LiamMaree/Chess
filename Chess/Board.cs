using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    
    internal class Board
    {
        private int selected_x;
        private int selected_y;
        private bool selected = false;
        private bool whiteTurn;
        private Pieces[,] chessboard = new Pieces[8, 8];
        private Dictionary<String, Texture2D> pieceIcons;
        //0 = Menu,1 = Game, 2 = Selecting Promotion Piece
        public const int MENU = 0;
        public const int GAME = 1;
        public const int PROMOTION_SELECTION = 2;
        private int gameMode;
        Pawn promotionPawn;
        public Board(Dictionary<String, Texture2D> textures)
        {
            for (int i = 0; i <= 7; i++)
            {
                chessboard[i, 6] = new Pawn(new Vectors(i, 6), false, this, textures["BP"]);
                chessboard[i, 1] = new Pawn(new Vectors(i, 1), true, this, textures["WP"]);
            }
            chessboard[0, 7] = new Rook(new Vectors(0, 7), false, this, textures["BR"]);
            chessboard[7, 7] = new Rook(new Vectors(7, 7), false, this, textures["BR"]);

            chessboard[0, 0] = new Rook(new Vectors(0, 0), true, this, textures["WR"]);
            chessboard[7, 0] = new Rook(new Vectors(7, 0), true, this, textures["WR"]);

            chessboard[1, 7] = new Knight(new Vectors(1, 7), false, this, textures["BK"]);
            chessboard[6, 7] = new Knight(new Vectors(6, 7), false, this, textures["BK"]);

            chessboard[1, 0] = new Knight(new Vectors(1, 0), true, this, textures["WK"]);
            chessboard[6, 0] = new Knight(new Vectors(6, 0), true, this, textures["WK"]);

            chessboard[2, 7] = new Bishop(new Vectors(2, 7), false, this, textures["BB"]);
            chessboard[5, 7] = new Bishop(new Vectors(5, 7), false, this, textures["BB"]);

            chessboard[2, 0] = new Bishop(new Vectors(2, 0), true, this, textures["WB"]);
            chessboard[5, 0] = new Bishop(new Vectors(5, 0), true, this, textures["WB"]);

            chessboard[3, 0] = new Queen(new Vectors(3, 0), true, this, textures["WQ"]);
            chessboard[3, 7] = new Queen(new Vectors(3, 7), false, this, textures["BQ"]);

            chessboard[4, 7] = new King(new Vectors(4, 7), false, this, textures["BKK"]);
            chessboard[4, 0] = new King(new Vectors(4, 0), true, this, textures["WKK"]);
            whiteTurn = true;
            pieceIcons = textures;
            gameMode = GAME;
        }
        public Pieces[,] getChessboard()
        {
            return chessboard;
        }
        public void setChessboard(Pieces[,] newBoard)
        {
            chessboard = newBoard;
        }
        //moves pieces
        public void move(Vectors start, Vectors end)
        {
            Pieces tempP = getPiece(start);
            if(tempP.getisWhite() == whiteTurn)
            {
                tempP.setPositions(end);
                chessboard[end.getX(), end.getY()] = tempP;
                chessboard[start.getX(), start.getY()] = null;
                if (tempP is Pawn || tempP is Rook || tempP is King)
                {
                    tempP.SetMoved(true);
                }
                if(tempP is Pawn)
                {
                    Pawn tempPawn = (Pawn)tempP;
                    tempPawn.promotion();
                }
                if (tempP is King)
                {
                    if(start.compareTo(new Vectors(4,0)) && end.compareTo(new Vectors(6, 0)))
                    {
                        if (start.compareTo(new Vectors(4, 0)) && end.compareTo(new Vectors(6, 0)))
                        {
                            move(new Vectors(7, 0), new Vectors(5, 0));
                            whiteTurn = !whiteTurn;
                        }
                        if (start.compareTo(new Vectors(4, 0)) && end.compareTo(new Vectors(2, 0)))
                        {
                            move(new Vectors(0, 0), new Vectors(3, 0));
                            whiteTurn = !whiteTurn;
                        }
                    }
                    if (start.compareTo(new Vectors(4, 0)) && end.compareTo(new Vectors(2, 0)))
                    {
                        move(new Vectors(0, 0), new Vectors(3, 0));
                        whiteTurn = !whiteTurn;
                    }
                    if (start.compareTo(new Vectors(4, 7)) && end.compareTo(new Vectors(6, 7)))
                    {
                        move(new Vectors(7, 7), new Vectors(5, 7));
                        whiteTurn = !whiteTurn;
                    }
                    if (start.compareTo(new Vectors(4, 7)) && end.compareTo(new Vectors(2, 7)))
                    {
                        move(new Vectors(0, 7), new Vectors(3, 7));
                        whiteTurn = !whiteTurn;
                    }
                }

                whiteTurn = !whiteTurn;
                selected = false;
            }               
            
            
        }
        public Pieces getPiece(Vectors vector)
        {
            return chessboard[vector.getX(), vector.getY()];
        }
        public void setSelected(int x, int y)
        {
            
            if(x <= 800 && x >= 0 && y <= 800 && y >= 0)
            {
                selected_x = (int)Math.Round((double)(((x)/100)));
                selected_y = (int)Math.Round((double)(7 - ((y ) / 100)));
                //Debug.WriteLine(selected_x + "," + selected_y + "," + selected);
                if (chessboard[selected_x, selected_y] != null && chessboard[selected_x, selected_y].getisWhite() == whiteTurn)
                {              
                        selected = true;               
                }
                else
                {
                    selected = false;
                }
            }
            
        }
        //Displays moveable locations
        public void showMoveablePositions(SpriteBatch circleBatch, Texture2D circle)
        {
            
            Vectors[] moves = chessboard[selected_x, selected_y].getMoves();
            if(moves != null )

            for(int i = 0; i < moves.Length;i++)
            {
                if (!moveCauseCheck(chessboard[selected_x, selected_y], moves[i]))
                {
                    circleBatch.Draw(circle, new Rectangle(-10 + (moves[i].getX() * 100), 685 - (moves[i].getY() * 100), 125, 125), Color.White);
                    //Debug.WriteLine(moves[i].getX() + "," + moves[i].getY());
                }
                    
            }
        }
           
        public bool getSelected()
        {
            return selected;
        }
        //Selects move and checks if any piece is blocking check
        public void moveChosen(int x, int y)
        {
            if (chessboard[selected_x, selected_y] != null)
            {
                Vectors[] moves = chessboard[selected_x, selected_y].getMoves();
                if (moves != null)
                    for (int i = 0; i < moves.Length; i++)
                    {
                        if (moves[i].compareTo(new Vectors(x, y)))
                        {
                            if(!moveCauseCheck(chessboard[selected_x, selected_y], moves[i]))
                            {
                                move(new Vectors(selected_x, selected_y), new Vectors(x, y));
                            }

                        }
                    }
            }
            
        }
        //checks if tile is safe
        public bool isTileSafe(Vectors tile,bool isWhite)
        {
            bool temp = true;
            //checks all enemy moves 
            foreach(Pieces piece in chessboard)
            {
                if (piece != null && piece.getisWhite() != isWhite)
                {
                    if (piece is King)
                    {
                        Vectors kingPosition = piece.getPositions();


                        for (int dX = -1; dX <= 1; dX++)
                        {
                            for (int dY = -1; dY <= 1; dY++)
                            {

                                if (dX == 0 && dY == 0)
                                {
                                    continue;
                                }

                                Vectors move = kingPosition.move(dX, dY);
                                if (move != null && move.rangeCheck() && move.compareTo(tile))
                                {
                                    temp = false;
                                    break;
                                }
                            }
                            
                        }
                        if (!temp) break;
                    }
                    else
                    {
                        Vectors[] moves = piece.getMoves();
                        if (moves != null)
                        {
                            for (int k = 0; k < moves.Length; k++)
                            {
                                if (moves[k].compareTo(tile))
                                {
                                    temp = false;
                                    //Debug.WriteLine(piece.getisWhite());
                                    break;
                                }
                            }
                        }
                    }
                    if (!temp) break;
                }

            }
            
            return temp;
        }
        
        public bool checkmate(bool isWhite)
        {
            bool temp = false;
            foreach(Pieces piece in chessboard)
            {
                if(piece is King && piece.getisWhite() == isWhite)
                {
                    if(!isTileSafe(piece.getPositions(),isWhite) && piece.getMoves().Length == 0)
                    {
                        temp = true;
                    }   
                }
                
            }
            return temp;
        }

        public bool moveCauseCheck( Pieces blockingPiece,Vectors moveDestination)
        {
            Vectors kingPosition = null;
            Board cloneBoard = new Board(pieceIcons);
            Pieces[,] arrCloneBoard = new Pieces[8, 8];
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (this.getPiece(new Vectors(i, j)) != null)
                        if (!(this.getPiece(new Vectors(i, j)).Equals(blockingPiece)))
                        {
                            arrCloneBoard[i, j] = chessboard[i, j].clone(cloneBoard);
                            if (this.getPiece(new Vectors(i, j)) is King && this.getPiece(new Vectors(i, j)).getisWhite() == blockingPiece.getisWhite())
                            {
                                kingPosition = new Vectors(i, j);
                            }
                        }
                }
            }
            if(moveDestination.rangeCheck())
            {
                arrCloneBoard[moveDestination.getX(), moveDestination.getY()] = blockingPiece.clone(cloneBoard);
            }

            cloneBoard.setChessboard(arrCloneBoard);
            if (kingPosition != null)
            {
                if (cloneBoard.isTileSafe(kingPosition, blockingPiece.getisWhite()))
                {
                    
                    return false;
                    

                }
                else return true;
            }
            else return false;




        }

        public int getGameMode()
        {
            return gameMode;
        }
        public void setGameMode(int gameMode)
        {
            this.gameMode = gameMode;
        }
        public void promotePiece(Pawn pawnToPromote)
        {
            gameMode = PROMOTION_SELECTION;
            promotionPawn = pawnToPromote;

        }
        public void showPromotionChoice(SpriteBatch promotionBatch, Dictionary<String, Texture2D> textures,Texture2D promotionBackground)
        {
            promotionBatch.Draw(promotionBackground, new Rectangle(100,300, 600, 200), Color.White);
            if(promotionPawn.getisWhite() == true)
            {
                promotionBatch.Draw(textures["WR"], new Rectangle(100,315 , 150, 150), Color.White);
                promotionBatch.Draw(textures["WK"], new Rectangle(250, 315, 150, 150), Color.White);
                promotionBatch.Draw(textures["WB"], new Rectangle(400, 315, 150, 150), Color.White);
                promotionBatch.Draw(textures["WQ"], new Rectangle(550, 315, 150, 150), Color.White);
            }
            else
            {
                promotionBatch.Draw(textures["BR"], new Rectangle(100, 315, 150, 150), Color.White);
                promotionBatch.Draw(textures["BK"], new Rectangle(250, 315, 150, 150), Color.White);
                promotionBatch.Draw(textures["BB"], new Rectangle(400, 315, 150, 150), Color.White);
                promotionBatch.Draw(textures["BQ"], new Rectangle(550, 315, 150, 150), Color.White);
            }
            
        }
        public void promotionChoice(int x,int y, Dictionary<String, Texture2D> textures)
        {
            if (promotionPawn.getisWhite() == true)
            {
                if (y >= 340 && y <= 450)
                {
                    if (x >= 135 && x <= 400)
                    {
                        chessboard[promotionPawn.getPositions().getX(), promotionPawn.getPositions().getY()] = new Rook(promotionPawn.getPositions(), promotionPawn.getisWhite(), this, textures["WR"]);
                        gameMode = GAME;
                    }
                    if (x >= 270 && x <= 375)
                    {
                        chessboard[promotionPawn.getPositions().getX(), promotionPawn.getPositions().getY()] = new Knight(promotionPawn.getPositions(), promotionPawn.getisWhite(), this, textures["WK"]);
                        gameMode = GAME;
                    }
                    if (x >= 430 && x <= 520)
                    {
                        chessboard[promotionPawn.getPositions().getX(), promotionPawn.getPositions().getY()] = new Bishop(promotionPawn.getPositions(), promotionPawn.getisWhite(), this, textures["WB"]);
                        gameMode = GAME;
                    }
                    if (x >= 570 && x <= 670)
                    {
                        chessboard[promotionPawn.getPositions().getX(), promotionPawn.getPositions().getY()] = new Rook(promotionPawn.getPositions(), promotionPawn.getisWhite(), this, textures["WQ"]);
                        gameMode = GAME;
                    }
                }
            }
            else
            {
                if (x >= 135 && x <= 400)
                {
                    chessboard[promotionPawn.getPositions().getX(), promotionPawn.getPositions().getY()] = new Rook(promotionPawn.getPositions(), promotionPawn.getisWhite(), this, textures["BR"]);
                    gameMode = GAME;
                }
                if (x >= 270 && x <= 375)
                {
                    chessboard[promotionPawn.getPositions().getX(), promotionPawn.getPositions().getY()] = new Knight(promotionPawn.getPositions(), promotionPawn.getisWhite(), this, textures["BK"]);
                    gameMode = GAME;
                }
                if (x >= 430 && x <= 520)
                {
                    chessboard[promotionPawn.getPositions().getX(), promotionPawn.getPositions().getY()] = new Bishop(promotionPawn.getPositions(), promotionPawn.getisWhite(), this, textures["BB"]);
                    gameMode = GAME;
                }
                if (x >= 570 && x <= 670)
                {
                    chessboard[promotionPawn.getPositions().getX(), promotionPawn.getPositions().getY()] = new Rook(promotionPawn.getPositions(), promotionPawn.getisWhite(), this, textures["BQ"]);
                    gameMode = GAME;
                }
            }
            
        }
    }

}

