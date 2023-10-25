using Chess.Pieces;
using UnityEngine;

namespace Chess.Moves
{
	public class StandardMoveData : IMoveData
	{
		public readonly Piece piece;
		public readonly Vector3Int targetPosition;

		public StandardMoveData(Piece piece, Vector3Int targetPosition)
		{
			this.piece = piece;
			this.targetPosition = targetPosition;
		}

		public Piece PieceToMove => piece;
		public Vector3Int TargetPosition => targetPosition;
	}
}