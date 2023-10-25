using System;
using System.Collections.Generic;

namespace Chess.Moves
{
	public class PawnMoveValidator : MoveValidator
	{
		public override void FilterMovesBasedOnPieceRules(List<IMoveData> moves)
		{
			moves.RemoveAll(move => !IsValidMove(move));
		}

		private bool IsValidMove(IMoveData move)
		{
			return move switch
			{
				StandardMoveData => IsStandardMoveValid(move),
				PawnDoubleMoveData doubleMove => IsDoubleMoveValid(doubleMove),
				PawnDiagonalMoveData => IsDiagonalMoveValid(move),
				PawnEnPassantMoveData enPassantMove => IsEnPassantMoveValid(enPassantMove),
				_ => throw new InvalidOperationException("Invalid move type.")
			};
		}

		private bool IsDoubleMoveValid(PawnDoubleMoveData move)
		{
			return !IsPositionOutOfBounds(move.TargetPosition)
				&& !IsPositionOccupiedByAnyPiece(move.TargetPosition)
				&& move.pawn.Rank == 0
				&& !IsPathBlocked(move.pawn.Position, move.TargetPosition);
		}

		private bool IsDiagonalMoveValid(IMoveData move)
		{
			return IsPositionOccupiedByAnyPiece(move.TargetPosition)
				&& !IsPositionOccupiedByOwnPiece(move.PieceToMove.Team, move.TargetPosition);
		}

		private bool IsStandardMoveValid(IMoveData move)
		{
			return !IsPositionOutOfBounds(move.TargetPosition)
				&& !IsPositionOccupiedByAnyPiece(move.TargetPosition);
		}

		private bool IsEnPassantMoveValid(PawnEnPassantMoveData enPassantMove)
		{
			bool hasValidPawnToCapture = enPassantMove.capturedPawn != null
				&& enPassantMove.capturedPawn.Team != enPassantMove.capturingPawn.Team;

			return hasValidPawnToCapture
				&& enPassantMove.capturingPawn.Rank == 3
				&& enPassantMove.capturedPawn.WasPromotedTwoRanksInThePastTurn();
		}
	}
}