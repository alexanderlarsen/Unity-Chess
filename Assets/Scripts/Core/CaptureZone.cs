using Chess.Pieces;
using UnityEngine;

namespace Chess.Core
{
	public class CaptureZone
	{
		private int whiteZ = 0;
		private int blackZ = 0;

		public Vector3Int GetAvailablePosition(Piece piece)
		{
			Vector3Int output;

			switch (piece.Team)
			{
				case PlayerTeam.White:
				int whiteX = whiteZ < 8 ? -2 : -3;
				output = new Vector3Int(whiteX, 0, whiteZ % 8);
				whiteZ++;
				break;

				case PlayerTeam.Black:
				int blackX = blackZ < 8 ? 9 : 10;
				output = new Vector3Int(blackX, 0, 7 - (blackZ % 8));
				blackZ++;
				break;

				default:
				throw new System.InvalidOperationException("Invalid team");
			}

			return output;
		}
	}
}