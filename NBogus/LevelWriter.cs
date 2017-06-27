using A000FF.Units;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A000FF
{
	public class LevelWriter
	{
		internal static Random Random = new Random();

		public string LightingEnvironment { get; set; }
		public Dictionary<string, UnitList> Units { get; set; }
		public List<WorldScript> WorldScripts { get; set; }

		private Dictionary<uint, TileNode> _leaves = new Dictionary<uint, TileNode>();
		private int _unitID = 1;

		public LevelWriter()
		{
			LightingEnvironment = "system/default.env";
			Units = new Dictionary<string, UnitList>();
			WorldScripts = new List<WorldScript>();
		}

		private uint GetNodeKey(short x, short y)
		{
			return (uint)(((ushort)y << 16) | (ushort)x);
		}

		private int Round(float f)
		{
			if (f < 0) {
				if (f % 1.0f == -0.5f) {
					return (int)Math.Ceiling(f);
				}
				return (int)Math.Round(f);
			}
			return (int)Math.Round(f, MidpointRounding.AwayFromZero);
		}

		internal int GetNextUnitID()
		{
			return _unitID++;
		}

		public TileNode GetNodeAtPoint(int x, int y, int size)
		{
			short lx = (short)Round(x / (float)size);
			short ly = (short)Round(y / (float)size);
			uint key = GetNodeKey(lx, ly);

			TileNode ret = null;
			if (!_leaves.TryGetValue(key, out ret)) {
				ret = new TileNode(lx, ly);
				_leaves[key] = ret;
			}
			return ret;
		}

		public TileWriter BeginTiles(string tileset, int pixelSize = 32, bool borders = true)
		{
			return new TileWriter(this, tileset, pixelSize, borders);
		}

		public Doodad PlaceDoodad(int pixelX, int pixelY, string filename)
		{
			UnitList list = null;
			if (!Units.TryGetValue(filename, out list)) {
				list = new UnitList(filename);
				Units[filename] = list;
			}
			var ret = new Doodad(pixelX, pixelY);
			list.Add(ret);
			return ret;
		}

		public WorldScript PlaceWorldscript(int pixelX, int pixelY, string className)
		{
			var ret = new WorldScript(pixelX, pixelY, className);
			WorldScripts.Add(ret);
			return ret;
		}

		public void Save(string filename)
		{
			if (File.Exists(filename)) {
				File.Delete(filename);
			}

			using (var writer = new StreamWriter(filename)) {
				writer.WriteLine("<dict>");

				writer.WriteLine("  <int name=\"version\">1</int>");

				writer.WriteLine("  <dict name=\"lighting\">");
				writer.WriteLine("    <string name=\"environment\">{0}</string>", LightingEnvironment);
				writer.WriteLine("  </dict>");

				writer.WriteLine("  <dict name=\"game-mode\">");
				/*
				<string name="Class">Deathmatch</string>
				<string name="MapName">hi</string>
				<string name="CreatorName">me</string>
				*/
				writer.WriteLine("  </dict>");

				writer.WriteLine("  <array name=\"tiles\">");
				foreach (var node in _leaves.OrderBy(k => k.Key)) {
					node.Value.WriteToStream(writer);
				}
				writer.WriteLine("  </array>");

				writer.WriteLine("  <dict name=\"units\">");
				foreach (var list in Units.Values) {
					writer.Write("    ");
					list.WriteToStream(writer, this);
				}
				writer.WriteLine("  </dict>");

				writer.WriteLine("  <array name=\"scripts\">");
				foreach (var ws in WorldScripts) {
					writer.Write("    ");
					ws.WriteToStream(writer, GetNextUnitID());
				}
				writer.WriteLine("  </array>");

				writer.WriteLine("</dict>");
			}
		}
	}
}
