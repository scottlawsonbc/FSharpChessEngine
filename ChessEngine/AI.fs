namespace ChessEngine.Core

// Handles the artificial intelligence for the CPU player
module AI =   
    
    open System
    open System.Collections.Generic
    
    /// Opening moves used by the AI
    type OpeningMove(startFen, endFen) = 
        let mutable endingFEN = startFen // ending Forsyth-Edwards notation
        let mutable startingFEN = endFen // starting Forysyth-Edwards notation
        let mutable moves = new List<MoveContent>()
        let mutable currentGameBook = new List<OpeningMove>() // Holds a record of all game moves

        member x.EndingFEN 
            with get() = endingFEN
            and set value = endingFEN <- value

        member x.StartingFEN
            with get() = startingFEN
            and set value = startingFEN <- value

        member x.Moves
            with get() = moves
            and set value = moves <- value

        member x.CurrentGameBook 
            with get() = currentGameBook
            and set value = currentGameBook <- value

        new () = new OpeningMove("", "")

        member x.SaveCurrentGameMove (board:Board) (previousBoard:Board) (gameBook:ICollection<OpeningMove>) (bestMove:MoveContent) =
            try
                let move = 
                move.star
            with
            | ex -> failwith ex.Message
