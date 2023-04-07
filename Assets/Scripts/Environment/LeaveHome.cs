using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class LeaveHome : MonoBehaviour 
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
					TranparentObj.instance.PlayerLeaveHome();
					getHomeCol.enabled = true;
					leaveHomeCol.enabled = false;
				}
			}
		}

		#endregion
	}
}
