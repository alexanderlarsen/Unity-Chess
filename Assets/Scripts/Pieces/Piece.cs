using Chess.Core;
using Chess.Moves;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Chess.Pieces
{
	public abstract class Piece : MonoBehaviour
	{
		[Inject]
		protected readonly BoardManager board;

		[Inject]
		private readonly CaptureZone captureZone;

		[field: SerializeField]
		public PlayerTeam Team { get; private set; }

		protected static Pawn blackPawnPromotedTwoRanksInThePastTurn;
		protected static Pawn whitePawnPromotedTwoRanksInThePastTurn;

		public bool IsCaptured { get; set; }

		public Vector3Int Position => Vector3Int.RoundToInt(transform.position);

		private void Start()
		{
			board.RegisterPiece(this);
		}

		public abstract List<IMoveData> GetMoves();

		public abstract List<IMoveData> GetMovesWithoutCheckValidation();

		public virtual void Move(Vector3Int targetPosition)
		{
			switch (Team)
			{
				case PlayerTeam.White:
				whitePawnPromotedTwoRanksInThePastTurn = null;
				break;

				case PlayerTeam.Black:
				blackPawnPromotedTwoRanksInThePastTurn = null;
				break;
			}

			StartCoroutine(AnimateMoveToPosition(targetPosition));
		}

		public void Capture()
		{
			IsCaptured = true;
			StartCoroutine(AnimateMoveToPosition(captureZone.GetAvailablePosition(this)));
		}

		public void SetTeam(PlayerTeam team)
		{
			Team = team;
		}

		private IEnumerator AnimateMoveToPosition(Vector3Int targetPosition)
		{
			Vector3 startPosition = transform.position;
			float duration = 0.2f;
			float elapsedTime = 0;

			while (elapsedTime < duration)
			{
				transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
				elapsedTime += Time.deltaTime;
				yield return null;
			}

			transform.position = targetPosition;
		}
	}
}