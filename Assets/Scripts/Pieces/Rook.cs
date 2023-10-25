using Chess.Moves;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Chess.Pieces
{
	public class Rook : Piece
	{
		[Inject]
		private readonly MoveValidator rookMoveValidator;

		private readonly Vector3Int[] moveDirections =
		{
		new (1, 0, 0),
		new (-1, 0, 0),
		new (0, 0, 1),
		new (0, 0, -1)
	};

		public bool HasMoved { get; private set; }

		public override List<IMoveData> GetMoves()
		{
			List<IMoveData> moves = GetPotentialMoves();
			rookMoveValidator.FilterMovesBasedOnPieceRules(moves);
			rookMoveValidator.FilterOutMovesThatExposeKingToCheck(moves);
			return moves;
		}

		public override List<IMoveData> GetMovesWithoutCheckValidation()
		{
			List<IMoveData> moves = GetPotentialMoves();
			rookMoveValidator.FilterMovesBasedOnPieceRules(moves);
			return moves;
		}

		public override void Move(Vector3Int targetPosition)
		{
			base.Move(targetPosition);
			HasMoved = true;
		}

		private List<IMoveData> GetPotentialMoves()
		{
			List<IMoveData> moves = new();

			foreach (Vector3Int direction in moveDirections)
				for (int i = 0; i <= 7; i++)
					moves.Add(new StandardMoveData(this, Position + direction * i));

			return moves;
		}
	}
}