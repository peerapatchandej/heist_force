using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class GetHome : MonoBehaviour 
	{
		#region Private Variables

		[SerializeField]
		private BoxCollider getHomeCol;

		[SerializeField]
		private BoxCollider leaveHomeCol;

		#endregion

		#region MonoBehaviour Callback

		void OnTriggerEnter(Collider other)
		{
			if(other.CompareTag("Player"))
			{
				if(other.gameObject.GetComponent<PhotonView>().photonView.isMine)
				{
					TranparentObj.instance.PlayerGetHome();
					getHomeCol.enabled = false;
					leaveHomeCol.enabled = true;
				}
			}
		}

		#endregion
	}
}
