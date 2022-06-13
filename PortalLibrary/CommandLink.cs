using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace PortalLibrary
{
    public class CommandLink : Command_VerbTarget
    {
        public override string Label => "Link".Translate();
        public override string Desc => this.verb.ToString();
        public override bool GroupsWith(Gizmo other)
        {
            return false;
        }

        public override void ProcessInput(Event ev)
        {
            Find.Targeter.targetingSourceAdditionalPawns = new List<Pawn>();
            Find.Targeter.BeginTargeting(this.portal.def.Verbs.First().targetParams, this.verb, (LocalTargetInfo x) => this.verb.TryStartCastOn(x));
        }

        public Building portal;
    }
}
