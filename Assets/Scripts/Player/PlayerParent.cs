using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class PlayerParent : MonoBehaviour 
	{	
		public static Transform getPlayerParent;
	
		void Awake () 
		{
			getPlayerParent = transform;
		}
	}
}
