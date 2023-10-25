using Chess.Moves;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Chess.Pieces
{
	public class King : Piece
	{
		[Inject]
		private readonly MoveValidator moveValidator;

		private readonly Vector3Int[] moveOffsets =
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

		public bool HasMoved { get; private set; }

		public override List<IMoveData> GetMoves()
		{
			List<IMoveData> moves = GetPotentialBasicMoves();
			moves.AddRange(GetPotentialCastleMoves());
			moveValidator.FilterMovesBasedOnPieceRules(moves);
			moveValidator.FilterOutMovesThatExposeKingToCheck(moves);
			return moves;
		}

		public override List<IMoveData> GetMovesWithoutCheckValidation()
		{
			List<IMoveData> moves = GetPotentialBasicMoves();
			moveValidator.FilterMovesBasedOnPieceRules(moves);
			return moves;
		}

		public override void Move(Vector3Int targetPosition)
		{
			base.Move(targetPosition);
			HasMoved = true;
		}

		private List<IMoveData> GetPotentialBasicMoves()
		{
			List<IMoveData> moves = new();

			foreach (Vector3Int offset in moveOffsets)
				moves.Add(new StandardMoveData(this, Position + offset));

			return moves;
		}

		private List<KingCastleMoveData> GetPotentialCastleMoves()
		{
			List<KingCastleMoveData> moves = new();
			IEnumerable<Rook> rooks = board.GetPiecesByType<Rook>(Team);

			foreach (Rook rook in rooks)
			{
				Vector3Int direction = rook.Position.x < Position.x ? Vector3Int.left : Vector3Int.right;
				Vector3Int kingTargetPosition = Position + direction * 2;
				Vector3Int rookTargetPosition = Position + direction * 1;
				KingCastleMoveData castleMove = new(this, rook, kingTargetPosition, rookTargetPosition);
				moves.Add(castleMove);
			}

			return moves;
		}
	}
}