﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace PGD_40kFauna
{
    public class IncidentWorker_KhymeraAttack : IncidentWorker
    {
        public override bool CanFireNowSub(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            IntVec3 intVec;
            return map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(ThingDef.Named("PGD_Khymera")) && this.TryFindEntryCell(map, out intVec);
        }

        private bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Animal + 0.2f);
        }

        public override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            PawnKindDef pawnKindDef = PawnKindDef.Named("PGD_Khymera");
            IntVec3 intVec;
            if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal))
            {
                return false;
            }

            Rot4 rot = Rot4.FromAngleFlat((map.Center - intVec).AngleFlat);

            List<Thing> spawned = new List<Thing>();
            int toSpawn = Rand.Range(4, 9);
            int i = 0;
            for (i = 0; i < toSpawn; i++)
            {
                IntVec3 loc2 = CellFinder.RandomClosewalkCellNear(intVec, map, 1, null);
                Pawn newThing = PawnGenerator.GeneratePawn(pawnKindDef, null);
                GenSpawn.Spawn(newThing, loc2, map, WipeMode.Vanish);
                spawned.Add(newThing);
                newThing.mindState.mentalStateHandler.TryStartMentalState(DefDatabase<MentalStateDef>.GetNamed("Manhunter"), null, false, false, null, false);

            }
            Find.LetterStack.ReceiveLetter("LetterLabelKhymeraAttack".Translate(), "KhymeraAttack".Translate(), LetterDefOf.ThreatBig, spawned, null, null);
            return true;
        }
    }

}
