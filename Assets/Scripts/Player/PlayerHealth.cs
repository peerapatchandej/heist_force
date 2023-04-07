using UnityEngine;
using UnityEngine.UI;

namespace FenrirStudio.HeistForce
{
	public class PlayerHealth : Photon.PunBehaviour, IPunObservable
	{
		#region Public Variables

		public bool isDead;

		#endregion

		#region Private Variables

		[SerializeField]
		private Slider healthSlider;

		private int healthStart = 100;
		private int healthCurrent;
		private int localPlayerID;
		private float time;
		private float timeDead = 2f;
		private PlaceBag placeBag;
		private PlayerManager playerManager;
		

		#endregion

		#region MonoBehaviour Callback

		void Start () 
		{
			healthCurrent = healthStart;
			healthSlider.value = healthCurrent;
			localPlayerID = GetComponent<PhotonView>().viewID;
			placeBag = GetComponent<PlaceBag>();
			playerManager = GetComponent<PlayerManager>();
		}

		void Update()
		{
			healthSlider.value = healthCurrent;

			if(isDead && gameObject.activeSelf)
			{
				time += Time.deltaTime;

				if(time < timeDead)
				{
					transform.Translate (-Vector3.up * 2.5f * Time.deltaTime);
				}
				else
				{
					GameManager.instance.playerDown++;

					if(GameManager.instance.playerDown == PhotonNetwork.playerList.Length)
					{
						if(PhotonNetwork.isMasterClient)
						{
							GameOver.instance.MissionFail();
						}
					}

					gameObject.SetActive(false);
				}
			}
		}

		#endregion

		#region Public Methods

		public void TakeDamage(int amount)
		{
			if(healthCurrent > 0)
			{
				healthCurrent -= amount;
			}
			else
			{
				if(healthCurrent <= 0)
				{
					//GameManager.instance.isEndGame = true;
					if(playerManager.haveBag)
					{
						placeBag.PlayerPlaceBag();
					}
					
					photonView.RPC("RPC_PlayerDown", PhotonTargets.All, localPlayerID);
				}
			}
		}

		#endregion

		#region RPC Methods

		[PunRPC]
		private void RPC_PlayerDown(int playerID)
		{
			GameObject player = PhotonView.Find(playerID).gameObject;
			
			player.GetComponent<CameraFollow>().enabled = false;
			player.GetComponent<Rigidbody>().isKinematic = true;
        	isDead = true;
		}

		#endregion

		#region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.isWriting)
			{
				stream.SendNext(this.healthCurrent);
			}
			else
            {
                this.healthCurrent = (int)stream.ReceiveNext();
            }
        }

        #endregion

    }
}
