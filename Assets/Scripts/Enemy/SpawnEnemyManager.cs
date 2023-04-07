using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace FenrirStudio.HeistForce
{
	public class SpawnEnemyManager : Photon.PunBehaviour 
	{
		#region  Public Variables

		public static SpawnEnemyManager instance;

		[HideInInspector]
		public float spawnTime;

		#endregion

		#region Private Variables

		[SerializeField]
		private GameObject enemyPrefeb;

		[SerializeField]
		private Text logText;

		private EnemyMovement enemyMove;
		private bool isHaveFirstEnemy = false;
		public float spawnRateTime;

		#endregion

		#region MonoBehaviour Callback

		private void Awake()
		{
			instance = this;
		}

		private void Start() 
		{
			spawnRateTime = Random.Range(5, 11);
		}

		private void Update()
		{
			if(PhotonNetwork.isMasterClient && !isHaveFirstEnemy)
			{
				if(PhotonNetwork.playerList.Length == GameManager.instance.countPlayerSpawn)
				{
					BeginSpawnEnemy();
					isHaveFirstEnemy = true;
				}
			}

			if(isHaveFirstEnemy)
			{
				if(spawnTime < spawnRateTime)
				{
					spawnTime += Time.deltaTime;
				}
				else
				{
					spawnTime = 0;
					SpawnEnemy();
				}
			}
		}
		
		#endregion

		#region Private Methods

		private void BeginSpawnEnemy()
		{
			GameManager.instance.GetPlayerList();
			SpawnEnemy();
		}

		private void SpawnEnemy()
		{
			photonView.RPC("RPC_SpawnEnemy", PhotonTargets.MasterClient);
		}

		#endregion

		[PunRPC]
		private void RPC_SpawnEnemy()
		{
			GameObject enemyTmp = PhotonNetwork.Instantiate(enemyPrefeb.name, transform.position, Quaternion.identity, 0);
			photonView.RPC("RPC_InitEnemy", PhotonTargets.All, enemyTmp.GetComponent<PhotonView>().viewID, GameManager.instance.playerList[Random.Range(0, GameManager.instance.playerList.Count)].NickName);
			spawnRateTime = Random.Range(5, 11);
		}

		[PunRPC]
		private void RPC_InitEnemy(int enemyID, string playerName)
		{
			GameObject enemyObj = PhotonView.Find(enemyID).gameObject;
			enemyMove = enemyObj.GetComponent<EnemyMovement>();
			enemyMove.playerTarget = PlayerParent.getPlayerParent.Find(playerName).transform;
			enemyObj.transform.SetParent(EnemyParent.getEnemyParent);
		}
	}
}
