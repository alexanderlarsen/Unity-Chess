using Chess.Pieces;
using UnityEngine;

namespace Chess.Moves
{
	public class KingCastleMoveData : IMoveData
	{
		public readonly King king;
		public readonly Rook rook;
		public readonly Vector3Int kingTargetPosition;
		public readonly Vector3Int rookTargetPosition;

		public KingCastleMoveData(King king, Rook rook, Vector3Int kingTargetPosition, Vector3Int rookTargetPosition)
		{
			this.king = king;
			this.rook = rook;
			this.kingTargetPosition = kingTargetPosition;
			this.rookTargetPosition = rookTargetPosition;
		}

		public Piece PieceToMove => king;
		public Vector3Int TargetPosition => kingTargetPosition;
	}
}