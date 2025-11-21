using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace AceAttorneyTrilogyThaiMod_BepInEx_5_Mono.obj;

    [HarmonyPatch]
    public class JudgmentCtrlPatch
    {
        public static void Patch()
        {
            Console.WriteLine($"Patching class: {nameof(judgmentCtrl)}...");
            Harmony.CreateAndPatchAll(typeof(JudgmentCtrlPatch));
            Console.WriteLine($"Patching class: {nameof(judgmentCtrl)}... Done");
        }

        #region Patches
        [HarmonyPatch(typeof(judgmentCtrl), "SetSprite")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> JudgmentCtrl_SetSprite_Transpiler(
            IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Console.WriteLine($"Patching method: {nameof(judgmentCtrl)}.SetSprite...");
            var codeInstructions = instructions.ToList();
            var arrayArgIndex = 3; //3 is "array" local variable
            var numArgIndex = 4; //4 is "num" local variable
            var numOverrider = new CodeMatcher(codeInstructions)
                .SearchForward(x => x.opcode == OpCodes.Stloc_S &&
                                    ((LocalBuilder)x.operand).LocalIndex == numArgIndex);
            
            if (numOverrider.IsInvalid)
            {
                Console.WriteLine(
                    $"Failed to find OpCodes: {OpCodes.Stloc_S}, LocalIndex: {numArgIndex} in {nameof(judgmentCtrl)}.SetSprite");
                return codeInstructions;
            }

            var arrayOverrider = numOverrider
                .Clone()
                .SearchBack(x => x.opcode == OpCodes.Stloc_3); // Stloc_3 is for local variable at index 3 (the array)
            if (arrayOverrider.IsInvalid)
            {
                Console.WriteLine($"Failed to find OpCodes: {OpCodes.Stloc_3} in {nameof(judgmentCtrl)}.SetSprite");
                return codeInstructions;
            }
            var arrayOverriderInstructions = new[]
            {
                new CodeInstruction(OpCodes.Ldloc_S, arrayArgIndex), // Load the "array" local variable
                new CodeInstruction(OpCodes.Ldarg_1), // Load the "in_type" argument
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(JudgmentCtrlPatch),
                    nameof(OverrideSpritePositions))),
                new CodeInstruction(OpCodes.Stloc_S, arrayArgIndex)
            };
            
            var numOverriderInstructions = new[]
            {
                new CodeInstruction(OpCodes.Ldloc_S, numArgIndex), // Load the "num" local variable
                new CodeInstruction(OpCodes.Ldarg_0), // Load "this"
                new CodeInstruction(OpCodes.Call, 
                    AccessTools.PropertyGetter(typeof(judgmentCtrl), 
                        "not_count_")),
                new CodeInstruction(OpCodes.Ldarg_0), // Load "this"
                new CodeInstruction(OpCodes.Call, 
                    AccessTools.PropertyGetter(typeof(judgmentCtrl), 
                        "guilty_count_")),
                new CodeInstruction(OpCodes.Ldarg_1), // Load the "in_type" argument
                new CodeInstruction(OpCodes.Call, 
                    AccessTools.Method(typeof(JudgmentCtrlPatch), 
                    nameof(GetCustomNumCalculation))),
                new CodeInstruction(OpCodes.Stloc_S, numArgIndex)
            };
            
            var final = new CodeMatcher(codeInstructions)
                .Start()
                .Advance(arrayOverrider.Pos + 1)
                .InsertAndAdvance(arrayOverriderInstructions)
                .Start()
                .Advance(numOverrider.Pos + arrayOverriderInstructions.Length + 1)
                .InsertAndAdvance(numOverriderInstructions);
            
            // var debug = final.Clone();
            // var modifiedCount = arrayOverriderInstructions.Length + numOverriderInstructions.Length;
            // var seekBackward = 10; // Arbitrary number to go back and see the changes
            // for (var i = final.Pos - modifiedCount - seekBackward; i < final.Pos; i++) // Debugging output
            // {
            //     Console.WriteLine($"Instruction {i}: {debug.Start().InstructionAt(i).opcode} {debug.Start().InstructionAt(i).operand}");
            // }
            
            Console.WriteLine($"Patching method: {nameof(judgmentCtrl)}.SetSprite... Done");
            return final.InstructionEnumeration();
        }

        [HarmonyPatch(typeof(judgmentCtrl), "CoroutineUSA", MethodType.Enumerator)]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> JudgmentCtrl_CoroutineUSA_Transpiler(
            IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Console.WriteLine($"Patching method: {nameof(judgmentCtrl)}.CoroutineUSA...");
            var codeInstructions = instructions.ToList();
            var matcher = new CodeMatcher(codeInstructions)
                .SearchForward(x => x.opcode == OpCodes.Ldc_I4_6);
            if (matcher.IsInvalid)
            {
                Console.WriteLine(
                    $"Failed to find OpCodes: {OpCodes.Ldc_I4_6} in {nameof(judgmentCtrl)}.CoroutineUSA");
                return codeInstructions;
            }
            
            var overrideInstructions = new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0), // Load "this"
                new CodeInstruction(OpCodes.Call, 
                    AccessTools.PropertyGetter(typeof(judgmentCtrl), 
                        "guilty_count_"))
            };
            
            var final = new CodeMatcher(codeInstructions)
                .Start()
                .Advance(matcher.Pos)
                .SetAndAdvance(OpCodes.Nop, null) // Remove the original Ldc_I4_6;
                .InsertAndAdvance(overrideInstructions);
            
            // var debug = final.Clone();
            // var modifiedCount = overrideInstructions.Length + 1;
            // var seekBackward = 10; // Arbitrary number to go back and see the changes
            // for (var i = final.Pos - modifiedCount - seekBackward; i < final.Pos; i++) // Debugging output
            // {
            //     Console.WriteLine($"Instruction {i}: {debug.Start().InstructionAt(i).opcode} {debug.Start().InstructionAt(i).operand}");
            // }  
            
            Console.WriteLine($"Patching method: {nameof(judgmentCtrl)}.CoroutineUSA... Done");
            return final.InstructionEnumeration();
        }

        [HarmonyPatch(typeof(judgmentCtrl), "not_count_", MethodType.Getter)]
        [HarmonyPostfix]
        public static void JudgmentCtrl_not_count_PostFix(judgmentCtrl __instance, ref int __result)
        {
            if (GSStatic.global_work_.language == Language.USA)
            {
                __result = 3; // Override the result for USA
            }
        }

        [HarmonyPatch(typeof(judgmentCtrl), "guilty_count_", MethodType.Getter)]
        [HarmonyPostfix]
        public static void JudgmentCtrl_guilty_count_PostFix(judgmentCtrl __instance, ref int __result)
        {
            if (GSStatic.global_work_.language == Language.USA)
            {
                __result = 7; // Override the result for USA
            }
        }
        #endregion

        #region Helpers
        public static int GetCustomNumCalculation(int original, int not_count_, int guilty_count_, int in_type)
        {
            if (GSUtility.GetLanguageLayoutType(GSStatic.global_work_.language) == Language.USA)
            {
                return (in_type != 0) ? (not_count_ + guilty_count_ - 1) : 0;
            }
            return original;
        }

        public static Vector2[] OverrideSpritePositions(Vector2[] array, int in_type)
        {
            if (GSUtility.GetLanguageLayoutType(GSStatic.global_work_.language) != Language.USA)
                return array; // Return original if not USA

            if (in_type == 0)
            {
                return new Vector2[]
                {
                    new(-686f, -80f),
                    new(-496f, -80f),
                    new(-306f, -80f),
                    new(-126f, -80f),
                    new(39f, -80f),
                    new(190f, -80f),
                    new(350f, -80f),
                    new(534f, -80f),
                    new(714f, -80f)
                };
            }

            return new Vector2[]
            {
                new(-498f, -80f),
                new(-318f, -80f),
                new(-153f, -80f),
                new(-3f, -80f),
                new(157f, -80f),
                new(342f, -80f),
                new(522f, -80f)
            };
        }
        #endregion
    }