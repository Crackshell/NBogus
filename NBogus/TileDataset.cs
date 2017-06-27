using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A000FF
{
	public class TileDataset
	{
		public string Tileset { get; set; }
		public int GridSize { get; set; }
		public byte[][] Tiles { get; set; }

		public TileDataset(string tileset, int pixelSize)
		{
			Tileset = tileset;
			GridSize = 512 / pixelSize;
			Tiles = new byte[GridSize][];
			for (int i = 0; i < GridSize; i++) {
				Tiles[i] = new byte[GridSize];
			}
		}

		public byte Get(int x, int y)
		{
			if (x < 0 || x >= GridSize) {
				return 0;
			}
			if (y < 0 || y >= GridSize) {
				return 0;
			}
			return Tiles[y][x];
		}

		public void Set(int x, int y, bool borders)
		{
			byte v = (byte)LevelWriter.Random.Next(0x01, 0x7F);
			if (!borders) {
				v |= 0x80;
			}
			Set(x, y, v);
		}

		public void Set(int x, int y, byte v)
		{
			x += GridSize / 2;
			y += GridSize / 2;
			if (x < 0 || x >= GridSize) {
				return;
			}
			if (y < 0 || y >= GridSize) {
				return;
			}
			Tiles[y][x] = v;
		}

		private string EncodeTiles()
		{
			var sb = new StringBuilder();
			for (int y = 0; y < GridSize; y++) {
				for (int x = 0; x < GridSize; x++) {
					sb.Append(Tiles[y][x].ToString("x2"));
				}
			}
			return sb.ToString();
		}

		internal void WriteToStream(StreamWriter writer)
		{
			writer.WriteLine("        <dict>");
			writer.WriteLine("          <string name=\"tileset\">{0}</string>", Tileset);
			writer.WriteLine("          <bytes name=\"data\">{0}</bytes>", EncodeTiles());
			writer.WriteLine("        </dict>");
		}
	}
}
