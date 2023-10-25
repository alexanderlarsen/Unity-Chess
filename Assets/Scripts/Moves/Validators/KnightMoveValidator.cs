using System.Collections.Generic;

namespace Chess.Moves
{
	public class KnightMoveValidator : MoveValidator
	{
		public override void FilterMovesBasedOnPieceRules(List<IMoveData> moves)
		{
			moves.RemoveAll(move => IsPositionOutOfBounds(move.TargetPosition)
			|| IsPositionOccupiedByOwnPiece(move.PieceToMove.Team, move.TargetPosition));
		}
	}
}