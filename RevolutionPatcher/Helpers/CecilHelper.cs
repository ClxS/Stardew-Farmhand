using Mono.Cecil;
using Mono.Cecil.Cil;
using Revolution.Cecil;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Revolution.Helpers
{
    public static class CecilHelper
    {
        //System.Void StardewValley.Game1::.ctor()

        private static void InjectMethod(ILProcessor ilProcessor, Instruction target, MethodReference method)
        {
            Instruction callEnterInstruction = ilProcessor.Create(OpCodes.Call, method);

            if (method.HasThis)
            {
                Instruction loadObjInstruction = ilProcessor.Create(OpCodes.Ldarg_0);
                ilProcessor.InsertBefore(target, loadObjInstruction);
            }

            if (method.HasParameters)
            {
                Instruction loadObjInstruction = ilProcessor.Create(OpCodes.Ldarg_0);
                ilProcessor.InsertBefore(target, loadObjInstruction);
                ilProcessor.InsertAfter(loadObjInstruction, callEnterInstruction);
            }
            else
            {
                ilProcessor.InsertBefore(target, callEnterInstruction);
            }

        }

        private static void InjectMethod(ILProcessor ilProcessor, IEnumerable<Instruction> targets, MethodReference method)
        {
            foreach (var target in targets.ToList())
            {
                InjectMethod(ilProcessor, target, method);
            }
        }

        private static List<Instruction> GetMatchingInstructions(Collection<Instruction> instructions, OpCode opcode, object @object)
        {
            return instructions.Where(n => n.OpCode == opcode && n.Operand == @object).ToList();
        }

        /*public static void RedirectConstructor(CecilContext stardewContext, CecilContext smapiContext,
            string typeToAlter, string methodToAlter,
            string injecteeType, string injecteeMethod,
            string injectedType, string injectedMethod)
        {
            var ilProcessor = stardewContext.GetMethodILProcessor(typeToAlter, methodToAlter);
            var methodDefinition = stardewContext.GetMethodDefinition(injecteeType, injecteeMethod);

            var methodInfo = smapiContext.GetSMAPITypeContructor(injectedType);
            var reference = smapiContext.ImportSMAPIMethodInStardew(stardewContext, methodInfo);

            var instructionsToAlter = GetMatchingInstructions(ilProcessor.Body.Instructions, OpCodes.Newobj, methodDefinition);

            var newInstruction = ilProcessor.Create(OpCodes.Newobj, reference);
            foreach (var instruction in instructionsToAlter)
            {
                ilProcessor.Replace(instruction, newInstruction);
            }
        }*/

        public static void InjectEntryMethod(CecilContext stardewContext, string injecteeType, string injecteeMethod,
            string injectedType, string injectedMethod)
        {
            MethodDefinition methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            ILProcessor ilProcessor = stardewContext.GetMethodILProcessor(injecteeType, injecteeMethod);
            InjectMethod(ilProcessor, ilProcessor.Body.Instructions.First(), methodDefinition);
        }

        public static void InjectExitMethod(CecilContext stardewContext, string injecteeType, string injecteeMethod,
            string injectedType, string injectedMethod)
        {
            MethodDefinition methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            ILProcessor ilProcessor = stardewContext.GetMethodILProcessor(injecteeType, injecteeMethod);
            InjectMethod(ilProcessor, ilProcessor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret), methodDefinition);
        }
    }
}
