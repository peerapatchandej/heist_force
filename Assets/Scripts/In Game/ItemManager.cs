using UnityEngine;

namespace FenrirStudio.HeistForce
{
    public class ItemManager : Photon.PunBehaviour
    {
        #region Private Variables
        
        [SerializeField]
        private BoxCollider BoxTriggerCollider;

        [SerializeField]
		private GameObject blueBagPrefeb;
        
        private bool isLoot;

        #endregion

        #region MonoBehaviour

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
                if(other.gameObject.GetComponent<PhotonView>().photonView.isMine)
                {
                    if(Input.GetKeyUp("e"))
                    {
                        if(!other.gameObject.GetComponent<PlayerManager>().haveBag)
                        {
                            photonView.RPC("RPC_LootBag", PhotonTargets.All);
                        }
                    }
                }

                if(isLoot)
                {
                    if(Admix.instance != null)
                    {
                        if(Admix.instance.admixComplete)
                        {
                            if(PhotonNetwork.isMasterClient)
                            {
                                photonView.RPC("CreateDrugBag", PhotonTargets.MasterClient);
                            }
                        }
                        lootItem(other);
                    }
                    else
                    {
                        lootItem(other);
                    }

                    isLoot = false;
                    other.gameObject.GetComponent<PlayerManager>().inPlaceArea = false;
                }
            }
        }

        #endregion

        #region Private Methods

        private void lootItem(Collider other)
        {
            var Parent = other.transform.Find("Backpack").transform;
            transform.SetParent(Parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            BoxTriggerCollider.enabled = false;
            other.gameObject.GetComponent<PlayerManager>().haveBag = true;
        }

        #endregion

        #region RPC Methods

        [PunRPC]
        private void RPC_LootBag()
        {
            isLoot = true;
        }

        [PunRPC]
        private void CreateDrugBag()
        {
            GameObject bag = PhotonNetwork.Instantiate("Blue Bag", Admix.instance.bagPos, Admix.instance.bagRotate, 0);
            photonView.RPC("AdmixSetValue", PhotonTargets.All, bag.GetComponent<PhotonView>().viewID);
        }

        [PunRPC]
        private void AdmixSetValue(int bagID)
        {
            GameObject bagObj = PhotonView.Find(bagID).gameObject;

            bagObj.transform.SetParent(Admix.instance.transform);
            bagObj.transform.localScale = Admix.instance.bagScale;
            bagObj.SetActive(false);

            Admix.instance.isPlace = false;
            Admix.instance.isAdmix = false;
            Admix.instance.BoxTriggerCollider.enabled = true;
            Admix.instance.drugBag = bagObj;
            Admix.instance.admixComplete = false;
        }

        #endregion
    }
}
