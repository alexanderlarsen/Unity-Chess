using Chess.Core;
using Chess.Moves;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Chess.Pieces
{
	public class Pawn : Piece
	{
		[Inject]
		private readonly MoveValidator moveValidator;

		[Inject]
		private readonly Chess.UI.PawnPromotionPanelHandler pawnPromotionPanelHandler;

		[Inject]
		private readonly PieceSpawner pieceSpawner;

		public int Rank { get; private set; } = 0;

		public override List<IMoveData> GetMoves()
		{
			List<IMoveData> moves = GetPotentialMoves();
			moves.AddRange(GetPotentialEnPassantMoves());
			moveValidator.FilterMovesBasedOnPieceRules(moves);
			moveValidator.FilterOutMovesThatExposeKingToCheck(moves);
			return moves;
		}

		public override List<IMoveData> GetMovesWithoutCheckValidation()
		{
			List<IMoveData> moves = GetPotentialMoves();
			moveValidator.FilterMovesBasedOnPieceRules(moves);
			return moves;
		}

		public Vector3Int GetMoveDirection()
		{
			return Team switch
			{
				PlayerTeam.White => Vector3Int.forward,
				PlayerTeam.Black => Vector3Int.back,
				_ => throw new InvalidOperationException("Invalid team")
			};
		}

		public bool WasPromotedTwoRanksInThePastTurn()
		{
			return Team switch
			{
				PlayerTeam.White => whitePawnPromotedTwoRanksInThePastTurn != null && whitePawnPromotedTwoRanksInThePastTurn == this,
				PlayerTeam.Black => blackPawnPromotedTwoRanksInThePastTurn != null && blackPawnPromotedTwoRanksInThePastTurn == this,
				_ => throw new InvalidOperationException("Invalid team."),
			};
		}

		public override void Move(Vector3Int targetPosition)
		{
			base.Move(targetPosition);

			int movedRanks = Mathf.Abs(targetPosition.z - Position.z);
			Rank += movedRanks;

			if (movedRanks == 2)
			{
				switch (Team)
				{
					case PlayerTeam.White:
					whitePawnPromotedTwoRanksInThePastTurn = this;
					break;

					case PlayerTeam.Black:
					blackPawnPromotedTwoRanksInThePastTurn = this;
					break;
				}
			}

			if (Rank == 6)
				pawnPromotionPanelHandler.OpenPanel(this);
		}

		public void PromoteTo<T>() where T : Piece
		{
			Vector3Int position = Position;
			PlayerTeam team = Team;
			board.RemovePieceAtPosition(position);
			pieceSpawner.SpawnPiece<T>(position, team);
			Destroy(gameObject);
		}

		private List<IMoveData> GetPotentialMoves()
		{
			List<IMoveData> moves = new();
			moves.AddRange(GetForwardMoves());
			moves.AddRange(GetDiagonalMoves());
			return moves;
		}

		private List<IMoveData> GetForwardMoves()
		{
			List<IMoveData> moves = new();
			Vector3Int direction = GetMoveDirection();
			moves.Add(new StandardMoveData(this, Position + new Vector3Int(0, 0, 1) * direction));
			moves.Add(new PawnDoubleMoveData(this, Position + new Vector3Int(0, 0, 2) * direction));
			return moves;
		}

		private List<IMoveData> GetDiagonalMoves()
		{
			List<IMoveData> moves = new();
			Vector3Int direction = GetMoveDirection();
			moves.Add(new PawnDiagonalMoveData(this, Position + new Vector3Int(-1, 0, direction.z)));
			moves.Add(new PawnDiagonalMoveData(this, Position + new Vector3Int(1, 0, direction.z)));
			return moves;
		}

		private List<IMoveData> GetPotentialEnPassantMoves()
		{
			List<IMoveData> moves = new();
			Piece leftPiece = board.GetPieceAtPosition(Position + Vector3Int.left);
			Piece rightPiece = board.GetPieceAtPosition(Position + Vector3Int.right);

			if (leftPiece != null && leftPiece is Pawn leftPawn)
				moves.Add(new PawnEnPassantMoveData(this, leftPawn, Position + new Vector3Int(-1, 0, GetMoveDirection().z)));

			if (rightPiece != null && rightPiece is Pawn rightPawn)
				moves.Add(new PawnEnPassantMoveData(this, rightPawn, Position + new Vector3Int(1, 0, GetMoveDirection().z)));

			return moves;
		}
	}
}