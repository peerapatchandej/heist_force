using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class MainCanvasManager : MonoBehaviour 
	{
		#region Public Veriables

		public static MainCanvasManager Instance;
		public LobbyCanvas lobbyCanvas;
		public CurrentRoomCanvas currentRoomCanvas;

		#endregion

		#region Monobehaviour

		private void Awake()
		{
			Instance = this;
		}

		#endregion
	}
}
