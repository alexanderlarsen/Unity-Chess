using Chess.Core;
using Chess.UI;
using System;
using UnityEngine;
using Zenject;

namespace Chess.Interaction
{
	public class TileSelector : MonoBehaviour
	{
		[Inject]
		private readonly GameManager gameManager;

		[Inject]
		private readonly CameraController cameraController;

		[Inject]
		private readonly PausePanelHandler pausePanelHandler;

		[Inject]
		private readonly PawnPromotionPanelHandler pawnPromotionPanel;

		private Camera mainCamera;
		private LayerMask boardTilesMask;

		public event Action<Vector3Int> OnTileClicked;

		private void Awake()
		{
			mainCamera = Camera.main;
			boardTilesMask = LayerMask.GetMask("BoardTiles");
		}

		private void Update()
		{
			if (CanPerformSelection()
			&& Input.GetMouseButtonDown(0)
			&& Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, boardTilesMask))
			{
				OnTileClicked?.Invoke(Vector3Int.RoundToInt(hit.collider.gameObject.transform.position));
			}
		}

		private bool CanPerformSelection()
		{
			return !gameManager.IsGameOver
			&& !cameraController.IsRotating
			&& !pawnPromotionPanel.IsPanelOpen
			&& !pausePanelHandler.IsGamePaused;
		}
	}
}