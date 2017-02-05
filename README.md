# FSharpChessEngine
An F# chess engine inspired by the ChessBin chess engine

# Why?
I started developing this chess engine in 2013 because I wanted to explore and learn more about F\#, an interesting function-first programming language. F# is a strongly typed, funtional-first programming language that encompasses functional, imperative, and object-oriented paradigms.

Certain aspects of the chess engine can be implemented elegantly using F#. For example, F#'s typing system and minimal syntax are well suited for representing chess pieces

'''fsharp
    type PieceType = 
        | None
        | Pawn
        | Knight
        | Bishop
        | Rook
        | Queen
        | King
  
    type PieceColor = 
        | White
        | Black
```

*Active Patterns* provide a syntactically elegant way to process Forsyth-Edwards chess notation

```fsharp
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
```


Unlike Haskell, F# is not a purely functional language; F# is *functional-first*. Imperative programming is used when it makes sense to do so. Handling mutable board state is one example

```fsharp
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

...

```

Pattern matching is frequently used for tasks such as evaluating valid piece moves.

```fsharp
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
```

# Chess Engine Overview

This chess engine is similar to and inspired by the C# ChessBin engine, however, functional programming style was used whenever practical to do so.

The most important chess engine modules are:

* `AI.fs` implements the chess engine AI
* `ChessBoard.fs` provides a class representing a chess board which contains chess piece locations and game state
* `Piece.fs` provides types for representing chess pieces
* `PieceValidMoves.fs` provides functions for analysing the game board and determining which chess moves are valid.
* `Engine.fs` manages all actions and provides a high level abstraction of the chess game
