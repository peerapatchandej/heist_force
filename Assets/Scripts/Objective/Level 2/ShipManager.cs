using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class ShipManager : Photon.PunBehaviour 
	{
		#region Private Variables

		[SerializeField]
		private Transform dock1Pos;

		[SerializeField]
		private Transform dock2Pos;

		[SerializeField]
		private GameObject placeDrugArea1;

		[SerializeField]
		private GameObject placeDrugArea2;

		private Transform tarPos;
		private GameObject placeDrugArea;
		private float speed = 10f;
		private float reduceSpeedDock1 = 4.2f;
		private float reduceSpeedDock2 = 7.3f;
		private Rigidbody shipRigidBody;
		private float time;
		private float timeSpawn = 5f;

		#endregion

		#region MonoBehaviour CallBack

		void Start()
		{
			shipRigidBody = GetComponent<Rigidbody>();

			if(PhotonNetwork.isMasterClient)
			{
				photonView.RPC("SetDock", PhotonTargets.All, Random.Range(1, 3));
			}
		}

		void Update () 
		{
			if(time > timeSpawn)
			{
				shipRigidBody.position = Vector3.MoveTowards(transform.position, tarPos.position, speed * Time.deltaTime);

				if(shipRigidBody.position == tarPos.position)
				{
					placeDrugArea.SetActive(true);
				}
			}
			else
			{
				time += Time.deltaTime;
			}
		}

		#endregion

		#region RPC Methods

		[PunRPC]
		private void RandomDock()
		{
			photonView.RPC("SetDock", PhotonTargets.All, Random.Range(1, 3));
		}

		[PunRPC]
		private void SetDock(int dockNum)
		{
			if(dockNum == 1)
			{
				tarPos = dock1Pos;
				placeDrugArea = placeDrugArea1;
			}
			else
			{
				tarPos = dock2Pos;
				placeDrugArea = placeDrugArea2;
			}
		}

		#endregion
	}
}
