using UnityEngine;
using UnityEngine.AI;

namespace FenrirStudio.HeistForce
{
	public class EnemyHealth : Photon.PunBehaviour 
	{
		#region Public Variables

		public bool isDead;

		#endregion

		#region Private Variables

		[SerializeField]
		private int health = 3;

		private float time;
		private float timeDead = 2f;

		#endregion

		#region MonoBehaviour Callback

		private void Update()
		{
			if(isDead)
			{
				time += Time.deltaTime;

				if(time < timeDead)
				{
					transform.Translate (-Vector3.up * 8f * Time.deltaTime);
				}
				else
				{
					Destroy(gameObject);
				}
			}
		}

		#endregion

		#region Public Methods

		public void TakeDamage()
		{
			health--;
		
			if(health <= 0 && !isDead)
			{
				photonView.RPC("RPC_EnemyDie", PhotonTargets.All);
			}
		}

		#endregion

		#region RPC Methods

		[PunRPC]
		private void RPC_EnemyDie()
		{
			isDead = true;
			GetComponent<NavMeshAgent>().enabled = false;
			//Destroy(gameObject);
		}

		#endregion
	}
}
