using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class EnemyParent : MonoBehaviour 
	{
		public static Transform getEnemyParent;

		void Awake()
		{
			getEnemyParent = transform;
		}
	}
}
