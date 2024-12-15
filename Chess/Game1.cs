using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;



namespace Chess
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch tileBatch;
        private SpriteBatch pieceBatch;
        private SpriteBatch circleBatch;
        private SpriteBatch promotionBatch;
        private SpriteBatch winner;

        Texture2D blackSquare;
        Texture2D whiteSquare;

        Texture2D wPawn;
        Texture2D wRook;
        Texture2D wBishop;
        Texture2D wKnight;
        Texture2D wQueen;
        Texture2D wKing;

        Texture2D bPawn;
        Texture2D bRook;
        Texture2D bBishop;
        Texture2D bKnight;
        Texture2D bQueen;
        Texture2D bKing;

        Texture2D promotionBackground;
        Dictionary<String, Texture2D> textures;
        Texture2D circle;

        Texture2D whiteWins;
        Texture2D blackWins;
        
        Board chessBoard;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //Game resolution
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 800;
        }

        protected override void Initialize()
        {
            tileBatch = new SpriteBatch(GraphicsDevice);
            pieceBatch = new SpriteBatch(GraphicsDevice);
            circleBatch = new SpriteBatch(GraphicsDevice);
            promotionBatch = new SpriteBatch(GraphicsDevice);
            winner = new SpriteBatch(GraphicsDevice);
            blackSquare = new Texture2D(GraphicsDevice, 1, 1);
            blackSquare.SetData(new Color[] { Color.LightGray });
            whiteSquare = new Texture2D(GraphicsDevice, 1, 1);
            whiteSquare.SetData(new Color[] { Color.White });
            promotionBackground = new Texture2D(GraphicsDevice, 1, 1);
            promotionBackground.SetData(new Color[] { Color.Gray });

            wPawn = this.Content.Load<Texture2D>("PawnW");
            wRook = this.Content.Load<Texture2D>("RookW");
            wBishop = this.Content.Load<Texture2D>("BishopW");
            wKnight = this.Content.Load<Texture2D>("KnightW");
            wQueen = this.Content.Load<Texture2D>("QueenW");
            wKing = this.Content.Load<Texture2D>("KingW");

            bPawn = this.Content.Load<Texture2D>("PawnB");
            bRook = this.Content.Load<Texture2D>("RookB");
            bBishop = this.Content.Load<Texture2D>("BishopB");
            bKnight = this.Content.Load<Texture2D>("KnightB");
            bQueen = this.Content.Load<Texture2D>("QueenB");
            bKing = this.Content.Load<Texture2D>("KingB");
            textures = new Dictionary<string, Texture2D>();

            whiteWins = this.Content.Load<Texture2D>("White Wins");
            blackWins = this.Content.Load<Texture2D>("Black Wins");
            circle = this.Content.Load<Texture2D>("Circle");
            textures.Add("WP", wPawn);
            textures.Add("WR", wRook);
            textures.Add("WB", wBishop);
            textures.Add("WK", wKnight);
            textures.Add("WQ", wQueen);
            textures.Add("WKK", wKing);

            textures.Add("BP", bPawn);
            textures.Add("BR", bRook);
            textures.Add("BB", bBishop);
            textures.Add("BK", bKnight);
            textures.Add("BQ", bQueen);
            textures.Add("BKK", bKing);

            chessBoard = new Board(textures);
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
                
            //Handles Mouse input
            MouseState mouse = Mouse.GetState();
            //Debug.WriteLine(mouse.Position.X + "," + mouse.Position.Y);
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                Debug.WriteLine(mouse.Position.X + "," + mouse.Position.Y);
                if (chessBoard.getGameMode() == Board.GAME)
                {
                    if (chessBoard.getSelected() == true)
                    {
                        chessBoard.moveChosen((int)Math.Round((double)(((mouse.Position.X) / 100))), (int)Math.Round((double)(7 - ((mouse.Position.Y) / 100))));

                    }
                    else chessBoard.setSelected(mouse.Position.X, mouse.Position.Y);
                    chessBoard.setSelected(mouse.Position.X, mouse.Position.Y);
                }
                if (chessBoard.getGameMode() == Board.PROMOTION_SELECTION)
                {
                    chessBoard.promotionChoice(mouse.Position.X, mouse.Position.Y,textures);
                }
                
                    
            }
                
                
        
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            //Draws board
            tileBatch.Begin();
            for (int i = 0; i <= 8; i++)
            {
                if (i % 2 == 0)
                {
                    tileBatch.Draw(whiteSquare, new Rectangle(100 * i, 0, 100, 100), Color.White);
                    for (int j = 1; j <= 8; j++)
                    {
                        if (j % 2 == 0)
                        {
                            tileBatch.Draw(whiteSquare, new Rectangle(100 * i, 100 * j, 100, 100), Color.White);
                        }
                        else
                        {
                            tileBatch.Draw(blackSquare, new Rectangle(100 * i, 100 * j, 100, 100), Color.Gray);
                        }

                    }
                }
                else
                {
                    tileBatch.Draw(blackSquare, new Rectangle(100 * i, 0, 100, 100), Color.Gray);
                    for (int j = 1; j <= 8; j++)
                    {
                        if (j % 2 == 0)
                        {

                            tileBatch.Draw(blackSquare, new Rectangle(100 * i, 100 * j, 100, 100), Color.Gray);
                        }
                        else
                        {
                            tileBatch.Draw(whiteSquare, new Rectangle(100 * i, 100 * j, 100, 100), Color.White);
                        }

                    }
                }

            }
            tileBatch.End();

            //Draws Pieces
            pieceBatch.Begin();
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (chessBoard.getPiece(new Vectors(j, i)) != null)
                    {

                        pieceBatch.Draw(chessBoard.getPiece(new Vectors(j, i)).getTexture(), new Rectangle(-10 + (j * 100), 685 - (i * 100), 125, 125), Color.White);
                    }
                }
            }
            pieceBatch.End();
            if (chessBoard.getGameMode() == Board.PROMOTION_SELECTION)
            {
                promotionBatch.Begin();
                chessBoard.showPromotionChoice(promotionBatch, textures, promotionBackground);
                promotionBatch.End();
            }
            if (chessBoard.getGameMode() == Board.GAME)
            {
                //Displays moveable positions
                if (chessBoard.getSelected() == true)
                {
                    circleBatch.Begin();
                    chessBoard.showMoveablePositions(circleBatch, circle);
                    circleBatch.End();
                }
            }
            if (chessBoard.checkmate(false))
            {
                chessBoard.setGameMode(Board.MENU);
                winner.Begin();
                winner.Draw(whiteWins, new Rectangle(200, 200, 400, 400), Color.White);
                winner.End();

            }
            if (chessBoard.checkmate(true))
            {
                chessBoard.setGameMode(Board.MENU);
                winner.Begin();
                winner.Draw(blackWins, new Rectangle(200, 200, 400, 400), Color.White);
                winner.End();
            }
            base.Draw(gameTime);
        }
    }
}
