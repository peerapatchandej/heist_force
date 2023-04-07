using UnityEngine;
using System.Collections;
using UnityEngine.AI;

namespace FenrirStudio.HeistForce
{
	public class EnemyMovement : Photon.PunBehaviour, IPunObservable
	{
		#region Public Veriables

		[HideInInspector]
		public Transform playerTarget;
	
		#endregion

		#region Private Variables

		private Rigidbody enemyRigidBody;
		private NavMeshAgent nav;
		private bool isCollision;
		private float distance;

		[SerializeField]
		private float speed;

		[SerializeField]
		private float nearDistance;

		#endregion

		#region MonoBehaviour Callback

		void Awake()
		{
			enemyRigidBody = GetComponent<Rigidbody>();
			nav = GetComponent<NavMeshAgent>();
		}
		
		void FixedUpdate()
		{
			if(playerTarget != null)
			{
				if(!playerTarget.gameObject.activeSelf)
				{
					playerTarget = null;
				}
			}

			Movement();
			Rotate();
		}

		void OnCollisionEnter(Collision other)
		{
			if(other.gameObject.tag == "Environment")
			{
				isCollision = true;
			}
		}

		void OnCollisionExit(Collision other)
		{
			if(other.gameObject.tag == "Environment")
			{
				isCollision = false;
			}
		}

		#endregion

		#region Private Methods

        private void Movement()
        {
            if(playerTarget == null) 
			{
				GameManager gameInstace = GameManager.instance;
				gameInstace.GetPlayerList();
				string playerName = gameInstace.playerList[Random.Range(0, gameInstace.playerList.Count)].NickName;

				GameObject player = PlayerParent.getPlayerParent.Find(playerName).gameObject;

				if(player.activeSelf)
				{
					playerTarget = player.transform;
				}
			}
			else
			{
				distance = Vector3.Distance(transform.position, playerTarget.position);

				if(distance < nearDistance && !isCollision)
				{
					enemyRigidBody.position = Vector3.MoveTowards(transform.position, playerTarget.position, -speed * Time.deltaTime);
				}
				else if(distance < nearDistance && isCollision)
				{
					enemyRigidBody.position = this.transform.position;
				}
				else if(distance > nav.stoppingDistance && nav.enabled)
				{
					nav.SetDestination(playerTarget.position);
				}
			}
        }

        private void Rotate()
        {
            if(playerTarget == null) 
			{
				return;
			}

			Vector3 relativePos = playerTarget.position - transform.position;
        	Quaternion rotation = Quaternion.LookRotation(relativePos);
			enemyRigidBody.rotation = rotation;
        }

        #endregion

		#region IPunObservable implementation

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
				stream.SendNext(this.enemyRigidBody.position);
				stream.SendNext(this.enemyRigidBody.rotation);
				stream.SendNext(this.enemyRigidBody.velocity);
            }
            else
            {
                this.enemyRigidBody.position = (Vector3)stream.ReceiveNext();
				this.enemyRigidBody.rotation = (Quaternion)stream.ReceiveNext();
				this.enemyRigidBody.velocity = (Vector3) stream.ReceiveNext();

				float lag = Mathf.Abs((float) (PhotonNetwork.time - info.timestamp));
				this.enemyRigidBody.position += enemyRigidBody.velocity * lag;
            }
        }

		#endregion
    }
}