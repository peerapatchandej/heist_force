using UnityEngine;
using UnityEngine.UI;

namespace FenrirStudio.HeistForce
{
	public class PlayerListing : MonoBehaviour 
	{
		#region Public Variables

		public PhotonPlayer photonPlayer{get; private set;}

		#endregion
		
		#region Private Variables

		[SerializeField]
		private Text PlayerName;

		#endregion

		#region Public Methods

		public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
		{
			this.photonPlayer = photonPlayer;
			PlayerName.text = photonPlayer.NickName;
		}

		#endregion
	}
}
