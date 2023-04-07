using UnityEngine;
using UnityEngine.UI;

namespace FenrirStudio.HeistForce
{
	public class CreateRoom : Photon.PunBehaviour 
	{
		#region Public Variables

		[SerializeField]
		private Text RoomName;

		#endregion

		#region Public Methods

		public void OnClickCreateRoom()
		{
			if(RoomName.text != "")
			{
				RoomOptions roomOptions = new RoomOptions(){IsVisible = true, IsOpen = true, MaxPlayers = 4};

				if(PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default))
				{
					Debug.Log("Create room successfully sent.");
				}
				else
				{
					Debug.Log("Create room failed to sent.");
				}
			}
		}

		#endregion

		#region PunBehavior Callback

		public override void OnCreatedRoom()
		{
			Debug.Log("Room created successfully.");
		}

		public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
		{
			Debug.Log("Create room failed : " + codeAndMsg[1]);
		}

		#endregion
	}
}
