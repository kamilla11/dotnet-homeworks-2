.class public abstract sealed auto ansi beforefieldinit
  Hw2.Calculator extends System.Object
{
.method public static float64
    Calculate(
      float64 value1,
      valuetype Hw2.CalculatorOperation operation,
      float64 value2
    ) cil managed
  {
    ldarg.0 // value1
    ldarg.2 // value2
   
    ldarg.1      // operation
    switch       (Add, Sub, Mul, Div)
    br.s         Exception

    Add:  add
    ret

    Sub:  sub
    ret

    Mul: mul
    ret

    Div:  div
    ret      

    Exception: newobj       instance void [System.Runtime]System.ArgumentOutOfRangeException::.ctor()
    throw
  } 
}



