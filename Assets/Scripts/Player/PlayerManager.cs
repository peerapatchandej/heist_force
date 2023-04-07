using UnityEngine;
using UnityEngine.UI;

namespace FenrirStudio.HeistForce
{
	public class PlayerManager : Photon.PunBehaviour
	{
		#region Public Variables

		public static GameObject instance;

		[HideInInspector]
		public bool haveBag = false;

		[HideInInspector]
		public bool inPlaceArea = false;

		[HideInInspector]
		public bool stayEscapeArea = false;

		#endregion

		#region Private Variables

		[SerializeField]
		private Text playerName;

		#endregion

		#region MonoBehaviour
		
		void Awake() 
		{
			if(photonView.isMine)
			{
				instance = this.gameObject;
				photonView.RPC("RPC_SetNamePlayerObj", PhotonTargets.All);
			}

			//photonView.RPC("RPC_SetNamePlayerObj", PhotonTargets.Others);
			gameObject.transform.SetParent(PlayerParent.getPlayerParent);
		}

		#endregion

		#region RPC Methods

		[PunRPC]
		private void RPC_SetNamePlayerObj()
		{
			gameObject.name = photonView.owner.NickName;
			playerName.text = photonView.owner.NickName;
		}

		#endregion
	}
}
