using System;

namespace Pathfinding
{
	public class SearchPathNode : IPathNode<Object>
	{
		public bool Pathable { get; set;}
		public SearchPathNode(Tile tile) {
			Pathable = tile.Type == Tile.TileType.Empty;
		}

		public bool IsWalkable(Object unused) {
			return Pathable;
		}
	}
}

