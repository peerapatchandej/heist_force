using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class CollectableParent : MonoBehaviour 
	{
		public static Transform getCollectableParent;

		void Awake () 
		{
			getCollectableParent = transform;
		}
	}
}
