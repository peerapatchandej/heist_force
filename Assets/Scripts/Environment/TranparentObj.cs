using UnityEngine;

namespace FenrirStudio.HeistForce
{
	public class TranparentObj : MonoBehaviour 
	{
		#region Public Variables

		public static TranparentObj instance;

		#endregion

		#region Private Variables

		[SerializeField]
		private Material transperentMat;

		[SerializeField]
		private GameObject[] environmentObj;

		public Material[,] oldMat;
		private int maxLength = 0;

		#endregion
		
		#region MonoBehaviour Callback

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			for(int i = 0; i < environmentObj.Length; i++)
			{
				Material[] currentMat = environmentObj[i].GetComponent<MeshRenderer>().materials;

				if(currentMat.Length > maxLength)
				{
					maxLength = currentMat.Length;
				}
			}

			oldMat = new Material[environmentObj.Length, maxLength];

			for(int i = 0; i < environmentObj.Length; i++)
			{
				Material[] currentMat = environmentObj[i].GetComponent<MeshRenderer>().materials;

				if(currentMat.Length == 1)
				{
					oldMat[i, 0] = currentMat[0];
				}
				else
				{
					for(int x = 0; x < currentMat.Length; x++)
					{
						oldMat[i, x] = currentMat[x];
					}
				}
			}
		}
		#endregion

		#region Public Methods

		public void PlayerGetHome()
		{
			for(int i = 0; i < environmentObj.Length; i++)
			{
				Material[] currentMat = environmentObj[i].GetComponent<MeshRenderer>().materials;

				if(currentMat.Length == 1)
				{
					environmentObj[i].GetComponent<MeshRenderer>().material = transperentMat;
				}
				else
				{
					for(int x = 0; x < currentMat.Length; x++)
					{
						currentMat[x] = transperentMat;
					}
					environmentObj[i].GetComponent<MeshRenderer>().materials = currentMat;
				}
			}
		}

		public void PlayerLeaveHome()
		{
			for(int i = 0; i < environmentObj.Length; i++)
			{
				Material[] currentMat = environmentObj[i].GetComponent<MeshRenderer>().materials;

				if(currentMat.Length == 1)
				{
					environmentObj[i].GetComponent<MeshRenderer>().material = oldMat[i, 0];
				}
				else
				{
					for(int x = 0; x < currentMat.Length; x++)
					{
						currentMat[x] = oldMat[i, x];
					}
					environmentObj[i].GetComponent<MeshRenderer>().materials = currentMat;
				}
			}
		}

		#endregion
	}
}
