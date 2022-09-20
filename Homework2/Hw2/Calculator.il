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
   
    IL_0005: ldarg.1      // operation
    IL_0006: switch       (Add, Sub, Mul, Div)
    IL_001b: br.s         IL_0035

    Add:  add
    IL_0020: ret

    Sub:  sub
    IL_0026: ret

    Mul: mul
    IL_002c: ret

    Div:  div
    IL_0032: ret      // V_0

    IL_0035: newobj       instance void [System.Runtime]System.ArgumentOutOfRangeException::.ctor()
    IL_003a: throw
    
  } 
}



