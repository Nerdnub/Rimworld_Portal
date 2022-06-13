using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace PortalLibrary
{
    [StaticConstructorOnStartup]
    public class Portal_Building : Building
    {
        public override Graphic Graphic 
        {
            get
            {
                if (this.comp == null) 
                {
                    this.comp = this.TryGetComp<CompPortal>();
                }
                Graphic_Single graphic_Single = (Graphic_Single)Portal_Building.Transmitting;
                graphic_Single.drawSize = new Vector2(5,5);
                return this.comp != null && this.comp.transmitting ? graphic_Single : base.Graphic;
            }
        }
        public CompPortal comp = null;
        public static readonly Graphic Transmitting = GraphicDatabase.Get<Graphic_Single>("TransmittingPortal");
    }
}
