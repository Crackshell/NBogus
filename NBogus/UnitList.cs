using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A000FF
{
	public class UnitList : List<Unit>
	{
		public string Filename { get; set; }

		public UnitList(string filename)
		{
			Filename = filename;
		}

		internal void WriteToStream(StreamWriter writer, LevelWriter level)
		{
			writer.Write("<array name=\"{0}\">", Filename);
			foreach (var unit in this) {
				unit.WriteToStream(writer, level.GetNextUnitID());
			}
			writer.WriteLine("</array>");
		}
	}
}
