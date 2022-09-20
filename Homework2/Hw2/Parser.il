.class public abstract sealed auto ansi beforefieldinit
  Hw2.Parser extends System.Object
{
  .method public static void
    ParseCalcArguments(
      string[] args,
      [out] float64& val1,
      [out] valuetype Hw2.CalculatorOperation& operation,
      [out] float64& val2
    ) cil managed
  {

    ldarg.0      // args
    call         bool Hw2.Parser::IsArgLengthSupported(string[])
    brfalse.s    Exception1  //if false (0), then OK

    ldarg.0      // args
    ldc.i4.0
    ldelem.ref   // Load the element at index onto the top of the stack as an O. The type of the O is the same as the element type of the array pushed on the CIL stack.
    ldarg.1      // val1
    call         bool [System.Runtime]System.Double::TryParse(string, float64&)
    brfalse.s    IncorrectValue1
    
    
    ldarg.0      // args
    ldc.i4.2
    ldelem.ref
    ldarg.3      // val2
    call         bool [System.Runtime]System.Double::TryParse(string, float64&)
    brfalse.s    IncorrectValue2

    ldarg.2      // operation
    ldarg.0      // args
    ldc.i4.1
    ldelem.ref
    call         valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
    stind.i4   //Store value of type int32 into memory at address.
   
    ret

    Exception1: ldstr        "Incorrect data. Length more than 3."
    newobj       instance void [System.Runtime]System.ArgumentException::.ctor(string)
    throw

    IncorrectValue1: ldstr        "Incorrect first argument!"
    newobj       instance void [System.Runtime]System.ArgumentException::.ctor(string)
    throw

    IncorrectValue2: ldstr        "Incorrect second argument!"
    newobj       instance void [System.Runtime]System.ArgumentException::.ctor(string)
    throw
  }

  .method private hidebysig static bool
    IsArgLengthSupported(
      string[] args
    ) cil managed
  {
    ldarg.0      // args
    ldlen
    conv.i4
    ldc.i4.3
    ceq
    ret
  } 

  .method private hidebysig static valuetype Hw2.CalculatorOperation
    ParseOperation(
      string arg
    ) cil managed
  {
    ldarg.0      // arg
    ldstr        "+"
    call         bool [System.Runtime]System.String::op_Equality(string, string)
    brtrue.s     Add
    
    ldarg.0      // arg
    ldstr        "-"
    call         bool [System.Runtime]System.String::op_Equality(string, string)
    brtrue.s     Sub
    
    ldarg.0      // arg
    ldstr        "*"
    call         bool [System.Runtime]System.String::op_Equality(string, string)
    brtrue.s     Mul
    
    ldarg.0      // arg
    ldstr        "/"
    call         bool [System.Runtime]System.String::op_Equality(string, string)
    brtrue.s     Div
    br.s         Exception

    Add: ldc.i4.0
    ret
    Sub: ldc.i4.1
    ret
    Mul: ldc.i4.2
    ret
    Div: ldc.i4.3
    ret

    Exception: ldstr        "Incorrect operation!"
    newobj       instance void [System.Runtime]System.InvalidOperationException::.ctor(string)
    throw
  } 
} 

