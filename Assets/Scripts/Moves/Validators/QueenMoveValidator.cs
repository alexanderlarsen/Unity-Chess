using System.Collections.Generic;

namespace Chess.Moves
{
	public class QueenMoveValidator : MoveValidator
	{
		public override void FilterMovesBasedOnPieceRules(List<IMoveData> moves)
		{
			moves.RemoveAll(move => IsPositionOutOfBounds(move.TargetPosition)
			|| IsPathBlocked(move.PieceToMove.Position, move.TargetPosition)
			|| IsPositionOccupiedByOwnPiece(move.PieceToMove.Team, move.TargetPosition));
		}
	}
}