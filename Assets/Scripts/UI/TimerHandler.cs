using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace Chess.UI
{
	public class TimerHandler : MonoBehaviour
	{
		[Inject]
		private readonly PausePanelHandler pausePanelHandler;

		private Coroutine updateElapsedTimeCoroutine;
		private TextMeshProUGUI timerTmp;
		private double elapsedTime = 0f;

		private void Awake()
		{
			timerTmp = GetComponent<TextMeshProUGUI>();
			pausePanelHandler.OnGamePaused += PausePanelHandler_OnGamePaused;
		}

		private void OnDestroy()
		{
			pausePanelHandler.OnGamePaused -= PausePanelHandler_OnGamePaused;
		}

		private void Start()
		{
			StartTimer();
		}

		public void StartTimer()
		{
			updateElapsedTimeCoroutine = StartCoroutine(UpdateElapsedTime());
		}

		public void PauseTimer()
		{
			StopCoroutine(updateElapsedTimeCoroutine);
		}

		private IEnumerator UpdateElapsedTime()
		{
			while (true)
			{
				yield return new WaitForEndOfFrame();

				elapsedTime += Time.deltaTime;
				int hours = (int)(elapsedTime / 3600);
				int minutes = (int)(elapsedTime % 3600 / 60);
				int seconds = (int)(elapsedTime % 60);
				timerTmp.text = $"{hours:00}:{minutes:00}:{seconds:00}";
			}
		}

		private void PausePanelHandler_OnGamePaused(bool isPaused)
		{
			if (isPaused)
				PauseTimer();
			else
				StartTimer();
		}
	}
}