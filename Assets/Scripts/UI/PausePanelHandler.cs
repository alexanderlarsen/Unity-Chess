using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Chess.UI
{
	public class PausePanelHandler : MonoBehaviour
	{
		[SerializeField]
		private GameObject panel;

		[SerializeField]
		private Button resumeGameButton;

		[SerializeField]
		private Button restartGameButton;

		[SerializeField]
		private Button quitGameButton;

		public bool IsGamePaused => panel.activeInHierarchy;

		public event Action<bool> OnGamePaused;

		private void Awake()
		{
			resumeGameButton.onClick.AddListener(ResumeGame);
			restartGameButton.onClick.AddListener(RestartGameButton_OnClick);
			quitGameButton.onClick.AddListener(QuitGameButton_OnClick);
		}

		private void OnDestroy()
		{
			resumeGameButton.onClick.AddListener(ResumeGame);
			restartGameButton.onClick.AddListener(RestartGameButton_OnClick);
			quitGameButton.onClick.AddListener(QuitGameButton_OnClick);
		}

		private void Start()
		{
			panel.SetActive(false);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (panel.activeInHierarchy)
					ResumeGame();
				else
					PauseGame();
			}
		}

		private void ResumeGame()
		{
			panel.SetActive(false);
			OnGamePaused?.Invoke(false);
		}

		private void PauseGame()
		{
			panel.SetActive(true);
			OnGamePaused?.Invoke(true);
		}

		private void RestartGameButton_OnClick()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		private void QuitGameButton_OnClick()
		{
			Application.Quit();
		}
	}
}