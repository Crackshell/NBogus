using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A000FF
{
	public class TileWriter : IDisposable
	{
		public string Tileset { get; set; }

		private LevelWriter _level;
		private int _pixelSize;
		private int _size;
		private bool _borders;

		private TileNode _lastNode;
		private TileDataset _lastSet;

		public TileWriter(LevelWriter level, string tileset, int pixelSize, bool borders)
		{
			_level = level;
			Tileset = tileset;
			_pixelSize = pixelSize;
			_size = 512 / _pixelSize;
			_borders = borders;
		}

		public void Dispose()
		{
		}

		private void SetSingle(int x, int y)
		{
			var node = _level.GetNodeAtPoint(x, y, _size);
			if (_lastSet == null || _lastNode != node) {
				_lastSet = node.GetDataset(Tileset, _pixelSize);
				_lastNode = node;
			}
			_lastSet.Set(x - node.IndexX * _size, y - node.IndexY * _size, _borders);
		}

		public void Set(int x, int y)
		{
			SetSingle(x, y);
			if (_borders) {
				SetSingle(x + 1, y);
				SetSingle(x + 1, y + 1);
				SetSingle(x, y + 1);
			}
		}

		public void SetSized(int x, int y, int pixelSize)
		{
			if (pixelSize < _pixelSize) {
				throw new ArgumentException("Given pixel size is smaller than current tileset pixel size", "pixelSize");
			}

			if (pixelSize == _pixelSize) {
				Set(x, y);
				return;
			}

			int multiples = pixelSize / _pixelSize;
			x *= multiples;
			y *= multiples;

			for (int ty = 0; ty < multiples; ty++) {
				for (int tx = 0; tx < multiples; tx++) {
					Set(x + tx, y + ty);
				}
			}
		}
	}
}
