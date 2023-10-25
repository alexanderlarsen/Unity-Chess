using Chess.Moves;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Chess.Pieces
{
	public class Queen : Piece
	{
		[Inject]
		private readonly MoveValidator moveValidator;

		private readonly Vector3Int[] moveDirections =
		{
		new (1, 0, 1),
		new (-1, 0, 1),
		new (-1, 0, -1),
		new (1, 0, -1),
		new (1, 0, 0),
		new (-1, 0, 0),
		new (0, 0, 1),
		new (0, 0, -1)
	};

		public override List<IMoveData> GetMoves()
		{
			List<IMoveData> moves = GetPotentialMoves();
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