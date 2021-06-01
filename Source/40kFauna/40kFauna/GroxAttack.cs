﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace PGD_40kFauna
{

	[DefOf]
	public static class PGD_40kFaunaDefOf
    {
        static PGD_40kFaunaDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PGD_40kFaunaDefOf));
		}
		public static ThingDef PGD_Grox;

	}

	[StaticConstructorOnStartup]
	public class MainHarmonyInstance : Mod
	{
		public MainHarmonyInstance(ModContentPack content) : base(content)
		{
			var harmony = new Harmony("com.pagoda.rimworld.mod.fauna");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}
	}
	//PawnUtility.Mated
	//Pawn_InteractionsTracker.TryInteractWith()

	[HarmonyPatch(typeof(Pawn_InteractionsTracker), "TryInteractWith")]
		public static class Pawn_InteractionsTracker_TryInteractWith_Grox_Attack
		{
			
			[HarmonyPostfix]
			public static void Postfix(Pawn_InteractionsTracker __instance, Pawn ___pawn, Pawn recipient, InteractionDef intDef, ref bool __result)
			{
						
			if (recipient.def == PGD_40kFaunaDefOf.PGD_Grox && Rand.Chance(0.05f))
				{

				string MessageGroxAttack = "{0} has become enraged and is making a territorial display.";
								
					String.Format(MessageGroxAttack, recipient.Name);

				recipient.mindState.mentalStateHandler.TryStartMentalState(DefDatabase<MentalStateDef>.GetNamed("Manhunter"), null, false, false, null, false);

				}

			}
		}
	[HarmonyPatch(typeof(PawnUtility), "Mated")]
	public static class Pawn_UtilityMated_Grox_Attack
	{

		[HarmonyPostfix]
		public static void Postfix(Pawn __male, Pawn __female)
		{

			if (__female.def == PGD_40kFaunaDefOf.PGD_Grox && Rand.Chance(0.05f))
			{

				string MessageGroxAttack = "{0} has become enraged and is making a territorial display.";

				String.Format(MessageGroxAttack, __female.Name);

				__female.mindState.mentalStateHandler.TryStartMentalState(DefDatabase<MentalStateDef>.GetNamed("Manhunter"), null, false, false, null, false);

			}

		}
	}

}

