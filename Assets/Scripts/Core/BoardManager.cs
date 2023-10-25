using Chess.Moves;
using Chess.Pieces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Chess.Core
{
	public class BoardManager : MonoBehaviour
	{
		[Inject]
		private readonly GameManager turnManager;

		private Dictionary<Vector3Int, Piece> boardState = new();

		public void RegisterPiece(Piece piece)
		{
			boardState.Add(piece.Position, piece);
		}

		public Piece GetPieceAtPosition(Vector3Int position)
		{
			if (boardState.ContainsKey(position))
				return boardState[position];
			else
				return null;
		}

		public T GetPieceByType<T>(PlayerTeam team) where T : Piece
		{
			return boardState.Values.FirstOrDefault(piece => piece.Team == team && piece is T) as T;
		}

		public IEnumerable<T> GetPiecesByType<T>(PlayerTeam team) where T : Piece
		{
			return boardState.Values.Where(piece => piece.Team == team && piece is T).Cast<T>();
		}

		public IEnumerable<Piece> GetActiveOpponentPieces()
		{
			return boardState.Values.Where(piece => !piece.IsCaptured && piece.Team == turnManager.OpponentTeam);
		}

		public int GetValidMoveCountForTeam(PlayerTeam team)
		{
			return boardState.Values.ToList()
				.Where(piece => piece.Team == team && !piece.IsCaptured)
				.Sum(piece => piece.GetMoves().Count);
		}

		public Dictionary<Vector3Int, Piece> GetBoardState()
		{
			return new Dictionary<Vector3Int, Piece>(boardState);
		}

		public void SetBoardState(Dictionary<Vector3Int, Piece> state)
		{
			boardState = state;
		}

		public void AddPieceAtPosition(Vector3Int position, Piece piece)
		{
			boardState.Add(position, piece);
		}

		public void RemovePieceAtPosition(Vector3Int piecePosition)
		{
			if (boardState.ContainsKey(piecePosition))
				boardState.Remove(piecePosition);
		}

		public void UpdatePiecePosition(Piece piece, Vector3Int targetPosition)
		{
			boardState.Remove(piece.Position);
			boardState.Add(targetPosition, piece);
		}

		public bool TryCaptureAtPosition(Vector3Int tilePosition)
		{
			if (boardState.ContainsKey(tilePosition))
			{
				boardState[tilePosition].Capture();
				boardState.Remove(tilePosition);
				return true;
			}

			return false;
		}

		public bool IsPositionReachableByOpponent(Vector3Int position)
		{
			IEnumerable<Piece> opponentsPieces = GetActiveOpponentPieces();

			foreach (Piece opponent in opponentsPieces)
				foreach (IMoveData move in opponent.GetMovesWithoutCheckValidation())
					if (move.TargetPosition == position)
						return true;

			return false;
		}

		[ContextMenu(nameof(DebugLogBoardData))]
		private void DebugLogBoardData()
		{
			string[,] output = new string[8, 8];
			string boardRepresentation = "";

			for (int x = 0; x <= 7; x++)
			{
				for (int z = 0; z <= 7; z++)
				{
					string coordLetter = x switch
					{
						0 => "A",
						1 => "B",
						2 => "C",
						3 => "D",
						4 => "E",
						5 => "F",
						6 => "G",
						7 => "H"
					};

					var pos = new Vector3Int(x, 0, z);

					if (boardState.ContainsKey(pos))
					{
						string team = boardState[pos].Team == PlayerTeam.White ? "W" : "B";

						output[7 - z, x] = boardState[pos] switch
						{
							King => $"{coordLetter}{z + 1}-K-{team}",
							Queen => $"{coordLetter}{z + 1}-Q-{team}",
							Rook => $"{coordLetter}{z + 1}-R-{team}",
							Knight => $"{coordLetter}{z + 1}-k-{team}",
							Bishop => $"{coordLetter}{z + 1}-B-{team}",
							Pawn => $"{coordLetter}{z + 1}-P-{team}",
							_ => "o",
						};
					}
					else
					{
						output[7 - z, x] = $"{coordLetter}{z + 1}-0";
					}
				}
			}

			for (int z = 0; z <= 7; z++)
			{
				for (int x = 0; x <= 7; x++)
					boardRepresentation += output[z, x] + " | ";

				boardRepresentation += "\n";
			}

			Debug.Log(boardRepresentation);
		}
	}
}