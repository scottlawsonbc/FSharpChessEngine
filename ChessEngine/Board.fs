namespace ChessEngine.Core

[<AutoOpenAttribute>]
module ChessBoard = 
    
    open ChessPiece    
    open System

    type System.Int32 with
        /// Returns the corresponding row on the board
        member inline this.ToRow() = this / 8
        /// Returns the corresponding column on the board
        member inline this.ToCol() = this % 8

    /// Gets the index representation of the row and column coordinates
    let ToIndex (row, col) = 8 * row + col

    /// A square that contains a chess piece
    type Square (squarePiece) = 
        let mutable piece:Piece = squarePiece
        member x.Piece
            with get() = piece
            and set value = piece <- value

    [<Sealed>] 
    /// A chessboard containing squares
    type Board(fen:string) = 
        
        // Possible board states
        let mutable blackCheck = false
        let mutable blackMate = false
        let mutable blackCastled = false
        let mutable whiteCheck = false
        let mutable whiteMate = false
        let mutable whiteCastled = false
        let mutable repeatedMove = 0

        let mutable staleMate = false
        let mutable endGamePhase = false
        let mutable fiftyMove = 0
        let mutable whoseMove = White

        let mutable enPassantColor = White
        let mutable enPassantPosition = 0
        let mutable moveCount = 0
        let mutable score = 0

        let mutable lastMove = new MoveContent()

        /// The array that contains the chess pieces
        let squares = Array.create (64) (new Square(new Piece(PieceColor.White, PieceType.None)))

        /// Converts a (row, col) tuple into the corresponding 1D index
        let CoordToIndex (row, col) = 8 * row + col
        
        /// Converts a 1D index into the corresponding (row, col) tuple
        let IndexToCoord (index) = (index / 8, index % 8)

        do match fen with
            | "" -> ()
            | _ -> 
                let mutable index = 0
                let mutable spc = 0
                let mutable spacers = 0

                whiteCastled <- true
                blackCastled <- true
                whoseMove <- White

                match fen with
                // Check x3s
                | x when x.Contains "a3" ->
                    enPassantColor <- White
                    enPassantPosition <- 40
                | x when x.Contains "b3" ->
                    enPassantColor <- White
                    enPassantPosition <- 41
                | x when x.Contains "c3" ->
                    enPassantColor <- White
                    enPassantPosition <- 42
                | x when x.Contains "d3" ->
                    enPassantColor <- White
                    enPassantPosition <- 43
                | x when x.Contains "e3" ->
                    enPassantColor <- White
                    enPassantPosition <- 44
                | x when x.Contains "f3" ->
                    enPassantColor <- White
                    enPassantPosition <- 45
                | x when x.Contains "g3" ->
                    enPassantColor <- White
                    enPassantPosition <- 46
                | x when x.Contains "h3" ->
                    enPassantColor <- White
                    enPassantPosition <- 47
                // Check x6s
                | x when x.Contains "a6" ->
                    enPassantColor <- White
                    enPassantPosition <- 16
                | x when x.Contains "b6" ->
                    enPassantColor <- White
                    enPassantPosition <- 17
                | x when x.Contains "c6" ->
                    enPassantColor <- White
                    enPassantPosition <- 18
                | x when x.Contains "d6" ->
                    enPassantColor <- White
                    enPassantPosition <- 19
                | x when x.Contains "e6" ->
                    enPassantColor <- White
                    enPassantPosition <- 20
                | x when x.Contains "f6" ->
                    enPassantColor <- White
                    enPassantPosition <- 21
                | x when x.Contains "g6" ->
                    enPassantColor <- White
                    enPassantPosition <- 22
                | x when x.Contains "h6" ->
                    enPassantColor <- White
                    enPassantPosition <- 23
                | _ -> ()

                for c in fen do
                    if (index < 64) && (spc = 0) then
                        match c, index with
                        | '1', x when x < 63 -> index <- index + 1
                        | '2', x when x < 62 -> index <- index + 2
                        | '3', x when x < 61 -> index <- index + 3
                        | '4', x when x < 60 -> index <- index + 4
                        | '5', x when x < 59 -> index <- index + 5
                        | '6', x when x < 58 -> index <- index + 6
                        | '7', x when x < 57 -> index <- index + 7
                        | '8', x when x < 56 -> index <- index + 8
                        // White
                        | 'P', _ -> // Pawn
                            squares.[index].Piece <- new Piece(White, Pawn)
                            squares.[index].Piece.Moved <- true
                        | 'N', _ -> // Knight
                            squares.[index].Piece <- new Piece(White, Pawn)
                            squares.[index].Piece.Moved <- true
                        | 'B', _ -> // Bishop
                            squares.[index].Piece <- new Piece(White, Pawn)
                            squares.[index].Piece.Moved <- true
                        | 'R', _ -> // Rook
                            squares.[index].Piece <- new Piece(White, Pawn)
                            squares.[index].Piece.Moved <- true
                        | 'Q', _ -> // Queen
                            squares.[index].Piece <- new Piece(White, Pawn)
                            squares.[index].Piece.Moved <- true
                        | 'K', _ -> 
                            squares.[index].Piece <- new Piece(White, Pawn)
                            squares.[index].Piece.Moved <- true
                        // Black
                        | 'p', _ -> // Pawn
                            squares.[index].Piece <- new Piece(White, Pawn)
                            squares.[index].Piece.Moved <- true
                        | 'n', _ -> // Knight
                            squares.[index].Piece <- new Piece(White, Pawn)
                            squares.[index].Piece.Moved <- true
                        | 'b', _ -> // Bishop
                            squares.[index].Piece <- new Piece(White, Pawn)
                            squares.[index].Piece.Moved <- true
                        | 'r', _ -> // Rook
                            squares.[index].Piece <- new Piece(White, Pawn)
                            squares.[index].Piece.Moved <- true
                        | 'q', _ -> // Queen
                            squares.[index].Piece <- new Piece(White, Pawn)
                            squares.[index].Piece.Moved <- true
                        | 'k', _ -> 
                            squares.[index].Piece <- new Piece(White, Pawn)
                            squares.[index].Piece.Moved <- true
                        | ' ', _ -> spc <- spc + 1
                        | _, _ -> ()
                    else
                        match c with
                        | 'w' -> whoseMove <- White
                        | 'b' -> whoseMove <- Black
                        | 'K' -> // White king
                            if squares.[60].Piece.PieceType = King then
                                squares.[60].Piece.Moved <- false
                            if squares.[63].Piece.PieceType = Rook then
                                squares.[63].Piece.Moved <- false
                            whiteCastled <- false
                        | 'Q' -> // White queen
                            if squares.[60].Piece.PieceType = King then
                                squares.[60].Piece.Moved <- false
                            if squares.[56].Piece.PieceType = Rook then
                                squares.[56].Piece.Moved <- false
                            whiteCastled <- false
                        | 'k' -> // Black king
                            if squares.[4].Piece.PieceType = King then
                                squares.[4].Piece.Moved <- false
                            if squares.[7].Piece.PieceType = Rook then
                                squares.[7].Piece.Moved <- false 
                            blackCastled <- false
                        | 'q' -> // Black queen
                            if squares.[4].Piece.PieceType = King then
                                squares.[4].Piece.Moved <- false
                            if squares.[0].Piece.PieceType = Rook then
                                squares.[0].Piece.Moved <- false
                            blackCastled <- false
                        | ' ' -> spacers <- spacers + 1
                        | _ ->
                            match c, spacers with
                            // Fifty move
                            | '1', 4 -> fiftyMove <- fiftyMove * 10 + Int32.Parse(c.ToString())
                            | '2', 4 -> fiftyMove <- fiftyMove * 10 + Int32.Parse(c.ToString())
                            | '3', 4 -> fiftyMove <- fiftyMove * 10 + Int32.Parse(c.ToString())
                            | '4', 4 -> fiftyMove <- fiftyMove * 10 + Int32.Parse(c.ToString())
                            | '5', 4 -> fiftyMove <- fiftyMove * 10 + Int32.Parse(c.ToString())
                            | '6', 4 -> fiftyMove <- fiftyMove * 10 + Int32.Parse(c.ToString())
                            | '7', 4 -> fiftyMove <- fiftyMove * 10 + Int32.Parse(c.ToString())
                            | '8', 4 -> fiftyMove <- fiftyMove * 10 + Int32.Parse(c.ToString())
                            | '9', 4 -> fiftyMove <- fiftyMove * 10 + Int32.Parse(c.ToString())
                            | '0', 4 -> fiftyMove <- fiftyMove * 10 + Int32.Parse(c.ToString())

                            // Move count
                            | '1', 5 -> moveCount <- moveCount * 10 + Int32.Parse(c.ToString())
                            | '2', 5 -> moveCount <- moveCount * 10 + Int32.Parse(c.ToString())
                            | '3', 5 -> moveCount <- moveCount * 10 + Int32.Parse(c.ToString())
                            | '4', 5 -> moveCount <- moveCount * 10 + Int32.Parse(c.ToString())
                            | '5', 5 -> moveCount <- moveCount * 10 + Int32.Parse(c.ToString())
                            | '6', 5 -> moveCount <- moveCount * 10 + Int32.Parse(c.ToString())
                            | '7', 5 -> moveCount <- moveCount * 10 + Int32.Parse(c.ToString())
                            | '8', 5 -> moveCount <- moveCount * 10 + Int32.Parse(c.ToString())
                            | '9', 5 -> moveCount <- moveCount * 10 + Int32.Parse(c.ToString())
                            | '0', 5 -> moveCount <- moveCount * 10 + Int32.Parse(c.ToString())
                            | _, _ -> ()

        /// The raw squares of the chess board
        member x.Squares = squares

        /// Get or set a particular square given the zero-based coordinates
        member x.Square
            with get (row, col) = squares.[CoordToIndex (row,col)]
            and set (row, col) (value) = squares.[CoordToIndex(row,col)] <- value

        // Getters and setter for the mutable properties
        member x.BlackCheck
            with get() = blackCheck
            and set value = blackCheck <- value

        member x.BlackMate
            with get() = blackMate
            and set value = blackMate <- value
        
        member x.BlackCastled
            with get() = blackCastled
            and set value = blackCastled <- value

        member x.WhiteCheck
            with get() = whiteCheck
            and set value = whiteCheck <- value

        member x.WhiteMate
            with get() = whiteMate
            and set value = whiteMate <- value

        member x.WhiteCastled
            with get() = whiteCastled
            and set value = whiteCastled <- value
           
        member x.RepeatedMove 
            with get() = repeatedMove
            and set value = repeatedMove <- value

        member x.StaleMate
            with get() = staleMate
            and set value = staleMate <- value

        member x.EndGamePhase
            with get() = endGamePhase
            and set value = endGamePhase <- value

        member x.FiftyMove
            with get() = fiftyMove
            and set value = fiftyMove <- value
        
        member x.WhoseMove
            with get() = whoseMove
            and set value = whoseMove <- value

        member x.EnPassantColor
            with get() = enPassantColor
            and set value = enPassantColor <- value
        
        member x.EnPassantPosition
            with get() = enPassantPosition
            and set value = enPassantPosition <- value
            
        member x.MoveCount
            with get() = moveCount
            and set value = moveCount <- value

        member x.Score 
            with get() = score
            and set value = score <- value

        member x.LastMove
            with get() = lastMove
            and set value = lastMove <- value

        member x.PromotePawns (piece:Piece) (target:int) (promoteToPiece:PieceType) = 
            match piece.PieceType with
            | Pawn when (target < 8) || (target > 55) -> 
                squares.[target].Piece.PieceType <- promoteToPiece
                true
            | _ -> false

        member x.RecordEnPassant color pieceType source target =
            // Record En Passant if pawn is moving
            match pieceType with
            | Pawn ->
                // FiftyMove is reset if pawn is moved
                fiftyMove <- 0
                let difference = source - target
                match difference with
                | 16 | -16 -> 
                    enPassantPosition <- target + (difference / 2)
                    enPassantColor <- color
                | _ -> ()
            | _ -> ()
                
            

        member x.MovePiece (source, target) (promoteToPiece:PieceType) = 
            let piece = squares.[source].Piece
            let move = new MoveContent()
            fiftyMove <- fiftyMove + 1
            
            if piece.Color = Black then 
                moveCount <- moveCount + 1

            (*
            //En Passant
            if (board.EnPassantPosition > 0)
            {
                board.LastMove.EnPassantOccured = SetEnpassantMove(board, dstPosition, piece.PieceColor);
            }
            *)

        new() = new Board("")

    let BoardCopy (y:Board) = 
        let x = new Board()
        for i = 0 to 63 do x.Squares.[i] <- y.Squares.[i]
        x.EndGamePhase <- y.EndGamePhase
        x.FiftyMove <- y.FiftyMove
        x.RepeatedMove <- y.RepeatedMove
        x.WhiteCastled <- y.WhiteCastled
        x.BlackCastled <- y.BlackCastled
        x.BlackCheck <- y.BlackCheck
        x.WhiteCheck <- y.WhiteCheck
        x.StaleMate <- y.StaleMate
        x.WhiteMate <- y.WhiteMate
        x.BlackMate <- y.BlackMate
        x.WhoseMove <- y.WhoseMove
        x.EnPassantPosition <- y.EnPassantPosition
        x.EnPassantColor <- y.EnPassantColor
        // zobrist hash?
        x.LastMove <- y.LastMove
        x.Score <- y.Score
        x.MoveCount <- y.MoveCount
        x