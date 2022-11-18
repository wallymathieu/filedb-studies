module CoreFs.Threading
open System
/// small helper class in order to observe values and store the maximum value
type ThreadSafeMax<'t when 't :comparison>(initial:'t) =
    let monitor = Object()
    let mutable maximum = initial 
    /// get the current maximum
    member _.Value with get ()= maximum
    /// observe a value, store the value if it's greater than the current value
    member _.Observe value =
        lock monitor (fun ()-> maximum <- max maximum value)
        value
