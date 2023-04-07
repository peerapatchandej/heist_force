using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class PlayerMovement : Photon.MonoBehaviour, IPunObservable
	{
		#region Private Variables

		[SerializeField]
		private  float speed = 6f;
		private Vector3 movement;
		private Rigidbody PlayerRigidbody;
		private int floorMask;
		private float camRayLength = 100f;
		private PlayerHealth playerHealth;

        #endregion

        #region MonoBehaviour Callback

        void Awake () 
		{
			floorMask = LayerMask.GetMask("Floor");
			PlayerRigidbody = GetComponent<Rigidbody>();
			playerHealth = GetComponent<PlayerHealth>();
		}
		
		void FixedUpdate () 
		{
			if(!photonView.isMine)
			{
				return;
			}

			if(!playerHealth.isDead && !GameManager.instance.isEndGame)
			{
				float h = Input.GetAxis("Horizontal");
				float v = Input.GetAxis("Vertical");

				Move(h, v);
				Turning();
			}
			else
			{
				gameObject.layer = LayerMask.NameToLayer("Default");
			}
		}

		#endregion

		#region Private Methods

		private void Move(float h,float v)
		{
			movement.Set(h, 0f, v);
			movement = movement.normalized * speed * Time.deltaTime;
			PlayerRigidbody.position = transform.position + movement;
		}

		private void Turning()
		{
			Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit floorHit;

			if(Physics.Raycast(camRay,out floorHit, camRayLength, floorMask))
			{
				Vector3 playerToMouse = floorHit.point - transform.position;
				playerToMouse.y = 0f;

				Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
				PlayerRigidbody.rotation = newRotation;
			}
		}

		#endregion

		#region IPunObservable Implementation

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
			if (stream.isWriting)
			{
				stream.SendNext(PlayerRigidbody.position);
				stream.SendNext(PlayerRigidbody.rotation);
				stream.SendNext(PlayerRigidbody.velocity);
			}
			else
			{
				PlayerRigidbody.position = (Vector3) stream.ReceiveNext();
				PlayerRigidbody.rotation = (Quaternion) stream.ReceiveNext();
				PlayerRigidbody.velocity = (Vector3) stream.ReceiveNext();
		
				float lag = Mathf.Abs((float) (PhotonNetwork.time - info.timestamp));
				PlayerRigidbody.position += PlayerRigidbody.velocity * lag;
			}
        }

		#endregion
	}
}
