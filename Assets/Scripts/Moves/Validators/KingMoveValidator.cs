using Chess.Pieces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess.Moves
{
	public class KingMoveValidator : MoveValidator
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
				KingCastleMoveData castleMove => IsCastleMoveValid(castleMove),
				_ => throw new InvalidOperationException("Invalid move type.")
			};
		}

		private bool IsStandardMoveValid(IMoveData move)
		{
			return !IsPositionOutOfBounds(move.TargetPosition)
				&& !IsPositionOccupiedByOwnPiece(move.PieceToMove.Team, move.TargetPosition);
		}

		private bool IsCastleMoveValid(KingCastleMoveData castleMove)
		{
			/* Rules for castling:
			 * 1. You cannot castle if you have moved your king - or the rook.
			 * 2. You are not allowed to castle out of check.
			 * 3. You are not allowed to castle through check.
			 * 4. No pieces can be between the king and the rook. */

			King king = castleMove.king;
			Rook rook = castleMove.rook;
			bool isKingCurrentlyInCheck = board.IsPositionReachableByOpponent(king.Position);

			if (isKingCurrentlyInCheck || king.HasMoved || rook.HasMoved || IsPathBlocked(king.Position, rook.Position))
				return false;

			Vector3Int direction = rook.Position.x < king.Position.x ? Vector3Int.left : Vector3Int.right;

			for (int i = 1; i <= 2; i++)
				if (WillKingBeInCheckAfterMove(king, king.Position + direction * i))
					return false;

			return true;
		}
	}
}