using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FenrirStudio.HeistForce
{
	public class SpawnPlayerManager : Photon.PunBehaviour
	{
		#region Public Variables

		public static SpawnPlayerManager instance;

		#endregion

		#region Private Variables

		[SerializeField]
		private GameObject[] playerPrefeb;

		[SerializeField]
		private Transform spawnPos1, spawnPos2, spawnPos3, spawnPos4;

		#endregion

		#region MonoBehaviour Callback

		void Awake()
		{
			instance = this;
		}

		#endregion

		#region Public Methods
		public void SpawnPlayer()
		{
			if(playerPrefeb != null)
			{
				if(PlayerManager.instance == null)
				{
					Vector3 spawnPos = Vector3.zero;
					int playerNumber = PhotonNetwork.player.ID;
					
					PhotonPlayer[] playerArray = new PhotonPlayer[PhotonNetwork.playerList.Length];
					playerArray = PhotonNetwork.playerList;

					for (int i = 0; i < playerArray.Length - 1; i++)
					{
						for (int j = i + 1; j > 0; j--)
						{
							if (playerArray[j - 1].ID > playerArray[j].ID)
							{
								PhotonPlayer temp = playerArray[j - 1];
								playerArray[j - 1] = playerArray[j];
								playerArray[j] = temp;
							}
						}
					}

					for(int i = 0; i < playerArray.Length; i++)
					{
						if(playerArray[i].ID == playerNumber)
						{
							playerNumber = i + 1;
							break;
						}
					}

					if(playerNumber == 1)
					{
						spawnPos = spawnPos1.position;
					}
					else if(playerNumber == 2)
					{
						spawnPos = spawnPos2.position;
					}
					else if(playerNumber == 3)
					{
						spawnPos = spawnPos3.position;
					}
					else if(playerNumber == 4)
					{
						spawnPos = spawnPos4.position;
					}
				
					PhotonNetwork.Instantiate(playerPrefeb[playerNumber - 1].name, spawnPos, Quaternion.identity, 0);
					photonView.RPC("UpdatePlayerInScene", PhotonTargets.MasterClient);
				}
			}
			else
			{
				Debug.LogError("Not found player prefab.");
			}
		}

		#endregion

		#region RPC Methods

		[PunRPC]
		private void UpdatePlayerInScene()
		{
			GameManager.instance.countPlayerSpawn++;
		}

		#endregion
	}
}
