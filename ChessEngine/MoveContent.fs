namespace ChessEngine.Core

[<AutoOpenAttribute>]
module MoveContent = 
    type PieceMoving = 
        struct
            val Source : int
            val Target : int
            val Moved : bool
            val Color : PieceColor
            val Type : PieceType

            /// Creates a PieceMoving object using the specified values
            new (color, pieceType, moved, source, target) = 
                {
                Color = color
                Type = pieceType
                Moved = moved
                Source = source
                Target = target
                }

            /// Creates a copy of the specified PieceMoving object
            new (pieceMoving:PieceMoving) = 
                {
                Color = pieceMoving.Color
                Type = pieceMoving.Type
                Moved = pieceMoving.Moved
                Source = pieceMoving.Source
                Target = pieceMoving.Target
                }

            /// Creates a default PieceMoving object for the specified type
            new (pieceType: PieceType) = 
                {
                Color = White
                Type = pieceType
                Moved = false
                Source = 0
                Target = 0
                }
        end

    type PieceTaken =
        struct 
            val Moved : bool
            val Color : PieceColor
            val Type : PieceType
            val Position : int

            new (moved, color, pieceType, position) = 
                {
                Moved = moved
                Color = color
                Type = pieceType
                Position = position
                }

            new (pieceType) = 
                {
                Moved = false
                Color = White
                Type = pieceType
                Position = 0
                }
        end

    type MoveContent(primary:PieceMoving, secondary:PieceMoving, taken:PieceTaken) = 
        
        let mutable pawnPromoted = false
        let mutable enPassantOccured = false
        let mutable movingPiecePrimary = new PieceMoving(None)
        let mutable movingPieceSecondary = new PieceMoving(None)
        let mutable takenPiece = new PieceTaken(None)

        member x.PawnPromoted
            with get() = pawnPromoted
            and set value = pawnPromoted <- value

        member x.EnPassantOccured
            with get() = enPassantOccured 
            and set value = enPassantOccured <- value

        member x.MovingPiecePrimary 
            with get() = movingPiecePrimary
            and set value = movingPiecePrimary <- value

        member x.MovingPieceSecondary   
            with get() = movingPieceSecondary
            and set value = movingPieceSecondary <- value

        member x.TakenPiece
            with get() = takenPiece
            and set value = takenPiece <- value

        static member GetBoardIndex (col, row) = 
            col + (row * 8)

        static member GetColumnFromInt col =
            match col with
            | 0 -> "a"
            | 1 -> "b"
            | 2 -> "c"
            | 3 -> "d"
            | 4 -> "e"
            | 5 -> "f"
            | 6 -> "g"
            | 7 -> "h"
            | _ -> "Unknown"

        static member GetIntFromColumn col = 
            match col with
            | 'a' -> 0
            | 'b' -> 1
            | 'c' -> 2
            | 'd' -> 3
            | 'e' -> 4
            | 'f' -> 5
            | 'g' -> 6
            | 'h' -> 7
            | _ -> -1

        static member GetPieceType c = 
            match c with
            | 'B' -> Bishop
            | 'K' -> King
            | 'N' -> Knight
            | 'Q' -> Queen
            | 'R' -> Rook
            | _ -> None

        static member GetPgnMove pieceType = 
            match pieceType with
            | Bishop -> "B"
            | King -> "K"
            | Knight -> "N"
            | Queen -> "Q"
            | Rook -> "R"
            | _ -> ""

        // Overrides the ToString() method to provide proper notation
        override x.ToString() = 
            let mutable value = ""

            let sourceColumn = movingPiecePrimary.Source % 8
            let sourceRow = 8 - (movingPiecePrimary.Source / 8)
            let targetColumn = movingPiecePrimary.Target % 8 
            let targetRow = 8 - (movingPiecePrimary.Target / 8)

            match movingPieceSecondary.Type, movingPieceSecondary.Color with
            | Rook, Black ->
                match movingPieceSecondary.Source with
                | 0 -> value <- value + "O-O"
                | 7 -> value <- value + "O-O-O"
                | _ -> ()
            | Rook, White -> 
                match movingPieceSecondary.Source with
                | 63 -> value <- value + "O-O"
                | 56 -> value <- value + "O-O-O"
                | _ -> ()
            | _ ->  
                value <- value + MoveContent.GetPgnMove movingPiecePrimary.Type
                match movingPiecePrimary.Type with
                | Knight | Rook -> value <- value + MoveContent.GetColumnFromInt sourceColumn + sourceRow.ToString()
                | Pawn when sourceColumn <> targetColumn -> value <- value + MoveContent.GetColumnFromInt sourceColumn
                | _ -> ()

            if takenPiece.Type <> None then value <- value + "x"
            value <- value + MoveContent.GetColumnFromInt targetColumn
            value <- value + targetRow.ToString()
            if x.PawnPromoted then value <- value + "=Q"
            value
            
        new() = 
            let primary = new PieceMoving(Pawn)
            let secondary = new PieceMoving(Pawn)
            let taken = new PieceTaken(Pawn)
            MoveContent (primary, secondary, taken)

        new(moveContent:MoveContent) = 
            let primary = new PieceMoving(moveContent.MovingPiecePrimary)
            let secondary = new PieceMoving(moveContent.MovingPieceSecondary)
            let taken = new PieceTaken( moveContent.TakenPiece.Moved,
                                        moveContent.TakenPiece.Color,
                                        moveContent.TakenPiece.Type,
                                        moveContent.TakenPiece.Position)
            MoveContent (primary, secondary, taken)

    let GetMoveContentFromString (move:string) = 
        let mutable moveContent = new MoveContent()
        let mutable sourceColumn = -1
        let mutable comment = false
        let mutable sourceFound = false

        match move.Contains "=Q" with
        | true -> moveContent.PawnPromoted <- true
        | false ->
            for c in move do
                match c with
                | '{' -> comment <- true
                | '}' -> comment <- false
                | _ when not comment -> 
                    match moveContent.MovingPiecePrimary.Type with
                    | None ->
                        let x = moveContent.MovingPiecePrimary
                        let pieceType = match MoveContent.GetPieceType(c) with
                                        | None -> Pawn
                                        | x -> x
                        let primary = new PieceMoving(x.Color, pieceType, x.Moved, x.Source, x.Target)
                        moveContent <- new MoveContent(primary, moveContent.MovingPieceSecondary, moveContent.TakenPiece)
                        sourceColumn <- MoveContent.GetIntFromColumn c
                    | _ -> ()
                    match sourceColumn < 0 with
                    | true -> 
                        sourceColumn <- MoveContent.GetIntFromColumn c
                    | false -> 
                        let x = moveContent.MovingPiecePrimary
                        let sourceRow = int(System.Int64.Parse(c.ToString()))
                        let index = MoveContent.GetBoardIndex(sourceColumn, 8 - sourceRow)
                        let primary = match sourceFound with
                                      | true -> new PieceMoving(x.Color, x.Type, x.Moved, x.Source, index)
                                      | false ->  sourceFound <- true; new PieceMoving(x.Color, x.Type, x.Moved, index, x.Target)
                        moveContent <- new MoveContent(primary, moveContent.MovingPieceSecondary, moveContent.TakenPiece)
                    sourceColumn <- -1
                | _ -> ()

//        new (move:string) =
//            let mutable sourceColumn = -1
//            let mutable comment = false
//            let mutable sourceFound = false
//
//            match move.Contains "=Q" with
//            | true ->  
//                PawnPromoted <- 
//            | false -> 
//                
            


