using Chess.Core;
using Chess.UI;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Chess.Interaction
{
	public class CameraController : MonoBehaviour
	{
		[Inject]
		private readonly GameManager gameManager;

		[Inject]
		private readonly PawnPromotionPanelHandler pawnPromotionPanel;

		[SerializeField]
		private RotationMode rotationMode;

		[SerializeField]
		private float rotationDuration = 1;

		[SerializeField]
		private float delayBeforeRotation = 0.75f;

		public bool IsRotating { get; private set; }

		public enum RotationMode
		{
			RotateToActiveTeam,
			FocusOnWhite,
			FocusOnBlack
		}

		private void Awake()
		{
			gameManager.OnSwitchTurn += TurnManager_OnSwitchTurn;
		}

		private void OnDestroy()
		{
			gameManager.OnSwitchTurn -= TurnManager_OnSwitchTurn;
		}

		private void OnValidate()
		{
			if (!Application.isPlaying)
				return;

			switch (rotationMode)
			{
				case RotationMode.RotateToActiveTeam:
				if (gameManager != null)
					transform.rotation = Quaternion.Euler(0, gameManager.IsCurrentActiveTeam(PlayerTeam.White) ? 0 : 180, 0);
				break;

				case RotationMode.FocusOnWhite:
				transform.rotation = Quaternion.Euler(0, 0, 0);
				break;

				case RotationMode.FocusOnBlack:
				transform.rotation = Quaternion.Euler(0, 180, 0);
				break;
			}
		}

		private IEnumerator SmoothRotateToActiveTeam(Quaternion targetRotation)
		{
			yield return new WaitUntil(() => !pawnPromotionPanel.IsPanelOpen);

			IsRotating = true;

			yield return new WaitForSeconds(delayBeforeRotation);

			Quaternion initialRotation = transform.rotation;
			float elapsedTime = 0f;

			while (elapsedTime < rotationDuration)
			{
				//yield return new WaitUntil(() => !pausePanelHandler.IsGamePaused);
				elapsedTime += Time.deltaTime;
				transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotationDuration);
				yield return null;
			}

			transform.rotation = targetRotation;

			IsRotating = false;
		}

		private void TurnManager_OnSwitchTurn(PlayerTeam newTurnTakingTeam)
		{
			if (rotationMode != RotationMode.RotateToActiveTeam) return;

			switch (newTurnTakingTeam)
			{
				case PlayerTeam.White:
				StartCoroutine(SmoothRotateToActiveTeam(Quaternion.Euler(0, 0, 0)));
				break;

				case PlayerTeam.Black:
				StartCoroutine(SmoothRotateToActiveTeam(Quaternion.Euler(0, 180, 0)));
				break;
			}
		}
	}
}