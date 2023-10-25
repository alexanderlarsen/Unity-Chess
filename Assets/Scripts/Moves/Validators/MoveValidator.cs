using Chess.Core;
using Chess.Pieces;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Chess.Moves
{
	public abstract class MoveValidator
	{
		[Inject]
		protected readonly BoardManager board;

		public abstract void FilterMovesBasedOnPieceRules(List<IMoveData> moves);

		public void FilterOutMovesThatExposeKingToCheck(List<IMoveData> moves)
		{
			moves.RemoveAll(move => WillKingBeInCheckAfterMove(move.PieceToMove, move.TargetPosition));
		}

		protected bool WillKingBeInCheckAfterMove(Piece movingPiece, Vector3Int movePosition)
		{
			Dictionary<Vector3Int, Piece> boardStateBeforeSimulation = board.GetBoardState();
			board.RemovePieceAtPosition(movingPiece.Position);
			board.RemovePieceAtPosition(movePosition);
			board.AddPieceAtPosition(movePosition, movingPiece);
			Piece king = board.GetPieceByType<King>(movingPiece.Team);
			bool isKingNowInCheck = board.IsPositionReachableByOpponent(movingPiece is King ? movePosition : king.Position);
			board.SetBoardState(boardStateBeforeSimulation);
			return isKingNowInCheck;
		}

		protected bool IsPathBlocked(Vector3Int startPos, Vector3Int endPos)
		{
			Vector3 normalizedDirection = ((Vector3)(endPos - startPos)).normalized;
			Vector3Int direction = Vector3Int.RoundToInt(normalizedDirection);

			for (int i = 1; i < Mathf.Abs(endPos.x - startPos.x) || i < Mathf.Abs(endPos.z - startPos.z); i++)
				if (board.GetPieceAtPosition(startPos + direction * i) != null)
					return true;

			return false;
		}

		protected bool IsPositionOutOfBounds(Vector3Int position)
		{
			return position.x < 0 || position.x > 7 || position.z < 0 || position.z > 7;
		}

		protected bool IsPositionOccupiedByAnyPiece(Vector3Int position)
		{
			return board.GetPieceAtPosition(position) != null;
		}

		protected bool IsPositionOccupiedByOwnPiece(PlayerTeam ownTeam, Vector3Int position)
		{
			Piece occupyingPiece = board.GetPieceAtPosition(position);
			return occupyingPiece != null && occupyingPiece.Team == ownTeam;
		}

		protected bool IsPositionReachableByOpponent(Vector3Int pos)
		{
			return board.IsPositionReachableByOpponent(pos);
		}
	}
}