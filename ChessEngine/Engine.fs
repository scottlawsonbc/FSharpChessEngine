namespace ChessEngine.Core

module Engine = 

    open System
    open System.Collections.Generic

    type Engine(fen) = 
        
        let mutable board = new Board()
        let mutable previousBoard = new Board()
        let mutable HumanPlayer = White

        /// Gets or sets the color with the current turn
        member x.WhoseMove
            with get() = board.WhoseMove
            and set value = board.WhoseMove <- value

        /// Initiate board using Forsyth-Edwards notation
        member x.IniateBoard fen B= 
            HumanPlayer <- White
            board <- new Board(fen)
            board.WhoseMove <- White
            Moves.MoveArrays.InitiatePieceMotion()
            GenerateValidMoves board

        /// Returns the row and column of the enPassant position
        member x.GetEnPassantMoves() = 
            let returnArray = Array.create 2 0
            returnArray.[0] <- board.EnPassantPosition.ToCol()
            returnArray.[1] <- board.EnPassantPosition.ToRow()
            returnArray

        /// Determines if the given move can be made
        member x.IsValidMove (sourceColumn, sourceRow) (targetColumn, targetRow) = 
            let index = MoveContent.GetBoardIndex (sourceColumn, sourceRow)
            match board.Squares.[index].Piece.PieceType <> None with
            | true ->
                let pieceExists =
                    board.Squares.[index].Piece.ValidMoves.ToArray()
                    |> Array.exists (fun i -> i.ToRow() = targetRow && i.ToCol() = targetColumn)
                let enPassantValid = index = board.EnPassantPosition
                pieceExists || enPassantValid
            | false -> false 
       
        member x.GetPieceTypeAt index = board.Squares.[index].Piece.PieceType
        member x.GetPieceColorAt index = board.Squares.[index].Piece.Color
        member x.GetChessPieceSelected (row, col) = board.Squares.[(row, col) |> ToIndex].Piece.Selected
        
        /// Creates an array of (row, col) tuples representing the valid moves
        member x.GetValidMoves (row, col) = 
            let moves = board.Squares.[(row, col) |> ToIndex].Piece.ValidMoves
            [| for move in moves -> (move.ToRow(), move.ToCol)|]

        member x.SetPieceSelection (row, col, selection) = 
            let index = (row, col) |> ToIndex
            // Only allow selection of non-empty squares            
            if board.Squares.[index].Piece.PieceType <> None then
                board.Squares.[index].Piece.Selected <- selection

        /// Attempt to move the piece at the source coordinates to the target coordinates
        member x.MovePiece(sourceRow, sourceColumn) (targetRow, targetColumn) = 
            let sourceIndex = (sourceRow, sourceColumn) |> ToIndex
            let targetIndex = (targetRow, targetColumn) |> ToIndex
            
                
            let piece = board.Squares.[sourceIndex].Piece
            previousBoard <- BoardCopy board // Previous board is a copy of the current board
            
            board.MovePiece (sourceIndex, targetIndex) Queen |> ignore // Promote to queen by default
            ValidMoves.GenerateValidMoves board // generate the new valid moves

            // Look to see if there is still a check in place. If so, reject the move
            if (piece.Color = White && board.WhiteCheck) || (piece.Color = Black && board.BlackCheck) then 
                board <- BoardCopy board
                ValidMoves.GenerateValidMoves(board)
                false
            else 
                true

        /// Generates the valid moves for the chess board
        member x.GenerateValidMoves() = ValidMoves.GenerateValidMoves board

        /// Default constructor. Makes new board using default Forsyth-Edward notation
        new () = new Engine("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")

        

          