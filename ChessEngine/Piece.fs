namespace ChessEngine.Core

[<AutoOpenAttribute>]
module ChessPiece = 

    /// Denotes the type of chess piece
    type PieceType = 
        | None
        | Pawn
        | Knight
        | Bishop
        | Rook
        | Queen
        | King
    
    /// Denotes the color of a chess piece
    type PieceColor = 
        | White
        | Black
    
    /// Returns the relative piece value of the given piece
    /// Specific values can be turned
    let ComputePieceValue (piece:PieceType) = 
        match piece with
        | None -> 0
        | Pawn -> 100
        | Knight -> 320
        | Bishop -> 325
        | Rook -> 500
        | Queen -> 975
        | King -> 32767

    /// Returns the action value of the given piece
    /// Used by computer to determine which piece to attack with
    let ComputePieceActionValue (piece:PieceType) = 
        match piece with
        | None -> 0
        | Pawn -> 6
        | Knight -> 5
        | Bishop -> 4
        | Rook -> 3
        | Queen -> 2
        | King -> 1

    /// A single chess piece
    type Piece (color, chessPieceType) = 
        let mutable attackedValue = 0
        let mutable defendedValue = 0
        let mutable actionValue = 0
        let mutable selected = false
        let mutable lastValidMoveCount = 0
        let mutable moved = false
        let mutable validMoves = new System.Collections.Generic.Stack<int>()
        let mutable pieceType = chessPieceType

        /// Gets or sets the attacked value of the piece
        member x.AttackedValue
            with get() = attackedValue
            and set value = attackedValue <- value

        /// Gets or sets the defended alue of the piece
        member x.DefendedValue
           with get() = defendedValue
           and set value = defendedValue <- value

        /// Gets or sets the action value of the piece
        member x.ActionValue
            with get() = actionValue
            and set value = actionValue <- value
        
        /// Gets or sets whether the given piece is selected
        member x.Selected
            with get() = selected
            and set value = selected <- value

        /// Gets or sets the last valid move count of the piece
        member x.LastValidMoveCount
            with get() = lastValidMoveCount
            and set value = lastValidMoveCount <- value

        /// Gets or sets whether the piece has been moved this turn
        member x.Moved
            with get() = moved
            and set value = moved <- value

        /// The color of the chess piece
        member x.Color : PieceColor = color

        /// Gets or sets the type of chess piece
        member x.PieceType
            with get() = pieceType
            and set value = pieceType <- value
        
        /// The ranked value of the chess piece
        member x.Value = ComputePieceValue pieceType
        
        /// Gets or sets the posssible valid moves for the chess piece
        member x.ValidMoves
            with get() = validMoves
            and set value = validMoves <- value
        