using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC;

namespace MasterSeatFix
{
	public static class BuildInfo
	{
		public const string Name = "MasterSeatFix"; // Name of the Mod.  (MUST BE SET)
		public const string Author = "Magic3000"; // Author of the Mod.  (Set as null if none)
		public const string Company = null; // Company that made the Mod.  (Set as null if none)
		public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
		public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
	}
	public class MasterSeatFixMain : MelonMod
	{
        public override void OnApplicationStart() => MelonCoroutines.Start(InitButton());

        public static IEnumerator InitButton()
		{
			_sout("Starting MasterSeatFix...", ConsoleColor.Yellow);
			while (quickMenu == null)
				yield return null;
			MasterSeatFix = InstantiateGameobject("back", "");
			CreateButton(MasterSeatFix, "Master-seat Fix", "Fix world-master from seating on VRC_Station", 0, new Color(1f, 1f, 1f), 1f, 3f, MasterSeatFixMain.quickMenu.transform.Find("CameraMenu").gameObject.transform, new Action(() =>
			{
				FixMasterSitting();
			}));
			_sout("MasterSeatFix loaded!", ConsoleColor.Cyan);
			yield break;
		}

		private static void FixMasterSitting()
		{
			Room currentRoom = PhotonNetwork.prop_Room_0;
			if (currentRoom == null)
				_sout("FixMasterSitting currentRoom is null", ConsoleColor.Red);
			else
			{
				var ppList = new System.Collections.Generic.Dictionary<int, Photon.Realtime.Player>();
				foreach (var item in currentRoom.prop_Dictionary_2_Int32_Player_0)
					ppList[item.Key] = item.Value;
				if (ppList == null || ppList.Count == 0)
				{
					_sout("FixMasterSitting photonPlayerList is null", ConsoleColor.Red);
					return;
                }
				var arr = ppList.Select(x => x.Key).ToArray();
				Array.Sort(arr);
				VRC.Player masterPlayer = PlayerManager.Method_Public_Static_Player_Int32_0(arr.FirstOrDefault());
				if (masterPlayer == null)
				{
					_sout("FixMasterSitting masterPlayer is null", ConsoleColor.Red);
					return;
				}
				foreach (VRC_StationInternal vrc_station in Resources.FindObjectsOfTypeAll<VRC_StationInternal>())
				{
					if (vrc_station != null && vrc_station.prop_Player_0 == masterPlayer)
					{
						vrc_station.InteractWithStationRPC(false, masterPlayer);
						_sout("Forced vrc_station exit for master", ConsoleColor.Cyan);
					}
				}
			}
		}

		/*public static VRC.Player GetPlayer(int int32_0)
		{
			object players = playermanager.field_Private_List_1_Player_0;
			object obj = players;
			VRC.Player result;
			lock (obj)
			{
				VRC.Player player;
				playermanager.field_Private_Dictionary_2_Int32_Player_0.TryGetValue(int32_0, out player);
				result = player;
			}
			return result;
		}*/

		public static void _sout(object _in, ConsoleColor color = ConsoleColor.White)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(_in.ToString());
			Console.ForegroundColor = ConsoleColor.White;
		}

		internal static Transform InstantiateGameobject(string type, string go = "")
		{
			//("InstantiateGameobject for " + type + ", " + go)._sout(Green);
			if (type == "back")
				return UnityEngine.Object.Instantiate<GameObject>(quickMenu.transform.Find("CameraMenu/BackButton").gameObject).transform;
			if (type == "nameplates")
				return UnityEngine.Object.Instantiate<GameObject>(quickMenu.transform.Find("UIElementsMenu/ToggleHUDButton").gameObject).transform;
			if (type == "block")
				return UnityEngine.Object.Instantiate<GameObject>(quickMenu.transform.Find("NotificationInteractMenu/BlockButton").gameObject).transform;
			if (type == "next")
				return UnityEngine.Object.Instantiate<GameObject>(quickMenu.transform.Find("QuickMenu_NewElements/_CONTEXT/QM_Context_User_Selected/NextArrow_Button").gameObject).transform;
			if (type == "prev")
				return UnityEngine.Object.Instantiate<GameObject>(quickMenu.transform.Find("QuickMenu_NewElements/_CONTEXT/QM_Context_User_Selected/PreviousArrow_Button").gameObject).transform;
			if (type == "emojimenu")
				return UnityEngine.Object.Instantiate<GameObject>(quickMenu.transform.Find("EmojiMenu").gameObject).transform;
			if (type == "EarlyAccessText")
				return UnityEngine.Object.Instantiate<GameObject>(quickMenu.transform.Find("ShortcutMenu/EarlyAccessText").gameObject).transform;
			if (!string.IsNullOrEmpty(go))
				_sout(("InstantiateGameobject " + type + " for " + go + " is null", ConsoleColor.Red));
			throw new ArgumentOutOfRangeException(type);
		}

		public static void CreateButton(Transform transform, string text, string tooltip, int textSize, Color color, float x_pos, float y_pos, Transform parent, UnityAction listener)
		{
			float x_button = quickMenu.transform.Find("UserInteractMenu/WarnButton").localPosition.x - quickMenu.transform.Find("UserInteractMenu/BanButton").localPosition.x;
			float y_button = quickMenu.transform.Find("UserInteractMenu/WarnButton").localPosition.x - quickMenu.transform.Find("UserInteractMenu/BanButton").localPosition.x;
			transform.GetComponentInChildren<Text>().text = text;
			if (tooltip != "null")
				transform.GetComponentInChildren<UiTooltip>().field_Public_String_0 = tooltip;
			if (textSize != 0)
				transform.GetComponentInChildren<Text>().fontSize = textSize;
			transform.GetComponentInChildren<Text>().color = color;
			if (x_pos == 0 && y_pos == 0)
			{
				transform.localPosition = transform.localPosition;
			}
			else
			{
				transform.localPosition = new Vector3(transform.localPosition.x + x_button * x_pos, transform.localPosition.y + y_button * y_pos, transform.localPosition.z);
			}
			transform.SetParent(parent, false);
			transform.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
			transform.GetComponent<Button>().onClick.AddListener(listener);
			transform.gameObject.name = text;
		}

		internal static PlayerManager playermanager
		{
			get
			{
				return PlayerManager.prop_PlayerManager_0;
			}
		}

		internal static QuickMenu quickMenu
		{
			get
			{
				return QuickMenu.prop_QuickMenu_0;
			}
		}

		private static Transform MasterSeatFix;
	}
}
