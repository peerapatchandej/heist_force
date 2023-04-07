using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class PlayerShooting : Photon.PunBehaviour, IPunObservable
	{
		#region Private Variables

		[SerializeField]
		private int damagePerShot = 20;

		[SerializeField]
		private float timeBetweenBullets = 0.15f;

		[SerializeField]
		private float range = 100f;

		[SerializeField]
		private Transform SpawnBulletPoint;

		[SerializeField]
		private ParticleSystem gunParticles;

		[SerializeField]
		private LineRenderer gunLine;

		[SerializeField]
		private AudioSource aduioSource;
		
		private float timer;
		private Ray shootRay = new Ray();
		private RaycastHit shootHit;
		private int shootableMask;
		private float effectsDisplayTime = 0.2f;
		private bool IsFiring;

		#endregion

		#region MonoBehaviour Callback

		void Awake()
		{
			shootableMask = LayerMask.GetMask("Shootable");
		}

		void Update()
		{
			if(photonView.isMine)
			{
				ProcessInputs();
			}

			timer += Time.deltaTime;

			if(IsFiring)
			{
				if(timer >= timeBetweenBullets && Time.timeScale != 0)
				{
					Shoot();
				}
			}
			
			if(timer >= timeBetweenBullets * effectsDisplayTime)
			{
				DisableEffects();
			}
		}

		#endregion

		#region Private methods

		void ProcessInputs()
		{
			if(Input.GetButtonDown("Fire1"))
			{
				if(!IsFiring)
				{
					IsFiring = true;
				}
			}

			if(Input.GetButtonUp("Fire1"))
			{
				if(IsFiring)
				{
					IsFiring = false;
				}
			}
		}

		private void DisableEffects()
		{
			gunLine.enabled = false;
		}

		private void Shoot()
		{
			timer = 0f;
			gunParticles.Stop ();
			gunParticles.Play ();
			gunLine.enabled = true;
			gunLine.SetPosition (0, SpawnBulletPoint.position);
			shootRay.origin = SpawnBulletPoint.position;
			shootRay.direction = SpawnBulletPoint.forward;
			aduioSource.Play();
			
			if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
			{
				EnemyHealth EnemyHealth = shootHit.transform.GetComponent<EnemyHealth>();

				if(EnemyHealth != null)
				{
					EnemyHealth.TakeDamage();
				}

				gunLine.SetPosition (1, shootHit.point);
			}
			else
			{
				gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
			}
		}

		#endregion

		#region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(this.IsFiring);
            }
            else
            {
                // Network player, receive data
                this.IsFiring = (bool)stream.ReceiveNext();
            }
        }

        #endregion
	}
}

