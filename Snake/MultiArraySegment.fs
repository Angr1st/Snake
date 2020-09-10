namespace Snake

open System

type MultiArraySegment<'a> private() =
    static member Create arr offsetX offsetY countX countY =
           if arr = null || offsetX < 0 || offsetY < 0 || countX < 0 || countY < 0 then
               None
           else
               let mutable arrayOfArrSegments = ArraySegment<'a>[]
               for i = offsetX to countX do
                   for j = offsetY to countY do
                   
    member private self.Value

module MultiArraySegment =
    
    let create
                    

