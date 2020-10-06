using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VERP
{
    public class RoomRequirement_ThingAnyOfCount : RoomRequirement_ThingCount
	{
		public override bool Met(Room r, Pawn p = null)
		{
			this.thingDef = this.things[0];
			if (this.Count(r) >= this.count) return true;
			else return false;
		}

        public override IEnumerable<string> ConfigErrors()
        {
			if (this.things.NullOrEmpty())
			{
				yield return "things is null/empty";
			}
			yield break;
		}

		public new int Count(Room r)
        {
			int _count = 0;
			foreach (ThingDef def in this.things)
			{
				_count += r.ThingCount(def);
			}
			return _count;
		}

		public override string Label(Room r = null)
		{
			if (r != null && this.things?[0] != null) return this.things[0].label + " " + this.Count(r) + "/" + this.count;
			else return " ";
		}

		public List<ThingDef> things;
	}
}
