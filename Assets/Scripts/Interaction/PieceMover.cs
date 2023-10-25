using Chess.Core;
using Chess.Moves;
using Chess.Pieces;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Chess.Interaction
{
	public class PieceMover : MonoBehaviour
	{
		[Inject]
		private readonly GameManager turnManager;

		[Inject]
		private readonly TileSelector tileSelector;

		[Inject]
		private readonly BoardManager board;

		private Piece selectedPiece;

		public event Action OnCaptureOccurred;

		private void Awake()
		{
			tileSelector.OnTileClicked += TileSelector_OnTileClicked;
		}

		private void OnDestroy()
		{
			tileSelector.OnTileClicked -= TileSelector_OnTileClicked;
		}

		private void TileSelector_OnTileClicked(Vector3Int tilePosition)
		{
			bool selectedPieceWasMoved = false;
			IMoveData moveData = GetMoveData(tilePosition);

			if (moveData != null)
			{
				if (board.TryCaptureAtPosition(tilePosition))
					OnCaptureOccurred?.Invoke();

				DoMove(moveData);
				selectedPieceWasMoved = true;
				turnManager.SwitchToNextTeamsTurn();
			}

			HandleSelection(tilePosition, selectedPieceWasMoved);
		}

		private IMoveData GetMoveData(Vector3Int tilePosition)
		{
			IMoveData move = null;

			if (selectedPiece != null)
				move = selectedPiece.GetMoves().Where(m => m.TargetPosition == tilePosition).FirstOrDefault();

			return move;
		}

		private void DoMove(IMoveData move)
		{
			if (move is KingCastleMoveData castleMove)
			{
				board.UpdatePiecePosition(castleMove.rook, castleMove.rookTargetPosition);
				board.UpdatePiecePosition(castleMove.king, castleMove.kingTargetPosition);
				castleMove.king.Move(castleMove.kingTargetPosition);
				castleMove.rook.Move(castleMove.rookTargetPosition);
			}
			else if (move is PawnEnPassantMoveData enPassantMove)
			{
				board.TryCaptureAtPosition(enPassantMove.capturedPawn.Position);
				board.UpdatePiecePosition(enPassantMove.capturingPawn, enPassantMove.capturingPawnTargetPosition);
				enPassantMove.capturingPawn.Move(enPassantMove.capturingPawnTargetPosition);
			}
			else
			{
				board.UpdatePiecePosition(move.PieceToMove, move.TargetPosition);
				move.PieceToMove.Move(move.TargetPosition);
			}
		}

		private void HandleSelection(Vector3Int tilePosition, bool selectedPieceWasMoved)
		{
			selectedPiece = selectedPieceWasMoved ? null : board.GetPieceAtPosition(tilePosition);

			if (selectedPiece != null
				&& !turnManager.IsCurrentActiveTeam(selectedPiece.Team))
				selectedPiece = null;
		}
	}
}