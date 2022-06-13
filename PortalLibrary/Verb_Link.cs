using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace PortalLibrary
{
    public class Verb_Link : Verb_CastBase
    {
        public override bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false)
        {
            if (castTarg.Thing.def.defName == "Portal")
            {
                this.portal.TryGetComp<CompPortal>().linkedPortal = (Building)castTarg.Thing;
                castTarg.Thing.TryGetComp<CompPortal>().linkedPortal = this.portal;
            }
            return false;
        }

        protected override bool TryCastShot()
        {
            return true;
        }

        public override Verb GetVerb => this;

        public Building portal;
    }
}