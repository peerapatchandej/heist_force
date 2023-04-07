using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class EnemyShooting : Photon.PunBehaviour//, IPunObservable
	{
		#region Private Variables

		[SerializeField]
		private int damagePerShot = 20;

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
		
		private float timer, timeFiring;
		private float timeBetweenBullets = 0.15f, timeBetweenShoot = 3f, timeBetweenFire = 2f;
		private Ray shootRay = new Ray();
		private RaycastHit shootHit;
		private int shootableMask;
		private float effectsDisplayTime = 0.2f;
		private bool isBeginFire, isFiring;
		private EnemyMovement enemyMove;

		#endregion

		#region MonoBehaviour Callback

		void Awake()
		{
			shootableMask = LayerMask.GetMask("Shootable");
		}

		void Start()
		{
			enemyMove = GetComponent<EnemyMovement>();
		}

		void Update()
		{
			if(enemyMove.playerTarget == null)
			{
				return;
			}
			
			BeginShoot();
			HoldFiring();
		}

		#endregion

		#region Private methods

		void BeginShoot()
		{
			if(!isBeginFire)
			{
				timeBetweenShoot = Random.Range(3, 6);
			}

			if(!isFiring)
			{
				timer += Time.deltaTime;

				if(timer >= timeBetweenShoot)
				{
					isFiring = true;
					timer = 0;
				}
			}
		}

		void HoldFiring()
		{
			timer += Time.deltaTime;
			timeFiring += Time.deltaTime;

			if(isFiring)
			{
				if(timeFiring <= timeBetweenFire)
				{
					if(timer >= timeBetweenBullets && Time.timeScale != 0)
					{
						Shoot();
					}
				}
				else
				{
					isFiring = false;
					isBeginFire = false;
					timeFiring = 0;
					timeBetweenFire = Random.Range(1, 3);
				}
			}
			
			if(timer >= timeBetweenBullets * effectsDisplayTime)
			{
				DisableEffects();
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
				PlayerHealth playerHealth = shootHit.transform.GetComponent<PlayerHealth>();

				if(playerHealth != null)
				{
					playerHealth.TakeDamage(Random.Range(1, 6));
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
                stream.SendNext(this.isFiring);
            }
            else
            {
                this.isFiring = (bool)stream.ReceiveNext();
            }
        }
		
        #endregion
	}
}

