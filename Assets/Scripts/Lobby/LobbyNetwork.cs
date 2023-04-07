using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class LobbyNetwork : Photon.PunBehaviour 
	{

		#region Photon.PunBehaviour CallBacks

		public override void OnConnectedToMaster()
		{
			Debug.Log("LobbyNetwork : OnConnectedToMaster() was called by PUN");
			PhotonNetwork.automaticallySyncScene = false;
			PhotonNetwork.JoinLobby(TypedLobby.Default);
		}

		public override void OnDisconnectedFromPhoton()
		{
			Debug.LogWarning("LobbyNetwork : OnDisconnectedFromPhoton() was called by PUN");
			PhotonNetwork.LoadLevel("Main");
		}

		public override void OnJoinedLobby()
		{
			Debug.Log("LobbyNetwork : Joined Lobby");

			if (!PhotonNetwork.inRoom)
			{
				MainCanvasManager.Instance.lobbyCanvas.transform.SetAsLastSibling();
			}
		}

		#endregion
	}
}
