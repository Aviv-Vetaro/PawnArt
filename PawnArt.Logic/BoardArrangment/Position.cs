using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PawnArt.Logic.BoardArrangment
{
    /// <summary>
    /// represents a chess position. which meansthe arrangment of pieces on the squres in a certain moment, as well as additional metadata
    /// </summary>
    public record Position
    {
        public List<Square> squares
        {
            get;
            set;
        }
        /// <summary>
        /// the squres, (and the pices on them) in the position
        /// </summary>
        [NotMapped]
        public IReadOnlyList<Square> SquaresInPosition
        {
            get
            {
                return squares.AsReadOnly();
            }
        }
        private int At(int row, int file)
        {
            return row * 8 + file;
        }
        private bool canWhiteCastleKingSide;
        private bool canWhiteCastleQueenSide;
        private bool canBlackCastleKingSide;
        private bool canBlackCastleQueenSide;
        private Square? enPassentTakablePawnOfWhite;
        private Square? enPassentTakablePawnOfBlack;
        private const int RowAmount = 8;
        private const int FileAmount = 8;

        private int _50MovesRuleCounter;
        [NotMapped]
        private IDictionary<PieceType, Func<int, int, IEnumerable<Ply>>> possibleMovesGenerationFunctionsDictinary;
        /// <summary>
        /// 
        /// </summary>
        /// <returns>the starting position</returns>
        public static Position StartingPosition()
        {
            Position startingPosition = new Position() with
            {
                PlayerToPlayColor = PieceColor.White
            };


            startingPosition.possibleMovesGenerationFunctionsDictinary = new Dictionary<PieceType, Func<int, int, IEnumerable<Ply>>>()
            {
                {PieceType.Pawn, startingPosition.GeneratePawnPlys},
                {PieceType.Knight, startingPosition.GenerateKnightPlys},
                {PieceType.Bishop, startingPosition.GenerateBishopPlys},
                {PieceType.Rook, startingPosition.GenerateRookPly},
                {PieceType.Queen, startingPosition.GenerateQueenPlys},
                {PieceType.King, startingPosition.GenerateKingPlys}
            };
            startingPosition.squares = new List<Square>()
            {

                    new Square(new Piece(PieceType.Rook, PieceColor.Black)),
                    new Square(new Piece(PieceType.Knight, PieceColor.Black)),
                    new Square(new Piece(PieceType.Bishop, PieceColor.Black)),
                    new Square(new Piece(PieceType.Queen, PieceColor.Black)),
                    new Square(new Piece(PieceType.King, PieceColor.Black)),
                    new Square(new Piece(PieceType.Bishop, PieceColor.Black)),
                    new Square(new Piece(PieceType.Knight, PieceColor.Black)),
                    new Square(new Piece(PieceType.Rook, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Rook, PieceColor.White)),
                    new Square(new Piece(PieceType.Knight, PieceColor.White)),
                    new Square(new Piece(PieceType.Bishop, PieceColor.White)),
                    new Square(new Piece(PieceType.Queen, PieceColor.White)),
                    new Square(new Piece(PieceType.King, PieceColor.White)),
                    new Square(new Piece(PieceType.Bishop, PieceColor.White)),
                    new Square(new Piece(PieceType.Knight, PieceColor.White)),
                    new Square(new Piece(PieceType.Rook, PieceColor.White))
            };
            startingPosition.squares.Reverse();
            for(int row = 0; row < RowAmount; row++)
            {
                for(int file = 0; file < FileAmount; file++)
                {
                    startingPosition.squares[startingPosition.At(row, file)].SetLocation(row, file);
                }
            }
            startingPosition._50MovesRuleCounter = 0;
            return startingPosition;
        }
        private Position()
        {

        }
        /// <summary>
        /// returns weather the position is drawn becuse of the 50 move rule
        /// </summary>
        public bool DrawnBy50MovesRule
        {
            get
            {
                if(_50MovesRuleCounter == 100)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// the player to play color
        /// </summary>
        public PieceColor PlayerToPlayColor
        {
            get;
            init;
        }
        /// <summary>
        /// preform a ply on the position (play it)
        /// </summary>
        /// <param name="plyMade">the ply that was played</param>
        /// <returns>the position after the ply</returns>
        public Position AfterPly(Ply plyMade)
        {
            Position newPosition = this with
            {
            };
            newPosition.squares = squares.AsEnumerable().ToList();
            if(plyMade.Desitination.Occupier is not null || plyMade.Piece.Type is PieceType.Pawn)
                newPosition._50MovesRuleCounter = 0;
            else
                newPosition._50MovesRuleCounter++;
            if(PlayerToPlayColor is PieceColor.White)
                newPosition.enPassentTakablePawnOfWhite = null;
            else
                newPosition.enPassentTakablePawnOfBlack = null;
            if(plyMade.Piece.Type is PieceType.King)
            {
                if(PlayerToPlayColor is PieceColor.White)
                {
                    newPosition.canWhiteCastleKingSide = false;
                    newPosition.canWhiteCastleQueenSide = false;
                }
                else
                {
                    newPosition.canBlackCastleKingSide = false;
                    newPosition.canBlackCastleQueenSide = false;
                }
            }
            else if(plyMade.Piece.Type is PieceType.Rook)
            {
                if(plyMade.Piece.Color is PieceColor.White)
                {
                    if(plyMade.Source == squares[At(0, 0)])
                        newPosition.canWhiteCastleQueenSide = false;
                    else if(plyMade.Source == squares[At(0, 7)])
                        newPosition.canWhiteCastleKingSide = false;
                }
                else
                {
                    if(plyMade.Source == squares[At(7, 0)])
                        newPosition.canBlackCastleQueenSide = false;
                    else if(plyMade.Source == squares[At(7, 7)])
                        newPosition.canBlackCastleKingSide = false;
                }
            }
            else if(plyMade.Piece.Type is PieceType.Pawn)
            {
                if(plyMade.Piece.Color is PieceColor.White)
                {
                    if(Math.Abs(plyMade.Source.Row - plyMade.Desitination.Row) == 2)
                        newPosition.enPassentTakablePawnOfWhite = plyMade.Desitination;
                }
                else
                {
                    if(Math.Abs(plyMade.Source.Row - plyMade.Desitination.Row) == 2)
                        newPosition.enPassentTakablePawnOfBlack = plyMade.Desitination;
                }
            }
            if(plyMade.Piece.Type is PieceType.Rook)
            {
                if(PlayerToPlayColor is PieceColor.White)
                {
                    newPosition.canWhiteCastleKingSide = false;
                    newPosition.canWhiteCastleQueenSide = false;
                }
                else
                {
                    newPosition.canBlackCastleKingSide = false;
                    newPosition.canBlackCastleQueenSide = false;
                }

            }
            if(plyMade == Ply.WhiteKingSideCastle)
            {
                newPosition.squares[At(0, 4)] = newPosition.squares[At(0, 4)] with
                {
                    Occupier = null
                };
                newPosition.squares[At(0, 7)] = newPosition.squares[At(0, 7)] with
                {
                    Occupier = null
                };
                newPosition.squares[At(0, 6)] = newPosition.squares[At(0, 6)] with
                {
                    Occupier = new Piece(PieceType.King, PieceColor.White)
                };
                newPosition.squares[At(0, 5)] = newPosition.squares[At(0, 5)] with
                {
                    Occupier = new Piece(PieceType.Rook, PieceColor.White)
                };
                return newPosition;
            }
            else if(plyMade == Ply.WhiteQueenSideCastle)
            {
                newPosition.squares[At(0, 4)] = newPosition.squares[At(0, 4)] with
                {
                    Occupier = null
                };
                newPosition.squares[At(0, 0)] = newPosition.squares[At(0, 0)] with
                {
                    Occupier = null
                };
                newPosition.squares[At(0, 2)] = newPosition.squares[At(0, 2)] with
                {
                    Occupier = new Piece(PieceType.King, PieceColor.White)
                };
                newPosition.squares[At(0, 3)] = newPosition.squares[At(0, 3)] with
                {
                    Occupier = new Piece(PieceType.Rook, PieceColor.White)
                };
                return newPosition;
            }
            else if(plyMade == Ply.BlackKingSideCastle)
            {
                newPosition.squares[At(7, 4)] = newPosition.squares[At(7, 4)] with
                {
                    Occupier = null
                };
                newPosition.squares[At(7, 0)] = newPosition.squares[At(7, 0)] with
                {
                    Occupier = null
                };
                newPosition.squares[At(7, 2)] = newPosition.squares[At(7, 2)] with
                {
                    Occupier = new Piece(PieceType.King, PieceColor.White)
                };
                newPosition.squares[At(7, 3)] = newPosition.squares[At(7, 3)] with
                {
                    Occupier = new Piece(PieceType.Rook, PieceColor.White)
                };
                return newPosition;
            }
            else if(plyMade == Ply.BlackQueenSideCastle)
            {
                newPosition.squares[At(7, 4)] = newPosition.squares[At(7, 4)] with
                {
                    Occupier = null
                };
                newPosition.squares[At(7, 0)] = newPosition.squares[At(7, 0)] with
                {
                    Occupier = null
                };
                newPosition.squares[At(7, 2)] = newPosition.squares[At(7, 2)] with
                {
                    Occupier = new Piece(PieceType.King, PieceColor.White)
                };
                newPosition.squares[At(7, 3)] = newPosition.squares[At(7, 3)] with
                {
                    Occupier = new Piece(PieceType.Rook, PieceColor.White)
                };
                return newPosition;
            }
            else
            {
                if(plyMade.Desitination.Occupier is null)
                {
                    if(PlayerToPlayColor is PieceColor.White)
                    {
                        if(( InRange(plyMade.Source.Row, plyMade.Source.File + 1) && squares[At(plyMade.Source.Row, plyMade.Source.File + 1)] == enPassentTakablePawnOfBlack ) ||
                            ( InRange(plyMade.Source.Row, plyMade.Source.File - 1) && squares[At(plyMade.Source.Row, plyMade.Source.File - 1)] == enPassentTakablePawnOfBlack )
                            )
                        {
                            newPosition.squares[At(enPassentTakablePawnOfBlack.Row, enPassentTakablePawnOfBlack.File)] = newPosition.enPassentTakablePawnOfBlack with
                            {
                                Occupier = null
                            };
                        }
                    }
                    else
                    {
                        if(( InRange(plyMade.Source.Row, plyMade.Source.File + 1) && squares[At(plyMade.Source.Row, plyMade.Source.File + 1)] == enPassentTakablePawnOfWhite ) ||
                            ( InRange(plyMade.Source.Row, plyMade.Source.File - 1) && squares[At(plyMade.Source.Row, plyMade.Source.File - 1)] == enPassentTakablePawnOfWhite )
                            )
                        {
                            newPosition.squares[At(enPassentTakablePawnOfWhite.Row, enPassentTakablePawnOfWhite.File)] = newPosition.enPassentTakablePawnOfWhite with
                            {
                                Occupier = null
                            };
                        }
                    }
                    newPosition.squares[At(plyMade.Desitination.Row, plyMade.Desitination.File)] = plyMade.Desitination with
                    {
                        Occupier = plyMade.Source.Occupier.Type == PieceType.Pawn && ( plyMade.Desitination.Row == 0 || plyMade.Desitination.Row == 7 ) ? new Piece((PieceType) plyMade.PiecePromoted, plyMade.Source.Occupier.Color) : plyMade.Source.Occupier
                    };
                    newPosition.squares[At(plyMade.Source.Row, plyMade.Source.File)] = plyMade.Source with
                    {
                        Occupier = null
                    };
                }
            }
            newPosition = newPosition with
            {
                PlayerToPlayColor = this.PlayerToPlayColor is PieceColor.White ? PieceColor.Black : PieceColor.White
            };
            return newPosition;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns><see langword="abstract"/>all the possible moves for black at the current postion</returns>
        public IEnumerable<Ply> PossiblePlysForBlack()
        {
            if(PlayerToPlayColor == PieceColor.White)
                return new List<Ply>();
            return PossiblePlysForBlackIgnoreTurn();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns><see langword="abstract"/>all the possible moves for white at the current position</returns>
        public IEnumerable<Ply> PossiblePlysForWhite()
        {
            if(PlayerToPlayColor == PieceColor.Black)
                return new List<Ply>();
            return PossiblePlysForWhiteIgnoreTurn();
        }

        private IEnumerable<Ply> PossiblePlysForBlackIgnoreTurn()
        {
            List<Ply> result = new List<Ply>();
            for(int row = 0; row < RowAmount; row++)
            {
                for(int file = 0; file < FileAmount; file++)
                {
                    Square currentSquare = squares[At(row, file)];
                    if(currentSquare.Occupier is not null && currentSquare.Occupier.Color == PieceColor.Black)
                    {
                        result.AddRange(possibleMovesGenerationFunctionsDictinary[currentSquare.Occupier.Type](row, file));
                    }
                }
            }
            return result;
        }
        private IEnumerable<Ply> PossiblePlysForWhiteIgnoreTurn()
        {
            List<Ply> result = new List<Ply>();
            for(int row = 0; row < RowAmount; row++)
            {
                for(int file = 0; file < FileAmount; file++)
                {
                    Square currentSquare = squares[At(row, file)];
                    if(currentSquare.Occupier is not null && currentSquare.Occupier.Color == PieceColor.White)
                    {
                        result.AddRange(possibleMovesGenerationFunctionsDictinary[currentSquare.Occupier.Type](row, file));
                    }
                }
            }
            return result;
        }
        private IEnumerable<Ply> GeneratePawnPlys(int row, int file)
        {
            Ply currentPlyGenerated;
            List<Ply> possiblePlys = new List<Ply>();
            Square fromSqure = squares[At(row, file)];
            Piece pieceMoved = fromSqure.Occupier;
            if(PlayerToPlayColor is PieceColor.White && row == 6)
            {
                currentPlyGenerated = new Ply(fromSqure, squares[At(row + 1, file)], PieceType.Bishop);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Pawn, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                currentPlyGenerated = new Ply(fromSqure, squares[At(row + 1, file)], PieceType.Rook);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Pawn, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                currentPlyGenerated = new Ply(fromSqure, squares[At(row + 1, file)], PieceType.Queen);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Pawn, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                currentPlyGenerated = new Ply(fromSqure, squares[At(row + 1, file)], PieceType.Knight);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Pawn, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            else if(PlayerToPlayColor is PieceColor.Black && row == 1)
            {
                currentPlyGenerated = new Ply(fromSqure, squares[At(row - 1, file)], PieceType.Bishop);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Pawn, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                currentPlyGenerated = new Ply(fromSqure, squares[At(row - 1, file)], PieceType.Rook);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Pawn, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                currentPlyGenerated = new Ply(fromSqure, squares[At(row - 1, file)], PieceType.Queen);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Pawn, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                currentPlyGenerated = new Ply(fromSqure, squares[At(row - 1, file)], PieceType.Knight);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Pawn, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            else
            {
                currentPlyGenerated = new Ply(fromSqure, squares[At(row + 1, file)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Pawn, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            currentPlyGenerated = new Ply(fromSqure, squares[At(row + 2, file)]);
            currentPlyGenerated = currentPlyGenerated with
            {
                Piece = new Piece(PieceType.Pawn, PlayerToPlayColor)
            };
            possiblePlys.Add(currentPlyGenerated);

            currentPlyGenerated = new Ply(fromSqure, squares[At(row + 1, file + 1)]);
            currentPlyGenerated = currentPlyGenerated with
            {
                Piece = new Piece(PieceType.Pawn, PlayerToPlayColor)
            };
            possiblePlys.Add(currentPlyGenerated);

            currentPlyGenerated = new Ply(fromSqure, squares[At(row + 1, file - 1)]);
            currentPlyGenerated = currentPlyGenerated with
            {
                Piece = new Piece(PieceType.Pawn, PlayerToPlayColor)
            };
            possiblePlys.Add(currentPlyGenerated);

            return possiblePlys.Where(ply => IsLeaglePawnPlyInsquaresContext(ply));
        }
        private IEnumerable<Ply> GenerateKnightPlys(int row, int file)
        {
            Ply currentPlyGenerated;
            List<Ply> possiblePlys = new List<Ply>();
            Square fromSqure = squares[At(row, file)];
            Piece pieceMoved = fromSqure.Occupier;

            if(InRange(row + 1, file + 2))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row + 1, file + 2)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Knight, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 1, file + 2))
            {
                currentPlyGenerated = new Ply(fromSqure, squares[At(row - 1, file + 2)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Knight, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row + 1, file - 2))
            {
                currentPlyGenerated = new Ply(fromSqure, squares[At(row + 1, file - 2)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Knight, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 1, file - 2))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row - 1, file - 2)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Knight, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row + 2, file + 1))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row + 2, file + 1)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Knight, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 2, file + 1))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row - 2, file + 1)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Knight, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row + 2, file - 1))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row + 2, file - 1)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Knight, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 2, file - 1))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row - 2, file - 1)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Knight, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            return possiblePlys.Where(ply => IsLeagleKnightPlyInsquaresContext(ply));
        }
        private IEnumerable<Ply> GenerateBishopPlys(int row, int file)
        {
            Ply currentPlyGenerated;
            List<Ply> possiblePlys = new List<Ply>();
            Square fromSqure = squares[At(row, file)];
            Piece pieceMoved = fromSqure.Occupier;

            int possibleMoveRow = row + 1;
            int possibleMoveFile = file + 1;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(fromSqure, squares[At(possibleMoveRow, possibleMoveFile)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Knight, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveRow += 1;
                possibleMoveFile += 1;
            }
            possibleMoveRow = row + 1;
            possibleMoveFile = file - 1;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(fromSqure, squares[At(possibleMoveRow, possibleMoveFile)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Knight, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveRow += 1;
                possibleMoveFile -= 1;
            }
            possibleMoveRow = row - 1;
            possibleMoveFile = file + 1;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(fromSqure, squares[At(possibleMoveRow, possibleMoveFile)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Knight, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveRow -= 1;
                possibleMoveFile += 1;
            }
            possibleMoveRow = row - 1;
            possibleMoveFile = file - 1;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(fromSqure, squares[At(possibleMoveRow, possibleMoveFile)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Knight, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveRow -= 1;
                possibleMoveFile -= 1;
            }
            return possiblePlys.Where(ply => IsLeagleBishopPlyInsquaresContext(ply));
        }
        private IEnumerable<Ply> GenerateRookPly(int row, int file)
        {
            Ply currentPlyGenerated;
            List<Ply> possiblePlys = new List<Ply>();
            Square fromSqure = squares[At(row, file)];
            Piece pieceMoved = fromSqure.Occupier;

            int possibleMoveRow = row;
            int possibleMoveFile = file + 1;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(fromSqure, squares[At(possibleMoveRow, possibleMoveFile)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Rook, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveFile += 1;
            }
            possibleMoveRow = row;
            possibleMoveFile = file - 1;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(fromSqure, squares[At(possibleMoveRow, possibleMoveFile)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Rook, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveFile -= 1;
            }
            possibleMoveRow = row + 1;
            possibleMoveFile = file;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(fromSqure, squares[At(possibleMoveRow, possibleMoveFile)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Rook, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveRow += 1;
            }
            possibleMoveRow = row - 1;
            possibleMoveFile = file;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(fromSqure, squares[At(possibleMoveRow, possibleMoveFile)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Rook, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveRow -= 1;
            }
            return possiblePlys.Where(ply => IsLeagleRookPlyInsquaresContext(ply));

        }
        private IEnumerable<Ply> GenerateQueenPlys(int row, int file)
        {
            return GenerateRookPly(row, file).Concat(GenerateBishopPlys(row, file)).Select(q => q with { Piece = new Piece(PieceType.Queen, PlayerToPlayColor) });
        }
        private IEnumerable<Ply> GenerateKingPlys(int row, int file)
        {
            Ply currentPlyGenerated;
            List<Ply> possiblePlys = new List<Ply>();
            Square fromSqure = squares[At(row, file)];
            Piece pieceMoved = fromSqure.Occupier;
            if(InRange(row + 1, file + 1))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row + 1, file + 1)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.Knight, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 1, file + 1))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row - 1, file + 1)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.King, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row + 1, file - 1))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row + 1, file - 1)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.King, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 1, file - 1))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row - 1, file - 1)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.King, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row, file + 1))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row, file + 1)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.King, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row, file - 1))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row, file - 1)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.King, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row + 1, file))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row + 1, file)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.King, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 1, file))
            {

                currentPlyGenerated = new Ply(fromSqure, squares[At(row - 1, file)]);
                currentPlyGenerated = currentPlyGenerated with
                {
                    Piece = new Piece(PieceType.King, PlayerToPlayColor)
                };
                possiblePlys.Add(currentPlyGenerated);
            }
            if(PlayerToPlayColor is PieceColor.White)
            {
                possiblePlys.Add(Ply.WhiteKingSideCastle);
                possiblePlys.Add(Ply.WhiteQueenSideCastle);
            }
            else
            {
                possiblePlys.Add(Ply.BlackKingSideCastle);
                possiblePlys.Add(Ply.BlackQueenSideCastle);
            }

            return possiblePlys.Where(ply => IsLeagleKingPlyInsquaresContext(ply));
        }
        private bool IsKingSafeAfterPly(Ply plyToSearch)
        {
            Position nextPosition = this.AfterPly(plyToSearch);
            if(PlayerToPlayColor == PieceColor.White)
            {
                var plys = nextPosition.PossiblePlysForBlack();
                if(plys.Any(ply => ply.Desitination.Occupier == new Piece(PieceType.King, PieceColor.White)))
                    return false;
                return true;
            }
            else
            {
                var plys = nextPosition.PossiblePlysForWhite();
                if(plys.Any(ply => ply.Desitination.Occupier == new Piece(PieceType.King, PieceColor.Black)))
                    return false;
                return true;
            }
        }
        private bool IsLeaglePawnPlyInsquaresContext(Ply ply)
        {
            if(!IsLeaglePlyInsquaresContext(ply))
                return false;
            if(ply.Source.File != ply.Desitination.File)
            {
                if(ply.Desitination.Occupier is not null)
                    return true;
                else
                {
                    if(PlayerToPlayColor is PieceColor.White)
                    {
                        if(squares[At(ply.Desitination.Row - 1, ply.Desitination.File)] == enPassentTakablePawnOfBlack)
                            return true;
                    }
                    else
                    {
                        if(squares[At(ply.Desitination.Row + 1, ply.Desitination.File)] == enPassentTakablePawnOfWhite)
                            return true;
                    }
                    return false;
                }

            }
            return true;
        }
        private bool IsLeagleKingPlyInsquaresContext(Ply ply)
        {
            if(PlayerToPlayColor is PieceColor.White)
            {
                if(ply == Ply.WhiteKingSideCastle && canWhiteCastleKingSide)
                    return true;
                if(ply == Ply.WhiteQueenSideCastle && canWhiteCastleQueenSide)
                    return true;
            }
            else
            {
                if(ply == Ply.BlackKingSideCastle && canBlackCastleKingSide)
                    return true;
                if(ply == Ply.BlackQueenSideCastle && canBlackCastleQueenSide)
                    return true;
            }
            return IsKingSafeAfterPly(ply);
        }
        private bool IsLeagleKnightPlyInsquaresContext(Ply ply)
        {
            return IsLeaglePlyInsquaresContext(ply);
        }
        private bool IsLeagleBishopPlyInsquaresContext(Ply ply)
        {
            if(!IsLeaglePlyInsquaresContext(ply))
                return false;
            for(int row = ply.Source.Row, file = ply.Source.File;
                row != ply.Desitination.Row && file != ply.Desitination.File;
                row += row < ply.Desitination.Row ? +1 : -1, file += file < ply.Desitination.File ? +1 : -1)
            {
                if(squares[At(row, file)] is not null)
                    return false;
            }
            return true;
        }
        private bool IsLeagleQueenPlyInsquaresContext(Ply ply)
        {
            if(!IsLeaglePlyInsquaresContext(ply))
                return false;
            if(IsLeagleBishopPlyInsquaresContext(ply))
                return true;
            if(IsLeagleRookPlyInsquaresContext(ply))
                return true;
            return false;
        }
        private bool IsLeagleRookPlyInsquaresContext(Ply ply)
        {
            if(!IsLeaglePlyInsquaresContext(ply))
                return false;
            if(ply.Desitination.File == ply.Source.File)
            {
                for(int row = ply.Source.Row; row != ply.Desitination.Row; row += row < ply.Desitination.Row ? +1 : -1)
                {
                    if(squares[At(row, ply.Desitination.File)] is not null)
                        return false;
                }
            }
            else
            {
                for(int file = ply.Source.File; file != ply.Desitination.File; file += file < ply.Desitination.File ? +1 : -1)
                {
                    if(squares[At(ply.Desitination.Row, file)] is not null)
                        return false;
                }
            }
            return true;

        }
        private bool IsLeaglePlyInsquaresContext(Ply ply)
        {
            if(ply.Desitination.Occupier is not null && ply.Desitination.Occupier.Color == PlayerToPlayColor)
                return false;
            return IsKingSafeAfterPly(ply);
        }
        private bool InRange(int row, int file)
        {
            return row < RowAmount && row >= 0 && file < FileAmount && file >= 0;
        }

    }


























    /*
    public record Position
    {
        //squaresInPosition
        private List<Square> squaresInPosition;
        public IReadOnlyList<Square> SquaresInPosition
        {
            get
            {
                return squaresInPosition.AsReadOnly();
            }
            private set
            {
                squaresInPosition = value.ToList();
            }
        }
        private int At(int row, int file)
        {
            return row * 8 + file;
        }
        private bool canWhiteCastleKingSide;
        private bool canWhiteCastleQueenSide;
        private bool canBlackCastleKingSide;
        private bool canBlackCastleQueenSide;
        public Square? EnPassentTakablePawnOfWhite
        {
            get;
            private set;
        }
        public Square? EnPassentTakablePawnOfBlack
        {
            get;
            private set;
        }
        private const int RowAmount = 8;
        private const int FileAmount = 8;

        private int _50MovesRuleCounter;
        private readonly IDictionary<PieceType, Func<int, int, IEnumerable<Ply>>> possibleMovesGenerationFunctionsDictinary;
        public Position()
        {
            PlayerToPlayColor = PieceColor.White;
            possibleMovesGenerationFunctionsDictinary = new Dictionary<PieceType, Func<int, int, IEnumerable<Ply>>>()
            {
                {PieceType.Pawn, GeneratePawnPlys},
                {PieceType.Knight, GenerateKnightPlys},
                {PieceType.Bishop, GenerateBishopPlys},
                {PieceType.Rook, GenerateRookPly},
                {PieceType.Queen, GenerateQueenPlys},
                {PieceType.King, GenerateKingPlys}
            };
            squaresInPosition = new List<Square>()
            {

                    new Square(new Piece(PieceType.Rook, PieceColor.Black)),
                    new Square(new Piece(PieceType.Knight, PieceColor.Black)),
                    new Square(new Piece(PieceType.Bishop, PieceColor.Black)),
                    new Square(new Piece(PieceType.Queen, PieceColor.Black)),
                    new Square(new Piece(PieceType.King, PieceColor.Black)),
                    new Square(new Piece(PieceType.Bishop, PieceColor.Black)),
                    new Square(new Piece(PieceType.Knight, PieceColor.Black)),
                    new Square(new Piece(PieceType.Rook, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.Black)),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(null),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Pawn, PieceColor.White)),
                    new Square(new Piece(PieceType.Rook, PieceColor.White)),
                    new Square(new Piece(PieceType.Knight, PieceColor.White)),
                    new Square(new Piece(PieceType.Bishop, PieceColor.White)),
                    new Square(new Piece(PieceType.Queen, PieceColor.White)),
                    new Square(new Piece(PieceType.King, PieceColor.White)),
                    new Square(new Piece(PieceType.Bishop, PieceColor.White)),
                    new Square(new Piece(PieceType.Knight, PieceColor.White)),
                    new Square(new Piece(PieceType.Rook, PieceColor.White))
            };
            squaresInPosition.Reverse();
            for(int row = 0; row < RowAmount; row++)
            {
                for(int file = 0; file < FileAmount; file++)
                {
                    squaresInPosition[At(row, file)].SetLocation(row, file);
                }
            }
            _50MovesRuleCounter = 0;
        }
        public bool DrawnBy50MovesRule
        {
            get
            {
                if(_50MovesRuleCounter == 100)
                    return true;
                return false;
            }
        }
        public PieceColor PlayerToPlayColor
        {
            get;
        }
        public Position AfterPly(Ply plyMade)
        {
            Position newPosition = new Position(this);
            newPosition.squaresInPosition.Clear();
            newPosition.squaresInPosition.AddRange(this.squaresInPosition);
            if(plyMade.Desitination.Occupier is not null || plyMade.Piece.Type is PieceType.Pawn)
                newPosition._50MovesRuleCounter = 0;
            else
                newPosition._50MovesRuleCounter++;
            if(PlayerToPlayColor is PieceColor.White)
                newPosition.EnPassentTakablePawnOfWhite = null;
            else
                newPosition.EnPassentTakablePawnOfBlack = null;
            if(plyMade.Piece.Type is PieceType.King)
            {
                if(PlayerToPlayColor is PieceColor.White)
                {
                    newPosition.canWhiteCastleKingSide = false;
                    newPosition.canWhiteCastleQueenSide = false;
                }
                else
                {
                    newPosition.canBlackCastleKingSide = false;
                    newPosition.canBlackCastleQueenSide = false;
                }
            }
            else if(plyMade.Piece.Type is PieceType.Rook)
            {
                if(plyMade.Piece.Color is PieceColor.White)
                {
                    if(plyMade.Source == squaresInPosition[At(0, 0)])
                        newPosition.canWhiteCastleQueenSide = false;
                    else if(plyMade.Source == squaresInPosition[At(0, 7)])
                        newPosition.canWhiteCastleKingSide = false;
                }
                else
                {
                    if(plyMade.Source == squaresInPosition[At(7, 0)])
                        newPosition.canBlackCastleQueenSide = false;
                    else if(plyMade.Source == squaresInPosition[At(7, 7)])
                        newPosition.canBlackCastleKingSide = false;
                }
            }
            else if(plyMade.Piece.Type is PieceType.Pawn)
            {
                if(plyMade.Piece.Color is PieceColor.White)
                {
                    if(Math.Abs(plyMade.Source.Row - plyMade.Desitination.Row) == 2)
                        newPosition.EnPassentTakablePawnOfWhite = plyMade.Desitination;
                }
                else
                {
                    if(Math.Abs(plyMade.Source.Row - plyMade.Desitination.Row) == 2)
                        newPosition.EnPassentTakablePawnOfBlack = plyMade.Desitination;
                }
            }
            if(plyMade.Piece.Type is PieceType.Rook)
            {
                if(PlayerToPlayColor is PieceColor.White)
                {
                    newPosition.canWhiteCastleKingSide = false;
                    newPosition.canWhiteCastleQueenSide = false;
                }
                else
                {
                    newPosition.canBlackCastleKingSide = false;
                    newPosition.canBlackCastleQueenSide = false;
                }

            }
            if(plyMade == Ply.WhiteKingSideCastle)
            {
                newPosition.squaresInPosition[At(0, 4)] = newPosition.squaresInPosition[At(0, 4)] with
                {
                    Occupier = null
                };
                newPosition.squaresInPosition[At(0, 7)] = newPosition.squaresInPosition[At(0, 7)] with
                {
                    Occupier = null
                };
                newPosition.squaresInPosition[At(0, 6)] = newPosition.squaresInPosition[At(0, 6)] with
                {
                    Occupier = new Piece(PieceType.King, PieceColor.White)
                };
                newPosition.squaresInPosition[At(0, 5)] = newPosition.squaresInPosition[At(0, 5)] with
                {
                    Occupier = new Piece(PieceType.Rook, PieceColor.White)
                };
                return newPosition;
            }
            else if(plyMade == Ply.WhiteQueenSideCastle)
            {
                newPosition.squaresInPosition[At(0, 4)] = newPosition.squaresInPosition[At(0, 4)] with
                {
                    Occupier = null
                };
                newPosition.squaresInPosition[At(0, 0)] = newPosition.squaresInPosition[At(0, 0)] with
                {
                    Occupier = null
                };
                newPosition.squaresInPosition[At(0, 2)] = newPosition.squaresInPosition[At(0, 2)] with
                {
                    Occupier = new Piece(PieceType.King, PieceColor.White)
                };
                newPosition.squaresInPosition[At(0, 3)] = newPosition.squaresInPosition[At(0, 3)] with
                {
                    Occupier = new Piece(PieceType.Rook, PieceColor.White)
                };
                return newPosition;
            }
            else if(plyMade == Ply.BlackKingSideCastle)
            {
                newPosition.squaresInPosition[At(7, 4)] = newPosition.squaresInPosition[At(7, 4)] with
                {
                    Occupier = null
                };
                newPosition.squaresInPosition[At(7, 0)] = newPosition.squaresInPosition[At(7, 0)] with
                {
                    Occupier = null
                };
                newPosition.squaresInPosition[At(7, 2)] = newPosition.squaresInPosition[At(7, 2)] with
                {
                    Occupier = new Piece(PieceType.King, PieceColor.White)
                };
                newPosition.squaresInPosition[At(7, 3)] = newPosition.squaresInPosition[At(7, 3)] with
                {
                    Occupier = new Piece(PieceType.Rook, PieceColor.White)
                };
                return newPosition;
            }
            else if(plyMade == Ply.BlackQueenSideCastle)
            {
                newPosition.squaresInPosition[At(7, 4)] = newPosition.squaresInPosition[At(7, 4)] with
                {
                    Occupier = null
                };
                newPosition.squaresInPosition[At(7, 0)] = newPosition.squaresInPosition[At(7, 0)] with
                {
                    Occupier = null
                };
                newPosition.squaresInPosition[At(7, 2)] = newPosition.squaresInPosition[At(7, 2)] with
                {
                    Occupier = new Piece(PieceType.King, PieceColor.White)
                };
                newPosition.squaresInPosition[At(7, 3)] = newPosition.squaresInPosition[At(7, 3)] with
                {
                    Occupier = new Piece(PieceType.Rook, PieceColor.White)
                };
                return newPosition;
            }
            else
            {
                if(plyMade.Desitination.Occupier is null)
                {
                    if(PlayerToPlayColor is PieceColor.White)
                    {
                        if(( InRange(plyMade.Source.Row, plyMade.Source.File + 1) && squaresInPosition[At(plyMade.Source.Row, plyMade.Source.File + 1)] == EnPassentTakablePawnOfBlack ) ||
                            ( InRange(plyMade.Source.Row, plyMade.Source.File - 1) && squaresInPosition[At(plyMade.Source.Row, plyMade.Source.File - 1)] == EnPassentTakablePawnOfBlack )
                            )
                        {
                            newPosition.squaresInPosition[At(EnPassentTakablePawnOfBlack.Row, EnPassentTakablePawnOfBlack.File)] = newPosition.EnPassentTakablePawnOfBlack with
                            {
                                Occupier = null
                            };
                        }
                    }
                    else
                    {
                        if(( InRange(plyMade.Source.Row, plyMade.Source.File + 1) && squaresInPosition[At(plyMade.Source.Row, plyMade.Source.File + 1)] == EnPassentTakablePawnOfWhite ) ||
                            ( InRange(plyMade.Source.Row, plyMade.Source.File - 1) && squaresInPosition[At(plyMade.Source.Row, plyMade.Source.File - 1)] == EnPassentTakablePawnOfWhite )
                            )
                        {
                            newPosition.squaresInPosition[At(EnPassentTakablePawnOfWhite.Row, EnPassentTakablePawnOfWhite.File)] = newPosition.EnPassentTakablePawnOfWhite with
                            {
                                Occupier = null
                            };
                        }
                    }
                    newPosition.squaresInPosition[At(plyMade.Desitination.Row, plyMade.Desitination.File)] = plyMade.Desitination with
                    {
                        Occupier = plyMade.Source.Occupier.Type == PieceType.Pawn && ( plyMade.Desitination.Row == 0 || plyMade.Desitination.Row == 7 ) ? new Piece((PieceType) plyMade.PiecePromoted, plyMade.Source.Occupier.Color) : plyMade.Source.Occupier
                    };
                    newPosition.squaresInPosition[At(plyMade.Source.Row, plyMade.Source.File)] = plyMade.Source with
                    {
                        Occupier = null
                    };
                }
            }
            return newPosition;
        }
        public IEnumerable<Ply> PossiblePlysForBlack()
        {
            if(PlayerToPlayColor == PieceColor.White)
                return new List<Ply>();
            return PossiblePlysForBlackIgnoreTurn();
        }
        public IEnumerable<Ply> PossiblePlysForWhite()
        {
            if(PlayerToPlayColor == PieceColor.Black)
                return new List<Ply>();
            return PossiblePlysForWhiteIgnoreTurn();
        }
        private IEnumerable<Ply> PossiblePlysForBlackIgnoreTurn()
        {
            List<Ply> result = new List<Ply>();
            for(int row = 0; row < RowAmount; row++)
            {
                for(int file = 0; file < FileAmount; file++)
                {
                    Square currentSquare = squaresInPosition[At(row, file)];
                    if(currentSquare.Occupier is not null && currentSquare.Occupier.Color == PieceColor.Black)
                    {
                        result.AddRange(possibleMovesGenerationFunctionsDictinary[currentSquare.Occupier.Type](row, file));
                    }
                }
            }
            return result;
        }
        private IEnumerable<Ply> PossiblePlysForWhiteIgnoreTurn()
        {
            List<Ply> result = new List<Ply>();
            for(int row = 0; row < RowAmount; row++)
            {
                for(int file = 0; file < FileAmount; file++)
                {
                    Square currentSquare = squaresInPosition[At(row, file)];
                    if(currentSquare.Occupier is not null && currentSquare.Occupier.Color == PieceColor.White)
                    {
                        result.AddRange(possibleMovesGenerationFunctionsDictinary[currentSquare.Occupier.Type](row, file));
                    }
                }
            }
            return result;
        }
        private IEnumerable<Ply> GeneratePawnPlys(int row, int file)
        {
            Ply currentPlyGenerated;
            List<Ply> possiblePlys = new List<Ply>();
            Square fromSqure = squaresInPosition[At(row, file)];
            Piece pieceMoved = fromSqure.Occupier;
            if(PlayerToPlayColor is PieceColor.White && row == 6)
            {
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 1, file)], PieceType.Bishop);
                possiblePlys.Add(currentPlyGenerated);
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 1, file)], PieceType.Rook);
                possiblePlys.Add(currentPlyGenerated);
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 1, file)], PieceType.Queen);
                possiblePlys.Add(currentPlyGenerated);
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 1, file)], PieceType.Knight);
                possiblePlys.Add(currentPlyGenerated);
            }
            else if(PlayerToPlayColor is PieceColor.Black && row == 1)
            {
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row - 1, file)], PieceType.Bishop);
                possiblePlys.Add(currentPlyGenerated);
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row - 1, file)], PieceType.Rook);
                possiblePlys.Add(currentPlyGenerated);
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row - 1, file)], PieceType.Queen);
                possiblePlys.Add(currentPlyGenerated);
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row - 1, file)], PieceType.Knight);
                possiblePlys.Add(currentPlyGenerated);
            }
            else
            {
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 1, file)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 2, file)]);
            possiblePlys.Add(currentPlyGenerated);

            currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 1, file + 1)]);
            possiblePlys.Add(currentPlyGenerated);

            currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 1, file - 1)]);
            possiblePlys.Add(currentPlyGenerated);

            return possiblePlys.Where(ply => IsLeaglePawnPlyInsquaresContext(ply));
        }
        private IEnumerable<Ply> GenerateKnightPlys(int row, int file)
        {
            Ply currentPlyGenerated;
            List<Ply> possiblePlys = new List<Ply>();
            Square fromSqure = squaresInPosition[At(row, file)];
            Piece pieceMoved = fromSqure.Occupier;

            if(InRange(row + 1, file + 2))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 1, file + 2)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 1, file + 2))
            {


                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row - 1, file + 2)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row + 1, file - 2))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 1, file - 2)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 1, file - 2))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row - 1, file - 2)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row + 2, file + 1))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 2, file + 1)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 2, file + 1))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row - 2, file + 1)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row + 2, file - 1))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 2, file - 1)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 2, file - 1))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row - 2, file - 1)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            return possiblePlys.Where(ply => IsLeagleKnightPlyInsquaresContext(ply));
        }
        private IEnumerable<Ply> GenerateBishopPlys(int row, int file)
        {
            Ply currentPlyGenerated;
            List<Ply> possiblePlys = new List<Ply>();
            Square fromSqure = squaresInPosition[At(row, file)];
            Piece pieceMoved = fromSqure.Occupier;

            int possibleMoveRow = row + 1;
            int possibleMoveFile = file + 1;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(possibleMoveRow, possibleMoveFile)]);
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveRow += 1;
                possibleMoveFile += 1;
            }
            possibleMoveRow = row + 1;
            possibleMoveFile = file - 1;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(possibleMoveRow, possibleMoveFile)]);
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveRow += 1;
                possibleMoveFile -= 1;
            }
            possibleMoveRow = row - 1;
            possibleMoveFile = file + 1;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(possibleMoveRow, possibleMoveFile)]);
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveRow -= 1;
                possibleMoveFile += 1;
            }
            possibleMoveRow = row - 1;
            possibleMoveFile = file - 1;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(possibleMoveRow, possibleMoveFile)]);
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveRow -= 1;
                possibleMoveFile -= 1;
            }
            return possiblePlys.Where(ply => IsLeagleBishopPlyInsquaresContext(ply));
        }
        private IEnumerable<Ply> GenerateRookPly(int row, int file)
        {
            Ply currentPlyGenerated;
            List<Ply> possiblePlys = new List<Ply>();
            Square fromSqure = squaresInPosition[At(row, file)];
            Piece pieceMoved = fromSqure.Occupier;

            int possibleMoveRow = row;
            int possibleMoveFile = file + 1;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(possibleMoveRow, possibleMoveFile)]);
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveFile += 1;
            }
            possibleMoveRow = row;
            possibleMoveFile = file - 1;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(possibleMoveRow, possibleMoveFile)]);
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveFile -= 1;
            }
            possibleMoveRow = row + 1;
            possibleMoveFile = file;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(possibleMoveRow, possibleMoveFile)]);
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveRow += 1;
            }
            possibleMoveRow = row - 1;
            possibleMoveFile = file;
            while(InRange(possibleMoveRow, possibleMoveFile))
            {
                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(possibleMoveRow, possibleMoveFile)]);
                possiblePlys.Add(currentPlyGenerated);
                possibleMoveRow -= 1;
            }
            return possiblePlys.Where(ply => IsLeagleRookPlyInsquaresContext(ply));

        }
        private IEnumerable<Ply> GenerateQueenPlys(int row, int file)
        {
            return GenerateRookPly(row, file).Concat(GenerateBishopPlys(row, file));
        }
        private IEnumerable<Ply> GenerateKingPlys(int row, int file)
        {
            Ply currentPlyGenerated;
            List<Ply> possiblePlys = new List<Ply>();
            Square fromSqure = squaresInPosition[At(row, file)];
            Piece pieceMoved = fromSqure.Occupier;
            if(InRange(row + 1, file + 1))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 1, file + 1)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 1, file + 1))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row - 1, file + 1)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row + 1, file - 1))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 1, file - 1)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 1, file - 1))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row - 1, file - 1)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row, file + 1))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row, file + 1)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row, file - 1))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row, file - 1)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row + 1, file))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row + 1, file)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(InRange(row - 1, file))
            {

                currentPlyGenerated = new Ply(pieceMoved, fromSqure, squaresInPosition[At(row - 1, file)]);
                possiblePlys.Add(currentPlyGenerated);
            }
            if(PlayerToPlayColor is PieceColor.White)
            {
                possiblePlys.Add(Ply.WhiteKingSideCastle);
                possiblePlys.Add(Ply.WhiteQueenSideCastle);
            }
            else
            {
                possiblePlys.Add(Ply.BlackKingSideCastle);
                possiblePlys.Add(Ply.BlackQueenSideCastle);
            }

            return possiblePlys.Where(ply => IsLeagleKingPlyInsquaresContext(ply));
        }
        private bool IsKingSafeAfterPly(Ply plyToSearch)
        {
            Position nextPosition = this.AfterPly(plyToSearch);
            if(PlayerToPlayColor == PieceColor.White)
            {
                var plys = nextPosition.PossiblePlysForBlack();
                if(plys.Any(ply => ply.Desitination.Occupier == new Piece(PieceType.King, PieceColor.White)))
                    return false;
                return true;
            }
            else
            {
                var plys = nextPosition.PossiblePlysForWhite();
                if(plys.Any(ply => ply.Desitination.Occupier == new Piece(PieceType.King, PieceColor.Black)))
                    return false;
                return true;
            }
        }
        private bool IsLeaglePawnPlyInsquaresContext(Ply ply)
        {
            if(!IsLeaglePlyInsquaresContext(ply))
                return false;
            if(ply.Source.File != ply.Desitination.File)
            {
                if(ply.Desitination.Occupier is not null)
                    return true;
                else
                {
                    if(PlayerToPlayColor is PieceColor.White)
                    {
                        if(squaresInPosition[At(ply.Desitination.Row - 1, ply.Desitination.File)] == EnPassentTakablePawnOfBlack)
                            return true;
                    }
                    else
                    {
                        if(squaresInPosition[At(ply.Desitination.Row + 1, ply.Desitination.File)] == EnPassentTakablePawnOfWhite)
                            return true;
                    }
                    return false;
                }

            }
            return true;
        }
        private bool IsLeagleKingPlyInsquaresContext(Ply ply)
        {
            if(!IsLeaglePlyInsquaresContext(ply))
                return false;
            if(PlayerToPlayColor is PieceColor.White)
            {
                if(ply == Ply.WhiteKingSideCastle && canWhiteCastleKingSide)
                    return true;
                if(ply == Ply.WhiteQueenSideCastle && canWhiteCastleQueenSide)
                    return true;
            }
            else
            {
                if(ply == Ply.BlackKingSideCastle && canBlackCastleKingSide)
                    return true;
                if(ply == Ply.BlackQueenSideCastle && canBlackCastleQueenSide)
                    return true;
            }
            return IsKingSafeAfterPly(ply);
        }
        private bool IsLeagleKnightPlyInsquaresContext(Ply ply)
        {
            return IsLeaglePlyInsquaresContext(ply);
        }
        private bool IsLeagleBishopPlyInsquaresContext(Ply ply)
        {
            if(!IsLeaglePlyInsquaresContext(ply))
                return false;
            for(int row = ply.Source.Row, file = ply.Source.File;
                row != ply.Desitination.Row && file != ply.Desitination.File;
                row += row < ply.Desitination.Row ? +1 : -1, file += file < ply.Desitination.File ? +1 : -1)
            {
                if(squaresInPosition[At(row, file)] is not null)
                    return false;
            }
            return true;
        }
        private bool IsLeagleQueenPlyInsquaresContext(Ply ply)
        {
            if(!IsLeaglePlyInsquaresContext(ply))
                return false;
            if(IsLeagleBishopPlyInsquaresContext(ply))
                return true;
            if(IsLeagleRookPlyInsquaresContext(ply))
                return true;
            return false;
        }
        private bool IsLeagleRookPlyInsquaresContext(Ply ply)
        {
            if(!IsLeaglePlyInsquaresContext(ply))
                return false;
            if(ply.Desitination.File == ply.Source.File)
            {
                for(int row = ply.Source.Row; row != ply.Desitination.Row; row += row < ply.Desitination.Row ? +1 : -1)
                {
                    if(squaresInPosition[At(row, ply.Desitination.File)] is not null)
                        return false;
                }
            }
            else
            {
                for(int file = ply.Source.File; file != ply.Desitination.File; file += file < ply.Desitination.File ? +1 : -1)
                {
                    if(squaresInPosition[At(ply.Desitination.Row, file)] is not null)
                        return false;
                }
            }
            return true;

        }
        private bool IsLeaglePlyInsquaresContext(Ply ply)
        {
            if(ply.Desitination.Occupier is not null && ply.Desitination.Occupier.Color == PlayerToPlayColor)
                return false;
            return IsKingSafeAfterPly(ply);
        }
        private bool InRange(int row, int file)
        {
            return row < RowAmount && row >= 0 && file < FileAmount && file >= 0;
        }

    }*/
}
