using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld;
using UnityEngine;
using Verse;

namespace PortalLibrary
{
    public class CommandDelivery : Command
    {
        public override string Label => "Transmit".Translate();
        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            CompPortal compPortal = portal.TryGetComp<CompPortal>();
            compPortal.transmitting = true;
            compPortal.mainPortal = true;
            CompPortal compLinked = linkedPortal.TryGetComp<CompPortal>();
            compLinked.transmitting = true;
        }
        public override void GizmoUpdateOnMouseover()
        {
            base.GizmoUpdateOnMouseover();
            GenDraw.DrawFieldEdges(GenRadial.RadialCellsAround(this.portal.Position, 3f, true).ToList());
        }
        public override bool GroupsWith(Gizmo other)
        {
            return false;
        }
        public Building portal;
        public Building linkedPortal;
    }
}
