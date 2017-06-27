using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A000FF
{
	public class TileNode
	{
		public int IndexX { get; set; }
		public int IndexY { get; set; }

		public int PixelX
		{
			get { return IndexX * 512; }
		}

		public int PixelY
		{
			get { return IndexY * 512; }
		}

		public Dictionary<string, TileDataset> Datasets { get; set; }

		public TileNode(int x, int y)
		{
			IndexX = x;
			IndexY = y;

			Datasets = new Dictionary<string, TileDataset>();
		}

		public TileDataset GetDataset(string tileset, int pixelSize)
		{
			TileDataset ret = null;
			if (!Datasets.TryGetValue(tileset, out ret)) {
				ret = new TileDataset(tileset, pixelSize);
				Datasets[tileset] = ret;
			}
			return ret;
		}

		internal void WriteToStream(StreamWriter writer)
		{
			writer.WriteLine("    <dict>");
			writer.WriteLine("      <vec2 name=\"pos\">{0} {1}</vec2>", PixelX, PixelY);
			writer.WriteLine("      <array name=\"datasets\">");
			foreach (var set in Datasets) {
				set.Value.WriteToStream(writer);
			}
			writer.WriteLine("      </array>");
			writer.WriteLine("    </dict>");
		}
	}
}
