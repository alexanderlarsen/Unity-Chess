using Chess.Pieces;
using UnityEngine;

namespace Chess.Moves
{
	public class PawnEnPassantMoveData : IMoveData
	{
		public readonly Pawn capturingPawn;
		public readonly Pawn capturedPawn;
		public readonly Vector3Int capturingPawnTargetPosition;

		public PawnEnPassantMoveData(
			Pawn capturingPawn,
			Pawn capturedPawn,
			Vector3Int capturingPawnTargetPosition)
		{
			this.capturingPawn = capturingPawn;
			this.capturedPawn = capturedPawn;
			this.capturingPawnTargetPosition = capturingPawnTargetPosition;
		}

		public Piece PieceToMove => capturingPawn;
		public Vector3Int TargetPosition => capturingPawnTargetPosition;
	}
}