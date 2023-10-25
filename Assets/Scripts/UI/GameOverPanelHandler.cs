using Chess.Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Chess.UI
{
	public class GameOverPanelHandler : MonoBehaviour
	{
		[Inject]
		private readonly GameManager gameManager;

		[SerializeField]
		private GameObject panel;

		[SerializeField]
		private TextMeshProUGUI infoTextTmp;

		[SerializeField]
		private Button startNewGameButton;

		private void Awake()
		{
			gameManager.OnGameOver += GameManager_OnGameOver;
			startNewGameButton.onClick.AddListener(RestartGame);
		}

		private void OnDestroy()
		{
			gameManager.OnGameOver -= GameManager_OnGameOver;
			startNewGameButton.onClick.RemoveListener(RestartGame);
		}

		private void Start()
		{
			panel.SetActive(false);
		}

		private void RestartGame()
		{
			Scene activeScene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(activeScene.name);
		}

		private void GameManager_OnGameOver(GameManager.GameOverEventArgs args)
		{
			panel.SetActive(true);

			switch (args.gameOverType)
			{
				case GameManager.GameOverEventArgs.GameOverType.Checkmate:
				infoTextTmp.text = $"{args.winningTeam} wins by checkmate";
				break;

				case GameManager.GameOverEventArgs.GameOverType.Draw:
				infoTextTmp.text = $"The game ended in a draw";
				break;
			}
		}
	}
}