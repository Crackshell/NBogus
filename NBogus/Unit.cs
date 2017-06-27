using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A000FF
{
	public abstract class Unit
	{
		public int X { get; set; }
		public int Y { get; set; }

		protected Unit(int x, int y)
		{
			X = x;
			Y = y;
		}

		internal virtual void WriteToStream(StreamWriter writer, int id)
		{
			writer.Write("<vec2>{0} {1}</vec2><int>{2}</int>", X, Y, id);
		}
	}
}
