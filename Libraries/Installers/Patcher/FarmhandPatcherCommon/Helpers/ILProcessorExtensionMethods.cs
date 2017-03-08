namespace Farmhand.Installers.Patcher.Helpers
{
    using System.Collections.Generic;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    /// <summary>
    ///     Extension methods for <see cref="ILProcessor" />
    /// </summary>
    public static class IlProcessorExtensionMethods
    {
        /// <summary>
        ///     Pushes a string to the stack.
        /// </summary>
        /// <param name="il">
        ///     The <see cref="ILProcessor" />.
        /// </param>
        /// <param name="str">
        ///     The string to push.
        /// </param>
        /// <returns>
        ///     The IL <see cref="Instruction" /> for this operation.
        /// </returns>
        public static Instruction PushStringToStack(this ILProcessor il, string str)
        {
            return il.Create(OpCodes.Ldstr, str);
        }

        /// <summary>
        ///     Pushes an integer to the stack.
        /// </summary>
        /// <param name="il">
        ///     The <see cref="ILProcessor" />.
        /// </param>
        /// <param name="index">
        ///     The integer to push.
        /// </param>
        /// <returns>
        ///     The IL <see cref="Instruction" /> for this operation.
        /// </returns>
        public static Instruction PushInt32ToStack(this ILProcessor il, int index)
        {
            return il.Create(OpCodes.Ldc_I4, index);
        }

        /// <summary>
        ///     Pushes a variable reference to the stack.
        /// </summary>
        /// <param name="il">
        ///     The <see cref="ILProcessor" />.
        /// </param>
        /// <param name="var">
        ///     The variable reference to push.
        /// </param>
        /// <returns>
        ///     The IL <see cref="Instruction" /> for this operation.
        /// </returns>
        public static Instruction PushVariableReferenceToStack(this ILProcessor il, VariableDefinition var)
        {
            return il.Create(OpCodes.Ldloca, var);
        }

        /// <summary>
        ///     Creates a new array
        /// </summary>
        /// <param name="il">
        ///     The <see cref="ILProcessor" />.
        /// </param>
        /// <param name="type">
        ///     The type of the array.
        /// </param>
        /// <param name="count">
        ///     The number of elements of the array.
        /// </param>
        /// <returns>
        ///     The IL <see cref="Instruction" /> for this operation.
        /// </returns>
        public static Instruction[] CreateArray(this ILProcessor il, TypeReference type, int count)
        {
            return new[] { il.Create(OpCodes.Ldc_I4, count), il.Create(OpCodes.Newarr, type) };
        }

        /// <summary>
        ///     Inserts a parameter into an array
        /// </summary>
        /// <param name="il">
        ///     The <see cref="ILProcessor" />.
        /// </param>
        /// <param name="param">
        ///     The parameter to insert into the array.
        /// </param>
        /// <param name="index">
        ///     The index in which to insert the parameter.
        /// </param>
        /// <returns>
        ///     The IL <see cref="Instruction" /> for this operation.
        /// </returns>
        public static Instruction[] InsertParameterIntoArray(this ILProcessor il, ParameterDefinition param, int index)
        {
            var instructions = new List<Instruction>
                                   {
                                       il.Create(OpCodes.Dup),
                                       il.Create(OpCodes.Ldc_I4, index),
                                       il.Create(OpCodes.Ldarg, param)
                                   };

            if (param.ParameterType.IsPrimitive)
            {
                instructions.Add(il.Create(OpCodes.Box, param.ParameterType));
            }

            instructions.Add(il.Create(OpCodes.Stelem_Ref));
            return instructions.ToArray();
        }

        /// <summary>
        ///     Makes a call to a method
        /// </summary>
        /// <param name="il">
        ///     The <see cref="ILProcessor" />.
        /// </param>
        /// <param name="method">
        ///     The method to call.
        /// </param>
        /// <returns>
        ///     The IL <see cref="Instruction" /> for this operation.
        /// </returns>
        public static Instruction Call(this ILProcessor il, MethodDefinition method)
        {
            return il.Create(OpCodes.Call, method);
        }

        /// <summary>
        ///     Makes a branch if the value at the top of the stack is false.
        /// </summary>
        /// <param name="il">
        ///     The <see cref="ILProcessor" />.
        /// </param>
        /// <param name="dest">
        ///     The <see cref="Instruction" /> to branch to.
        /// </param>
        /// <returns>
        ///     The IL <see cref="Instruction" /> for this operation.
        /// </returns>
        public static Instruction BranchIfFalse(this ILProcessor il, Instruction dest)
        {
            return il.Create(OpCodes.Brfalse, dest);
        }

        /// <summary>
        ///     Makes an unconditional branch to the provided instruction.
        /// </summary>
        /// <param name="il">
        ///     The <see cref="ILProcessor" />.
        /// </param>
        /// <param name="dest">
        ///     The <see cref="Instruction" /> to branch to.
        /// </param>
        /// <returns>
        ///     The IL <see cref="Instruction" /> for this operation.
        /// </returns>
        public static Instruction BranchUnconditional(this ILProcessor il, Instruction dest)
        {
            return il.Create(OpCodes.Br, dest);
        }

        /// <summary>
        ///     Pushes a field to the stack.
        /// </summary>
        /// <param name="il">
        ///     The <see cref="ILProcessor" />.
        /// </param>
        /// <param name="field">
        ///     The field to push.
        /// </param>
        /// <returns>
        ///     The IL <see cref="Instruction" /> for this operation.
        /// </returns>
        public static Instruction PushFieldToStack(this ILProcessor il, FieldDefinition field)
        {
            return il.Create(OpCodes.Ldsfld, field);
        }

        /// <summary>
        ///     Pops the value at the top of the stack into a variable.
        /// </summary>
        /// <param name="il">
        ///     The <see cref="ILProcessor" />.
        /// </param>
        /// <param name="var">
        ///     The variable to pop into.
        /// </param>
        /// <returns>
        ///     The IL <see cref="Instruction" /> for this operation.
        /// </returns>
        public static Instruction PopStackIntoVariable(this ILProcessor il, VariableDefinition var)
        {
            return il.Create(OpCodes.Stloc, var);
        }

        /// <summary>
        ///     Pushes a variable to the stack and unboxes it.
        /// </summary>
        /// <param name="il">
        ///     The <see cref="ILProcessor" />.
        /// </param>
        /// <param name="var">
        ///     The variable to push to the stack.
        /// </param>
        /// <param name="type">
        ///     The type of the variable being pushed.
        /// </param>
        /// <returns>
        ///     The IL <see cref="Instruction" />s for this operation.
        /// </returns>
        public static Instruction[] LoadAndUnboxObject(this ILProcessor il, VariableDefinition var, TypeReference type)
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