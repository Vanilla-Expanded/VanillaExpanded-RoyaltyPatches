using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Verse;

namespace VERP
{
    public class PatchOperationToggable : PatchOperation
	{
#pragma warning disable 0649
		public bool enabled;
		public string label;
		public PatchOperation match;

		protected override bool ApplyWorker(XmlDocument xml)
		{
			if (this.enabled)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			return true;
		}
	}
}
