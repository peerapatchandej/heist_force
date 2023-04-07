using UnityEngine;
using UnityEngine.UI;

namespace FenrirStudio.HeistForce
{
	public class RoomListing : MonoBehaviour 
	{
		#region Private Variables

		[SerializeField]
		private Text RoomNameText;

		#endregion

		#region Public Variables

		public string RoomName{get; private set;}
		public bool Updated{ get; set; }

		#endregion

		#region Monobehaviour

		private void Start()
		{
			GameObject LobbyCanvasObj = MainCanvasManager.Instance.lobbyCanvas.gameObject;

			if(LobbyCanvasObj == null)
			{
				return;
			}

			LobbyCanvas lobbyCanvas = LobbyCanvasObj.GetComponent<LobbyCanvas>();

			Button button = GetComponent<Button>();
			button.onClick.AddListener(() => lobbyCanvas.OnClickJoinRoom(RoomNameText.text));
		}

		#endregion

		#region Public Methods

		public void SetRoomName(string name)
		{
			RoomName = name;
			RoomNameText.text = RoomName;
		}

		#endregion

		#region Pirvate Methods

		private void OnDestroy()
		{
			Button button = GetComponent<Button>();
			button.onClick.RemoveAllListeners();
		}

		#endregion
	}
}
