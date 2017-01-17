using System;

public class Tile {

	public enum TileType { Empty, Tree };

	public TileType Type;

	public Tile(TileType type) {
		Type = type;
	}
}
