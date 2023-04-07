using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class PlaceBlueBag : Photon.PunBehaviour 
	{
		#region Private Variables

		[SerializeField]
		private Transform[] placeBagPoint;

		[SerializeField]
		private GameObject escapeArea;

		[SerializeField]
		private Animator canvasAnim;

		private BoxCollider boxCollider;
		private bool isPlace;
		private int countBag = 0;
		private int localPlayerID;
		private GameObject drugBag;

		#endregion

		#region MonoBehaviour Callback

		private void Start()
		{
			boxCollider = GetComponent<BoxCollider>();
		}

		private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if(other.gameObject.GetComponent<PhotonView>().photonView.isMine)
                {
                    other.gameObject.GetComponent<PlayerManager>().inPlaceArea = true;
					//photonView.RPC("RPC_DisableCollider", PhotonTargets.Others);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if(other.gameObject.GetComponent<PhotonView>().photonView.isMine)
                {
                    other.gameObject.GetComponent<PlayerManager>().inPlaceArea = false;
					//photonView.RPC("RPC_EnableCollider", PhotonTargets.Others);
                }
            }
        }

		private void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if(other.gameObject.GetComponent<PhotonView>().photonView.isMine)
                {
                    if(Input.GetKeyUp("g"))
                    {
						if(other.GetComponent<PlayerManager>().haveBag)
						{
							if(other.transform.Find("Backpack").GetChild(0).transform.name.IndexOf("Blue Bag") != -1)
							{
								localPlayerID = other.gameObject.GetComponent<PhotonView>().viewID;
								photonView.RPC("RPC_PlaceBag", PhotonTargets.All, localPlayerID);
							}
						}
                    }
                }

                if(isPlace)
                {
                    //var bag = other.transform.Find("Backpack").GetChild(0).gameObject;
					Transform parent = null;

					for(int i = 0; i < placeBagPoint.Length; i++)
					{
						if(placeBagPoint[i].childCount == 0)
						{
							parent = placeBagPoint[i];
							break;
						}
					}

					drugBag.transform.SetParent(parent);
					drugBag.transform.localPosition = Vector3.zero;
					drugBag.transform.localRotation = Quaternion.Euler(0, 0, 0);
					drugBag.GetComponent<BoxCollider>().enabled = false;
                    //other.gameObject.GetComponent<PlayerManager>().haveBag = false;
					countBag++;

					if(countBag == 4)
					{
						photonView.RPC("RPC_PlayAnimation", PhotonTargets.All);
						photonView.RPC("RPC_LeaveGame", PhotonTargets.MasterClient);
						//escapeArea.SetActive(true);
					}

					isPlace = false;
					//other.gameObject.GetComponent<PlayerManager>().inPlaceArea = false;
                }
            }
        }

		#endregion

		private void AllPlayerLeaveGame()
		{
			LeaveGame.instance.OnClickLeaveGame();
		}

		#region RPC Methods

        [PunRPC]
        private void RPC_PlaceBag(int playerID)
        {
			GameObject player = PhotonView.Find(playerID).gameObject;
			drugBag = player.transform.Find("Backpack").GetChild(0).gameObject;
			player.GetComponent<PlayerManager>().haveBag = false;
			player.GetComponent<PlayerManager>().inPlaceArea = false;
            isPlace = true;
        }

		[PunRPC]
		private void RPC_PlayAnimation()
		{
			GameManager.instance.isEndGame = true;
			canvasAnim.SetBool("isWinner", true);
		}

		[PunRPC]
		private void RPC_LeaveGame()
		{
			Invoke("AllPlayerLeaveGame", 3f);
		}

		/*[PunRPC]
        private void RPC_DisableCollider()
        {
            GetComponent<BoxCollider>().enabled = false;
        }

        [PunRPC]
        private void RPC_EnableCollider()
        {
            GetComponent<BoxCollider>().enabled = true;
        }*/

        #endregion
		
	}
}
