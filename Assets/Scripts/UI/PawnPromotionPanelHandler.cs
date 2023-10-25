using Chess.Core;
using Chess.Pieces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Chess.UI
{
	public class PawnPromotionPanelHandler : MonoBehaviour
	{
		[Inject]
		private readonly GameManager gameManager;

		[SerializeField]
		private GameObject panel;

		[SerializeField]
		private Button queenButton;

		[SerializeField]
		private Button rookButton;

		[SerializeField]
		private Button knightButton;

		[SerializeField]
		private Button bishopButton;

		private Pawn selectedPawn;

		private UnityAction promoteToQueenAction;
		private UnityAction promoteToRookAction;
		private UnityAction promoteToKnightAction;
		private UnityAction promoteToBishopAction;

		public bool IsPanelOpen => panel.activeInHierarchy;

		private void Awake()
		{
			promoteToQueenAction = PromoteSelectedPawnTo<Queen>;
			promoteToRookAction = PromoteSelectedPawnTo<Rook>;
			promoteToKnightAction = PromoteSelectedPawnTo<Knight>;
			promoteToBishopAction = PromoteSelectedPawnTo<Bishop>;

			queenButton.onClick.AddListener(promoteToQueenAction);
			rookButton.onClick.AddListener(promoteToRookAction);
			knightButton.onClick.AddListener(promoteToKnightAction);
			bishopButton.onClick.AddListener(promoteToBishopAction);
		}

		private void OnDestroy()
		{
			queenButton.onClick.RemoveListener(promoteToQueenAction);
			rookButton.onClick.RemoveListener(promoteToRookAction);
			knightButton.onClick.RemoveListener(promoteToKnightAction);
			bishopButton.onClick.RemoveListener(promoteToBishopAction);
		}

		private void Start()
		{
			panel.SetActive(false);
		}

		private void PromoteSelectedPawnTo<T>() where T : Piece
		{
			if (selectedPawn == null)
				return;

			selectedPawn.PromoteTo<T>();
			selectedPawn = null;
			gameManager.CheckIfGameOver();
			panel.SetActive(false);
		}

		public void OpenPanel(Pawn pawnToPromote)
		{
			selectedPawn = pawnToPromote;
			panel.SetActive(true);
		}
	}
}