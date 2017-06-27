using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A000FF
{
	public enum SValueType
	{
		String,
		Int,
		Float,
		Bool,
		Dict
	}

	public class SValue
	{
		public SValueType Type { get; set; }

		public string String { get; set; }
		public int Int { get; set; }
		public float Float { get; set; }
		public bool Bool { get; set; }
		public Dictionary<string, SValue> Dict { get; set; }

		public SValue(SValueType type)
		{
			Type = type;
			Dict = new Dictionary<string, SValue>();
		}

		public string EncodeValue()
		{
			switch (Type) {
				case SValueType.String: return String;
				case SValueType.Int: return Int.ToString(CultureInfo.InvariantCulture.NumberFormat);
				case SValueType.Float: return Float.ToString(CultureInfo.InvariantCulture.NumberFormat);
				case SValueType.Bool: return Bool ? "t" : "f";
			}
			return "";
		}

		internal void WriteToStream(StreamWriter writer, string key = null)
		{
			string tagName = "";
			switch (Type) {
				case SValueType.String: tagName = "string"; break;
				case SValueType.Int: tagName = "int"; break;
				case SValueType.Float: tagName = "float"; break;
				case SValueType.Bool: tagName = "bool"; break;
				case SValueType.Dict: tagName = "dict"; break;
			}

			writer.Write("<{0}", tagName);
			if (key != null) {
				writer.Write(" name=\"{0}\"", key);
			}
			writer.Write(">");

			if (Type == SValueType.Dict) {
				foreach (var pair in Dict) {
					pair.Value.WriteToStream(writer, pair.Key);
				}
			} else {
				writer.Write(EncodeValue());
			}

			writer.Write("</{1}>", tagName);
		}
	}
}
