﻿using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace AlphaBehavioursAndEvents
{
    public class CompChangeDef : ThingComp
    {




        public CompProperties_ChangeDef Props
        {
            get
            {
                return (CompProperties_ChangeDef)this.props;
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (!AlphaAnimalsEvents_Mod.settings.flagAlphaMechanoids) {
                if (parent.Map != null && Find.FactionManager.FirstFactionOfDef(FactionDef.Named(Props.factionToChangeTo))!=null)
                {
                    PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDef.Named(Props.defToChangeTo), Find.FactionManager.FirstFactionOfDef(FactionDef.Named(Props.factionToChangeTo)), PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, false);
                    Pawn pawn = PawnGenerator.GeneratePawn(request);
                    GenSpawn.Spawn(pawn, this.parent.Position, parent.Map, WipeMode.Vanish);

                    Lord lord = null;
                    if (pawn.Map.mapPawns.SpawnedPawnsInFaction(Find.FactionManager.FirstFactionOfDef(FactionDef.Named(Props.factionToChangeTo))).Any((Pawn p) => p != pawn))
                    {
                        lord = ((Pawn)GenClosest.ClosestThing_Global(pawn.Position, pawn.Map.mapPawns.SpawnedPawnsInFaction(Find.FactionManager.FirstFactionOfDef(FactionDef.Named(Props.factionToChangeTo))), 99999f, (Thing p) => p != pawn && ((Pawn)p).GetLord() != null, null)).GetLord();
                    }
                    if (lord == null)
                    {
                        LordJob_DefendPoint lordJob = new LordJob_DefendPoint(pawn.Position, null, false, true);
                        lord = LordMaker.MakeNewLord(Find.FactionManager.FirstFactionOfDef(FactionDef.Named(Props.factionToChangeTo)), lordJob, Find.CurrentMap, null);
                    }
                    lord.AddPawn(pawn);


                    this.parent.Destroy();
                }

            }
            
           
        }




    }
}