using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class Winner : Photon.PunBehaviour 
	{
		#region Public Variables

		public static Winner instance;

		#endregion

		#region Private Variables

		[SerializeField]
		private Animator canvasAnim;

		[SerializeField]
		private int playerCount = 0;

		#endregion

		#region MonoBehaviour Callback

		private void Awake()
		{
			instance = this;
		}

		private void Update()
		{
			if(playerCount == (PhotonNetwork.playerList.Length - GameManager.instance.playerDown))
			{
				photonView.RPC("RPC_PlayAnimation", PhotonTargets.All);
				photonView.RPC("RPC_LeaveGame", PhotonTargets.MasterClient);
			}
		}

		private void OnTriggerStay(Collider other)
		{
			if(other.CompareTag("Player"))
            {
                if(other.gameObject.GetComponent<PhotonView>().photonView.isMine)
                {
					if(!other.gameObject.GetComponent<PlayerManager>().stayEscapeArea)
					{
						other.gameObject.GetComponent<PlayerManager>().stayEscapeArea = true;
						photonView.RPC("RPC_Comein", PhotonTargets.MasterClient);  
					}
                }
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if(other.CompareTag("Player"))
            {
				if(other.gameObject.GetComponent<PhotonView>().photonView.isMine)
                {
					other.gameObject.GetComponent<PlayerManager>().stayEscapeArea = false;
					photonView.RPC("RPC_Comeout", PhotonTargets.MasterClient);  
                }
			}
		}

		#endregion

		#region Private Methods

		private void AllPlayerLeaveGame()
		{
			LeaveGame.instance.OnClickLeaveGame();
		}

		#endregion

		#region RPC Methods

		[PunRPC]
		private void RPC_Comein()
		{
			playerCount++;
		}

		[PunRPC]
		private void RPC_Comeout()
		{
			playerCount--;
		}

		[PunRPC]
		private void RPC_LeaveGame()
		{
			Invoke("AllPlayerLeaveGame", 3f);
		}

		[PunRPC]
		private void RPC_PlayAnimation()
		{
			GameManager.instance.isEndGame = true;
			canvasAnim.SetBool("isWinner", true);
		}

		#endregion
	}
}
