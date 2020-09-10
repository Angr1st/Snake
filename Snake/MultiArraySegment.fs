namespace Snake

open System

type MultiArraySegment<'a when 'a : equality> private(arrSegList) =
    static member Create (arr: 'a[,]) offsetX offsetY countX countY =
           if arr = null || offsetX < 0 || offsetY < 0 || countX < 0 || countY < 0 then
               None
           else
               let mutable ArrSegments: ArraySegment<'a> list = List.Empty
               for i = offsetX to countX do
                  ArrSegments <- ArrSegments @ [ArraySegment<'a>(arr.[i,*], offsetY, countY)]
               Some (MultiArraySegment ArrSegments)

    member self.Value = arrSegList

module MultiArraySegment =
    

                    

