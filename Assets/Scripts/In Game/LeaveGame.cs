using UnityEngine;
using UnityEngine.SceneManagement;

namespace FenrirStudio.HeistForce
{
	public class LeaveGame : Photon.PunBehaviour 
	{
		#region Public Variables

		public static LeaveGame instance;

		#endregion

		#region MonoBehaviour Callback

		private void Awake()
		{
			instance = this;
		}

		#endregion

		#region Public Methods

		public void OnClickLeaveGame()
		{
			PhotonNetwork.LeaveRoom();
		}

		#endregion

		#region Photon.PunBehaviour Callback

		public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
		{
			PhotonNetwork.LeaveRoom();
		}

		public override void OnLeftRoom()
		{
			DestroyImmediate(GameObject.Find("Game Manager"));
            PhotonNetwork.LoadLevel("Lobby");
		}

		#endregion
	}
}
