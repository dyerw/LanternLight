using System;

namespace Pathfinding
{
	public class SearchPathNode : IPathNode<Object>
	{
		public bool Pathable { get; set;}
		public SearchPathNode(Tile tile) {
			Pathable = tile.GetType == Tile.TileType.Empty;
		}

		public bool IsWalkable(Object unused) {
			return Pathable;
		}
	}
}

