using UnityEngine;
using System.Collections.Generic;

namespace FenrirStudio.HeistForce
{
	public class RoomLayoutGroup : Photon.PunBehaviour
	{
		#region Private Variables

		[SerializeField]
		private GameObject RoomListingPrefab;
		private List<RoomListing> RoomListingButtton = new List<RoomListing>();

		#endregion

		#region PunBehaviour Callback

		public override void OnReceivedRoomListUpdate()
		{
			Debug.Log("Room List Update");
			
			RoomInfo[] rooms = PhotonNetwork.GetRoomList();

			foreach(RoomInfo room in rooms)
			{
				RoomRecieved(room);
			}

			RemoveOldRoom();
		}

		#endregion

		#region Private Methods

		private void RoomRecieved(RoomInfo room)
		{
			int index = RoomListingButtton.FindIndex(x => x.RoomName == room.Name);

			if(index == -1)
			{
				if(room.IsVisible && room.PlayerCount < room.MaxPlayers)
				{
					GameObject RoomListingObj = Instantiate(RoomListingPrefab);
					RoomListingObj.transform.SetParent(transform, false);

					RoomListing roomListing = RoomListingObj.GetComponent<RoomListing>();
					RoomListingButtton.Add(roomListing);

					index = RoomListingButtton.Count - 1; 
				}
			}

			if(index != -1)
			{
				RoomListing roomListing = RoomListingButtton[index];
				roomListing.SetRoomName(room.Name);
				roomListing.Updated = true;
			}
		}

		private void RemoveOldRoom()
		{
			List<RoomListing> removeRoom = new List<RoomListing>();

			foreach(RoomListing roomListing in RoomListingButtton)
			{
				if(!roomListing.Updated)
				{
					removeRoom.Add(roomListing);
				}
				else
				{
					roomListing.Updated = false;
				}
			}

			foreach(RoomListing roomListing in removeRoom)
			{
				GameObject roomListingObj = roomListing.gameObject;
				RoomListingButtton.Remove(roomListing);
				Destroy(roomListingObj);
			}
		}

		#endregion
	}
}
