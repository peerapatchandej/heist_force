using UnityEngine;
using UnityEngine.UI;

namespace FenrirStudio.HeistForce
{
	public class CurrentRoomCanvas : Photon.PunBehaviour 
	{
		#region Public Variables

		public static CurrentRoomCanvas instance;

		#endregion

		#region Private Variables

		[SerializeField]
		private Image level1Image;

		[SerializeField]
		private Image level2Image;

		private string levelName = "Level 1"; 
		public bool levelChange;

		#endregion

		#region MonoBehaviour Callback

		private void Awake()
		{
			instance = this;
		}

		private void Update()
		{
			if(levelChange == true)
			{
				photonView.RPC("UpdateLevelSelect", PhotonTargets.All, GameManager.instance.levelName);
				levelChange = false;
			}
		}

		#endregion

		#region Public Methods

		public void OnClickStartSync()
		{
			PhotonNetwork.LoadLevel(levelName);
		}

		public void OnClickStartDelayed()
		{
			if(PhotonNetwork.isMasterClient)
			{
				PhotonNetwork.room.IsOpen = false;
				PhotonNetwork.room.IsVisible = false;
				PhotonNetwork.LoadLevel(levelName);
			}
		}

		public void ChangeLevel()
		{
			if(PhotonNetwork.isMasterClient)
			{
				photonView.RPC("RPC_ChangeLevel", PhotonTargets.All);
			}
		}

		#endregion

		#region RPC Methods

		[PunRPC]
		private void RPC_ChangeLevel()
		{
			if(level1Image.gameObject.activeSelf)
			{
				level1Image.gameObject.SetActive(false);
				level2Image.gameObject.SetActive(true);
				levelName = "Level 2";
			}
			else
			{
				level2Image.gameObject.SetActive(false);
				level1Image.gameObject.SetActive(true);
				levelName = "Level 1";
			}

			GameManager.instance.levelName = levelName;
		}

		[PunRPC]
		private void UpdateLevelSelect(string levelName)
		{
			if(levelName == "Level 1")
			{
				level2Image.gameObject.SetActive(false);
				level1Image.gameObject.SetActive(true);
				levelName = "Level 1";
			}
			else
			{
				level1Image.gameObject.SetActive(false);
				level2Image.gameObject.SetActive(true);
				levelName = "Level 2";
			}
		}

		#endregion
	}
}
