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

    IL_0001: ldarg.0      // args
    IL_0002: call         bool Hw2.Parser::IsArgLengthSupported(string[])
    IL_000c: brfalse.s    Exception1  //if false (0), then OK

    correctLength: ldarg.0      // args
    IL_001a: ldc.i4.0
    IL_001b: ldelem.ref   //Load the element at index onto the top of the stack as an O. The type of the O is the same as the element type of the array pushed on the CIL stack.
    IL_001c: ldarg.1      // val1
    IL_001d: call         bool [System.Runtime]System.Double::TryParse(string, float64&)
    IL_0027: brfalse.s    IncorrectValue1
    
    
    ldarg.0      // args
    IL_0035: ldc.i4.2
    IL_0036: ldelem.ref
    IL_0037: ldarg.3      // val2
    IL_0038: call         bool [System.Runtime]System.Double::TryParse(string, float64&)
    IL_0042: brfalse.s    IncorrectValue2

    ldarg.2      // operation
    IL_0050: ldarg.0      // args
    IL_0051: ldc.i4.1
    IL_0052: ldelem.ref
    IL_0053: call         valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
    IL_0058: stind.i4   //Store value of type int32 into memory at address.
   
    IL_0059: ret

    Exception1: ldstr        "Incorrect data. Length more than 3."
    IL_0013: newobj       instance void [System.Runtime]System.ArgumentException::.ctor(string)
    IL_0018: throw

    IncorrectValue1: ldstr        "Incorrect first argument!"
    IL_002e: newobj       instance void [System.Runtime]System.ArgumentException::.ctor(string)
    IL_0033: throw

    IncorrectValue2: ldstr        "Incorrect second argument!"
    IL_0049: newobj       instance void [System.Runtime]System.ArgumentException::.ctor(string)
    IL_004e: throw
  }

  .method private hidebysig static bool
    IsArgLengthSupported(
      string[] args
    ) cil managed
  {
    IL_0000: ldarg.0      // args
    IL_0001: ldlen
    IL_0002: conv.i4
    IL_0003: ldc.i4.3
    IL_0004: ceq
    IL_0006: ret
  } 

  .method private hidebysig static valuetype Hw2.CalculatorOperation
    ParseOperation(
      string arg
    ) cil managed
  {
    IL_0005: ldarg.0      // arg
    IL_0006: ldstr        "+"
    IL_000b: call         bool [System.Runtime]System.String::op_Equality(string, string)
    IL_0010: brtrue.s     Add
    
    IL_0012: ldarg.0      // arg
    IL_0013: ldstr        "-"
    IL_0018: call         bool [System.Runtime]System.String::op_Equality(string, string)
    IL_001d: brtrue.s     Sub
    
    IL_001f: ldarg.0      // arg
    IL_0020: ldstr        "*"
    IL_0025: call         bool [System.Runtime]System.String::op_Equality(string, string)
    IL_002a: brtrue.s     Mul
    
    IL_002c: ldarg.0      // arg
    IL_002d: ldstr        "/"
    IL_0032: call         bool [System.Runtime]System.String::op_Equality(string, string)
    IL_0037: brtrue.s     Div
    IL_0039: br.s         Exception

    Add: ldc.i4.0
    ret

    Sub: ldc.i4.1
    ret

    Mul: ldc.i4.2
    ret
    
    Div: ldc.i4.3
    ret

    Exception: ldstr        "Incorrect operation!"
    IL_0050: newobj       instance void [System.Runtime]System.InvalidOperationException::.ctor(string)
    IL_0055: throw
  } 
} 

