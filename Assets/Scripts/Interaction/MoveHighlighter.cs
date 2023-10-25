using Chess.Core;
using Chess.Moves;
using Chess.Pieces;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Chess.Interaction
{
	[DefaultExecutionOrder(-1)]
	public class MoveHighlighter : MonoBehaviour
	{
		[Inject]
		private readonly TileSelector tileSelector;

		[Inject]
		private readonly BoardManager board;

		[Inject]
		private readonly GameManager turnController;

		[Inject]
		private readonly PieceMover chessPieceMover;

		[SerializeField]
		private GameObject selectionMarkerPrefab;

		[SerializeField]
		private GameObject moveMarkerPrefab;

		private GameObject currentSelectionMarker;
		private readonly List<GameObject> currentMoveMarkers = new();

		private void Awake()
		{
			tileSelector.OnTileClicked += TileSelector_OnTileClicked;
			chessPieceMover.OnCaptureOccurred += Board_OnCaptureOccurred;
		}

		private void OnDestroy()
		{
			tileSelector.OnTileClicked -= TileSelector_OnTileClicked;
			chessPieceMover.OnCaptureOccurred -= Board_OnCaptureOccurred;
		}

		private void HighlightPieceMoves(Piece piece)
		{
			currentSelectionMarker = Instantiate(selectionMarkerPrefab, piece.Position, Quaternion.identity);

			foreach (IMoveData moveData in piece.GetMoves())
			{
				GameObject instance = Instantiate(moveMarkerPrefab, moveData.TargetPosition, Quaternion.identity);
				currentMoveMarkers.Add(instance);
			}
		}

		private void StopHighlightingMoves()
		{
			if (currentSelectionMarker != null)
				Destroy(currentSelectionMarker);

			currentSelectionMarker = null;

			foreach (GameObject instance in currentMoveMarkers)
				Destroy(instance);

			currentMoveMarkers.Clear();
		}

		private void TileSelector_OnTileClicked(Vector3Int tilePosition)
		{
			StopHighlightingMoves();

			Piece selectedPiece = board.GetPieceAtPosition(tilePosition);

			if (selectedPiece != null && !turnController.IsCurrentActiveTeam(selectedPiece.Team))
				return;

			if (selectedPiece != null)
				HighlightPieceMoves(selectedPiece);
		}

		private void Board_OnCaptureOccurred()
		{
			StopHighlightingMoves();
		}
	}
}