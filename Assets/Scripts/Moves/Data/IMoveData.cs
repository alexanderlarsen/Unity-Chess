using Chess.Pieces;
using UnityEngine;

namespace Chess.Moves
{
	public interface IMoveData
	{
		public Piece PieceToMove { get; }
		public Vector3Int TargetPosition { get; }
	}
}