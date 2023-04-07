using UnityEngine;
using UnityEngine.UI;

namespace FenrirStudio.HeistForce
{
	public class Launcher : Photon.PunBehaviour 
	{
		#region Public Varaibles

		public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

		[Tooltip("The UI Panel to let the user enter name, connect and play")] 
		public GameObject controlPanel;
		[Tooltip("The UI Label to inform the user that the connection is in progress")]
		public GameObject progressLabel;

		#endregion

		#region Private Variables

		[SerializeField]
		private Text playerName;

		private string _gameVersion = "1";

		#endregion

		#region MonoBehavior Callback

			void Awake()
			{
				PhotonNetwork.autoJoinLobby = false;
				PhotonNetwork.automaticallySyncScene = false;	//When you want to start match type sync. You need to set true value.
				PhotonNetwork.logLevel = Loglevel;
			}

			void Start()
			{
				progressLabel.SetActive(false);
				controlPanel.SetActive(true);
			}

		#endregion

		#region Public Methods

		public void Connect()
		{
			if(playerName.text != "")
			{
				progressLabel.SetActive(true);
				controlPanel.SetActive(false);

				if(PhotonNetwork.connected)
				{
					PhotonNetwork.LoadLevel("Lobby");
				}
				else
				{
					PhotonNetwork.ConnectUsingSettings(_gameVersion);
				}
			}
		}

		#endregion

		#region Photon.PunBehaviour CallBacks

		public override void OnConnectedToMaster()
		{
			Debug.Log("Launcher : OnConnectedToMaster() was called by PUN");
			PhotonNetwork.JoinLobby(TypedLobby.Default);
		}

		public override void OnDisconnectedFromPhoton()
		{
			Debug.LogWarning("Launcher : OnDisconnectedFromPhoton() was called by PUN");
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
		}

		public override void OnJoinedLobby()
		{
			Debug.Log("Launcher : Joined Lobby");
			PhotonNetwork.LoadLevel("Lobby");
		}

		#endregion
	}
}
