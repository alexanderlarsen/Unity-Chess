using Chess.Pieces;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Chess.Core
{
	public class GameManager : MonoBehaviour
	{
		[Inject]
		private readonly BoardManager board;

		public event Action<PlayerTeam> OnSwitchTurn;

		public Action<GameOverEventArgs> OnGameOver;

		public PlayerTeam CurrentTurnTakingTeam { get; private set; }
		public PlayerTeam OpponentTeam { get; private set; }

		public bool IsGameOver { get; private set; }

		public bool IsCurrentActiveTeam(PlayerTeam team)
		{
			return CurrentTurnTakingTeam == team;
		}

		public void SwitchToNextTeamsTurn()
		{
			if (IsCurrentActiveTeam(PlayerTeam.White))
			{
				CurrentTurnTakingTeam = PlayerTeam.Black;
				OpponentTeam = PlayerTeam.White;
			}
			else if (IsCurrentActiveTeam(PlayerTeam.Black))
			{
				CurrentTurnTakingTeam = PlayerTeam.White;
				OpponentTeam = PlayerTeam.Black;
			}

			OnSwitchTurn?.Invoke(CurrentTurnTakingTeam);
			CheckIfGameOver();
		}

		public void CheckIfGameOver()
		{
			StartCoroutine(CheckIfGameOverDelayed());
		}

		private IEnumerator CheckIfGameOverDelayed()
		{
			yield return new WaitForSeconds(0.3f);

			int moveCount = board.GetValidMoveCountForTeam(CurrentTurnTakingTeam);
			bool isOutOfMoves = moveCount == 0;

			Debug.Log($"{CurrentTurnTakingTeam} moves: {moveCount}");

			if (isOutOfMoves)
			{
				IsGameOver = true;

				Vector3Int kingPosition = board.GetPieceByType<King>(CurrentTurnTakingTeam).Position;
				bool isKingInCheck = board.IsPositionReachableByOpponent(kingPosition);

				GameOverEventArgs.GameOverType gameOverType = isKingInCheck
					? GameOverEventArgs.GameOverType.Checkmate
					: GameOverEventArgs.GameOverType.Draw;

				OnGameOver?.Invoke(new GameOverEventArgs(gameOverType, OpponentTeam));
			}
		}

		public class GameOverEventArgs
		{
			public readonly GameOverType gameOverType;
			public readonly PlayerTeam winningTeam;

			public GameOverEventArgs(GameOverType gameOverType, PlayerTeam winningTeam)
			{
				this.gameOverType = gameOverType;
				this.winningTeam = winningTeam;
			}

			public enum GameOverType
			{
				Checkmate,
				Draw
			}
		}
	}
}