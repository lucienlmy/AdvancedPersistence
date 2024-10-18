using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace AdvancedPersistence;

public static class GUI
{
	public class RGBA
	{
		public int Red;

		public int Green;

		public int Blue;

		public int Alpha;

		public RGBA(int r, int g, int b, int a = 255)
		{
			Red = r;
			Green = g;
			Blue = b;
			Alpha = a;
		}
	}

	public static class Phone
	{
		public enum HomescreenLocation
		{
			TopLeft,
			TopMiddle,
			TopRight,
			MiddleLeft,
			Middle,
			MiddleRight,
			BottomLeft,
			BottomMiddle,
			BottomRight
		}

		public enum HomeIcon
		{
			Camera = 1,
			TextMessage = 2,
			Calendar = 3,
			Email = 4,
			Call = 5,
			Eyefind = 6,
			Map = 7,
			Apps = 8,
			Media = 9,
			NewContact = 11,
			BAWSAQ = 13,
			Multiplayer = 14,
			Music = 15,
			GPS = 16,
			Spare = 17,
			Settings2 = 24,
			MissedCall = 27,
			UnreadEmail = 28,
			ReadEmail = 29,
			ReplyEmail = 30,
			ReplayMission = 31,
			ShitSkip = 32,
			UnreadSMS = 33,
			ReadSMS = 34,
			PlayerList = 35,
			CopBackup = 36,
			GangTaxi = 37,
			RepeatPlay = 38,
			Sniper = 40,
			ZitIT = 41,
			Trackify = 42,
			Save = 43,
			AddTag = 44,
			RemoveTag = 45,
			Location = 46,
			Party = 47,
			Broadcast = 49,
			Gamepad = 50,
			Silent = 51,
			InvitesPending = 52,
			OnCall = 53,
			HLock = 54,
			PushToTalk = 55,
			Bennys = 56,
			Gang = 57,
			Tracker = 58,
			SightSeer = 59,
			Beacon = 60
		}

		public enum ListIcons
		{
			None = 0,
			Attachment = 10,
			SideTasks = 12,
			RingTone = 18,
			TextTone = 19,
			VibrateOn = 20,
			VibrateOff = 21,
			Volume = 22,
			Settings1 = 23,
			Profile = 25,
			SleepMode = 26,
			Checklist = 39,
			Ticked = 48,
			Silent = 51
		}

		public enum Theme
		{
			LightBlue = 1,
			Green,
			Red,
			Orange,
			Grey,
			Purple,
			Pink,
			Black
		}

		public enum Direction
		{
			Up = 1,
			Right,
			Down,
			Left
		}

		public enum BackgroundImage
		{
			Default = 0,
			None = 1,
			PurpleGlow = 4,
			GreenSquares = 5,
			OrangeHerringBone = 6,
			OrangeHalfTone = 7,
			GreenTriangles = 8,
			GreenShards = 9,
			BlueAngles = 10,
			BlueShards = 11,
			BlueCircles = 12,
			Diamonds = 13,
			GreenGlow = 14,
			Orange8Bit = 15,
			OrangeTriangles = 16,
			PurpleTartan = 17
		}

		public enum SoftKey
		{
			Left = 1,
			Middle,
			Right
		}

		public enum SoftkeyIcon
		{
			Blank = 1,
			Select,
			Pages,
			Back,
			Call,
			Hangup,
			Hangup_Human,
			Hide_Phone,
			Keypad,
			Open,
			Reply,
			Delete,
			Yes,
			No,
			Sort,
			Website,
			Police,
			Ambulance,
			Fire,
			Pages2
		}

		public enum AppContainer
		{
			HomeMenu = 1,
			Contacts = 2,
			CallScreen = 4,
			MessageList = 6,
			MessageView = 7,
			EmailList = 8,
			EmailView = 9,
			Settings = 22,
			ToDoList = 17,
			TodoView = 15,
			MissionRepeat = 18,
			MissionStats = 19,
			JobList = 20,
			EmailResponse = 21
		}

		public interface AppItem
		{
			string Type { get; set; }

			AppObject Forward { get; set; }

			Action Invoker { get; set; }

			string Id { get; set; }

			Action OnSoftkey_Left { get; set; }

			Action OnSoftkey_Right { get; set; }

			Action OnSoftkey_Middle { get; set; }
		}

		public class AppSettingItem : AppItem
		{
			public string Name;

			public ListIcons Icon;

			public string Type { get; set; } = "Setting";


			public string Id { get; set; }

			public AppObject Forward { get; set; }

			public Action Invoker { get; set; }

			public Action OnSoftkey_Left { get; set; }

			public Action OnSoftkey_Right { get; set; }

			public Action OnSoftkey_Middle { get; set; }

			public AppSettingItem(string id, string name, ListIcons icon, Action invoke = null, AppObject forward = null, Action onleft = null, Action onright = null, Action onmiddle = null)
			{
				Id = id;
				Name = name;
				Icon = icon;
				Forward = forward;
				Invoker = invoke;
				OnSoftkey_Left = onleft;
				OnSoftkey_Middle = onmiddle;
				OnSoftkey_Right = onright;
			}
		}

		public class AppMessageItem : AppItem
		{
			public string Hour;

			public string Minute;

			public bool Seen;

			public string FromAddress;

			public string SubjectTitle;

			public string Type { get; set; } = "Message";


			public string Id { get; set; }

			public AppObject Forward { get; set; }

			public Action Invoker { get; set; }

			public Action OnSoftkey_Left { get; set; }

			public Action OnSoftkey_Right { get; set; }

			public Action OnSoftkey_Middle { get; set; }

			public AppMessageItem(string id, string hour, string minute, bool seen, string from, string subject, Action act = null, AppObject forward = null, Action onleft = null, Action onright = null, Action onmiddle = null)
			{
				Id = id;
				Hour = hour;
				Minute = minute;
				Seen = seen;
				FromAddress = from;
				SubjectTitle = subject;
				Forward = forward;
				Invoker = act;
				OnSoftkey_Left = onleft;
				OnSoftkey_Middle = onmiddle;
				OnSoftkey_Right = onright;
			}
		}

		public class AppCallscreenItem : AppItem
		{
			public string FromAddress;

			public string JobTitle;

			public string Icon;

			public string Type { get; set; } = "Message";


			public string Id { get; set; }

			public AppObject Forward { get; set; }

			public Action Invoker { get; set; }

			public Action OnSoftkey_Left { get; set; }

			public Action OnSoftkey_Right { get; set; }

			public Action OnSoftkey_Middle { get; set; }

			public AppCallscreenItem(string id, string from, string title, string icon, Action act = null, AppObject forward = null, Action onleft = null, Action onright = null, Action onmiddle = null)
			{
				Id = id;
				FromAddress = from;
				JobTitle = title;
				Icon = icon;
				Forward = forward;
				Invoker = act;
				OnSoftkey_Left = onleft;
				OnSoftkey_Middle = onmiddle;
				OnSoftkey_Right = onright;
			}
		}

		public class AppMessageViewItem : AppItem
		{
			public string FromAddress;

			public string Message;

			public string Icon = "CHAR_HUMANDEFAULT";

			public string Type { get; set; } = "Message";


			public string Id { get; set; }

			public AppObject Forward { get; set; }

			public Action Invoker { get; set; }

			public Action OnSoftkey_Left { get; set; }

			public Action OnSoftkey_Right { get; set; }

			public Action OnSoftkey_Middle { get; set; }

			public AppMessageViewItem(string from, string msg, string icon, Action act = null, AppObject forward = null, Action onleft = null, Action onright = null, Action onmiddle = null)
			{
				FromAddress = from;
				Message = msg;
				Icon = icon;
				Invoker = act;
				Forward = forward;
				OnSoftkey_Left = onleft;
				OnSoftkey_Middle = onmiddle;
				OnSoftkey_Right = onright;
			}
		}

		public class AppContactItem : AppItem
		{
			public bool MissedCall;

			public string Name;

			public string Icon;

			public string Type { get; set; } = "Message";


			public AppObject Forward { get; set; }

			public string Id { get; set; }

			public Action Invoker { get; set; }

			public Action OnSoftkey_Left { get; set; }

			public Action OnSoftkey_Right { get; set; }

			public Action OnSoftkey_Middle { get; set; }

			public AppContactItem(string id, bool missedcall, string name, string icon, Action act = null, AppObject forward = null, Action onleft = null, Action onright = null, Action onmiddle = null)
			{
				Id = id;
				MissedCall = missedcall;
				Name = name;
				Icon = icon;
				Invoker = act;
				Forward = forward;
				OnSoftkey_Left = onleft;
				OnSoftkey_Middle = onmiddle;
				OnSoftkey_Right = onright;
			}
		}

		public class AppObject
		{
			public string Name;

			public AppContainer Container;

			public List<AppItem> Items = new List<AppItem>();

			public AppObject Backward;

			public Action OnBack;

			public Action Invoker;

			public SoftkeyObject SoftKey_Left = new SoftkeyObject(SoftkeyIcon.Blank, vis: false, new RGBA(0, 0, 0));

			public SoftkeyObject SoftKey_Right = new SoftkeyObject(SoftkeyIcon.Blank, vis: false, new RGBA(0, 0, 0));

			public SoftkeyObject SoftKey_Middle = new SoftkeyObject(SoftkeyIcon.Blank, vis: false, new RGBA(0, 0, 0));

			public Action OnSoftKey_Right;

			public Action OnSoftKey_Left;

			public Action OnSoftKey_Middle;

			public int Selection;

			public AppObject(string name, AppContainer cont)
			{
				SoftKey_Left = new SoftkeyObject(SoftkeyIcon.Select, vis: true, new RGBA(46, 204, 113));
				SoftKey_Right = new SoftkeyObject(SoftkeyIcon.Back, vis: true, new RGBA(255, 255, 255));
				Name = name;
				Container = cont;
			}

			public T GetItemByID<T>(string id)
			{
				return (T)Items.FirstOrDefault((AppItem x) => x.Id == id);
			}

			public void AddItem(object item)
			{
				if (Container == AppContainer.Settings)
				{
					if (item is AppSettingItem)
					{
						Items.Add((AppItem)item);
					}
				}
				else if (Container == AppContainer.MessageList)
				{
					if (item is AppMessageItem)
					{
						Items.Add((AppItem)item);
					}
				}
				else if (Container == AppContainer.MessageView)
				{
					if (item is AppMessageViewItem)
					{
						Items.Add((AppItem)item);
					}
				}
				else if (Container == AppContainer.Contacts)
				{
					if (item is AppContactItem)
					{
						Items.Add((AppItem)item);
					}
				}
				else if (Container == AppContainer.CallScreen && item is AppCallscreenItem)
				{
					Items.Add((AppItem)item);
				}
			}
		}

		public class HomeObject
		{
			public int NotificationNumber = 5;

			public AppObject Forward;

			public HomeIcon Icon;

			public string Name = "NULL";

			public HomescreenLocation Location;

			public int Alpha = 100;

			public HomeObject(string name, HomeIcon icon, HomescreenLocation location, int notifications, AppObject link = null, int alpha = 100)
			{
				Name = name;
				NotificationNumber = notifications;
				Icon = icon;
				Location = location;
				Forward = link;
				Alpha = alpha;
			}
		}

		public class SoftkeyObject
		{
			public SoftkeyIcon Icon = SoftkeyIcon.Blank;

			public bool Visible;

			public RGBA RGBA = new RGBA(255, 255, 255);

			public SoftkeyObject(SoftkeyIcon icon, bool vis, RGBA col)
			{
				Icon = icon;
				Visible = vis;
				RGBA = col;
			}
		}

		public static Theme ActiveTheme = Theme.Red;

		public static int PhoneColor = 0;

		public static int PhoneBrightness = 5;

		private static AppObject ControlMain = null;

		private static AppObject AppScroll = null;

		private static AppObject AppScroll2 = null;

		private static AppObject AppScroll3 = null;

		private static AppObject Converter = null;

		private static AppObject CurrentVehicleApp = null;

		private static AppObject VehicleMarketApp = null;

		private static AppObject VehicleTemplate = null;

		private static AppObject VehicleTemplate2 = null;

		private static AppObject VehicleTemplate3 = null;

		private static AppObject VehicleTemplate4 = null;

		private static AppObject VehicleTemplate_Doors = null;

		private static AppObject VehicleTemplate_Engine = null;

		private static AppObject VehicleTemplate_Alarm = null;

		private static AppObject VehicleTemplate_Lights = null;

		private static AppObject VehicleTemplate_Windows = null;

		private static AppObject VehicleTemplate_Neons = null;

		private static AppObject VehicleTemplate_Anchor = null;

		private static AppObject ChangelogApp = null;

		private static AppObject Settings = null;

		private static AppObject About = null;

		private static AppObject ThemeApp = null;

		private static AppObject BackApp = null;

		private static AppObject BrightnessApp = null;

		private static AppObject ColorApp = null;

		private static AppObject ToneApp = null;

		private static AppObject ModelApp = null;

		private static AppObject PhoneSettings = null;

		private static AppObject BlipApp = null;

		public static VehicleDataV1 activatedCar = null;

		public static int PhoneTone = 0;

		public static int PhoneModel = 0;

		public static int Interface = 0;

		private static Vector3 PhonePosition_Final = new Vector3(99.62f, -51f, -113f);

		private static Vector3 PhoneRotation_Final = new Vector3(-90f, 0f, 0f);

		private static Vector3 PhonePosition_Start = new Vector3(99.62f, -150f, -113f);

		private static Vector3 PhoneRotation_Start = new Vector3(-90f, 180f, 0f);

		private static Vector3 PhonePosition_Current = new Vector3(99.62f, -180f, -113f);

		private static Vector3 PhoneRotation_Current = new Vector3(-90f, 180f, 0f);

		private static float PhoneScale = 525f;

		private static int PhoneRenderID = -1;

		public static Scaleform PhoneScaleform;

		public static bool RawOn = false;

		private static int HomescreenSelection = 0;

		private static int CurrentAppSelection = 0;

		public static AppObject CurrentApp = null;

		public static BackgroundImage HomescreenImage = BackgroundImage.BlueAngles;

		private static List<List<HomeObject>> HomeObjects_Stored = new List<List<HomeObject>>
		{
			new List<HomeObject> { null, null, null, null, null, null, null, null, null },
			new List<HomeObject> { null, null, null, null, null, null, null, null, null },
			new List<HomeObject> { null, null, null, null, null, null, null, null, null }
		};

		private static SoftkeyObject SoftkeyLeft = new SoftkeyObject(SoftkeyIcon.Blank, vis: false, new RGBA(0, 0, 0));

		private static SoftkeyObject SoftkeyRight = new SoftkeyObject(SoftkeyIcon.Blank, vis: false, new RGBA(0, 0, 0));

		private static SoftkeyObject SoftkeyMiddle = new SoftkeyObject(SoftkeyIcon.Blank, vis: false, new RGBA(0, 0, 0));

		public static SoftkeyObject Home_SoftkeyLeft = new SoftkeyObject(SoftkeyIcon.Select, vis: true, new RGBA(46, 204, 113));

		public static SoftkeyObject Home_SoftkeyRight = new SoftkeyObject(SoftkeyIcon.Blank, vis: false, new RGBA(0, 0, 0));

		public static SoftkeyObject Home_SoftkeyMiddle = new SoftkeyObject(SoftkeyIcon.Blank, vis: false, new RGBA(0, 0, 0));

		public static bool SleepMode = false;

		public static bool TriggerLoaded = false;

		private static float CurLerp = 0f;

		private static float TimeLerp = 1.5f;

		private static float BlackLerp = 1f;

		private static float BlackLerpInverse = 0.125f;

		private static bool DoLerpUp = false;

		private static float BlackValue = 255f;

		private static bool DoLerpDown = false;

		private static bool DoBlackLerp = false;

		private static bool DoBlackLerpInverse = false;

		private static float CurBlackLerp = 0f;

		public static bool IsOn { get; private set; } = false;


		public static void RedoScroll()
		{
			try
			{
				SetDataSlotEmpty((int)AppScroll.Container);
				AppScroll.Items.Clear();
				SetDataSlotEmpty((int)AppScroll2.Container);
				AppScroll2.Items.Clear();
				SetDataSlotEmpty((int)ControlMain.Container);
				ControlMain.Items.Clear();
				SetDataSlotEmpty((int)AppScroll3.Container);
				AppScroll3.Items.Clear();
				if (GetHomeObjectByIndex(0) != null)
				{
					GetHomeObjectByIndex(0).NotificationNumber = AdvancedPersistence.VehicleDatabase.Count;
				}
				int num = 1;
				foreach (VehicleDataV1 dat in AdvancedPersistence.VehicleDatabase)
				{
					string text = Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, dat.Hash);
					string text2 = Function.Call<string>(Hash.GET_FILENAME_FOR_AUDIO_CONVERSATION, text);
					if (text2 == "NULL" && dat.Handle != null && dat.Handle.Exists())
					{
						text2 = dat.Handle.Mods.LicensePlate;
					}
					string name = $"[{num}]: {text2}";
					if (dat.Tag != "" && !string.IsNullOrEmpty(dat.Tag) && !string.IsNullOrWhiteSpace(dat.Tag))
					{
						name = dat.Tag;
					}
					AppScroll.AddItem(new AppSettingItem(dat.Id, name, ListIcons.None, delegate
					{
						string text9 = Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, dat.Hash);
						string text10 = Function.Call<string>(Hash.GET_FILENAME_FOR_AUDIO_CONVERSATION, text9);
						if (text10 == "NULL" && dat.Handle != null && dat.Handle.Exists())
						{
							text10 = dat.Handle.Mods.LicensePlate;
						}
						VehicleTemplate2.Name = text10;
						if (dat.Tag != "")
						{
							VehicleTemplate2.Name = dat.Tag;
						}
						activatedCar = dat;
						if (activatedCar != null && activatedCar.Handle != null)
						{
							AdvancedPersistence.DrawTrace = true;
							if (activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null && ModSettings.EnableBlips && activatedCar.Handle.AttachedBlip.Exists())
							{
								if (activatedCar.Handle.Model.IsHelicopter)
								{
									activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
								}
								else if (activatedCar.Handle.Model.IsAmphibiousQuadBike || activatedCar.Handle.Model.IsBicycle || activatedCar.Handle.Model.IsBike || activatedCar.Handle.Model.IsQuadBike)
								{
									activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
								}
								else if (activatedCar.Handle.Model.IsJetSki)
								{
									activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Seashark;
								}
								else if (activatedCar.Handle.Model.IsBoat)
								{
									activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Boat;
								}
								else if (activatedCar.Handle.Model.IsPlane)
								{
									activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Plane;
								}
								else
								{
									activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
								}
								activatedCar.Handle.AttachedBlip.IsShortRange = true;
								activatedCar.Handle.AttachedBlip.Scale = 0.75f;
								activatedCar.Handle.AttachedBlip.Name = "Saved Vehicle";
								activatedCar.Handle.AttachedBlip.Priority = 255;
								Function.Call(Hash.SHOW_TICK_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, true);
								activatedCar.Handle.AttachedBlip.Color = (BlipColor)activatedCar.BlipColor;
								Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
							}
						}
						RefreshDisplay();
					}, VehicleTemplate2));
					ControlMain.AddItem(new AppSettingItem(dat.Id, name, ListIcons.None, delegate
					{
						string text7 = Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, dat.Hash);
						string text8 = Function.Call<string>(Hash.GET_FILENAME_FOR_AUDIO_CONVERSATION, text7);
						if (text8 == "NULL" && dat.Handle != null && dat.Handle.Exists())
						{
							text8 = dat.Handle.Mods.LicensePlate;
						}
						VehicleTemplate.Name = text8;
						if (dat.Tag != "" && !string.IsNullOrEmpty(dat.Tag) && !string.IsNullOrWhiteSpace(dat.Tag))
						{
							VehicleTemplate.Name = dat.Tag;
						}
						activatedCar = dat;
						if (activatedCar != null)
						{
							if (activatedCar.Handle != null)
							{
								VehicleTemplate.GetItemByID<AppSettingItem>("id_engine_stat").Name = (activatedCar.Handle.IsEngineRunning ? "ENGINE: ON" : "ENGINE: OFF");
								VehicleTemplate.GetItemByID<AppSettingItem>("id_lights_stat").Name = ((activatedCar.Handle.AreLightsOn || activatedCar.Handle.AreHighBeamsOn) ? "LIGHTS: ON" : "LIGHTS: OFF");
								VehicleTemplate.GetItemByID<AppSettingItem>("id_alarm_stat").Name = (activatedCar.Handle.IsAlarmSet ? "ALARM: ON" : "ALARM: OFF");
								VehicleTemplate.GetItemByID<AppSettingItem>("id_locked_stat").Name = ((activatedCar.Handle.LockStatus == VehicleLockStatus.CannotEnter) ? "LOCKED: TRUE" : "LOCKED: FALSE");
							}
							else
							{
								VehicleTemplate.GetItemByID<AppSettingItem>("id_engine_stat").Name = (activatedCar.EngineState ? "ENGINE: ON" : "ENGINE: OFF");
								VehicleTemplate.GetItemByID<AppSettingItem>("id_lights_stat").Name = ((activatedCar.LightState2 == 1 || activatedCar.LightState2 == 2) ? "LIGHTS: ON" : "LIGHTS: OFF");
								VehicleTemplate.GetItemByID<AppSettingItem>("id_alarm_stat").Name = (activatedCar.AlarmState ? "ALARM: ON" : "ALARM: OFF");
								VehicleTemplate.GetItemByID<AppSettingItem>("id_locked_stat").Name = (activatedCar.LockState ? "LOCKED: TRUE" : "LOCKED: FALSE");
							}
							if (activatedCar.Handle != null)
							{
								AdvancedPersistence.DrawTrace = true;
								if (activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null && ModSettings.EnableBlips && activatedCar.Handle.AttachedBlip.Exists())
								{
									if (activatedCar.Handle.Model.IsHelicopter)
									{
										activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
									}
									else if (activatedCar.Handle.Model.IsAmphibiousQuadBike || activatedCar.Handle.Model.IsBicycle || activatedCar.Handle.Model.IsBike || activatedCar.Handle.Model.IsQuadBike)
									{
										activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
									}
									else if (activatedCar.Handle.Model.IsJetSki)
									{
										activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Seashark;
									}
									else if (activatedCar.Handle.Model.IsBoat)
									{
										activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Boat;
									}
									else if (activatedCar.Handle.Model.IsPlane)
									{
										activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Plane;
									}
									else
									{
										activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
									}
									activatedCar.Handle.AttachedBlip.IsShortRange = true;
									activatedCar.Handle.AttachedBlip.Scale = 0.75f;
									activatedCar.Handle.AttachedBlip.Name = "Saved Vehicle";
									activatedCar.Handle.AttachedBlip.Priority = 255;
									Function.Call(Hash.SHOW_TICK_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, true);
									activatedCar.Handle.AttachedBlip.Color = (BlipColor)activatedCar.BlipColor;
									Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
								}
							}
						}
						RefreshDisplay();
					}, VehicleTemplate));
					AppScroll2.AddItem(new AppSettingItem(dat.Id, name, ListIcons.None, delegate
					{
						string text5 = Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, dat.Hash);
						string text6 = Function.Call<string>(Hash.GET_FILENAME_FOR_AUDIO_CONVERSATION, text5);
						if (text6 == "NULL" && dat.Handle != null && dat.Handle.Exists())
						{
							text6 = dat.Handle.Mods.LicensePlate;
						}
						VehicleTemplate3.Name = text6;
						if (dat.Tag != "" && !string.IsNullOrEmpty(dat.Tag) && !string.IsNullOrWhiteSpace(dat.Tag))
						{
							VehicleTemplate3.Name = dat.Tag;
						}
						activatedCar = dat;
						if (activatedCar != null)
						{
							if (activatedCar.Handle != null)
							{
								AdvancedPersistence.DrawTrace = true;
								if (Function.Call<bool>(Hash.IS_BOAT_ANCHORED, activatedCar.Handle.Handle))
								{
									VehicleTemplate4.GetItemByID<AppSettingItem>("id_anchor_status").Name = "ANCHOR: TRUE";
								}
								else
								{
									VehicleTemplate4.GetItemByID<AppSettingItem>("id_anchor_status").Name = "ANCHOR: FALSE";
								}
							}
							else
							{
								VehicleTemplate4.GetItemByID<AppSettingItem>("id_anchor_status").Name = "ANCHOR: FALSE";
							}
							if (activatedCar.Handle != null && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null && ModSettings.EnableBlips && activatedCar.Handle.AttachedBlip.Exists())
							{
								if (activatedCar.Handle.Model.IsHelicopter)
								{
									activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
								}
								else if (activatedCar.Handle.Model.IsAmphibiousQuadBike || activatedCar.Handle.Model.IsBicycle || activatedCar.Handle.Model.IsBike || activatedCar.Handle.Model.IsQuadBike)
								{
									activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
								}
								else if (activatedCar.Handle.Model.IsJetSki)
								{
									activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Seashark;
								}
								else if (activatedCar.Handle.Model.IsBoat)
								{
									activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Boat;
								}
								else if (activatedCar.Handle.Model.IsPlane)
								{
									activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Plane;
								}
								else
								{
									activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
								}
								activatedCar.Handle.AttachedBlip.IsShortRange = true;
								activatedCar.Handle.AttachedBlip.Scale = 0.75f;
								activatedCar.Handle.AttachedBlip.Name = "Saved Vehicle";
								activatedCar.Handle.AttachedBlip.Priority = 255;
								Function.Call(Hash.SHOW_TICK_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, true);
								activatedCar.Handle.AttachedBlip.Color = (BlipColor)activatedCar.BlipColor;
								Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
							}
						}
						RefreshDisplay();
					}, VehicleTemplate3));
					Model model = new Model(dat.Hash);
					if (model.IsBoat || model.IsAmphibiousVehicle || model.IsAmphibiousQuadBike || model.IsAmphibiousCar || model.IsSubmarineCar || model.IsJetSki || AdvancedPersistence.IsSubmarine(model) || AdvancedPersistence.IsAmphCar(model))
					{
						AppScroll3.AddItem(new AppSettingItem(dat.Id, name, ListIcons.None, delegate
						{
							string text3 = Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, dat.Hash);
							string text4 = Function.Call<string>(Hash.GET_FILENAME_FOR_AUDIO_CONVERSATION, text3);
							if (text4 == "NULL" && dat.Handle != null && dat.Handle.Exists())
							{
								text4 = dat.Handle.Mods.LicensePlate;
							}
							VehicleTemplate4.Name = text4;
							if (dat.Tag != "" && !string.IsNullOrEmpty(dat.Tag) && !string.IsNullOrWhiteSpace(dat.Tag))
							{
								VehicleTemplate4.Name = dat.Tag;
							}
							activatedCar = dat;
							if (activatedCar != null && activatedCar.Handle != null)
							{
								AdvancedPersistence.DrawTrace = true;
								if (activatedCar.Handle.Exists())
								{
									if (Function.Call<bool>(Hash.IS_BOAT_ANCHORED, activatedCar.Handle.Handle))
									{
										VehicleTemplate4.GetItemByID<AppSettingItem>("id_anchor_status").Name = "ANCHOR: TRUE";
									}
									else
									{
										VehicleTemplate4.GetItemByID<AppSettingItem>("id_anchor_status").Name = "ANCHOR: FALSE";
									}
									if (activatedCar.Handle.AttachedBlip != null && ModSettings.EnableBlips && activatedCar.Handle.AttachedBlip.Exists())
									{
										if (activatedCar.Handle.Model.IsHelicopter)
										{
											activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
										}
										else if (activatedCar.Handle.Model.IsAmphibiousQuadBike || activatedCar.Handle.Model.IsBicycle || activatedCar.Handle.Model.IsBike || activatedCar.Handle.Model.IsQuadBike)
										{
											activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
										}
										else if (activatedCar.Handle.Model.IsJetSki)
										{
											activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Seashark;
										}
										else if (activatedCar.Handle.Model.IsBoat)
										{
											activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Boat;
										}
										else if (activatedCar.Handle.Model.IsPlane)
										{
											activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Plane;
										}
										else
										{
											activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
										}
										activatedCar.Handle.AttachedBlip.IsShortRange = true;
										activatedCar.Handle.AttachedBlip.Scale = 0.75f;
										activatedCar.Handle.AttachedBlip.Name = "Saved Vehicle";
										activatedCar.Handle.AttachedBlip.Priority = 255;
										Function.Call(Hash.SHOW_TICK_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, true);
										activatedCar.Handle.AttachedBlip.Color = (BlipColor)activatedCar.BlipColor;
										Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
									}
								}
							}
							RefreshDisplay();
						}, VehicleTemplate4));
					}
					num++;
				}
			}
			catch (Exception ex)
			{
				Screen.ShowSubtitle("ERROR: " + ex.ToString());
			}
		}

		public static void Initialize()
		{
			ControlMain = new AppObject("Settings", AppContainer.Settings);
			Converter = new AppObject("Converter", AppContainer.Settings);
			AppScroll = new AppObject("List", AppContainer.Settings);
			AppScroll2 = new AppObject("List", AppContainer.Settings);
			AppScroll3 = new AppObject("List", AppContainer.Settings);
			VehicleTemplate = new AppObject("CAR", AppContainer.Settings);
			VehicleTemplate2 = new AppObject("CAR", AppContainer.Settings);
			VehicleTemplate3 = new AppObject("CAR", AppContainer.Settings);
			VehicleTemplate4 = new AppObject("BOAT", AppContainer.Settings);
			Settings = new AppObject("Settings", AppContainer.Settings);
			PhoneSettings = new AppObject("Phone Settings", AppContainer.Settings);
			PhoneSettings.Backward = Settings;
			About = new AppObject("About", AppContainer.Settings);
			About.Backward = Settings;
			ThemeApp = new AppObject("Theme", AppContainer.Settings);
			ThemeApp.Backward = PhoneSettings;
			BackApp = new AppObject("Background", AppContainer.Settings);
			BackApp.Backward = PhoneSettings;
			ColorApp = new AppObject("Color", AppContainer.Settings);
			ColorApp.Backward = PhoneSettings;
			CurrentVehicleApp = new AppObject("Current Vehicle", AppContainer.Settings);
			ChangelogApp = new AppObject("Changelog", AppContainer.MessageView);
			ChangelogApp.Backward = About;
			BrightnessApp = new AppObject("Brightness", AppContainer.Settings);
			BrightnessApp.Backward = PhoneSettings;
			ModelApp = new AppObject("Model", AppContainer.Settings);
			ModelApp.Backward = PhoneSettings;
			ToneApp = new AppObject("Tone", AppContainer.Settings);
			ToneApp.Backward = PhoneSettings;
			BlipApp = new AppObject("Blip Color", AppContainer.Settings);
			BlipApp.Backward = VehicleTemplate2;
			VehicleTemplate2.Backward = AppScroll;
			VehicleTemplate3.Backward = AppScroll2;
			VehicleTemplate4.Backward = AppScroll3;
			VehicleTemplate.Backward = ControlMain;
			VehicleTemplate_Doors = new AppObject("Doors", AppContainer.Settings);
			VehicleTemplate_Doors.Backward = VehicleTemplate;
			VehicleTemplate_Engine = new AppObject("Engine", AppContainer.Settings);
			VehicleTemplate_Engine.Backward = VehicleTemplate;
			VehicleTemplate_Alarm = new AppObject("Alarm", AppContainer.Settings);
			VehicleTemplate_Alarm.Backward = VehicleTemplate;
			VehicleTemplate_Lights = new AppObject("Lights", AppContainer.Settings);
			VehicleTemplate_Lights.Backward = VehicleTemplate;
			VehicleTemplate_Windows = new AppObject("Windows", AppContainer.Settings);
			VehicleTemplate_Windows.Backward = VehicleTemplate;
			VehicleTemplate_Neons = new AppObject("Neons", AppContainer.Settings);
			VehicleTemplate_Neons.Backward = VehicleTemplate;
			VehicleTemplate_Anchor = new AppObject("Anchoring", AppContainer.Settings);
			VehicleTemplate_Anchor.Backward = VehicleTemplate4;
			VehicleMarketApp = new AppObject("Vehicle Market", AppContainer.Settings);
			VehicleMarketApp.AddItem(new AppSettingItem("id_sell", "Sell Vehicle", ListIcons.Attachment));
			VehicleMarketApp.AddItem(new AppSettingItem("id_buy", "Buy Vehicle", ListIcons.Checklist));
			RedoScroll();
			VehicleTemplate.OnBack = delegate
			{
				AdvancedPersistence.DrawTrace = false;
				if (activatedCar.Handle != null && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null && ModSettings.EnableBlips && activatedCar.Handle.AttachedBlip.Exists())
				{
					if (activatedCar.Handle.Model.IsHelicopter)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
					}
					else if (activatedCar.Handle.Model.IsAmphibiousQuadBike || activatedCar.Handle.Model.IsBicycle || activatedCar.Handle.Model.IsBike || activatedCar.Handle.Model.IsQuadBike)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
					}
					else if (activatedCar.Handle.Model.IsJetSki)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Seashark;
					}
					else if (activatedCar.Handle.Model.IsBoat)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Boat;
					}
					else if (activatedCar.Handle.Model.IsPlane)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Plane;
					}
					else
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
					}
					activatedCar.Handle.AttachedBlip.IsShortRange = true;
					activatedCar.Handle.AttachedBlip.Color = (BlipColor)activatedCar.BlipColor;
					activatedCar.Handle.AttachedBlip.Scale = 0.75f;
					activatedCar.Handle.AttachedBlip.Name = "Saved Vehicle";
					activatedCar.Handle.AttachedBlip.Priority = 0;
					Function.Call(Hash.SHOW_TICK_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
					Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
				}
			};
			VehicleTemplate2.OnBack = delegate
			{
				AdvancedPersistence.DrawTrace = false;
				if (activatedCar.Handle != null && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null && ModSettings.EnableBlips && activatedCar.Handle.AttachedBlip.Exists())
				{
					if (activatedCar.Handle.Model.IsHelicopter)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
					}
					else if (activatedCar.Handle.Model.IsAmphibiousQuadBike || activatedCar.Handle.Model.IsBicycle || activatedCar.Handle.Model.IsBike || activatedCar.Handle.Model.IsQuadBike)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
					}
					else if (activatedCar.Handle.Model.IsJetSki)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Seashark;
					}
					else if (activatedCar.Handle.Model.IsBoat)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Boat;
					}
					else if (activatedCar.Handle.Model.IsPlane)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Plane;
					}
					else
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
					}
					activatedCar.Handle.AttachedBlip.IsShortRange = true;
					activatedCar.Handle.AttachedBlip.Color = (BlipColor)activatedCar.BlipColor;
					activatedCar.Handle.AttachedBlip.Scale = 0.75f;
					activatedCar.Handle.AttachedBlip.Name = "Saved Vehicle";
					activatedCar.Handle.AttachedBlip.Priority = 0;
					Function.Call(Hash.SHOW_TICK_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
					Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
				}
			};
			VehicleTemplate3.OnBack = delegate
			{
				AdvancedPersistence.DrawTrace = false;
				if (activatedCar.Handle != null && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null && ModSettings.EnableBlips && activatedCar.Handle.AttachedBlip.Exists())
				{
					if (activatedCar.Handle.Model.IsHelicopter)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
					}
					else if (activatedCar.Handle.Model.IsAmphibiousQuadBike || activatedCar.Handle.Model.IsBicycle || activatedCar.Handle.Model.IsBike || activatedCar.Handle.Model.IsQuadBike)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
					}
					else if (activatedCar.Handle.Model.IsJetSki)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Seashark;
					}
					else if (activatedCar.Handle.Model.IsBoat)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Boat;
					}
					else if (activatedCar.Handle.Model.IsPlane)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Plane;
					}
					else
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
					}
					activatedCar.Handle.AttachedBlip.IsShortRange = true;
					activatedCar.Handle.AttachedBlip.Color = (BlipColor)activatedCar.BlipColor;
					activatedCar.Handle.AttachedBlip.Scale = 0.75f;
					activatedCar.Handle.AttachedBlip.Name = "Saved Vehicle";
					activatedCar.Handle.AttachedBlip.Priority = 0;
					Function.Call(Hash.SHOW_TICK_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
					Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
				}
			};
			VehicleTemplate4.OnBack = delegate
			{
				AdvancedPersistence.DrawTrace = false;
				if (activatedCar.Handle != null && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null && ModSettings.EnableBlips && activatedCar.Handle.AttachedBlip.Exists())
				{
					if (activatedCar.Handle.Model.IsHelicopter)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
					}
					else if (activatedCar.Handle.Model.IsAmphibiousQuadBike || activatedCar.Handle.Model.IsBicycle || activatedCar.Handle.Model.IsBike || activatedCar.Handle.Model.IsQuadBike)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
					}
					else if (activatedCar.Handle.Model.IsJetSki)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Seashark;
					}
					else if (activatedCar.Handle.Model.IsBoat)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Boat;
					}
					else if (activatedCar.Handle.Model.IsPlane)
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Plane;
					}
					else
					{
						activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
					}
					activatedCar.Handle.AttachedBlip.IsShortRange = true;
					activatedCar.Handle.AttachedBlip.Color = (BlipColor)activatedCar.BlipColor;
					activatedCar.Handle.AttachedBlip.Scale = 0.75f;
					activatedCar.Handle.AttachedBlip.Name = "Saved Vehicle";
					activatedCar.Handle.AttachedBlip.Priority = 0;
					Function.Call(Hash.SHOW_TICK_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
					Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
				}
			};
			PhoneSettings.AddItem(new AppSettingItem("id_model", "Phone Model", ListIcons.Settings1, null, ModelApp));
			PhoneSettings.AddItem(new AppSettingItem("id_color", "Phone Tone", ListIcons.Settings1, null, ToneApp));
			PhoneSettings.AddItem(new AppSettingItem("id_color", "Phone Color", ListIcons.Settings1, null, ColorApp));
			PhoneSettings.AddItem(new AppSettingItem("id_theme", "Phone Theme", ListIcons.Settings1, null, ThemeApp));
			PhoneSettings.AddItem(new AppSettingItem("id_back", "Phone Background", ListIcons.Settings1, null, BackApp));
			PhoneSettings.AddItem(new AppSettingItem("id_bright", "Phone Brightness", ListIcons.Settings1, null, BrightnessApp));
			ChangelogApp.AddItem(new AppMessageViewItem("Current Version: " + 1.7f.ToString("0.00"), "=Version 1.70=\r\n-Fix some bugs and re-compile for latest version.\r\n", "CHAR_HUMANDEFAULT"));
			Settings.AddItem(new AppSettingItem("id_phsettings", "Phone Settings", ListIcons.Settings1, null, PhoneSettings));
			Settings.AddItem(new AppSettingItem("id_doors", "Fix Garage Doors", ListIcons.Ticked, delegate
			{
				if (!Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, 6969))
				{
					Function.Call(Hash.ADD_DOOR_TO_SYSTEM, 6969, Game.GenerateHash("prop_ch_025c_g_door_01"), 18.65038f, 546.3401f, 176.3448f, true, false, true);
				}
				Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, 6969, 10f, false, true);
				Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, 6969, 1f, false, true);
				if (!Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, 696969))
				{
					Function.Call(Hash.ADD_DOOR_TO_SYSTEM, 696969, Game.GenerateHash("prop_ld_garaged_01"), -815.2816f, 185.975f, 72.99993f, true, false, true);
				}
				Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, 696969, 10f, false, true);
				Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, 696969, 1f, false, true);
				if (!Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, 69696969))
				{
					Function.Call(Hash.ADD_DOOR_TO_SYSTEM, 69696969, Game.GenerateHash("prop_cs4_10_tr_gd_01"), 1972.787f, 3824.554f, 32.65174f, true, false, true);
				}
				Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, 69696969, 10f, false, true);
				Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, 69696969, 1f, false, true);
				Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, 6969, 0, true, true);
				Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, 696969, 0, true, true);
				Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, 69696969, 0, true, true);
				Screen.ShowSubtitle("Attempted fix. Check doors.", 3000);
			}));
			Settings.AddItem(new AppSettingItem("id_about", "About", ListIcons.Profile, null, About));
			About.AddItem(new AppSettingItem("id_mod", "Advanced Persistence", ListIcons.None));
			About.AddItem(new AppSettingItem("id_version", "Version: " + 1.7f.ToString("0.00"), ListIcons.Attachment));
			About.AddItem(new AppSettingItem("id_changelog", "Changelog", ListIcons.Checklist, null, ChangelogApp));
			About.AddItem(new AppSettingItem("id_author", "Author: systematic", ListIcons.Profile));
			ModelApp.AddItem(new AppSettingItem("id_ifruit", "iFruit Model", ListIcons.Settings1, delegate
			{
				PhoneModel = 0;
				Function.Call(Hash.DESTROY_MOBILE_PHONE);
				Function.Call(Hash.CREATE_MOBILE_PHONE, PhoneModel);
				Function.Call(Hash.SCRIPT_IS_MOVING_MOBILE_PHONE_OFFSCREEN, true);
				Script.Wait(1000);
				Function.Call(Hash.SCRIPT_IS_MOVING_MOBILE_PHONE_OFFSCREEN, false);
				Script.Wait(500);
				SetPhoneColor(PhoneColor);
			}));
			ModelApp.AddItem(new AppSettingItem("id_facade", "Facade Model", ListIcons.Settings1, delegate
			{
				PhoneModel = 1;
				Function.Call(Hash.DESTROY_MOBILE_PHONE);
				Function.Call(Hash.CREATE_MOBILE_PHONE, PhoneModel);
				Function.Call(Hash.SCRIPT_IS_MOVING_MOBILE_PHONE_OFFSCREEN, true);
				Script.Wait(1000);
				Function.Call(Hash.SCRIPT_IS_MOVING_MOBILE_PHONE_OFFSCREEN, false);
				Script.Wait(500);
				SetPhoneColor(PhoneColor);
			}));
			ModelApp.AddItem(new AppSettingItem("id_badger", "Badger Model", ListIcons.Settings1, delegate
			{
				PhoneModel = 2;
				Function.Call(Hash.DESTROY_MOBILE_PHONE);
				Function.Call(Hash.CREATE_MOBILE_PHONE, PhoneModel);
				Function.Call(Hash.SCRIPT_IS_MOVING_MOBILE_PHONE_OFFSCREEN, true);
				Script.Wait(1000);
				Function.Call(Hash.SCRIPT_IS_MOVING_MOBILE_PHONE_OFFSCREEN, false);
				Script.Wait(500);
				SetPhoneColor(PhoneColor);
			}));
			ToneApp.AddItem(new AppSettingItem("id_ifruit", "iFruit Tone", ListIcons.Settings1, delegate
			{
				PhoneTone = 0;
			}));
			ToneApp.AddItem(new AppSettingItem("id_facade", "Facade Tone", ListIcons.Settings1, delegate
			{
				PhoneTone = 1;
			}));
			ToneApp.AddItem(new AppSettingItem("id_badger", "Badger Tone", ListIcons.Settings1, delegate
			{
				PhoneTone = 2;
			}));
			BackApp.AddItem(new AppSettingItem("id_back1", "Blue Angles", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.BlueAngles);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back2", "Blue Circles", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.BlueCircles);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back3", "Blue Shards", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.BlueShards);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back4", "Default", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.Default);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back5", "Diamonds", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.Diamonds);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back6", "Green Glow", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.GreenGlow);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back7", "Green Shards", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.GreenShards);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back8", "Green Squares", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.GreenSquares);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back9", "Green Triangles", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.GreenTriangles);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back10", "None", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.None);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back11", "Orange 8Bit", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.Orange8Bit);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back12", "Orange Half Tone", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.OrangeHalfTone);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back13", "Orange Herring Bone", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.OrangeHerringBone);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back14", "Orange Triangles", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.OrangeTriangles);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back15", "Purple Glow", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.PurpleGlow);
				RefreshDisplay();
			}));
			BackApp.AddItem(new AppSettingItem("id_back16", "Purple Tartan", ListIcons.Settings1, delegate
			{
				SetBackgroundImage(BackgroundImage.PurpleTartan);
				RefreshDisplay();
			}));
			ThemeApp.AddItem(new AppSettingItem("id_red", "Red", ListIcons.Profile, delegate
			{
				SetTheme(Theme.Red);
				RefreshDisplay();
			}));
			ThemeApp.AddItem(new AppSettingItem("id_green", "Green", ListIcons.Profile, delegate
			{
				SetTheme(Theme.Green);
				RefreshDisplay();
			}));
			ThemeApp.AddItem(new AppSettingItem("id_blue", "Blue", ListIcons.Profile, delegate
			{
				SetTheme(Theme.LightBlue);
				RefreshDisplay();
			}));
			ThemeApp.AddItem(new AppSettingItem("id_orange", "Orange", ListIcons.Profile, delegate
			{
				SetTheme(Theme.Orange);
				RefreshDisplay();
			}));
			ThemeApp.AddItem(new AppSettingItem("id_pink", "Pink", ListIcons.Profile, delegate
			{
				SetTheme(Theme.Pink);
				RefreshDisplay();
			}));
			ThemeApp.AddItem(new AppSettingItem("id_purple", "Purple", ListIcons.Profile, delegate
			{
				SetTheme(Theme.Purple);
				RefreshDisplay();
			}));
			ThemeApp.AddItem(new AppSettingItem("id_grey", "Grey", ListIcons.Profile, delegate
			{
				SetTheme(Theme.Grey);
				RefreshDisplay();
			}));
			BrightnessApp.AddItem(new AppSettingItem("id_full", "Maximum", ListIcons.Profile, delegate
			{
				PhoneBrightness = 5;
			}));
			BrightnessApp.AddItem(new AppSettingItem("id_full", "4", ListIcons.Profile, delegate
			{
				PhoneBrightness = 4;
			}));
			BrightnessApp.AddItem(new AppSettingItem("id_full", "3", ListIcons.Profile, delegate
			{
				PhoneBrightness = 3;
			}));
			BrightnessApp.AddItem(new AppSettingItem("id_full", "2", ListIcons.Profile, delegate
			{
				PhoneBrightness = 2;
			}));
			BrightnessApp.AddItem(new AppSettingItem("id_full", "Minimum", ListIcons.Profile, delegate
			{
				PhoneBrightness = 1;
			}));
			ColorApp.AddItem(new AppSettingItem("id_red", "Red", ListIcons.Profile, delegate
			{
				SetPhoneColor(2);
				RefreshDisplay();
			}));
			ColorApp.AddItem(new AppSettingItem("id_green", "Green", ListIcons.Profile, delegate
			{
				SetPhoneColor(1);
				RefreshDisplay();
			}));
			ColorApp.AddItem(new AppSettingItem("id_blue", "Blue", ListIcons.Profile, delegate
			{
				SetPhoneColor(0);
				RefreshDisplay();
			}));
			ColorApp.AddItem(new AppSettingItem("id_orange", "Orange", ListIcons.Profile, delegate
			{
				SetPhoneColor(3);
				RefreshDisplay();
			}));
			ColorApp.AddItem(new AppSettingItem("id_pink", "Pink", ListIcons.Profile, delegate
			{
				SetPhoneColor(6);
				RefreshDisplay();
			}));
			ColorApp.AddItem(new AppSettingItem("id_purple", "Purple", ListIcons.Profile, delegate
			{
				SetPhoneColor(5);
				RefreshDisplay();
			}));
			ColorApp.AddItem(new AppSettingItem("id_grey", "Grey", ListIcons.Profile, delegate
			{
				SetPhoneColor(4);
				RefreshDisplay();
			}));
			VehicleTemplate.AddItem(new AppSettingItem("id_engine_stat", "ENGINE: OFF", ListIcons.None));
			VehicleTemplate.AddItem(new AppSettingItem("id_lights_stat", "LIGHTS: OFF", ListIcons.None));
			VehicleTemplate.AddItem(new AppSettingItem("id_locked_stat", "LOCKED: FALSE", ListIcons.None));
			VehicleTemplate.AddItem(new AppSettingItem("id_alarm_stat", "ALARM: OFF", ListIcons.None));
			VehicleTemplate.AddItem(new AppSettingItem("id_engine", "Engine", ListIcons.None, null, VehicleTemplate_Engine));
			VehicleTemplate.AddItem(new AppSettingItem("id_alarm", "Alarm", ListIcons.None, null, VehicleTemplate_Alarm));
			VehicleTemplate.AddItem(new AppSettingItem("id_doors", "Doors", ListIcons.None, null, VehicleTemplate_Doors));
			VehicleTemplate.AddItem(new AppSettingItem("id_lights", "Lights", ListIcons.None, null, VehicleTemplate_Lights));
			VehicleTemplate.AddItem(new AppSettingItem("id_windows", "Windows", ListIcons.None, null, VehicleTemplate_Windows));
			VehicleTemplate.AddItem(new AppSettingItem("id_neons", "Neons", ListIcons.None, null, VehicleTemplate_Neons));
			VehicleTemplate3.AddItem(new AppSettingItem("id_waypoint", "Set Waypoint", ListIcons.None, delegate
			{
				if (activatedCar != null)
				{
					Function.Call(Hash.SET_WAYPOINT_OFF);
					Function.Call(Hash.SET_NEW_WAYPOINT, activatedCar.Position.X, activatedCar.Position.Y);
				}
			}));
			VehicleTemplate3.AddItem(new AppSettingItem("id_request", "Request Vehicle", ListIcons.None, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					if (activatedCar.Handle.ClassType == VehicleClass.Helicopters || activatedCar.Handle.ClassType == VehicleClass.Boats || activatedCar.Handle.ClassType == VehicleClass.Planes || activatedCar.Handle.ClassType == VehicleClass.Trains)
					{
						Screen.ShowSubtitle("Cannot request obscure vehicles.", 3000);
					}
					else if (activatedCar.Handle == Game.Player.Character.CurrentVehicle)
					{
						Screen.ShowSubtitle("You are already in that vehicle.", 3000);
					}
					else
					{
						OutputArgument outputArgument3 = new OutputArgument();
						OutputArgument outputArgument4 = new OutputArgument();
						OutputArgument outputArgument5 = new OutputArgument();
						Random random = new Random();
						bool num3 = Function.Call<bool>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z, random.Next(30, 125), outputArgument3, outputArgument4, outputArgument5, 1, 3f, 0f);
						Vector3 result = outputArgument3.GetResult<Vector3>();
						float result2 = outputArgument4.GetResult<float>();
						outputArgument5.GetResult<int>();
						if (num3)
						{
							activatedCar.Handle.Position = result;
							activatedCar.Handle.Heading = result2;
							activatedCar.Handle.SteeringAngle = 0f;
							Function.Call(Hash.SET_VEHICLE_ENGINE_ON, activatedCar.Handle.Handle, true, true, false);
							activatedCar.Handle.IsHandbrakeForcedOn = false;
							activatedCar.Handle.IsInvincible = true;
							activatedCar.Handle.CanBeVisiblyDamaged = false;
							activatedCar.Handle.LockStatus = VehicleLockStatus.Unlocked;
							Function.Call(Hash.SET_VEHICLE_DOORS_SHUT, activatedCar.Handle, true);
							activatedCar.Handle.PlaceOnGround();
							Ped ped = World.CreatePed(PedHash.Xmech01SMY, result);
							ped.IsPersistent = true;
							ped.SetIntoVehicle(activatedCar.Handle, VehicleSeat.Driver);
							ped.IsInvincible = true;
							if (AdvancedPersistence.AttachedTasks.ContainsKey(activatedCar.Handle))
							{
								PedTask pedTask = AdvancedPersistence.AttachedTasks[activatedCar.Handle];
								pedTask.Clean();
								if (pedTask.Ped != null)
								{
									Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, pedTask.Ped.Handle);
									Function.Call(Hash.TASK_LEAVE_VEHICLE, pedTask.Ped.Handle, activatedCar.Handle.Handle, 16);
									pedTask.Ped.IsPersistent = true;
									while (pedTask.Ped.Exists())
									{
										pedTask.Ped.Delete();
									}
								}
							}
							PedTask pedTask2 = new PedTask(ped);
							pedTask2.Open();
							pedTask2.DriveTo(activatedCar.Handle, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 4f, 5f, 20f, DrivingStyle.AvoidTraffic);
							pedTask2.Brake(activatedCar.Handle);
							pedTask2.ExitVehicle(activatedCar.Handle, normal: true);
							pedTask2.FleeCoords(activatedCar.Handle.Position);
							pedTask2.Wander();
							pedTask2.Close();
							AdvancedPersistence.AttachedTasks[activatedCar.Handle] = pedTask2;
							ped.AlwaysKeepTask = true;
							pedTask2.Run();
							Screen.ShowSubtitle("Your mechanic is on his way! Be patient!\n(Request again if spawn position is undesired)", 7500);
						}
					}
				}
			}));
			VehicleTemplate2.AddItem(new AppSettingItem("id_spawn", "Spawn Vehicle", ListIcons.Attachment, delegate
			{
				if (activatedCar != null)
				{
					activatedCar.WasUserDespawned = false;
					if (activatedCar.Handle == null)
					{
						AdvancedPersistence.CreateVehicle(activatedCar);
					}
					else if (activatedCar.Handle != null && !activatedCar.Handle.Exists())
					{
						AdvancedPersistence.CreateVehicle(activatedCar);
					}
				}
			}));
			VehicleTemplate2.AddItem(new AppSettingItem("id_despawn", "Despawn Vehicle", ListIcons.Attachment, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null)
				{
					AdvancedPersistence.SaveVehicleData(activatedCar.Handle, activatedCar);
					activatedCar.WasUserDespawned = true;
					AdvancedPersistence.DeleteBlipsOnCar(activatedCar.Handle);
					if (AdvancedPersistence.AttachedVehicles.ContainsKey(activatedCar.Handle))
					{
						AdvancedPersistence.AttachedVehicles.Remove(activatedCar.Handle);
					}
					if (activatedCar.Handle.Exists())
					{
						OutputArgument outputArgument2 = new OutputArgument(activatedCar.Handle);
						Function.Call(Hash.DELETE_VEHICLE, outputArgument2);
					}
					if (AdvancedPersistence.AttachedTasks.ContainsKey(activatedCar.Handle))
					{
						AdvancedPersistence.AttachedTasks[activatedCar.Handle].Clean();
						if (AdvancedPersistence.AttachedTasks[activatedCar.Handle].Ped != null)
						{
							Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, AdvancedPersistence.AttachedTasks[activatedCar.Handle].Ped.Handle);
							Function.Call(Hash.TASK_LEAVE_VEHICLE, AdvancedPersistence.AttachedTasks[activatedCar.Handle].Ped.Handle, AdvancedPersistence.AttachedTasks[activatedCar.Handle].Handle, 16);
							if (AdvancedPersistence.AttachedTasks[activatedCar.Handle].Ped.Exists())
							{
								AdvancedPersistence.AttachedTasks[activatedCar.Handle].Ped.IsPersistent = true;
								AdvancedPersistence.AttachedTasks[activatedCar.Handle].Ped.Delete();
							}
						}
						AdvancedPersistence.AttachedTasks.Remove(activatedCar.Handle);
					}
					activatedCar.Handle = null;
				}
			}));
			VehicleTemplate2.AddItem(new AppSettingItem("id_spawn", "Move To Safe Spawn", ListIcons.Attachment, delegate
			{
				if (activatedCar != null)
				{
					if (activatedCar.Handle != null)
					{
						if (activatedCar.Handle.Exists())
						{
							if (activatedCar.SafeSpawnSet)
							{
								activatedCar.Handle.Position = activatedCar.SafeSpawn;
								activatedCar.Handle.Rotation = activatedCar.SafeRotation;
								Screen.ShowSubtitle("Moved!");
							}
							else
							{
								Screen.ShowSubtitle("Safe Spawn Not Set!");
							}
						}
						else
						{
							Screen.ShowSubtitle("Vehicle Not Spawned!");
						}
					}
					else
					{
						Screen.ShowSubtitle("Vehicle Not Spawned!");
					}
				}
			}));
			VehicleTemplate2.AddItem(new AppSettingItem("id_setspawn", "Set Safe Spawn", ListIcons.Checklist, delegate
			{
				if (activatedCar != null)
				{
					if (activatedCar.Handle != null)
					{
						if (activatedCar.Handle.Exists())
						{
							activatedCar.SafeSpawn = activatedCar.Handle.Position;
							activatedCar.SafeSpawnSet = true;
							activatedCar.SafeRotation = activatedCar.Handle.Rotation;
							Screen.ShowSubtitle("Vehicle Spawn Set!");
						}
						else
						{
							Screen.ShowSubtitle("Vehicle Does Not Exist!");
						}
					}
					else
					{
						Screen.ShowSubtitle("Vehicle Does Not Exist!");
					}
				}
				else
				{
					Screen.ShowSubtitle("Vehicle Does Not Exist!");
				}
			}));
			VehicleTemplate4.AddItem(new AppSettingItem("id_anchor_status", "ANCHOR: FALSE", ListIcons.None));
			VehicleTemplate4.AddItem(new AppSettingItem("id_anchoring", "Anchoring", ListIcons.None, null, VehicleTemplate_Anchor));
			VehicleTemplate_Anchor.AddItem(new AppSettingItem("id_anchor", "Anchor Boat", ListIcons.None, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null)
				{
					Function.Call(Hash.SET_BOAT_ANCHOR, activatedCar.Handle.Handle, true);
					VehicleTemplate4.GetItemByID<AppSettingItem>("id_anchor_status").Name = "ANCHOR: TRUE";
				}
			}));
			VehicleTemplate_Anchor.AddItem(new AppSettingItem("id_unanchor", "Unanchor Boat", ListIcons.None, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null)
				{
					Function.Call(Hash.SET_BOAT_ANCHOR, activatedCar.Handle.Handle, false);
					VehicleTemplate4.GetItemByID<AppSettingItem>("id_anchor_status").Name = "ANCHOR: FALSE";
				}
			}));
			VehicleTemplate2.AddItem(new AppSettingItem("id_fix", "Fix Vehicle", ListIcons.Attachment, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					activatedCar.Handle.Repair();
				}
			}));
			VehicleTemplate2.AddItem(new AppSettingItem("id_sogp", "Set On Ground Properly", ListIcons.Attachment, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					activatedCar.Handle.PlaceOnGround();
				}
			}));
			VehicleTemplate2.AddItem(new AppSettingItem("id_blip", "Set Blip Color", ListIcons.Checklist, null, BlipApp));
			VehicleTemplate2.AddItem(new AppSettingItem("id_tags", "Set Vehicle Name", ListIcons.Checklist, delegate
			{
				Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, 1, "", "", "New Name", "", "", "", 20);
				while (Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD) == 0)
				{
					Script.Yield();
				}
				if (Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD) != 2)
				{
					string text = Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
					if (text != null && text != "")
					{
						if (activatedCar != null)
						{
							activatedCar.Tag = Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
						}
						RedoScroll();
						OpenApp(VehicleTemplate2, 5);
					}
				}
			}));
			BlipApp.AddItem(new AppSettingItem("id_red", "Red", ListIcons.Attachment, delegate
			{
				if (activatedCar != null)
				{
					activatedCar.BlipColor = 1;
				}
				if (activatedCar.Handle != null && ModSettings.EnableBlips && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null)
				{
					activatedCar.Handle.AttachedBlip.Color = BlipColor.Red;
				}
			}));
			BlipApp.AddItem(new AppSettingItem("id_green", "Green", ListIcons.Attachment, delegate
			{
				if (activatedCar != null)
				{
					activatedCar.BlipColor = 2;
				}
				if (activatedCar.Handle != null && ModSettings.EnableBlips && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null)
				{
					activatedCar.Handle.AttachedBlip.Color = BlipColor.Green;
				}
			}));
			BlipApp.AddItem(new AppSettingItem("id_blue", "Blue", ListIcons.Attachment, delegate
			{
				if (activatedCar != null)
				{
					activatedCar.BlipColor = 3;
				}
				if (activatedCar.Handle != null && ModSettings.EnableBlips && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null)
				{
					activatedCar.Handle.AttachedBlip.Color = BlipColor.Blue;
				}
			}));
			BlipApp.AddItem(new AppSettingItem("id_white", "White", ListIcons.Attachment, delegate
			{
				if (activatedCar != null)
				{
					activatedCar.BlipColor = 0;
				}
				if (activatedCar.Handle != null && ModSettings.EnableBlips && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null)
				{
					activatedCar.Handle.AttachedBlip.Color = BlipColor.White;
				}
			}));
			BlipApp.AddItem(new AppSettingItem("id_yellow", "Yellow", ListIcons.Attachment, delegate
			{
				if (activatedCar != null)
				{
					activatedCar.BlipColor = 66;
				}
				if (activatedCar.Handle != null && ModSettings.EnableBlips && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null)
				{
					activatedCar.Handle.AttachedBlip.Color = BlipColor.Yellow;
				}
			}));
			BlipApp.AddItem(new AppSettingItem("id_orange", "Orange", ListIcons.Attachment, delegate
			{
				if (activatedCar != null)
				{
					activatedCar.BlipColor = 51;
				}
				if (activatedCar.Handle != null && ModSettings.EnableBlips && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null)
				{
					activatedCar.Handle.AttachedBlip.Color = BlipColor.Orange;
				}
			}));
			BlipApp.AddItem(new AppSettingItem("id_pink", "Pink", ListIcons.Attachment, delegate
			{
				if (activatedCar != null)
				{
					activatedCar.BlipColor = 8;
				}
				if (activatedCar.Handle != null && ModSettings.EnableBlips && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null)
				{
					activatedCar.Handle.AttachedBlip.Color = BlipColor.NetPlayer3;
				}
			}));
			BlipApp.AddItem(new AppSettingItem("id_purple", "Purple", ListIcons.Attachment, delegate
			{
				if (activatedCar != null)
				{
					activatedCar.BlipColor = 50;
				}
				if (activatedCar.Handle != null && ModSettings.EnableBlips && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null)
				{
					activatedCar.Handle.AttachedBlip.Color = BlipColor.Purple;
				}
			}));
			BlipApp.AddItem(new AppSettingItem("id_brown", "Brown", ListIcons.Attachment, delegate
			{
				if (activatedCar != null)
				{
					activatedCar.BlipColor = 31;
				}
				if (activatedCar.Handle != null && ModSettings.EnableBlips && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null)
				{
					activatedCar.Handle.AttachedBlip.Color = BlipColor.NetPlayer26;
				}
			}));
			BlipApp.AddItem(new AppSettingItem("id_grey", "Grey", ListIcons.Attachment, delegate
			{
				if (activatedCar != null)
				{
					activatedCar.BlipColor = 55;
				}
				if (activatedCar.Handle != null && ModSettings.EnableBlips && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null)
				{
					activatedCar.Handle.AttachedBlip.Color = BlipColor.Grey;
				}
			}));
			BlipApp.AddItem(new AppSettingItem("id_dgrey", "Dark Grey", ListIcons.Attachment, delegate
			{
				if (activatedCar != null)
				{
					activatedCar.BlipColor = 40;
				}
				if (activatedCar.Handle != null && ModSettings.EnableBlips && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null)
				{
					activatedCar.Handle.AttachedBlip.Color = BlipColor.GreyDark;
				}
			}));
			CurrentVehicleApp.AddItem(new AppSettingItem("id_add", "Track Vehicle", ListIcons.None, delegate
			{
				if (Game.Player.Character.CurrentVehicle != null)
				{
					if (AdvancedPersistence.AttachedVehicles.ContainsKey(Game.Player.Character.CurrentVehicle))
					{
						Notification.Show("ERROR: Vehicle already tracked");
					}
					else if (AdvancedPersistence.VehicleDatabase.Count >= ModSettings.MaxNumberOfCars)
					{
						Notification.Show($"ERROR: Max Vehicles Reached [{ModSettings.MaxNumberOfCars}]");
					}
					else
					{
						Vehicle currentVehicle2 = Game.Player.Character.CurrentVehicle;
						VehicleDataV1 vehicleDataV3 = new VehicleDataV1();
						AdvancedPersistence.SaveVehicleData(currentVehicle2, vehicleDataV3);
						vehicleDataV3.SafeSpawn = currentVehicle2.Position;
						vehicleDataV3.SafeSpawnSet = true;
						vehicleDataV3.SafeRotation = currentVehicle2.Rotation;
						if (ModSettings.EnableBlips)
						{
							if (currentVehicle2.AttachedBlip == null)
							{
								currentVehicle2.AddBlip();
							}
							if (currentVehicle2.Model.IsHelicopter)
							{
								currentVehicle2.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
							}
							else if (currentVehicle2.Model.IsAmphibiousQuadBike || currentVehicle2.Model.IsBicycle || currentVehicle2.Model.IsBike || currentVehicle2.Model.IsQuadBike)
							{
								currentVehicle2.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
							}
							else if (currentVehicle2.Model.IsJetSki)
							{
								currentVehicle2.AttachedBlip.Sprite = BlipSprite.Seashark;
							}
							else if (currentVehicle2.Model.IsBoat)
							{
								currentVehicle2.AttachedBlip.Sprite = BlipSprite.Boat;
							}
							else if (currentVehicle2.Model.IsPlane)
							{
								currentVehicle2.AttachedBlip.Sprite = BlipSprite.Plane;
							}
							else
							{
								currentVehicle2.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
							}
							currentVehicle2.AttachedBlip.IsShortRange = true;
							currentVehicle2.AttachedBlip.Scale = 0.75f;
							currentVehicle2.AttachedBlip.Name = "Saved Vehicle";
							currentVehicle2.AttachedBlip.Alpha = 255;
							currentVehicle2.AttachedBlip.Priority = 0;
							Function.Call(Hash.SHOW_TICK_ON_BLIP, currentVehicle2.AttachedBlip.Handle, false);
							currentVehicle2.AttachedBlip.Color = (BlipColor)vehicleDataV3.BlipColor;
							Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, currentVehicle2.AttachedBlip.Handle, false);
						}
						AdvancedPersistence.MainCharacter.CarAttach = vehicleDataV3.Id;
						AdvancedPersistence.AttachedVehicles[currentVehicle2] = vehicleDataV3;
						AdvancedPersistence.VehicleDatabase.Add(vehicleDataV3);
						AdvancedPersistence.VehicleMetabase.Add(vehicleDataV3.Meta);
						Notification.Show($"Vehicle Added [{AdvancedPersistence.VehicleDatabase.Count}]");
						RedoScroll();
						if (GetHomeObjectByIndex(0) != null)
						{
							GetHomeObjectByIndex(0).NotificationNumber = AdvancedPersistence.VehicleDatabase.Count;
						}
						RefreshDisplay(forced: true);
					}
				}
				else
				{
					Notification.Show("ERROR: Not in vehicle");
				}
			}));
			CurrentVehicleApp.AddItem(new AppSettingItem("id_remove", "Untrack Vehicle", ListIcons.None, delegate
			{
				if (Game.Player.Character.CurrentVehicle != null)
				{
					if (AdvancedPersistence.AttachedVehicles.ContainsKey(Game.Player.Character.CurrentVehicle))
					{
						Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
						VehicleDataV1 vehicleDataV2 = AdvancedPersistence.AttachedVehicles[currentVehicle];
						AdvancedPersistence.AttachedVehicles.Remove(currentVehicle);
						AdvancedPersistence.VehicleDatabase.Remove(vehicleDataV2);
						AdvancedPersistence.VehicleMetabase.Remove(vehicleDataV2.Meta);
						if (currentVehicle.AttachedBlip != null)
						{
							currentVehicle.AttachedBlip.Delete();
						}
						Blip[] attachedBlips2 = currentVehicle.AttachedBlips;
						for (int l = 0; l < attachedBlips2.Length; l++)
						{
							attachedBlips2[l].Delete();
						}
						currentVehicle.IsPersistent = false;
						Notification.Show($"Vehicle Removed [{AdvancedPersistence.VehicleDatabase.Count}]");
						RedoScroll();
						if (GetHomeObjectByIndex(0) != null)
						{
							GetHomeObjectByIndex(0).NotificationNumber = AdvancedPersistence.VehicleDatabase.Count;
						}
						RefreshDisplay(forced: true);
					}
					else
					{
						Notification.Show("ERROR: Vehicle already untracked");
					}
				}
				else
				{
					Notification.Show("ERROR: Not in vehicle");
				}
			}));
			VehicleTemplate2.AddItem(new AppSettingItem("id_warn1", "----------WARNING----------", ListIcons.None));
			VehicleTemplate2.AddItem(new AppSettingItem("id_delete", "Delete Vehicle", ListIcons.Attachment, delegate
			{
				AdvancedPersistence.VehicleDatabase.Remove(activatedCar);
				AdvancedPersistence.VehicleMetabase.Remove(activatedCar.Meta);
				if (activatedCar.Handle != null)
				{
					AdvancedPersistence.AttachedVehicles.Remove(activatedCar.Handle);
					if (AdvancedPersistence.AttachedTasks.ContainsKey(activatedCar.Handle))
					{
						AdvancedPersistence.AttachedTasks[activatedCar.Handle].Clean();
						if (AdvancedPersistence.AttachedTasks[activatedCar.Handle].Ped != null)
						{
							Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, AdvancedPersistence.AttachedTasks[activatedCar.Handle].Ped.Handle);
							Function.Call(Hash.TASK_LEAVE_VEHICLE, AdvancedPersistence.AttachedTasks[activatedCar.Handle].Ped.Handle, AdvancedPersistence.AttachedTasks[activatedCar.Handle].Handle, 16);
							if (AdvancedPersistence.AttachedTasks[activatedCar.Handle].Ped.Exists())
							{
								AdvancedPersistence.AttachedTasks[activatedCar.Handle].Ped.IsPersistent = true;
								AdvancedPersistence.AttachedTasks[activatedCar.Handle].Ped.Delete();
							}
						}
						AdvancedPersistence.AttachedTasks.Remove(activatedCar.Handle);
					}
					if (activatedCar.Handle.AttachedBlip != null)
					{
						activatedCar.Handle.AttachedBlip.Delete();
					}
					Blip[] attachedBlips = activatedCar.Handle.AttachedBlips;
					for (int k = 0; k < attachedBlips.Length; k++)
					{
						attachedBlips[k].Delete();
					}
					OutputArgument outputArgument = new OutputArgument(activatedCar.Handle);
					Function.Call(Hash.DELETE_VEHICLE, outputArgument);
					activatedCar.Handle = null;
					activatedCar = null;
					AdvancedPersistence.DrawTrace = false;
				}
				Notification.Show($"Vehicle Deleted [{AdvancedPersistence.VehicleDatabase.Count}]");
				RedoScroll();
				if (GetHomeObjectByIndex(0) != null)
				{
					GetHomeObjectByIndex(0).NotificationNumber = AdvancedPersistence.VehicleDatabase.Count;
				}
			}, AppScroll));
			VehicleTemplate2.AddItem(new AppSettingItem("id_warn2", "----------WARNING----------", ListIcons.None));
			VehicleTemplate_Doors.AddItem(new AppSettingItem("id_door_state", "[OPEN] | CLOSE", ListIcons.Settings1, delegate
			{
				if (doorState)
				{
					VehicleTemplate_Doors.GetItemByID<AppSettingItem>("id_door_state").Name = "OPEN | [CLOSE]";
				}
				else
				{
					VehicleTemplate_Doors.GetItemByID<AppSettingItem>("id_door_state").Name = "[OPEN] | CLOSE";
				}
				doorState = !doorState;
				RefreshDisplay();
			}));
			VehicleTemplate_Doors.AddItem(new AppSettingItem("id_hood", "Hood", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					if (doorState)
					{
						activatedCar.Handle.Doors[VehicleDoorIndex.Hood].Open();
					}
					else
					{
						activatedCar.Handle.Doors[VehicleDoorIndex.Hood].Close();
					}
				}
			}));
			VehicleTemplate_Doors.AddItem(new AppSettingItem("id_trunk", "Trunk", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					if (doorState)
					{
						activatedCar.Handle.Doors[VehicleDoorIndex.Trunk].Open();
					}
					else
					{
						activatedCar.Handle.Doors[VehicleDoorIndex.Trunk].Close();
					}
				}
			}));
			VehicleTemplate_Doors.AddItem(new AppSettingItem("id_lfd", "Left Front Door", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					if (doorState)
					{
						activatedCar.Handle.Doors[VehicleDoorIndex.FrontLeftDoor].Open();
					}
					else
					{
						activatedCar.Handle.Doors[VehicleDoorIndex.FrontLeftDoor].Close();
					}
				}
			}));
			VehicleTemplate_Doors.AddItem(new AppSettingItem("id_rfd", "Right Front Door", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					if (doorState)
					{
						activatedCar.Handle.Doors[VehicleDoorIndex.FrontRightDoor].Open();
					}
					else
					{
						activatedCar.Handle.Doors[VehicleDoorIndex.FrontRightDoor].Close();
					}
				}
			}));
			VehicleTemplate_Doors.AddItem(new AppSettingItem("id_lbd", "Left Back Door", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					if (doorState)
					{
						activatedCar.Handle.Doors[VehicleDoorIndex.BackLeftDoor].Open();
					}
					else
					{
						activatedCar.Handle.Doors[VehicleDoorIndex.BackLeftDoor].Close();
					}
				}
			}));
			VehicleTemplate_Doors.AddItem(new AppSettingItem("id_rbd", "Right Back Door", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					if (doorState)
					{
						activatedCar.Handle.Doors[VehicleDoorIndex.BackRightDoor].Open();
					}
					else
					{
						activatedCar.Handle.Doors[VehicleDoorIndex.BackRightDoor].Close();
					}
				}
			}));
			VehicleTemplate_Engine.AddItem(new AppSettingItem("id_engine_on", "Turn Engine ON", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					Function.Call(Hash.SET_VEHICLE_ENGINE_ON, activatedCar.Handle.Handle, true, true, false);
					VehicleTemplate.GetItemByID<AppSettingItem>("id_engine_stat").Name = "ENGINE: ON";
				}
			}));
			VehicleTemplate_Engine.AddItem(new AppSettingItem("id_engine_off", "Turn Engine OFF", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					Function.Call(Hash.SET_VEHICLE_ENGINE_ON, activatedCar.Handle.Handle, false, true, false);
					VehicleTemplate.GetItemByID<AppSettingItem>("id_engine_stat").Name = "ENGINE: OFF";
				}
			}));
			VehicleTemplate_Lights.AddItem(new AppSettingItem("id_lights_on", "Turn Lights ON", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					activatedCar.Handle.AreLightsOn = true;
					VehicleTemplate.GetItemByID<AppSettingItem>("id_lights_stat").Name = "LIGHTS: ON";
				}
			}));
			VehicleTemplate_Lights.AddItem(new AppSettingItem("id_lights_off", "Turn Lights OFF", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					activatedCar.Handle.AreLightsOn = false;
					VehicleTemplate.GetItemByID<AppSettingItem>("id_lights_stat").Name = "LIGHTS: OFF";
				}
			}));
			VehicleTemplate_Alarm.AddItem(new AppSettingItem("id_lock_on", "Lock Vehicle", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					activatedCar.Handle.LockStatus = VehicleLockStatus.CannotEnter;
					VehicleTemplate.GetItemByID<AppSettingItem>("id_locked_stat").Name = "LOCKED: TRUE";
				}
			}));
			VehicleTemplate_Alarm.AddItem(new AppSettingItem("id_lock_off", "Unlock Vehicle", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					activatedCar.Handle.LockStatus = VehicleLockStatus.Unlocked;
					VehicleTemplate.GetItemByID<AppSettingItem>("id_locked_stat").Name = "LOCKED: FALSE";
				}
			}));
			VehicleTemplate_Alarm.AddItem(new AppSettingItem("id_alarm_on", "Enable Alarm", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					activatedCar.Handle.IsAlarmSet = true;
					VehicleTemplate.GetItemByID<AppSettingItem>("id_alarm_stat").Name = "ALARM: ON";
				}
			}));
			VehicleTemplate_Alarm.AddItem(new AppSettingItem("id_alarm_off", "Disable Alarm", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					activatedCar.Handle.IsAlarmSet = false;
					VehicleTemplate.GetItemByID<AppSettingItem>("id_alarm_stat").Name = "ALARM: OFF";
				}
			}));
			VehicleTemplate_Windows.AddItem(new AppSettingItem("id_window_state", "[OPEN] | CLOSE", ListIcons.Settings1, delegate
			{
				if (windowState)
				{
					VehicleTemplate_Windows.GetItemByID<AppSettingItem>("id_window_state").Name = "OPEN | [CLOSE]";
				}
				else
				{
					VehicleTemplate_Windows.GetItemByID<AppSettingItem>("id_window_state").Name = "[OPEN] | CLOSE";
				}
				windowState = !windowState;
				RefreshDisplay();
			}));
			VehicleTemplate_Windows.AddItem(new AppSettingItem("id_flw", "Front Left Window", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					if (windowState)
					{
						activatedCar.Handle.Windows[VehicleWindowIndex.FrontLeftWindow].RollDown();
					}
					else
					{
						activatedCar.Handle.Windows[VehicleWindowIndex.FrontLeftWindow].RollUp();
					}
				}
			}));
			VehicleTemplate_Windows.AddItem(new AppSettingItem("id_frw", "Front Right Window", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					if (windowState)
					{
						activatedCar.Handle.Windows[VehicleWindowIndex.FrontRightWindow].RollUp();
					}
					else
					{
						activatedCar.Handle.Windows[VehicleWindowIndex.FrontRightWindow].RollDown();
					}
				}
			}));
			VehicleTemplate_Windows.AddItem(new AppSettingItem("id_blw", "Back Left Window", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					if (windowState)
					{
						activatedCar.Handle.Windows[VehicleWindowIndex.BackLeftWindow].RollUp();
					}
					else
					{
						activatedCar.Handle.Windows[VehicleWindowIndex.BackLeftWindow].RollDown();
					}
				}
			}));
			VehicleTemplate_Windows.AddItem(new AppSettingItem("id_brw", "Back Right Window", ListIcons.Checklist, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					if (windowState)
					{
						activatedCar.Handle.Windows[VehicleWindowIndex.BackRightWindow].RollUp();
					}
					else
					{
						activatedCar.Handle.Windows[VehicleWindowIndex.BackRightWindow].RollDown();
					}
				}
			}));
			VehicleTemplate_Neons.AddItem(new AppSettingItem("id_state", "[ON] | OFF", ListIcons.None, delegate
			{
				neonState = !neonState;
				if (neonState)
				{
					VehicleTemplate_Neons.GetItemByID<AppSettingItem>("id_state").Name = "[ON] | OFF";
				}
				else
				{
					VehicleTemplate_Neons.GetItemByID<AppSettingItem>("id_state").Name = "ON | [OFF]";
				}
				RefreshDisplay();
			}));
			VehicleTemplate_Neons.AddItem(new AppSettingItem("id_ln", "Left Neon", ListIcons.None, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					activatedCar.Handle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, neonState);
				}
			}));
			VehicleTemplate_Neons.AddItem(new AppSettingItem("id_rn", "Right Neon", ListIcons.None, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					activatedCar.Handle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, neonState);
				}
			}));
			VehicleTemplate_Neons.AddItem(new AppSettingItem("id_lf", "Front Neon", ListIcons.None, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					activatedCar.Handle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, neonState);
				}
			}));
			VehicleTemplate_Neons.AddItem(new AppSettingItem("id_bn", "Back Neon", ListIcons.None, delegate
			{
				if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists())
				{
					activatedCar.Handle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, neonState);
				}
			}));
			int num = 0;
			Vehicle[] allVehicles = World.GetAllVehicles();
			foreach (Vehicle vehicle in allVehicles)
			{
				if (vehicle.IsPersistent && !AdvancedPersistence.AttachedVehicles.ContainsKey(vehicle))
				{
					num++;
				}
			}
			Converter.AddItem(new AppSettingItem("id_num", "Found cars: " + num, ListIcons.Attachment));
			Converter.AddItem(new AppSettingItem("id_conv", "Convert All", ListIcons.Ticked, delegate
			{
				int num2 = 0;
				Vehicle[] allVehicles2 = World.GetAllVehicles();
				foreach (Vehicle vehicle2 in allVehicles2)
				{
					if (vehicle2.IsPersistent && !AdvancedPersistence.AttachedVehicles.ContainsKey(vehicle2))
					{
						VehicleDataV1 vehicleDataV = new VehicleDataV1();
						AdvancedPersistence.SaveVehicleData(vehicle2, vehicleDataV);
						vehicleDataV.SafeSpawn = vehicle2.Position;
						vehicleDataV.SafeSpawnSet = true;
						vehicleDataV.SafeRotation = vehicle2.Rotation;
						if (ModSettings.EnableBlips)
						{
							if (vehicle2.AttachedBlip == null)
							{
								vehicle2.AddBlip();
							}
							if (vehicle2.Model.IsHelicopter)
							{
								vehicle2.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
							}
							else if (vehicle2.Model.IsAmphibiousQuadBike || vehicle2.Model.IsBicycle || vehicle2.Model.IsBike || vehicle2.Model.IsQuadBike)
							{
								vehicle2.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
							}
							else if (vehicle2.Model.IsJetSki)
							{
								vehicle2.AttachedBlip.Sprite = BlipSprite.Seashark;
							}
							else if (vehicle2.Model.IsBoat)
							{
								vehicle2.AttachedBlip.Sprite = BlipSprite.Boat;
							}
							else if (vehicle2.Model.IsPlane)
							{
								vehicle2.AttachedBlip.Sprite = BlipSprite.Plane;
							}
							else
							{
								vehicle2.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
							}
							vehicle2.AttachedBlip.IsShortRange = true;
							vehicle2.AttachedBlip.Scale = 0.75f;
							vehicle2.AttachedBlip.Name = "Saved Vehicle";
							vehicle2.AttachedBlip.Alpha = 255;
							vehicle2.AttachedBlip.Priority = 0;
							Function.Call(Hash.SHOW_TICK_ON_BLIP, vehicle2.AttachedBlip.Handle, false);
							vehicle2.AttachedBlip.Color = (BlipColor)vehicleDataV.BlipColor;
							Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, vehicle2.AttachedBlip.Handle, false);
						}
						AdvancedPersistence.AttachedVehicles[vehicle2] = vehicleDataV;
						AdvancedPersistence.VehicleDatabase.Add(vehicleDataV);
						AdvancedPersistence.VehicleMetabase.Add(vehicleDataV.Meta);
						num2++;
					}
				}
				if (GetHomeObjectByIndex(6) != null)
				{
					GetHomeObjectByIndex(6).NotificationNumber = 0;
				}
				if (num2 > 0)
				{
					Screen.ShowSubtitle($"({num2}) Vehicles Converted, PLEASE RESTART YOUR GAME AND REMOVE THE OTHER MOD", 10000);
				}
				else
				{
					Screen.ShowSubtitle("No Vehicles Found", 5000);
				}
				Converter.GetItemByID<AppSettingItem>("id_num").Name = "Found cars: 0";
				RefreshDisplay();
			}));
			SetHomeObjectsData(new HomeObject("My Vehicles", HomeIcon.PlayerList, HomescreenLocation.TopLeft, AdvancedPersistence.VehicleDatabase.Count, AppScroll), new HomeObject("Vehicle Control", HomeIcon.SightSeer, HomescreenLocation.TopMiddle, 0, ControlMain), new HomeObject("Tracker", HomeIcon.Tracker, HomescreenLocation.TopRight, 0, AppScroll2), new HomeObject("Settings", HomeIcon.Settings2, HomescreenLocation.BottomRight, 0, Settings), new HomeObject("Converter", HomeIcon.Multiplayer, HomescreenLocation.BottomLeft, num, Converter), new HomeObject("Boat Control", HomeIcon.Broadcast, HomescreenLocation.MiddleLeft, 0, AppScroll3), new HomeObject("Current Vehicle", HomeIcon.Sniper, HomescreenLocation.Middle, 0, CurrentVehicleApp), new HomeObject("[Coming Soon]", HomeIcon.Bennys, HomescreenLocation.MiddleRight, 0, null, 50));
			SetTheme(ActiveTheme);
			SetPhoneColor(PhoneColor);
			RefreshDisplay();
		}

		public static void ClearHomescreen()
		{
			for (int i = 0; i < 9; i++)
			{
				SetDataSlotForHome(1, i, 23, 0, "");
			}
		}

		public static void HideCurrentSelection()
		{
			DisplayView(1, -1);
		}

		public static bool IsOnHomeScreen()
		{
			return CurrentApp == null;
		}

		public static int GetCurrentIndex()
		{
			if (IsOnHomeScreen())
			{
				return HomescreenSelection;
			}
			return 0;
		}

		public static void OpenApp(AppObject app, int LastSelection)
		{
			if (app == null)
			{
				return;
			}
			CurrentApp = app;
			CurrentAppSelection = LastSelection;
			app.Selection = LastSelection;
			SetDataSlotEmpty((int)app.Container);
			for (int i = 0; i < app.Items.Count; i++)
			{
				if (app.Container == AppContainer.Settings)
				{
					SetDataSlotForSetting(22, i, (int)(app.Items[i] as AppSettingItem).Icon, (app.Items[i] as AppSettingItem).Name);
				}
				else if (app.Container == AppContainer.MessageList)
				{
					SetDataSlotForMessageList(6, i, (app.Items[i] as AppMessageItem).Hour, (app.Items[i] as AppMessageItem).Minute, (app.Items[i] as AppMessageItem).Seen, (app.Items[i] as AppMessageItem).FromAddress, (app.Items[i] as AppMessageItem).SubjectTitle);
				}
				else if (app.Container == AppContainer.MessageView)
				{
					SetDataSlotForMessageView(7, i, (app.Items[i] as AppMessageViewItem).FromAddress, (app.Items[i] as AppMessageViewItem).Message, (app.Items[i] as AppMessageViewItem).Icon);
				}
				else if (app.Container == AppContainer.Contacts)
				{
					SetDataSlotForContactList(2, i, (app.Items[i] as AppContactItem).MissedCall, (app.Items[i] as AppContactItem).Name, (app.Items[i] as AppContactItem).Icon);
				}
				else if (app.Container == AppContainer.CallScreen)
				{
					SetDataSlotForCallscreen(4, i, (app.Items[i] as AppCallscreenItem).FromAddress, (app.Items[i] as AppCallscreenItem).JobTitle, (app.Items[i] as AppCallscreenItem).Icon);
				}
			}
			if (app.Invoker != null)
			{
				app.Invoker();
			}
			SetSoftKey_Data(SoftKey.Left, app.SoftKey_Left);
			SetSoftKey_Data(SoftKey.Right, app.SoftKey_Right);
			SetSoftKey_Data(SoftKey.Middle, app.SoftKey_Middle);
			SetHeaderText(app.Name);
			DisplayView((int)app.Container, CurrentAppSelection);
		}

		public static void CloseApp(AppObject app)
		{
			if (app != null)
			{
				if (app.Backward == null)
				{
					GoHome();
				}
				else
				{
					OpenApp(app.Backward, app.Backward.Selection);
				}
			}
		}

		public static HomeObject GetHomeObjectByIndex(int index)
		{
			if (index < 0)
			{
				index = 0;
			}
			if (index > 8)
			{
				index = 8;
			}
			return HomeObjects_Stored[0][index];
		}

		public static void SetHomeObjectsData(params HomeObject[] objs)
		{
			for (int i = 0; i < objs.Length; i++)
			{
				HomeObjects_Stored[0][(int)objs[i].Location] = objs[i];
			}
			for (int j = 0; j < 9; j++)
			{
				if (HomeObjects_Stored[0][j] == null)
				{
					HomeObjects_Stored[0][j] = new HomeObject(" ", HomeIcon.Spare, (HomescreenLocation)j, 0);
				}
			}
		}

		public static void SetHomeObjectsTemp(params HomeObject[] objs)
		{
			for (int i = 0; i < objs.Length; i++)
			{
				SetDataSlotForHome(1, (int)objs[i].Location, (int)objs[i].Icon, objs[i].NotificationNumber, objs[i].Name, objs[i].Alpha);
			}
		}

		public static void SwitchHomeScreen(int index)
		{
			SetHomeObjectsTemp(HomeObjects_Stored[index].ToArray());
			DisplayView(22, 0);
		}

		private static bool IsInvalid()
		{
			if (PhoneScaleform != null)
			{
				return !PhoneScaleform.IsValid;
			}
			return true;
		}

		public static int GetCurrentSelection()
		{
			if (CurrentApp == null)
			{
				return HomescreenSelection;
			}
			return CurrentAppSelection;
		}

		public static void SetSleepMode(bool active)
		{
			if (!IsInvalid())
			{
				SleepMode = active;
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_SLEEP_MODE");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, active);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
		}

		public static void SetBackgroundImage(BackgroundImage img)
		{
			if (!IsInvalid())
			{
				HomescreenImage = img;
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_BACKGROUND_IMAGE");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)img);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
		}

		public static void RefreshDisplay(bool forced = false)
		{
			if (IsOnHomeScreen())
			{
				DisplayView(1, HomescreenSelection);
				return;
			}
			int currentAppSelection = CurrentAppSelection;
			if (CurrentApp.Container == AppContainer.Settings)
			{
				CurrentApp.Selection = currentAppSelection;
				AppSettingItem appSettingItem = CurrentApp.Items[currentAppSelection] as AppSettingItem;
				SetDataSlotForSetting(22, currentAppSelection, (int)appSettingItem.Icon, appSettingItem.Name);
			}
			if (forced)
			{
				OpenApp(CurrentApp, CurrentAppSelection);
			}
			else
			{
				DisplayView((int)CurrentApp.Container, CurrentAppSelection);
			}
		}

		public static void SetHeaderText(string text)
		{
			if (!IsInvalid())
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_HEADER");
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, text, -1);
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
		}

		public static void SetPhoneColor(int theme)
		{
			PhoneColor = theme;
			int num = theme;
			if (PhoneModel == 1)
			{
				switch (num)
				{
				case 2:
					num = 1;
					break;
				case 1:
					num = 6;
					break;
				case 0:
					num = 5;
					break;
				case 3:
					num = 0;
					break;
				case 6:
					num = 4;
					break;
				case 5:
					num = 3;
					break;
				case 4:
					num = 2;
					break;
				}
			}
			else if (PhoneModel == 2)
			{
				switch (num)
				{
				case 2:
					num = 1;
					break;
				case 1:
					num = 0;
					break;
				case 0:
					num = 6;
					break;
				case 3:
					num = 2;
					break;
				case 6:
					num = 5;
					break;
				case 5:
					num = 4;
					break;
				case 4:
					num = 3;
					break;
				}
			}
			Function.Call(Hash.SET_PLAYER_PHONE_PALETTE_IDX, Game.Player, num);
		}

		public static void SetTitlebarTime(int hour, int minute, string day)
		{
			if (!IsInvalid())
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_TITLEBAR_TIME");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, hour);
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, minute);
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, day, -1);
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
		}

		public static void SetTitlebarTimeEx(string hour, string minute, string day)
		{
			if (!IsInvalid())
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_TITLEBAR_TIME");
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, hour, -1);
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, minute, -1);
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, day, -1);
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
		}

		public static void Click()
		{
			if (IsInvalid())
			{
				return;
			}
			if (IsOnHomeScreen())
			{
				OpenApp(HomeObjects_Stored[0][GetCurrentSelection()].Forward, 0);
			}
			else
			{
				int currentSelection = GetCurrentSelection();
				if (CurrentApp.Items.Count > 0)
				{
					if (CurrentApp.Items[currentSelection].Invoker != null)
					{
						CurrentApp.Items[currentSelection].Invoker();
					}
					if (CurrentApp.Items[currentSelection].OnSoftkey_Left != null)
					{
						CurrentApp.Items[currentSelection].OnSoftkey_Left();
					}
				}
				if (CurrentApp.OnSoftKey_Left != null)
				{
					CurrentApp.OnSoftKey_Left();
				}
				if (CurrentApp.Items.Count > 0 && CurrentApp.Items[currentSelection].Forward != null)
				{
					OpenApp(CurrentApp.Items[currentSelection].Forward, 0);
				}
			}
			Function.Call(Hash.CELL_SET_INPUT, 1);
			AdvancedPersistence.PlayFrontendAudio("Menu_Accept", PhoneTone);
		}

		public static void Back()
		{
			if (IsInvalid())
			{
				return;
			}
			if (!IsOnHomeScreen())
			{
				if (GetCurrentSelection() < CurrentApp.Items.Count && CurrentApp.Items[GetCurrentSelection()].OnSoftkey_Right != null)
				{
					CurrentApp.Items[GetCurrentSelection()].OnSoftkey_Right();
				}
				if (CurrentApp.OnSoftKey_Right != null)
				{
					CurrentApp.OnSoftKey_Right();
				}
				if (CurrentApp.OnBack != null)
				{
					CurrentApp.OnBack();
				}
				CloseApp(CurrentApp);
			}
			else
			{
				TurnOff();
			}
			Function.Call(Hash.CELL_SET_INPUT, 2);
			AdvancedPersistence.PlayFrontendAudio("Menu_Back", PhoneTone);
		}

		public static void Middle()
		{
			if (IsInvalid())
			{
				return;
			}
			if (!IsOnHomeScreen())
			{
				if (CurrentApp.Items[GetCurrentSelection()].OnSoftkey_Middle != null)
				{
					CurrentApp.Items[GetCurrentSelection()].OnSoftkey_Middle();
				}
				if (CurrentApp.OnSoftKey_Middle != null)
				{
					CurrentApp.OnSoftKey_Middle();
				}
			}
			Function.Call(Hash.CELL_SET_INPUT, 5);
		}

		public static void SetInputEvent(Direction dir)
		{
			if (IsInvalid())
			{
				return;
			}
			switch (dir)
			{
			case Direction.Left:
				Function.Call(Hash.CELL_SET_INPUT, 3);
				AdvancedPersistence.PlayFrontendAudio("Menu_Navigate", PhoneTone);
				break;
			case Direction.Right:
				Function.Call(Hash.CELL_SET_INPUT, 4);
				AdvancedPersistence.PlayFrontendAudio("Menu_Navigate", PhoneTone);
				break;
			case Direction.Down:
				Function.Call(Hash.CELL_SET_INPUT, 2);
				AdvancedPersistence.PlayFrontendAudio("Menu_Navigate", PhoneTone);
				break;
			case Direction.Up:
				Function.Call(Hash.CELL_SET_INPUT, 1);
				AdvancedPersistence.PlayFrontendAudio("Menu_Navigate", PhoneTone);
				break;
			}
			if (CurrentApp == null)
			{
				switch (dir)
				{
				case Direction.Left:
					HomescreenSelection--;
					break;
				case Direction.Right:
					HomescreenSelection++;
					break;
				case Direction.Up:
					HomescreenSelection -= 3;
					break;
				case Direction.Down:
					HomescreenSelection += 3;
					break;
				}
				if (HomescreenSelection > 8)
				{
					HomescreenSelection -= 9;
				}
				else if (HomescreenSelection < 0)
				{
					HomescreenSelection += 9;
				}
				DisplayView(1, HomescreenSelection);
			}
			else if (CurrentApp.Items.Count == 1)
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_INPUT_EVENT");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)dir);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
			else if (CurrentApp.Items.Count != 0)
			{
				switch (dir)
				{
				case Direction.Down:
					CurrentAppSelection++;
					break;
				case Direction.Up:
					CurrentAppSelection--;
					break;
				}
				if (CurrentAppSelection > CurrentApp.Items.Count - 1)
				{
					CurrentAppSelection = 0;
				}
				if (CurrentAppSelection < 0)
				{
					CurrentAppSelection = CurrentApp.Items.Count - 1;
				}
				CurrentApp.Selection = CurrentAppSelection;
				DisplayView((int)CurrentApp.Container, CurrentAppSelection);
			}
		}

		public static void SetSignalStrength(int strength)
		{
			if (!IsInvalid())
			{
				if (strength < 0)
				{
					strength = 0;
				}
				if (strength > 5)
				{
					strength = 5;
				}
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_SIGNAL_STRENGTH");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, strength);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
		}

		public static void SetTheme(Theme th)
		{
			if (!IsInvalid())
			{
				ActiveTheme = th;
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_THEME");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)th);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
		}

		public static void SetSoftKey_Data(SoftKey key, SoftkeyObject obj)
		{
			SetSoftKey_Visible(key, obj.Visible);
			SetSoftKey_Color(key, obj.RGBA);
			SetSoftKey_Icon(key, obj.Icon);
		}

		public static void SetSoftKey_Visible(SoftKey key, bool visible)
		{
			int num = 0;
			switch (key)
			{
			case SoftKey.Left:
				SoftkeyLeft.Visible = visible;
				num = (int)SoftkeyLeft.Icon;
				break;
			case SoftKey.Middle:
				SoftkeyMiddle.Visible = visible;
				num = (int)SoftkeyMiddle.Icon;
				break;
			default:
				SoftkeyRight.Visible = visible;
				num = (int)SoftkeyRight.Icon;
				break;
			}
			if (!IsInvalid())
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_SOFT_KEYS");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)key);
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, visible);
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
		}

		public static void SetSoftKey_Icon(SoftKey key, SoftkeyIcon icon)
		{
			bool flag = false;
			switch (key)
			{
			case SoftKey.Left:
				SoftkeyLeft.Icon = icon;
				flag = SoftkeyLeft.Visible;
				break;
			case SoftKey.Middle:
				SoftkeyMiddle.Icon = icon;
				flag = SoftkeyMiddle.Visible;
				break;
			default:
				SoftkeyRight.Icon = icon;
				flag = SoftkeyRight.Visible;
				break;
			}
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_SOFT_KEYS");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)key);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, flag);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)icon);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public static void SetSoftKey_Color(SoftKey key, RGBA rgba)
		{
			switch (key)
			{
			case SoftKey.Left:
				SoftkeyLeft.RGBA = rgba;
				break;
			case SoftKey.Middle:
				SoftkeyMiddle.RGBA = rgba;
				break;
			default:
				SoftkeyRight.RGBA = rgba;
				break;
			}
			if (!IsInvalid())
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_SOFT_KEYS_COLOUR");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)key);
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, rgba.Red);
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, rgba.Green);
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, rgba.Blue);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
		}

		private static void DisplayView(int view, int select)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "DISPLAY_VIEW");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, view);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, select);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private static void SetDataSlotForHome(int view, int slot, int icon, int notf, string name, int alpha = 100)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_DATA_SLOT");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, view);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, slot);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, icon);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, notf);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, name);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, alpha);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private static void SetDataSlotForSetting(int view, int slot, int icon, string name)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_DATA_SLOT");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, view);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, slot);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, icon);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, name);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private static void SetDataSlotForMessageList(int view, int slot, string hour, string minute, bool seen, string fromAddress, string subjectTitle)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_DATA_SLOT");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, view);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, slot);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, hour);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, minute);
			if (seen)
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 34);
			}
			else
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 33);
			}
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, fromAddress);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, subjectTitle);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private static void SetDataSlotForMessageView(int view, int slot, string from, string message, string icon)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_DATA_SLOT");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, view);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, slot);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, from);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, message);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, icon);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private static void SetDataSlotForContactList(int view, int slot, bool missedcall, string name, string icon)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_DATA_SLOT");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, view);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, slot);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, missedcall);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, name);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, "");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, icon);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private static void SetDataSlotForCallscreen(int view, int slot, string from, string title, string icon)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_DATA_SLOT");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, view);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, slot);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, "");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, from);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, icon);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, title);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private static void SetDataSlotEmpty(int view)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, PhoneScaleform.Handle, "SET_DATA_SLOT_EMPTY");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, view);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public static void GoHome()
		{
			SetSoftKey_Data(SoftKey.Left, Home_SoftkeyLeft);
			SetSoftKey_Data(SoftKey.Right, Home_SoftkeyRight);
			SetSoftKey_Data(SoftKey.Middle, Home_SoftkeyMiddle);
			SetBackgroundImage(HomescreenImage);
			foreach (HomeObject item in HomeObjects_Stored[0])
			{
				SetDataSlotForHome(1, (int)item.Location, (int)item.Icon, item.NotificationNumber, item.Name, item.Alpha);
			}
			DisplayView(1, HomescreenSelection);
			CurrentAppSelection = 0;
			CurrentApp = null;
		}

		public static void TurnOn()
		{
			if (!IsOn)
			{
				IsOn = true;
				RawOn = true;
				Function.Call(Hash.CREATE_MOBILE_PHONE, PhoneModel);
				Function.Call(Hash.SET_MOBILE_PHONE_POSITION, PhonePosition_Start.X, PhonePosition_Start.Y, PhonePosition_Start.Z);
				Function.Call(Hash.SET_MOBILE_PHONE_ROTATION, PhoneRotation_Start.X, PhoneRotation_Start.Y, PhoneRotation_Start.Z, 0);
				Function.Call(Hash.SET_MOBILE_PHONE_SCALE, PhoneScale);
				OutputArgument outputArgument = new OutputArgument();
				Function.Call(Hash.GET_MOBILE_PHONE_RENDER_ID, outputArgument);
				PhoneRenderID = outputArgument.GetResult<int>();
				Function.Call(Hash.SCRIPT_IS_MOVING_MOBILE_PHONE_OFFSCREEN, false);
				AdvancedPersistence.PlayFrontendAudio("Pull_Out", PhoneTone);
				SetDataSlotEmpty(1);
				SetSleepMode(active: false);
				SetSignalStrength(5);
				SetSoftKey_Icon(SoftKey.Left, SoftkeyIcon.Select);
				SetSoftKey_Color(SoftKey.Left, new RGBA(46, 204, 113));
				SetSoftKey_Visible(SoftKey.Left, visible: true);
				SetSoftKey_Icon(SoftKey.Middle, SoftkeyIcon.Keypad);
				SetSoftKey_Color(SoftKey.Middle, new RGBA(149, 165, 166));
				SetSoftKey_Visible(SoftKey.Middle, visible: true);
				SetSoftKey_Icon(SoftKey.Right, SoftkeyIcon.Website);
				SetSoftKey_Color(SoftKey.Right, new RGBA(52, 152, 219));
				SetSoftKey_Visible(SoftKey.Right, visible: true);
				SetTitlebarTimeEx("----------", "--------------", "");
				Initialize();
				GoHome();
				int num = Function.Call<int>(Hash.GET_CAM_ACTIVE_VIEW_MODE_CONTEXT);
				if (Function.Call<int>(Hash.GET_CAM_VIEW_MODE_FOR_CONTEXT, num) != 4)
				{
					DoBlackLerp = true;
					CurBlackLerp = 0f;
					CurLerp = 0f;
					DoLerpDown = false;
					DoLerpUp = true;
					DoBlackLerpInverse = false;
				}
				CurBlackLerp = 0f;
				DoBlackLerp = true;
				DoBlackLerpInverse = false;
			}
		}

		public static void ChangePhysicalColor(int col)
		{
			if (!IsOn)
			{
				return;
			}
			int num = 0;
			if (PhoneModel == 0)
			{
				num = Game.GenerateHash("prop_phone_ing");
			}
			else if (PhoneModel == 1)
			{
				num = Game.GenerateHash("prop_phone_ing_02");
			}
			else
			{
				if (PhoneModel != 2)
				{
					return;
				}
				num = Game.GenerateHash("prop_phone_ing_03");
			}
			Vector3 position = Game.Player.Character.Position;
			int num2 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 3f, num, false, false, false);
			if (num2 != 0 && num2 != -1)
			{
				Function.Call(Hash.SET_OBJECT_TINT_INDEX, num2, col);
			}
		}

		public static void BringUp()
		{
			if (IsOn)
			{
				CurLerp = 0f;
				DoLerpDown = false;
				DoLerpUp = true;
			}
		}

		public static void BringDown()
		{
			CurLerp = 0f;
			DoLerpUp = false;
			DoLerpDown = true;
		}

		public static void TurnOff()
		{
			if (!IsOn)
			{
				return;
			}
			IsOn = false;
			Function.Call(Hash.SCRIPT_IS_MOVING_MOBILE_PHONE_OFFSCREEN, true);
			AdvancedPersistence.PlayFrontendAudio("Put_Away", PhoneTone);
			CurLerp = 0f;
			DoLerpUp = false;
			DoLerpDown = true;
			DoBlackLerp = true;
			DoBlackLerpInverse = true;
			CurBlackLerp = 0f;
			if (activatedCar != null && activatedCar.Handle != null && activatedCar.Handle.Exists() && activatedCar.Handle.AttachedBlip != null && ModSettings.EnableBlips && activatedCar.Handle.AttachedBlip.Exists())
			{
				if (activatedCar.Handle.Model.IsHelicopter)
				{
					activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
				}
				else if (activatedCar.Handle.Model.IsAmphibiousQuadBike || activatedCar.Handle.Model.IsBicycle || activatedCar.Handle.Model.IsBike || activatedCar.Handle.Model.IsQuadBike)
				{
					activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleBike;
				}
				else if (activatedCar.Handle.Model.IsJetSki)
				{
					activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Seashark;
				}
				else if (activatedCar.Handle.Model.IsBoat)
				{
					activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Boat;
				}
				else if (activatedCar.Handle.Model.IsPlane)
				{
					activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.Plane;
				}
				else
				{
					activatedCar.Handle.AttachedBlip.Sprite = BlipSprite.PersonalVehicleCar;
				}
				activatedCar.Handle.AttachedBlip.IsShortRange = true;
				activatedCar.Handle.AttachedBlip.Color = (BlipColor)activatedCar.BlipColor;
				activatedCar.Handle.AttachedBlip.Scale = 0.75f;
				activatedCar.Handle.AttachedBlip.Name = "Saved Vehicle";
				activatedCar.Handle.AttachedBlip.Priority = 0;
				Function.Call(Hash.SHOW_TICK_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
				Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, activatedCar.Handle.AttachedBlip.Handle, false);
			}
			activatedCar = null;
		}

		public static void Draw()
		{
			if (RawOn)
			{
				SetTitlebarTimeEx(World.CurrentTimeOfDay.Hours.ToString(), World.CurrentTimeOfDay.Minutes.ToString(), World.CurrentDate.DayOfWeek.ToString().ToUpper().Substring(0, 3));
			}
			float deltaTime = GetDeltaTime();
			if (DoBlackLerp)
			{
				if (CurBlackLerp >= BlackLerp)
				{
					DoBlackLerp = false;
				}
				else
				{
					CurBlackLerp += deltaTime;
					if (DoBlackLerpInverse)
					{
						BlackValue = Lerp(0f, 255f, CurBlackLerp / BlackLerpInverse);
					}
					else
					{
						BlackValue = Lerp(255f, 0f, CurBlackLerp / BlackLerp);
					}
				}
			}
			if (DoLerpUp)
			{
				if (CurLerp >= TimeLerp)
				{
					DoLerpUp = false;
				}
				else
				{
					CurLerp += deltaTime;
					PhonePosition_Current.Y = Lerp(PhonePosition_Current.Y, PhonePosition_Final.Y, CurLerp / TimeLerp);
					PhoneRotation_Current.Y = Lerp(PhoneRotation_Current.Y, PhoneRotation_Final.Y, CurLerp / TimeLerp);
					Function.Call(Hash.SET_MOBILE_PHONE_POSITION, PhonePosition_Current.X, PhonePosition_Current.Y, PhonePosition_Current.Z);
					Function.Call(Hash.SET_MOBILE_PHONE_ROTATION, PhoneRotation_Current.X, PhoneRotation_Current.Y, PhoneRotation_Current.Z, 0);
				}
			}
			else if (DoLerpDown)
			{
				if (CurLerp >= TimeLerp)
				{
					DoLerpDown = false;
					if (!IsOn)
					{
						Function.Call(Hash.DESTROY_MOBILE_PHONE);
						PhoneRenderID = -1;
						PhoneScaleform.Dispose();
						RawOn = false;
					}
				}
				else
				{
					CurLerp += deltaTime;
					PhonePosition_Current.Y = Lerp(PhonePosition_Current.Y, PhonePosition_Start.Y, CurLerp / TimeLerp);
					PhoneRotation_Current.Y = Lerp(PhoneRotation_Current.Y, PhoneRotation_Start.Y, CurLerp / TimeLerp);
					Function.Call(Hash.SET_MOBILE_PHONE_POSITION, PhonePosition_Current.X, PhonePosition_Current.Y, PhonePosition_Current.Z);
					Function.Call(Hash.SET_MOBILE_PHONE_ROTATION, PhoneRotation_Current.X, PhoneRotation_Current.Y, PhoneRotation_Current.Z, 0);
				}
			}
			if (!IsInvalid() && PhoneRenderID != -1)
			{
				Function.Call(Hash.SET_TEXT_RENDER_ID, PhoneRenderID);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE, PhoneScaleform.Handle, 0.1f, 0.179f, 0.2f, 0.356f, 255, 0, 255, 255, 0);
				if (PhoneBrightness == 5)
				{
					Function.Call(Hash.DRAW_RECT, 0.5f, 0.5f, 1f, 1f, 0, 0, 0, 0, 0);
				}
				else if (PhoneBrightness == 4)
				{
					Function.Call(Hash.DRAW_RECT, 0.5f, 0.5f, 1f, 1f, 0, 0, 0, 50, 0);
				}
				else if (PhoneBrightness == 3)
				{
					Function.Call(Hash.DRAW_RECT, 0.5f, 0.5f, 1f, 1f, 0, 0, 0, 100, 0);
				}
				else if (PhoneBrightness == 2)
				{
					Function.Call(Hash.DRAW_RECT, 0.5f, 0.5f, 1f, 1f, 0, 0, 0, 175, 0);
				}
				else if (PhoneBrightness == 1)
				{
					Function.Call(Hash.DRAW_RECT, 0.5f, 0.5f, 1f, 1f, 0, 0, 0, 220, 0);
				}
				Function.Call(Hash.DRAW_RECT, 0.5f, 0.5f, 1f, 1f, 0, 0, 0, (int)BlackValue, 0);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
			}
		}
	}

	public static bool doorState = true;

	public static bool windowState = true;

	public static bool neonState = true;

	public static float GetDeltaTime()
	{
		return Game.LastFrameTime;
	}

	public static float Lerp(float a, float b, float t)
	{
		if (t > 1f)
		{
			t = 1f;
		}
		if (t < 0f)
		{
			t = 0f;
		}
		return (1f - t) * a + t * b;
	}
}
