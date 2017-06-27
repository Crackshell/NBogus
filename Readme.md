# NBogus

C# library for generating levels in Serious Sam's Bogus Detour.

## Example

```c#
var writer = new LevelWriter();

// Fill the level with grass
using (var tileWriter = writer.BeginTiles("tilesets/garden_grass.tileset")) {
  for (int y = -50; y < 50; y++) {
    for (int x = -50; x < 50; x++) {
      // Add grass to grid tile position
      tileWriter.Set(x, y);
    }
  }
}

// Add a statue on pixel position -10, -10
writer.PlaceDoodad(-10, -10, "doodads/generic/statue_anatolia_s.unit");

writer.Save("example.lvl");
```
