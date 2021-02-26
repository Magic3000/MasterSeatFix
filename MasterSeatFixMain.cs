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
		public override void OnApplicationStart()
		{
			MelonCoroutines.Start(MasterSeatFixMain.InitButton());
		}

		public static IEnumerator InitButton()
		{
			MasterSeatFixMain._sout("Starting MasterSeatFix...", ConsoleColor.Yellow);
			while (MasterSeatFixMain.quickMenu == null)
			{
				yield return null;
			}
			MasterSeatFixMain.MasterSeatFix = MasterSeatFixMain.InstantiateGameobject("back", "");
			MasterSeatFixMain.CreateButton(MasterSeatFixMain.MasterSeatFix, "Master-seat Fix", "Fix world-master from seating on VRC_Station", 0, new Color(1f, 1f, 1f), 1f, 3f, MasterSeatFixMain.quickMenu.transform.Find("CameraMenu").gameObject.transform, new Action(() =>
			{
				MasterSeatFixMain.FixMasterSitting();
			}));
			MasterSeatFixMain._sout("MasterSeatFix loaded!", ConsoleColor.Cyan);
			yield break;
		}

		private static void FixMasterSitting()
		{
			Room currentRoom = PhotonNetwork.prop_Room_0;
			bool flag = currentRoom == null;
			if (flag)
			{
				MasterSeatFixMain._sout("FixMasterSitting currentRoom is null", ConsoleColor.Red);
			}
			else
			{
				System.Collections.Generic.Dictionary<int, Photon.Realtime.Player> ppList = new System.Collections.Generic.Dictionary<int, Photon.Realtime.Player>();
				foreach (Il2CppSystem.Collections.Generic.KeyValuePair<int, Photon.Realtime.Player> item in currentRoom.prop_Dictionary_2_Int32_Player_0)
				{
					ppList[item.Key] = item.Value;
				}
				bool flag2 = ppList == null || ppList.Count == 0;
				if (flag2)
				{
					MasterSeatFixMain._sout("FixMasterSitting photonPlayerList is null", ConsoleColor.Red);
				}
				else
				{
					int[] arr = (from x in ppList
								 select x.Key).ToArray<int>();
					Array.Sort<int>(arr);
					VRC.Player masterPlayer = MasterSeatFixMain.GetPlayer(arr.FirstOrDefault<int>());
					foreach (VRC_StationInternal vrc_station in Resources.FindObjectsOfTypeAll<VRC_StationInternal>())
					{
						bool flag3 = vrc_station != null && vrc_station.prop_Player_0 == masterPlayer;
						if (flag3)
						{
							vrc_station.InteractWithStationRPC(false, masterPlayer);
							MasterSeatFixMain._sout("Forced vrc_station exit for master", ConsoleColor.Cyan);
						}
					}
				}
			}
		}

		public static VRC.Player GetPlayer(int int32_0)
		{
			object players = MasterSeatFixMain.playermanager.field_Private_List_1_Player_0;
			object obj = players;
			VRC.Player result;
			lock (obj)
			{
				VRC.Player player;
				MasterSeatFixMain.playermanager.field_Private_Dictionary_2_Int32_Player_0.TryGetValue(int32_0, out player);
				result = player;
			}
			return result;
		}

		public static void _sout(object _in, ConsoleColor color = ConsoleColor.White)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(_in.ToString());
			Console.ForegroundColor = ConsoleColor.White;
		}

		internal static Transform InstantiateGameobject(string type, string go = "")
		{
			bool flag = type == "back";
			Transform transform;
			if (flag)
			{
				transform = UnityEngine.Object.Instantiate<GameObject>(MasterSeatFixMain.quickMenu.transform.Find("CameraMenu/BackButton").gameObject).transform;
			}
			else
			{
				bool flag2 = type == "nameplates";
				if (flag2)
				{
					transform = UnityEngine.Object.Instantiate<GameObject>(MasterSeatFixMain.quickMenu.transform.Find("UIElementsMenu/ToggleHUDButton").gameObject).transform;
				}
				else
				{
					bool flag3 = type == "block";
					if (flag3)
					{
						transform = UnityEngine.Object.Instantiate<GameObject>(MasterSeatFixMain.quickMenu.transform.Find("NotificationInteractMenu/BlockButton").gameObject).transform;
					}
					else
					{
						bool flag4 = type == "next";
						if (flag4)
						{
							transform = UnityEngine.Object.Instantiate<GameObject>(MasterSeatFixMain.quickMenu.transform.Find("QuickMenu_NewElements/_CONTEXT/QM_Context_User_Selected/NextArrow_Button").gameObject).transform;
						}
						else
						{
							bool flag5 = type == "prev";
							if (flag5)
							{
								transform = UnityEngine.Object.Instantiate<GameObject>(MasterSeatFixMain.quickMenu.transform.Find("QuickMenu_NewElements/_CONTEXT/QM_Context_User_Selected/PreviousArrow_Button").gameObject).transform;
							}
							else
							{
								bool flag6 = type == "emojimenu";
								if (flag6)
								{
									transform = UnityEngine.Object.Instantiate<GameObject>(MasterSeatFixMain.quickMenu.transform.Find("EmojiMenu").gameObject).transform;
								}
								else
								{
									bool flag7 = type == "EarlyAccessText";
									if (!flag7)
									{
										bool flag8 = !string.IsNullOrEmpty(go);
										if (flag8)
										{
											MasterSeatFixMain._sout(string.Concat(new string[]
											{
												"InstantiateGameobject ",
												type,
												" for ",
												go,
												" is null"
											}), ConsoleColor.Red);
										}
										throw new ArgumentOutOfRangeException(type);
									}
									transform = UnityEngine.Object.Instantiate<GameObject>(MasterSeatFixMain.quickMenu.transform.Find("ShortcutMenu/EarlyAccessText").gameObject).transform;
								}
							}
						}
					}
				}
			}
			return transform;
		}

		public static void CreateButton(Transform transform, string text, string tooltip, int textSize, Color color, float x_pos, float y_pos, Transform parent, UnityAction listener)
		{
			float x_button = MasterSeatFixMain.quickMenu.transform.Find("UserInteractMenu/WarnButton").localPosition.x - MasterSeatFixMain.quickMenu.transform.Find("UserInteractMenu/BanButton").localPosition.x;
			float y_button = MasterSeatFixMain.quickMenu.transform.Find("UserInteractMenu/WarnButton").localPosition.x - MasterSeatFixMain.quickMenu.transform.Find("UserInteractMenu/BanButton").localPosition.x;
			transform.GetComponentInChildren<Text>().text = text;
			bool flag = tooltip != "null";
			if (flag)
			{
				transform.GetComponentInChildren<UiTooltip>().field_Public_String_0 = tooltip;
			}
			bool flag2 = textSize != 0;
			if (flag2)
			{
				transform.GetComponentInChildren<Text>().fontSize = textSize;
			}
			transform.GetComponentInChildren<Text>().color = color;
			bool flag3 = x_pos == 0f && y_pos == 0f;
			if (flag3)
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
