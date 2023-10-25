using Chess.Pieces;
using UnityEngine;

namespace Chess.Core
{
	public class PieceSpawner : MonoBehaviour
	{
		[SerializeField]
		private GameObject kingPrefab;

		[SerializeField]
		private GameObject queenPrefab;

		[SerializeField]
		private GameObject rookPrefab;

		[SerializeField]
		private GameObject knightPrefab;

		[SerializeField]
		private GameObject bishopPrefab;

		[SerializeField]
		private GameObject pawnPrefab;

		[SerializeField]
		private Transform whitePiecesParent;

		[SerializeField]
		private Transform blackPiecesParent;

		[SerializeField]
		private Material blackMaterial;

		[SerializeField]
		private Material whiteMaterial;

		public void SpawnPiece<T>(Vector3Int position, PlayerTeam team) where T : Piece
		{
			GameObject prefab;

			if (typeof(T) == typeof(King))
				prefab = kingPrefab;
			else if (typeof(T) == typeof(Queen))
				prefab = queenPrefab;
			else if (typeof(T) == typeof(Rook))
				prefab = rookPrefab;
			else if (typeof(T) == typeof(Bishop))
				prefab = bishopPrefab;
			else if (typeof(T) == typeof(Knight))
				prefab = knightPrefab;
			else if (typeof(T) == typeof(Pawn))
				prefab = pawnPrefab;
			else
			{
				Debug.LogError("Invalid piece type.");
				return;
			}

			Transform parent = team == PlayerTeam.White ? whitePiecesParent : blackPiecesParent;
			Material material = team == PlayerTeam.White ? whiteMaterial : blackMaterial;
			Quaternion rotation = Quaternion.Euler(0, team == PlayerTeam.White ? 0 : 180, 0);
			GameObject instance = Instantiate(prefab, position, rotation, parent);
			instance.GetComponentInChildren<MeshRenderer>().material = material;
			instance.GetComponent<Piece>().SetTeam(team);
		}
	}
}