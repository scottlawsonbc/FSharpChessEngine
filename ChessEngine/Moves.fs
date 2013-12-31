namespace ChessEngine.Core

[<AutoOpenAttribute>]
module Moves = 

    open System.Collections.Generic
   
    /// A set of constant moves
    type MoveSet = 
        struct
            val Moves : List<int>
            new (moves) = {Moves = moves}
        end
            
    type MoveArrays() =
        // Bishop
        [<DefaultValue>] static val mutable private bishop1 : MoveSet[]
        [<DefaultValue>] static val mutable private bishop1Total : int array
        static member Bishop1 with get() = MoveArrays.bishop1
                                and set x = MoveArrays.bishop1 <- x
        static member Bishop1Total with get() = MoveArrays.bishop1Total
                                    and set x = MoveArrays.bishop1Total <- x

        [<DefaultValue>] static val mutable private bishop2 : MoveSet[]
        [<DefaultValue>] static val mutable private bishop2Total : int array
        static member Bishop2 with get() = MoveArrays.bishop2
                                and set x = MoveArrays.bishop2 <- x        
        static member Bishop2Total with get() = MoveArrays.bishop2Total
                                    and set x = MoveArrays.bishop2Total <- x                                      
        [<DefaultValue>] static val mutable private bishop3 : MoveSet[]
        [<DefaultValue>] static val mutable private bishop3Total : int array
        static member Bishop3 with get() = MoveArrays.bishop3
                                and set x = MoveArrays.bishop3 <- x
        static member Bishop3Total with get() = MoveArrays.bishop3Total
                                    and set x = MoveArrays.bishop3Total <- x                              
        [<DefaultValue>] static val mutable private bishop4 : MoveSet[]
        [<DefaultValue>] static val mutable private bishop4Total : int array
        static member Bishop4 with get() = MoveArrays.bishop4
                                and set x = MoveArrays.bishop4 <- x 
        static member Bishop4Total with get() = MoveArrays.bishop4Total
                                    and set x = MoveArrays.bishop4Total <- x             

        // Black pawns               
        [<DefaultValue>] static val mutable private blackPawn : MoveSet[]
        [<DefaultValue>] static val mutable private blackPawnTotal : int array
        static member BlackPawn with get() = MoveArrays.blackPawn
                                and set x = MoveArrays.blackPawn <- x
        static member BlackPawnTotal with get() = MoveArrays.blackPawnTotal
                                        and set x = MoveArrays.blackPawnTotal <- x                                                     
        // White pawns               
        [<DefaultValue>] static val mutable private whitePawn : MoveSet[]
        [<DefaultValue>] static val mutable private whitePawnTotal : int array
        static member WhitePawn with get() = MoveArrays.whitePawn
                                and set x = MoveArrays.whitePawn <- x
        static member WhitePawnTotal with get() = MoveArrays.whitePawnTotal
                                        and set x = MoveArrays.whitePawnTotal <- x 

        // Knight                    
        [<DefaultValue>] static val mutable private knight : MoveSet[]
        [<DefaultValue>] static val mutable private knightTotal : int array
        static member Knight with get() = MoveArrays.knight
                                and set x = MoveArrays.knight <- x
        static member KnightTotal with get() = MoveArrays.knightTotal
                                    and set x = MoveArrays.knightTotal <- x                   
                                                     
        // Queen                     
        [<DefaultValue>] static val mutable private queen1 : MoveSet[]
        [<DefaultValue>] static val mutable private queen1Total : int array
        static member Queen1 with get() = MoveArrays.queen1
                                and set x = MoveArrays.queen1 <- x
        static member Queen1Total with get() = MoveArrays.queen1Total
                                    and set x = MoveArrays.queen1Total <- x                         
        [<DefaultValue>] static val mutable private queen2 : MoveSet[]
        [<DefaultValue>] static val mutable private queen2Total : int array
        static member Queen2 with get() = MoveArrays.queen2
                                and set x = MoveArrays.queen2 <- x
        static member Queen2Total with get() = MoveArrays.queen2Total
                                    and set x = MoveArrays.queen2Total <- x
                                                               
        [<DefaultValue>] static val mutable private queen3 : MoveSet[]
        [<DefaultValue>] static val mutable private queen3Total : int array
        static member Queen3 with get() = MoveArrays.queen3
                                and set x = MoveArrays.queen3 <- x
        static member Queen3Total with get() = MoveArrays.queen3Total
                                    and set x = MoveArrays.queen3Total <- x
                                         
        [<DefaultValue>] static val mutable private queen4 : MoveSet[]
        [<DefaultValue>] static val mutable private queen4Total : int array
        static member Queen4 with get() = MoveArrays.queen4
                                and set x = MoveArrays.queen4 <- x
        static member Queen4Total with get() = MoveArrays.queen4Total
                                    and set x = MoveArrays.queen4Total <- x
                                                     
        [<DefaultValue>] static val mutable private queen5 : MoveSet[]
        [<DefaultValue>] static val mutable private queen5Total : int array
        static member Queen5 with get() = MoveArrays.queen5
                                and set x = MoveArrays.queen5 <- x
        static member Queen5Total with get() = MoveArrays.queen5Total
                                    and set x = MoveArrays.queen5Total <- x           
                                         
        [<DefaultValue>] static val mutable private queen6 : MoveSet[]
        [<DefaultValue>] static val mutable private queen6Total : int array
        static member Queen6 with get() = MoveArrays.queen6
                                and set x = MoveArrays.queen6 <- x
        static member Queen6Total with get() = MoveArrays.queen6Total
                                    and set x = MoveArrays.queen6Total <- x                          
        [<DefaultValue>] static val mutable private queen7 : MoveSet[]
        [<DefaultValue>] static val mutable private queen7Total : int array
        static member Queen7 with get() = MoveArrays.queen7
                                and set x = MoveArrays.queen7 <- x
        static member Queen7Total with get() = MoveArrays.queen7Total
                                    and set x = MoveArrays.queen7Total <- x  
                                         
        [<DefaultValue>] static val mutable private queen8 : MoveSet[]
        [<DefaultValue>] static val mutable private queen8Total : int array
        static member Queen8 with get() = MoveArrays.queen8
                                and set x = MoveArrays.queen8 <- x
        static member Queen8Total with get() = MoveArrays.queen8Total
                                    and set x = MoveArrays.queen8Total <- x              

        // Rook                      
        [<DefaultValue>] static val mutable private rook1 : MoveSet[]
        [<DefaultValue>] static val mutable private rook1Total : int array
        static member Rook1 with get() = MoveArrays.rook1
                                and set x = MoveArrays.rook1 <- x
        static member Rook1Total with get() = MoveArrays.rook1Total
                                    and set x = MoveArrays.rook1Total <- x  
                                         
        [<DefaultValue>] static val mutable private rook2 : MoveSet[]
        [<DefaultValue>] static val mutable private rook2Total : int array
        static member Rook2 with get() = MoveArrays.rook2
                            and set x = MoveArrays.rook2 <- x
        static member Rook2Total with get() = MoveArrays.rook2Total
                                    and set x = MoveArrays.rook2Total <- x  
                                         
        [<DefaultValue>] static val mutable private rook3 : MoveSet[]
        [<DefaultValue>] static val mutable private rook3Total : int array
        static member Rook3 with get() = MoveArrays.rook3
                            and set x = MoveArrays.rook3 <- x
        static member Rook3Total with get() = MoveArrays.rook3Total
                                    and set x = MoveArrays.rook3Total <- x                               
        [<DefaultValue>] static val mutable private rook4 : MoveSet[]
        [<DefaultValue>] static val mutable private rook4Total : int array
        static member Rook4 with get() = MoveArrays.rook4
                            and set x = MoveArrays.rook4 <- x
        static member Rook4Total with get() = MoveArrays.rook4Total
                                    and set x = MoveArrays.rook4Total <- x 
        // King
        [<DefaultValue>] static val mutable private king : MoveSet[]
        [<DefaultValue>] static val mutable private kingTotal : int array
        static member King with get() = MoveArrays.king
                            and set x = MoveArrays.king <- x
        static member KingTotal with get() = MoveArrays.kingTotal
                                    and set x = MoveArrays.kingTotal <- x 

        static member InitiatePieceMotion() = 
            // Bishop
            MoveArrays.Bishop1 <- Array.create 64 (new MoveSet())
            MoveArrays.Bishop1Total <- Array.zeroCreate 64

            MoveArrays.Bishop2 <- Array.create 64 (new MoveSet())
            MoveArrays.Bishop2Total <- Array.zeroCreate 64

            MoveArrays.Bishop3 <- Array.create 64 (new MoveSet())
            MoveArrays.Bishop3Total <- Array.zeroCreate 64

            MoveArrays.Bishop4 <- Array.create 64 (new MoveSet())
            MoveArrays.Bishop4Total <- Array.zeroCreate 64

            // Pawns
            MoveArrays.BlackPawn <- Array.create 64 (new MoveSet())
            MoveArrays.BlackPawnTotal <- Array.zeroCreate 64

            MoveArrays.WhitePawn <- Array.create 64 (new MoveSet())
            MoveArrays.WhitePawnTotal <- Array.zeroCreate 64

            // Knight
            MoveArrays.Knight <- Array.create 64 (new MoveSet())
            MoveArrays.KnightTotal <- Array.zeroCreate 64

            // Queen
            MoveArrays.Queen1 <- Array.create 64 (new MoveSet())
            MoveArrays.Queen1Total <- Array.zeroCreate 64

            MoveArrays.Queen2 <- Array.create 64 (new MoveSet())
            MoveArrays.Queen2Total <- Array.zeroCreate 64

            MoveArrays.Queen3 <- Array.create 64 (new MoveSet())
            MoveArrays.Queen3Total <- Array.zeroCreate 64

            MoveArrays.Queen4 <- Array.create 64 (new MoveSet())
            MoveArrays.Queen4Total <- Array.zeroCreate 64

            MoveArrays.Queen5 <- Array.create 64 (new MoveSet())
            MoveArrays.Queen5Total <- Array.zeroCreate 64

            MoveArrays.Queen6 <- Array.create 64 (new MoveSet())
            MoveArrays.Queen6Total <- Array.zeroCreate 64

            MoveArrays.Queen7 <- Array.create 64 (new MoveSet())
            MoveArrays.Queen7Total <- Array.zeroCreate 64

            MoveArrays.Queen8 <- Array.create 64 (new MoveSet())
            MoveArrays.Queen8Total <- Array.zeroCreate 64

            // Rook
            MoveArrays.Rook1 <- Array.create 64 (new MoveSet())
            MoveArrays.Rook1Total <- Array.zeroCreate 64

            MoveArrays.Rook2 <- Array.create 64 (new MoveSet())
            MoveArrays.Rook2Total <- Array.zeroCreate 64

            MoveArrays.Rook3 <- Array.create 64 (new MoveSet())
            MoveArrays.Rook3Total <- Array.zeroCreate 64

            MoveArrays.Rook4 <- Array.create 64 (new MoveSet())
            MoveArrays.Rook4Total <- Array.zeroCreate 64

            // King
            MoveArrays.King <- Array.create 64 (new MoveSet())
            MoveArrays.King <- Array.zeroCreate 64

        // Black pawn moves
        static member SetMovesBlackPawn() = 
            for i = 8 to 55 do
                let moveSet = new MoveSet(new List<int>())
                let row = i.ToRow()
                let col = i.ToCol()
            
                // Diagonal kill
                if (col < 7) && (row < 7) then
                    moveSet.Moves.Add(i + 8 + 1)
                    MoveArrays.BlackPawnTotal.[i] <- MoveArrays.BlackPawnTotal.[i] + 1
                if (col > 0 ) && (row < 7) then
                    moveSet.Moves.Add(i + 8 - 1)
                    MoveArrays.BlackPawnTotal.[i] <- MoveArrays.BlackPawnTotal.[i] + 1

                // One forward
                moveSet.Moves.Add(i + 8)
                MoveArrays.BlackPawn.[i] <- moveSet

                // Starting position we can jump two places
                if row = 1 then
                    moveSet.Moves.Add(i + 16)
                    MoveArrays.BlackPawnTotal.[i] <- MoveArrays.BlackPawnTotal.[i] + 1

                MoveArrays.BlackPawn.[i] <- moveSet
        
        /// White pawn moves        
        static member SetMovesWhitePawn() = 
            for i = 8 to 55 do
                let moveSet = new MoveSet(new List<int>())
                let row = i.ToRow()
                let col = i.ToCol()
            
                // Diagonal kill
                if (col < 7) && (row > 0) then
                    moveSet.Moves.Add(i - 8 + 1)
                    MoveArrays.WhitePawnTotal.[i] <- MoveArrays.WhitePawnTotal.[i] + 1
                if (col > 0 ) && (row > 0) then
                    moveSet.Moves.Add(i - 8 - 1)
                    MoveArrays.WhitePawnTotal.[i] <- MoveArrays.WhitePawnTotal.[i] + 1

                // One forward
                moveSet.Moves.Add(i - 8)
                MoveArrays.WhitePawn.[i] <- moveSet

                // Starting position we can jump 2 places
                if row = 6 then
                    moveSet.Moves.Add(i + 16)
                    MoveArrays.WhitePawnTotal.[i] <- MoveArrays.WhitePawnTotal.[i] + 1

                MoveArrays.WhitePawn.[i] <- moveSet