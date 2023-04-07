using System.Collections.Generic;
using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class PlayerLayoutGroup : Photon.PunBehaviour 
	{
		#region Private Variables

		[SerializeField]
		private GameObject PlayerListingPrefab;
		private List<PlayerListing> PlayerListing = new List<PlayerListing>();

		#endregion

		#region PunBehaviour CallBack

		public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
		{
			PhotonNetwork.LeaveRoom();
		}

		public override void OnJoinedRoom()
		{
			foreach(Transform child in transform)
			{
				Destroy(child.gameObject);
			}

			MainCanvasManager.Instance.currentRoomCanvas.transform.SetAsLastSibling();
			
			PhotonPlayer[] photonPlayer = PhotonNetwork.playerList;

			for(int i = 0; i < photonPlayer.Length; i++)
			{
				PlayerJoinRoom(photonPlayer[i]);
			}
		}

		public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
		{
			PlayerJoinRoom(newPlayer);
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			PlayerLeftRoom(otherPlayer);
		}

		public override void OnLeftRoom()
		{
			GameManager.instance.levelName = "Level 1";
			CurrentRoomCanvas.instance.levelChange = true;
		}

		#endregion

		#region Private Methods

		private void PlayerJoinRoom(PhotonPlayer photonPlayer)
		{
			if(photonPlayer == null)
			{
				return;
			}

			PlayerLeftRoom(photonPlayer);

			GameObject PlayerListingObj = Instantiate(PlayerListingPrefab);
			PlayerListingObj.transform.SetParent(transform, false);

			PlayerListing playerListing = PlayerListingObj.GetComponent<PlayerListing>();
			playerListing.ApplyPhotonPlayer(photonPlayer);

			PlayerListing.Add(playerListing);
		}

		private void PlayerLeftRoom(PhotonPlayer photonPlayer)
		{
			int index = PlayerListing.FindIndex(x => x.photonPlayer == photonPlayer);

			if(index != -1)
			{
				Destroy(PlayerListing[index].gameObject);
				PlayerListing.RemoveAt(index);
			}
		}

		#endregion

		#region Public Methods

		public void OnClickRoomState()
		{
			if(!PhotonNetwork.isMasterClient)
			{
				return;
			}

			PhotonNetwork.room.IsOpen = !PhotonNetwork.room.IsOpen;
			PhotonNetwork.room.IsVisible = PhotonNetwork.room.IsOpen;
		}

		public void OnClickLeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
		}

		#endregion
	}
}
