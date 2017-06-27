using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A000FF.Units
{
	public class WorldScript : Unit
	{
		public string ClassName { get; set; }

		public bool Enabled { get; set; }
		public int TriggerTimes { get; set; }
		public bool ExecuteOnStartup { get; set; }

		public Dictionary<string, SValue> Parameters { get; set; }

		public WorldScript(int x, int y, string className)
			: base(x, y)
		{
			ClassName = className;
			Enabled = true;
			TriggerTimes = -1;

			Parameters = new Dictionary<string, SValue>();
		}

		internal override void WriteToStream(StreamWriter writer, int id)
		{
			writer.Write("<array>");
			writer.Write("<string>{0}</string>", ClassName);
			writer.Write("<int>{0}</int>", id);
			writer.Write("<vec3>{0} {1} 0</vec3>", X, Y);
			writer.Write("<bool>{0}</bool>", Enabled ? "t" : "f");
			writer.Write("<int>{0}</int>", TriggerTimes);
			writer.Write("<bool>{0}</bool>", ExecuteOnStartup ? "t" : "f");
			if (Parameters.Count > 0) {
				writer.Write("<dict>");
				foreach (var pair in Parameters) {
					pair.Value.WriteToStream(writer, pair.Key);
				}
				writer.Write("</dict>");
			}
			writer.WriteLine("</array>");
		}
	}
}
