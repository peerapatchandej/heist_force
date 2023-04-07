using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class PlaceBag : Photon.PunBehaviour 
	{
		#region Private Variables

		[SerializeField]
		private Transform backpack;

		[SerializeField]
		private Transform placePoint;

		private PlayerManager playerManager;
		private int localPlayerID;
		private bool isPlace;
		private GameObject bag;

		#endregion

		#region MonoBehaviour

		void Start()
		{
			if(photonView.isMine)
			{
				localPlayerID = GetComponent<PhotonView>().viewID;
				playerManager = GetComponent<PlayerManager>();
			}
		}
		
		void Update () 
		{
			if(photonView.isMine)
			{
				if(Input.GetKeyUp("g"))
				{
					if(playerManager.inPlaceArea == false && playerManager.haveBag)
					{
						PlayerPlaceBag();
					}
				}
			}
		}

		#endregion

		#region Private Methods

		public void PlayerPlaceBag()
		{
			photonView.RPC("RPC_PlaceBag", PhotonTargets.All, localPlayerID);
		}

		#endregion

		#region RPC Methods

		[PunRPC]
		private void RPC_PlaceBag(int playerID)
		{
			GameObject player = PhotonView.Find(playerID).gameObject;

			playerManager = player.GetComponent<PlayerManager>();
			bag = player.transform.Find("Backpack").transform.GetChild(0).gameObject;
			bag.transform.SetParent(CollectableParent.getCollectableParent);
			bag.transform.localPosition = placePoint.transform.position;
			bag.transform.localRotation = placePoint.transform.rotation;
			bag.GetComponent<BoxCollider>().enabled = true;
			playerManager.haveBag = false;

		}

		#endregion
	}
}
