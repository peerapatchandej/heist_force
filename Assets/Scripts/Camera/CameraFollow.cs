using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class CameraFollow : Photon.MonoBehaviour 
	{
		#region Private Variables
		
		[SerializeField]
		public float smoothing = 5f;
		private Transform target;
		private Camera cam;
		private Vector3 offset;

		#endregion

		#region MonoBehaviour Callback

		private void Awake()
		{
			target = this.transform;
			cam = Camera.main;
			cam.transform.position = new Vector3(target.position.x, target.position.y + 1.8f, target.position.z);
		}

		private void Start()
		{
			offset = cam.transform.position - target.position;
		}

		private void FixedUpdate()
		{
			if(!photonView.isMine)
			{
				return;
			}
			
			Vector3 targetCamPos = target.position + offset;
			cam.transform.position = Vector3.Lerp(cam.transform.position, targetCamPos, smoothing * Time.deltaTime);
		}

		#endregion
	}
}
