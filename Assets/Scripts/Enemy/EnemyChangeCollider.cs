using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class EnemyChangeCollider : MonoBehaviour
	{
		#region MonoBehaviour

			void OnTriggerExit(Collider other)
			{
				if(other.CompareTag("Enemy"))
				{
					other.GetComponent<CapsuleCollider>().isTrigger = false;
				}
			}

		#endregion
	}
}
