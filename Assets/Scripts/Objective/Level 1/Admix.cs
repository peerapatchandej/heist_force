using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class Admix : Photon.PunBehaviour 
	{   
        #region Public Variables

        public static Admix instance;

        [HideInInspector]
        public bool isPlace, isAdmix, admixComplete;

        [HideInInspector]
        public Vector3 bagScale;

        [HideInInspector]
        public Vector3 bagPos;

        [HideInInspector]
        public Quaternion bagRotate;

        public BoxCollider BoxTriggerCollider;
        public GameObject drugBag;

        #endregion

		#region Private Variables

        [SerializeField]
        private GameObject drug;

        private float time;
        private float timeAdmix = 3f;

        private int localPlayerID;
		private GameObject admixBag;

		#endregion
		
		#region MonoBehaviour

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            bagScale = drugBag.transform.localScale;
            bagPos = drugBag.transform.position;
            bagRotate = drugBag.transform.rotation;
        }

        private void Update()
        {
            if(isAdmix)
            {
                time += Time.deltaTime;

                if(time >= timeAdmix)
                {
                    if(PhotonNetwork.isMasterClient)
			        {
                        photonView.RPC("RPC_AdmixComplete", PhotonTargets.All);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if(other.gameObject.GetComponent<PhotonView>().photonView.isMine)
                {
                    other.gameObject.GetComponent<PlayerManager>().inPlaceArea = true;
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
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if(other.gameObject.GetComponent<PhotonView>().photonView.isMine && !isPlace)
                {
                    if(Input.GetKeyUp("g"))
                    {
                        if(other.GetComponent<PlayerManager>().haveBag)
						{
                            if(other.transform.Find("Backpack").GetChild(0).transform.name.IndexOf("Admixture Bag") != -1)
                            {
                                localPlayerID = other.gameObject.GetComponent<PhotonView>().viewID;
                                photonView.RPC("RPC_PlaceAdmix", PhotonTargets.All, localPlayerID);
                            }
                        }
                    }
                }

                if(isPlace)
                {
                    //var AdmixBag = other.transform.Find("Backpack").GetChild(0).gameObject;

                    Destroy(admixBag);
                    BoxTriggerCollider.enabled = false;
                    drug.SetActive(true);
                    isAdmix = true;
                    //other.gameObject.GetComponent<PlayerManager>().haveBag = false;
                    //other.gameObject.GetComponent<PlayerManager>().inPlaceArea = false;
                }
            }
        }

        #endregion

        #region RPC Methods

        [PunRPC]
        private void RPC_PlaceAdmix(int playerID)
        {
            GameObject player = PhotonView.Find(playerID).gameObject;
            admixBag = player.transform.Find("Backpack").GetChild(0).gameObject;
            player.GetComponent<PlayerManager>().haveBag = false;
            player.GetComponent<PlayerManager>().inPlaceArea = false;
            isPlace = true;
        }

        [PunRPC]
        private void RPC_AdmixComplete()
        {
            drug.SetActive(false);
            drugBag.SetActive(true);
            isAdmix = false;
            admixComplete = true;
            time = 0;
        }

        #endregion
	
	}
}
