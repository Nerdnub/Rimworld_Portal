using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using UnityEngine;
using System.Reflection;

namespace PortalLibrary
{
    [StaticConstructorOnStartup]
    public class CompPortal : ThingComp, IVerbOwner , ILoadReferenceable
    {
        public override string CompInspectStringExtra()
        {
            return this.linkedPortal == null ? "Unlinked".Translate() : "Linked".Translate();
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }

            if (this.verb == null)
            {
                this.InitVerb();
            }
            CommandLink link = new CommandLink();
            link.verb = this.verb;
            link.icon = CompPortal.link;
            link.portal = (Building)this.parent;
            yield return link;
            CommandDelivery commandDelivery = new CommandDelivery();
            commandDelivery.linkedPortal = this.linkedPortal;
            commandDelivery.portal = (Building)this.parent;
            commandDelivery.icon = CompPortal.transmit;
            if (this.linkedPortal == null || this.linkedPortal.Map == null)
            {
                commandDelivery.Disable();
            }
            yield return commandDelivery;
            yield break;
        }
        public override void CompTick()
        {
            base.CompTick();
            if (this.transmitting && this.mainPortal)
            {
                if (this.progress == null)
                {
                    this.progress = EffecterDefOf.ProgressBar.Spawn();
                }
                this.progress.EffectTick(this.parent,this.parent);
                MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progress.children[0]).mote;
                if(mote != null)
                {
                    mote.progress = (float)((float)this.time / 300f);
                    mote.alwaysShow = true;
                }
                this.time++;
                if (this.time > 300) 
                {
                    this.time = 0;
                    List<Thing> things = new List<Thing>();
                    foreach (IntVec3 intVec3 in GenRadial.RadialCellsAround(this.parent.Position, 3f, true))
                    {
                        if (intVec3.InBounds(this.parent.Map))
                        {
                            things.AddRange(intVec3.GetThingList(this.parent.Map));
                        }
                    }
                    things.Remove(things.Find((Thing x) => x == this.parent));

                    foreach (Thing thing in things)
                    {
                        if (thing.def.category != ThingCategory.Plant && thing.def.category != ThingCategory.Building && thing.def.defName != "Portal" && thing.def.destroyable)
                        {
                            thing.DeSpawn();
                            GenSpawn.Spawn(thing,this.linkedPortal.Position, this.linkedPortal.Map);
                        }
                    }
                    this.transmitting = false;
                    this.linkedPortal.TryGetComp<CompPortal>().transmitting = false;
                    this.progress.Cleanup();
                    this.progress = null;
                }
            }
        }
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look(ref this.linkedPortal, "linkedPortal");
            Scribe_Values.Look(ref this.transmitting, "transmitting");
            Scribe_Values.Look(ref this.mainPortal, "mainPortal");
            Scribe_Values.Look(ref this.time, "time");
        }
        private void InitVerb()
        {
            Verb_Link verb = (Verb_Link)Activator.CreateInstance(this.parent.def.Verbs.First().verbClass);
            verb.caster = this.parent;
            verb.portal = (Building)this.parent;
            verb.verbProps = this.parent.def.Verbs.First();
            verb.verbTracker = new VerbTracker(this);
            this.verb = verb;
        }
        public string UniqueVerbOwnerID()
        {
            return "Portal_" + this.parent.ThingID;
        }

        public bool VerbsStillUsableBy(Pawn p)
        {
            throw new NotImplementedException();
        }

        public string GetUniqueLoadID()
        {
            return this.parent.GetUniqueLoadID() + "_comp_" + base.GetType().FullName;
        }

        public VerbTracker VerbTracker
        {
            get
            {
                return new VerbTracker(this);
            }
        }

        public List<VerbProperties> VerbProperties => this.parent.def.Verbs;

        public List<Tool> Tools => throw new NotImplementedException();

        public ImplementOwnerTypeDef ImplementOwnerTypeDef
        {
            get
            {
                return ImplementOwnerTypeDefOf.NativeVerb;
            }
        }

        public Thing ConstantCaster => throw new NotImplementedException();

        public Effecter progress;
        public int time = 0;
        public bool mainPortal = false;
        public bool transmitting = false;
        public Building linkedPortal;
        public Verb verb;
        public static readonly Texture2D transmit = ContentFinder<Texture2D>.Get("UI/Transmit");
        public static readonly Texture2D link = ContentFinder<Texture2D>.Get("UI/Link");
    }
}
