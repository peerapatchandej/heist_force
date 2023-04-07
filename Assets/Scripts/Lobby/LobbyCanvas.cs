using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class LobbyCanvas : Photon.PunBehaviour
	{
		#region Private Variables

		[SerializeField]
		private RoomLayoutGroup roomLayoutGroup;

		[SerializeField]
		private GameObject levelButton;

		[SerializeField]
		private GameObject lobbyButtons;

		[SerializeField]
		private GameObject clientLeaveRoomBtn;

		#endregion

		#region MonoBehaviour Callback

		#endregion

		#region Public Methodes

		public void OnClickJoinRoom(string roomName)
		{
			if(PhotonNetwork.JoinRoom(roomName))
			{
				Debug.Log("Join room successfully.");
			}
			else
			{
				Debug.Log("Join room failed.");
			}
		}

		#endregion

		#region Photon.PunBehaviour Callback
		
		public override void OnJoinedRoom()
		{
			if(!PhotonNetwork.isMasterClient)
			{
				levelButton.SetActive(false);
				lobbyButtons.SetActive(false);
				clientLeaveRoomBtn.SetActive(true);
			}

			photonView.RPC("RPC_GetCurrentLevel", PhotonTargets.MasterClient);
		}

		#endregion

		#region RPC Methods

		[PunRPC]
		private void RPC_GetCurrentLevel()
		{
			photonView.RPC("RPC_UpdateLevel", PhotonTargets.All, GameManager.instance.levelName);
		}

		[PunRPC]
		private void RPC_UpdateLevel(string levelName)
		{
			GameManager.instance.levelName = levelName;
			CurrentRoomCanvas.instance.levelChange = true;
		}

		#endregion
	}
}