namespace ChessEngine.Core

[<AutoOpenAttribute>]
module ValidMoves = 
    
    // Indicates which squares are being attacked 
    let mutable BlackAttackBoard = Array.create(64) false
    let mutable WhiteAttackBoard = Array.create(64) false

    // Stores the positions of the two kings
    let mutable BlackKingPosition, WhiteKingPosition = 0,0
    
    /// Analyzes the move for the given piece
    let AnalyzeMove (board:Board) (pos:int) (piece:Piece) = 
        
        // If I am not a pawn, everywhere I move I can attack
        match piece.Color with
        | White -> WhiteAttackBoard.[pos] <- true
        | Black -> BlackAttackBoard.[pos] <- true

        // Look at what is occupying the target square
        match board.Squares.[pos].Piece.PieceType with
        | None -> // If location not occupied, just take the square
            piece.ValidMoves.Push pos
            true
        | _ ->
            let attackedPiece = board.Squares.[pos].Piece
            
            match attackedPiece.Color with
            | x when x <> piece.Color -> 
                // If king, then set it in check
                match piece.PieceType with
                | King when piece.Color = Black -> board.BlackCheck <- true
                | King when piece.Color = White -> board.WhiteCheck <- true
                | _ -> piece.ValidMoves.Push pos

                attackedPiece.AttackedValue <- attackedPiece.AttackedValue + piece.ActionValue
                false
            | _ -> 
                attackedPiece.DefendedValue <- attackedPiece.DefendedValue + piece.ActionValue
                false

    /// Because pawns are a special case, this analyzes the moves for pawns only
    let AnalyzeMovePawn (board:Board) (target:int) (piece:Piece) = 
        if (board.EnPassantPosition > 0) && (piece.Color <> board.EnPassantColor) 
            && (board.EnPassantPosition = target) then
            
            piece.ValidMoves.Push target
            match piece.Color with
            | White -> WhiteAttackBoard.[target] <- true
            | Black -> BlackAttackBoard.[target] <- true

        let attackedPiece = board.Squares.[target].Piece

        if attackedPiece.PieceType <> None then
            match attackedPiece.Color with
            | White -> 
                WhiteAttackBoard.[target] <- true  
                match attackedPiece.Color = piece.Color with
                | true -> 
                    attackedPiece.DefendedValue <- attackedPiece.DefendedValue + piece.ActionValue
                | false -> 
                    attackedPiece.AttackedValue <- attackedPiece.AttackedValue + piece.ActionValue
                    match attackedPiece.PieceType with
                    | King -> board.BlackCheck <- true
                    | _ -> piece.ValidMoves.Push target
            | Black ->
                BlackAttackBoard.[target] <- true  
                match attackedPiece.Color = piece.Color with
                | true -> 
                    attackedPiece.DefendedValue <- attackedPiece.DefendedValue + piece.ActionValue
                | false -> 
                    attackedPiece.AttackedValue <- attackedPiece.AttackedValue + piece.ActionValue
                    match attackedPiece.PieceType with
                    | King -> board.WhiteCheck <- true
                    | _ -> piece.ValidMoves.Push target

    /// Checks the valid moves for the given pawn
    let CheckValidMovesPawn (moves:System.Collections.Generic.List<int>) (piece:Piece) (pos:int) (board:Board) (count:int) =   
        for i = 0 to count do
            let target = moves.[i]
            
            match target with
            | x when x % 8 <> pos % 8->
                AnalyzeMovePawn board target piece // If piece exists at target, it might be killable
                match piece.Color with 
                | White -> WhiteAttackBoard.[target] <- true
                | Black -> BlackAttackBoard.[target] <- true
            | _ when board.Squares.[target].Piece.PieceType <> None -> () // Can't move if another piece blocks path
            | _ -> piece.ValidMoves.Push target

    let GenerateValidKingMoves (piece:Piece) (board:Board) (source:int) = 
        match piece.PieceType with
        | None -> ()
        | _ -> 
            for i = 0 to MoveArrays.KingTotal.[source] do
                let target = MoveArrays.King.[source].Moves.[i]

                match piece.Color with
                | White -> if BlackAttackBoard.[target] then WhiteAttackBoard.[target] <- true
                | Black -> if WhiteAttackBoard.[target] then BlackAttackBoard.[target] <- true

                AnalyzeMove board target (piece) |> ignore
    
    let GenerateValidMovesKingCastle (board:Board) (king:Piece) = 
        match king.Color with
        | _ when king.Moved -> ()
        | White when board.WhiteCastled || board.WhiteCheck -> ()
        | Black when board.BlackCastled || board.BlackCheck -> ()

        | White ->  // Castle scenario 1
                    if  board.Squares.[63].Piece.PieceType = PieceType.Rook
                        && board.Squares.[63].Piece.Color = king.Color
                        && board.Squares.[62].Piece.PieceType = None
                        && board.Squares.[61].Piece.PieceType = None
                        && BlackAttackBoard.[61] = false
                        && BlackAttackBoard.[62] = false then
                            king.ValidMoves.Push 62 // Ok move is valid, let's add it
                            WhiteAttackBoard.[62] <- true

                    // Castle scenario 2
                    if  board.Squares.[56].Piece.PieceType = PieceType.Rook
                        && board.Squares.[56].Piece.Color = king.Color
                        && board.Squares.[57].Piece.PieceType = None
                        && board.Squares.[58].Piece.PieceType = None
                        && BlackAttackBoard.[58] = false
                        && BlackAttackBoard.[59] = false then
                            king.ValidMoves.Push 58 // Ok move is valid, let's add it
                            WhiteAttackBoard.[58] <- true
        
        | Black ->  // Castle scenario 1
                    if  board.Squares.[7].Piece.PieceType = PieceType.Rook
                        && board.Squares.[7].Piece.Moved = false
                        && board.Squares.[7].Piece.Color = king.Color
                        && board.Squares.[6].Piece.PieceType = None
                        && board.Squares.[5].Piece.PieceType = None
                        && WhiteAttackBoard.[6] = false
                        && WhiteAttackBoard.[5] = false then
                            king.ValidMoves.Push 6 // Ok move is valid, let's add it
                            BlackAttackBoard.[6] <- true

                    // Castle scenario 2
                    if  board.Squares.[0].Piece.PieceType = PieceType.Rook
                        && board.Squares.[0].Piece.Moved = false
                        && board.Squares.[0].Piece.Color = king.Color
                        && board.Squares.[1].Piece.PieceType = None
                        && board.Squares.[2].Piece.PieceType = None
                        && board.Squares.[3].Piece.PieceType = None
                        && WhiteAttackBoard.[2] = false
                        && WhiteAttackBoard.[3] = false then
                            king.ValidMoves.Push 2 // Ok move is valid, let's add it
                            BlackAttackBoard.[2] <- true

    let GenerateValidMoves (board:Board) = 
        // Reset board
        board.BlackCheck <- false
        board.WhiteCheck <- false
        WhiteAttackBoard <- Array.create 64 false
        BlackAttackBoard <- Array.create 54 false

        for i = 0 to 63 do
            let square = board.Squares.[i]

            match square.Piece.PieceType, square.Piece.Color with
            | Pawn, White -> 
                CheckValidMovesPawn(MoveArrays.WhitePawn.[i].Moves) square.Piece i board MoveArrays.WhitePawnTotal.[i]
            | Pawn, Black -> 
                CheckValidMovesPawn(MoveArrays.BlackPawn.[i].Moves) square.Piece i board MoveArrays.BlackPawnTotal.[i]
            | Knight, _ -> 
                for x = 0 to MoveArrays.KnightTotal.[i]-1 do
                    AnalyzeMove board MoveArrays.Knight.[i].Moves.[x] square.Piece |> ignore
            | Bishop, _ ->
                // Bishop 1
                let rec b1 x = 
                    match AnalyzeMove board MoveArrays.Bishop1.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Bishop1Total.[i] -> b1 (x+1)
                    | _ -> ()
                b1 0

                // Bishop 2
                let rec b2 x = 
                    match AnalyzeMove board MoveArrays.Bishop2.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Bishop2Total.[i] -> b2 (x+1)
                    | _ -> ()
                b2 0
                    
                // Bishop 3
                let rec b3 x = 
                    match AnalyzeMove board MoveArrays.Bishop3.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Bishop3Total.[i] -> b3 (x+1)
                    | _ -> ()
                b3 0                    
                    
                // Bishop 4
                let rec b4 x = 
                    match AnalyzeMove board MoveArrays.Bishop4.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Bishop4Total.[i] -> b4 (x+1)
                    | _ -> ()
                b4 0
            | Rook, _ ->
                // Rook 1
                let rec r1 x = 
                    match AnalyzeMove board MoveArrays.Rook1.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Rook1Total.[i] -> r1 (x+1)
                    | _ -> ()
                r1 0

                // Rook 2
                let rec r2 x = 
                    match AnalyzeMove board MoveArrays.Rook2.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Rook2Total.[i] -> r2 (x+1)
                    | _ -> ()
                r2 0

                // Rook 3
                let rec r3 x = 
                    match AnalyzeMove board MoveArrays.Rook3.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Rook3Total.[i] -> r3 (x+1)
                    | _ -> ()
                r3 0

                // Rook 4
                let rec r4 x = 
                    match AnalyzeMove board MoveArrays.Rook4.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Rook4Total.[i] -> r4 (x+1)
                    | _ -> ()
                r4 0
            | Queen, _ ->
                // Queen 1
                let rec q1 x = 
                    match AnalyzeMove board MoveArrays.Queen1.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Queen1Total.[i] -> q1 (x+1)
                    | _ -> ()
                q1 0

                // Queen 2
                let rec q2 x = 
                    match AnalyzeMove board MoveArrays.Queen2.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Queen2Total.[i] -> q2 (x+1)
                    | _ -> ()
                q2 0

                // Queen 3
                let rec q3 x = 
                    match AnalyzeMove board MoveArrays.Queen3.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Queen3Total.[i] -> q3 (x+1)
                    | _ -> ()
                q3 0

                // Queen 4
                let rec q4 x = 
                    match AnalyzeMove board MoveArrays.Queen4.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Queen4Total.[i] -> q4 (x+1)
                    | _ -> ()
                q4 0

                // Queen 5
                let rec q5 x = 
                    match AnalyzeMove board MoveArrays.Queen5.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Queen5Total.[i] -> q5 (x+1)
                    | _ -> ()
                q5 0

                // Queen 6
                let rec q6 x = 
                    match AnalyzeMove board MoveArrays.Queen6.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Queen6Total.[i] -> q6 (x+1)
                    | _ -> ()
                q6 0
                    
                // Queen 7
                let rec q7 x = 
                    match AnalyzeMove board MoveArrays.Queen7.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Queen7Total.[i] -> q7 (x+1)
                    | _ -> ()
                q7 0

                // Queen 8
                let rec q8 x = 
                    match AnalyzeMove board MoveArrays.Queen8.[i].Moves.[x] square.Piece with
                    | true when x+1 < MoveArrays.Queen8Total.[i] -> q8 (x+1)
                    | _ -> ()
                q8 0
            | King, White -> WhiteKingPosition <- i
            | King, Black -> BlackKingPosition <- i
            | None, _ -> ()
        
        match board.WhoseMove with
        | White -> 
            GenerateValidKingMoves board.Squares.[BlackKingPosition].Piece board BlackKingPosition
            GenerateValidKingMoves board.Squares.[WhiteKingPosition].Piece board WhiteKingPosition
        | Black ->
            GenerateValidKingMoves board.Squares.[WhiteKingPosition].Piece board WhiteKingPosition
            GenerateValidKingMoves board.Squares.[BlackKingPosition].Piece board BlackKingPosition

        // After all moves examined, now we check to see if king is in check
        GenerateValidMovesKingCastle board board.Squares.[WhiteKingPosition].Piece
        GenerateValidMovesKingCastle board board.Squares.[BlackKingPosition].Piece