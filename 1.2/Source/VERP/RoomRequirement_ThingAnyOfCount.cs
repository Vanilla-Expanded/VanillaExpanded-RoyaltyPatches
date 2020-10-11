using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VERP
{
    public class RoomRequirement_ThingAnyOfCount : RoomRequirement_ThingAnyOf
	{
		public override IEnumerable<string> ConfigErrors()
        {
            foreach (string str in base.ConfigErrors()) yield return str;
            if (this.count <= 0) yield return "count must be greater than 0";
            yield break;
        }

        public override bool Met(Room r, Pawn p = null)
        {
            return this.Count(r) >= count;
        }

        public int Count(Room r)
        {
            int roomCount = 0;
            foreach (ThingDef def in this.things) roomCount += r.ThingCount(def);
            return roomCount;
        }

        public override string Label(Room r = null)
        {
            if (!this.labelKey.NullOrEmpty()) return this.labelKey.Translate().ToString() + ((r != null) ? " " + this.Count(r) + "/" + this.count : "");
            else if (this.things[0].label != null) return this.things[0].label + ((r != null) ? " " + this.Count(r) + "/" + this.count : "");
            else return GenLabel.ThingLabel(this.things[0], null, this.count) + ((r != null) ? " " + this.Count(r) + "/" + this.count : "");
        }

        public int count;
	}
}
