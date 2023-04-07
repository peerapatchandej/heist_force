using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class GameOver : Photon.PunBehaviour 
	{
		#region Public Varaibles

		public static GameOver instance;

		#endregion

		#region Private Variables

		[SerializeField]
		private Animator canvasAnim;

		#endregion

		#region MonoBehaviour Callback

		private void Awake()
		{
			instance = this;
		}

		#endregion

		#region Private Methods

		public void MissionFail()
		{
			photonView.RPC("RPC_GameOver", PhotonTargets.MasterClient);
		}

		private void AllPlayerLeaveGame()
		{
			LeaveGame.instance.OnClickLeaveGame();
		}

		#endregion

		#region RPC Methods

		[PunRPC]
		private void RPC_GameOver()
		{
			GameManager.instance.isEndGame = true;
			photonView.RPC("RPC_PlayAnimation", PhotonTargets.All);
			photonView.RPC("RPC_LeaveGame", PhotonTargets.MasterClient);
		}

		[PunRPC]
		private void RPC_LeaveGame()
		{
			Invoke("AllPlayerLeaveGame", 3f);
		}

		[PunRPC]
		private void RPC_PlayAnimation()
		{
			canvasAnim.SetBool("isGameOver", true);
		}

		#endregion
	}
}
