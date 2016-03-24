using Mono.Cecil;
using Mono.Cecil.Cil;
using Revolution.Cecil;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Reflection;

namespace Revolution.Helpers
{
    public static class CecilHelper
    {
        //System.Void StardewValley.Game1::.ctor()

        private static void InjectMethod(ILProcessor ilProcessor, Instruction target, MethodReference method, bool cancelable = false)
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

            if (cancelable)
            {
                var branch = ilProcessor.Create(OpCodes.Brtrue, ilProcessor.Body.Instructions.Last());
                var ret = ilProcessor.Create(OpCodes.Ret);
                ilProcessor.InsertAfter(callEnterInstruction, branch);
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
        
        public static void InjectEntryMethod(CecilContext stardewContext, string injecteeType, string injecteeMethod,
            string injectedType, string injectedMethod)
        {
            MethodDefinition methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            ILProcessor ilProcessor = stardewContext.GetMethodILProcessor(injecteeType, injecteeMethod);
            //InjectMethod(ilProcessor, ilProcessor.Body.Instructions.First(), methodDefinition);
            InjectMethod(ilProcessor, ilProcessor.Body.Instructions.First(), methodDefinition, methodDefinition.ReturnType != null && methodDefinition.ReturnType.FullName == typeof(bool).FullName);
        }

        public static void InjectExitMethod(CecilContext stardewContext, string injecteeType, string injecteeMethod,
            string injectedType, string injectedMethod)
        {
            MethodDefinition methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            ILProcessor ilProcessor = stardewContext.GetMethodILProcessor(injecteeType, injecteeMethod);
            InjectMethod(ilProcessor, ilProcessor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret), methodDefinition);
        }

        public static void RedirectConstructorFromBase(CecilContext stardewContext, Type asmType, string type, string method)
        {
            ILProcessor processor = stardewContext.GetMethodILProcessor("Revolution.Test", "Test3");
            ILProcessor processor2 = stardewContext.GetMethodILProcessor("Revolution.Test", "Test4");

            var newConstructor = asmType.GetConstructor(new Type[] { });
            var oldConstructor = asmType.BaseType.GetConstructor(new Type[] { });
            var newConstructorReference = stardewContext.GetMethodDefinition(asmType.FullName, newConstructor.Name);
            var oldConstructorReference = stardewContext.GetMethodDefinition(asmType.BaseType.FullName, oldConstructor.Name);

            ILProcessor ilProcessor = stardewContext.GetMethodILProcessor(type, method);
            var instructions = ilProcessor.Body.Instructions.Where(n => n.OpCode == OpCodes.Newobj && n.Operand == oldConstructorReference).ToList();
            foreach(var instruction in instructions)
            {
                ilProcessor.Replace(instruction, ilProcessor.Create(OpCodes.Newobj, newConstructorReference));
            }
        }

        public static void SetVirtualCallOnMethod(CecilContext cecilContext, string fullName, string name, string type, string method)
        {
            MethodDefinition methodDefinition = cecilContext.GetMethodDefinition(fullName, name);
            ILProcessor ilProcessor = cecilContext.GetMethodILProcessor(type, method);

            var instructions = ilProcessor.Body.Instructions.Where(n => n.OpCode == OpCodes.Call && n.Operand == methodDefinition).ToList();
            foreach (var instruction in instructions)
            {
                ilProcessor.Replace(instruction, ilProcessor.Create(OpCodes.Callvirt, methodDefinition));
            }
        }

        public static void SetVirtualOnBaseMethods(CecilContext stardewContext, string typeName)
        {
            var type = stardewContext.GetTypeDefinition(typeName);
            
            if (type.HasMethods)
            {
                foreach (MethodDefinition method in type.Methods.Where(n => !n.IsConstructor && !n.IsStatic))
                {
                    if (!method.IsVirtual)
                    {
                        method.IsVirtual = true;
                        method.IsNewSlot = true;
                        method.IsReuseSlot = false;
                        method.IsHideBySig = true;
                    }
                }
            }
        }

        public static void AlterProtectionOnTypeMembers(CecilContext stardewContext, bool @public, string typeName)
        {            
            var type = stardewContext.GetTypeDefinition(typeName);

            if (type.HasMethods)
            {
                foreach (MethodDefinition method in type.Methods)
                {
                    if (!@public)
                    {
                        if (method.IsPrivate)
                        {
                            method.IsPrivate = false;
                            method.IsFamily = true;
                        }
                    }
                    else
                    {
                        if (method.IsPrivate || method.IsFamily)
                        {
                            method.IsPrivate = false;
                            method.IsPublic = false;
                        }
                    }
                }
            }

            if (type.HasFields)
            {
                foreach (FieldDefinition field in type.Fields)
                {
                    if (!@public)
                    {
                        if (field.IsPrivate)
                        {
                            field.IsPrivate = false;
                            field.IsFamily = true;
                        }
                    }
                    else
                    {
                        if (field.IsPrivate || field.IsFamily)
                        {
                            field.IsPrivate = false;
                            field.IsPublic = false;
                        }
                    }
                }
            }
        }
    }
}
