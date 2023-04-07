using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace FenrirStudio.HeistForce
{
	public class GameManager : Photon.PunBehaviour 
	{
		#region Public Variables

		public static GameManager instance;

		[HideInInspector]
		public List<PhotonPlayer> playerList;

		[HideInInspector]
		public bool isEndGame;

		[HideInInspector]
		public int playerDown = 0;

		public string levelName = "Level 1";

		public int countPlayerSpawn = 0;

		#endregion

		#region Private Variables

		private PhotonView thisPhotonView;
		public int PlayerInGame = 0;
        private bool isLoaded = false;
		private bool canUseName = false;

		#endregion
		
		#region Monobehaviour

		void Awake ()
		{
			instance = this;
			thisPhotonView = GetComponent<PhotonView>();
			SceneManager.sceneLoaded += OnSceneLoaded;
			DontDestroyOnLoad(gameObject);
		}
		
		#endregion

		#region Public Methods

		public void GetPlayerList()
		{
			playerList = new List<PhotonPlayer>();

			foreach(PhotonPlayer player in PhotonNetwork.playerList)
			{
				playerList.Add(player);
			}
		}

		#endregion

		#region Private Methods

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
            if (scene.name == levelName && !isLoaded)
			{
                if (PhotonNetwork.isMasterClient)
				{
					MasterLoadedGame();
				}
				else
				{
					NonMasterLoadedGame();
				}

				SpawnPlayerManager.instance.SpawnPlayer();

                isLoaded = true;

            }
		}

		private void MasterLoadedGame()
		{
			PlayerInGame = 1;
			thisPhotonView.RPC("RPC_LoadGameOther", PhotonTargets.Others);
		}

		private void NonMasterLoadedGame()
		{
			thisPhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
		}

		#endregion

		#region RPC Methods

		[PunRPC]
		private void RPC_LoadGameOther()
		{
			PhotonNetwork.LoadLevel(levelName);
		}

		[PunRPC]
		private void RPC_LoadedGameScene()
		{
			PlayerInGame++;
			if(PlayerInGame == PhotonNetwork.playerList.Length)
			{
				Debug.Log("All player in game.");
			}
		}

		#endregion
	}
}
