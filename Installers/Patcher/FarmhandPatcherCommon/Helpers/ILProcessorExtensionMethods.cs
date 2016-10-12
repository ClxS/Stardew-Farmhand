using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;

namespace Farmhand.Helpers
{
    public static class ILProcessorExtensionMethods
    {
        public static Instruction PushStringToStack(this ILProcessor il, string str)
        {
            return il.Create(OpCodes.Ldstr, str);
        }

        public static Instruction PushInt32ToStack(this ILProcessor il, int index)
        {
            return il.Create(OpCodes.Ldc_I4, index);
        }

        public static Instruction PushVariableReferenceToStack(this ILProcessor il, VariableDefinition var)
        {
            return il.Create(OpCodes.Ldloca, var);
        }
        
        public static Instruction[] CreateArray(this ILProcessor il, TypeReference type, int count)
        {
            return new[] {
                il.Create(OpCodes.Ldc_I4, count),
                il.Create(OpCodes.Newarr, type)
            };
        }

        public static Instruction[] InsertParameterIntoArray(this ILProcessor il, ParameterDefinition param,
            int index)
        {
            var instructions = new List<Instruction>();
            instructions.Add(il.Create(OpCodes.Dup));
            instructions.Add(il.Create(OpCodes.Ldc_I4, index));
            instructions.Add(il.Create(OpCodes.Ldarg, param));
            if (param.ParameterType.IsPrimitive)
                instructions.Add(il.Create(OpCodes.Box, param.ParameterType));
            instructions.Add(il.Create(OpCodes.Stelem_Ref));
            return instructions.ToArray();
        }

        public static Instruction Call(this ILProcessor il, MethodDefinition method)
        {
            return il.Create(OpCodes.Call, method);
        }

        public static Instruction BranchIfFalse(this ILProcessor il, Instruction dest)
        {
            return il.Create(OpCodes.Brfalse, dest);
        }

        public static Instruction BranchUnconditional(this ILProcessor il, Instruction dest)
        {
            return il.Create(OpCodes.Br, dest);
        }

        public static Instruction PushFieldToStack(this ILProcessor il, FieldDefinition field)
        {
            return il.Create(OpCodes.Ldsfld, field);
        }

        public static Instruction PopStackIntoVariable(this ILProcessor il, VariableDefinition var)
        {
            return il.Create(OpCodes.Stloc, var);
        }

        public static Instruction[] LoadAndUnboxObject(this ILProcessor il, 
            VariableDefinition var, TypeReference type)
        {
            var instructions = new List<Instruction>();

            instructions.Add(il.Create(OpCodes.Ldloc, var));
            if (type.IsPrimitive || type.IsGenericParameter)
            {
                instructions.Add(il.Create(OpCodes.Unbox_Any, type));
            }
            else
            {
                instructions.Add(il.Create(OpCodes.Castclass, type));
            }

            return instructions.ToArray();
        }
    }
}
