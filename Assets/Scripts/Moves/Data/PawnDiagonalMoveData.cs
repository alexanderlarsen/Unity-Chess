using Chess.Pieces;
using UnityEngine;

namespace Chess.Moves
{
	public class PawnDiagonalMoveData : IMoveData
	{
		public readonly Pawn pawn;
		public readonly Vector3Int targetPosition;

		public PawnDiagonalMoveData(Pawn pawn, Vector3Int targetPosition)
		{
			this.pawn = pawn;
			this.targetPosition = targetPosition;
		}

		public Piece PieceToMove => pawn;
		public Vector3Int TargetPosition => targetPosition;
	}
}