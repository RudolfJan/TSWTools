﻿using Styles.Library.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;



namespace TSWTools
	{
	#region enums

	public enum HudStyleEnum
		{
		[Description("None")] None,
		[Description("HUD")] HUD,
		[Description("Marker")] Marker,
		[Description("Both")] Both
		}

	public enum TemperatureEnum
		{
		[Description("Celsius")] Celsius,
		[Description("FahrenHeit")] Fahrenheit
		}

	public enum UnitsEnum
		{
		[Description("Metric")] Metric,
		[Description("Imperial")] Imperial
		}

	public enum GradeUnitsEnum
		{
		[Description("Percentage")] Percentage,
		[Description("Ratio")] Ratio
		}

	public enum ScreenModeEnum
		{
		[Description("FullScreen")] FullScreen = 0,
		[Description("Windowd FullScreen")] WindowedFullScreen = 1,
		[Description("Windowd")] WindowedScreen = 2
		}

	public enum QualityEnum
		{
		[Description("Low")] Low = 0,
		[Description("Medium")] Medium = 1,
		[Description("High")] High = 2,
		[Description("Ultra")] Ultra = 3,
		}

	public enum AntiAliasingEnum
		{
		[Description("Off")] Off = 0,
		[Description("FXAA")] FXAA = 1,
		[Description("TAA")] TAA = 2
		}

	#endregion

	public class CSettingsManager : Notifier
		{
		#region Properties

		private String _SaveSetName;
		public String SaveSetName
			{
			get { return _SaveSetName; }
			set
				{
				_SaveSetName = value;
				OnPropertyChanged("SaveSetName");
				}
			}

		private SectionEnum _CurrentSection;

		public SectionEnum CurrentSection
			{
			get { return _CurrentSection; }
			set
				{
				_CurrentSection = value;
				OnPropertyChanged("CurrentSection");
				}
			}

		/*
		Dictionary to contain all settings
		*/
		private ObservableCollection<CSetting> _SettingsDictionary;

		public ObservableCollection<CSetting> SettingsDictionary
			{
			get { return _SettingsDictionary; }
			set
				{
				_SettingsDictionary = value;
				OnPropertyChanged("SettingsDictionary");
				}
			}

		private FileInfo _CurrentUserSettingsFile;

		public FileInfo CurrentUserSettingsFile
			{
			get { return _CurrentUserSettingsFile; }
			set
				{
				_CurrentUserSettingsFile = value;
				OnPropertyChanged("CurrentUserSettingsFile");
				}
			}

		private FileInfo _CurrentEngineIniSettingsFile;

		public FileInfo CurrentEngineIniSettingsFile
			{
			get { return _CurrentEngineIniSettingsFile; }
			set
				{
				_CurrentEngineIniSettingsFile = value;
				OnPropertyChanged("CurrentEngineIniSettingsFile");
				}
			}

		private ObservableCollection<DirectoryInfo> _SavedUserSettingsList;

		public ObservableCollection<DirectoryInfo> SavedUserSettingsList
			{
			get { return _SavedUserSettingsList; }
			set
				{
				_SavedUserSettingsList = value;
				OnPropertyChanged("SavedUserSettingsList");
				}
			}

		/*
		Contains text for [System.Core] part in Engine.ini
		*/
		private String _SectionCore;
		public String SectionCore
			{
			get { return _SectionCore; }
			set
				{
				_SectionCore = value;
				OnPropertyChanged("SectionCore");
				}
			}

		/*
		if true used advanced settings, otherwise do not write them to output
		*/
		private Boolean _UseAdvanced;
		public Boolean UseAdvanced
			{
			get { return _UseAdvanced; }
			set
				{
				_UseAdvanced = value;
				OnPropertyChanged("UseAdvanced");
				}
			}


		private ObservableCollection<FileInfo> _SavedEngineIniSettingsFileList;

		public ObservableCollection<FileInfo> SavedEngineIniSettingsFileList
			{
			get { return _SavedEngineIniSettingsFileList; }
			set
				{
				_SavedEngineIniSettingsFileList = value;
				OnPropertyChanged("SavedEngineIniSettingsFileList");
				}
			}

		private CSettingsHUD _SettingsHUD;

		public CSettingsHUD SettingsHUD
			{
			get { return _SettingsHUD; }
			set
				{
				_SettingsHUD = value;
				OnPropertyChanged("SettingsHUD");
				}
			}

		private CSettingsGamePlay _SettingsGamePlay;

		public CSettingsGamePlay SettingsGamePlay
			{
			get { return _SettingsGamePlay; }
			set
				{
				_SettingsGamePlay = value;
				OnPropertyChanged("SettingsGamePlay");
				}
			}

		private CSettingsSound _SettingsSound;

		public CSettingsSound SettingsSound
			{
			get { return _SettingsSound; }
			set
				{
				_SettingsSound = value;
				OnPropertyChanged("SettingsSound");
				}
			}

		private CSettingsScreen _SettingsScreen;

		public CSettingsScreen SettingsScreen
			{
			get { return _SettingsScreen; }
			set
				{
				_SettingsScreen = value;
				OnPropertyChanged("SettingsScreen");
				}
			}

		private CSettingsQuality _SettingsQuality;

		public CSettingsQuality SettingsQuality
			{
			get { return _SettingsQuality; }
			set
				{
				_SettingsQuality = value;
				OnPropertyChanged("SettingsQuality");
				}
			}

		private CSettingsAdvanced _SettingsAdvanced;

		public CSettingsAdvanced SettingsAdvanced
			{
			get { return _SettingsAdvanced; }
			set
				{
				_SettingsAdvanced = value;
				OnPropertyChanged("SettingsAdvanced");
				}
			}

		private String _Result;

		public String Result
			{
			get { return _Result; }
			set
				{
				_Result = value;
				OnPropertyChanged("Result");
				}
			}

		#endregion

		#region Constructors

		public CSettingsManager()
			{
			SettingsDictionary = new ObservableCollection<CSetting>();
			SettingsScreen = new CSettingsScreen(this); // Must be here to init video modes in time
			SavedUserSettingsList=new ObservableCollection<DirectoryInfo>();
			UseAdvanced = CTSWOptions.UseAdvancedSettings;
			GetSavedSettings();
			}

		public void Init()
			{
			SettingsHUD = new CSettingsHUD(this);
			SettingsAdvanced = new CSettingsAdvanced(this);
			SettingsGamePlay = new CSettingsGamePlay(this);
			SettingsSound = new CSettingsSound(this);
			SettingsQuality = new CSettingsQuality(this);
			SettingsAdvanced= new CSettingsAdvanced(this);
			SettingsHUD.Init();
			SettingsSound.Init();
			SettingsGamePlay.Init();
			SettingsScreen.Init();
			SettingsQuality.Init();
			SettingsAdvanced.Init();
			}

		public void Update()
			{
			SettingsHUD.Update();
			SettingsSound.Update();
			SettingsGamePlay.Update();
			SettingsScreen.Update();
			SettingsQuality.Update();
			SettingsAdvanced.Update();
			}

		#endregion

		#region Filehandling

		public FileInfo GetInGameSettingsLocation()
			{
			var MyPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			MyPath += @"\My Games\TS2Prototype\Saved\Config\WindowsNoEditor\GameUserSettings.ini";
			return new FileInfo(MyPath);
			}

		public FileInfo GetInGameEngineIniLocation()
			{
			var MyPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			MyPath += @"\My Games\TS2Prototype\Saved\Config\WindowsNoEditor\Engine.ini";
			return new FileInfo(MyPath);
			}

		public void GetSavedSettings()
			{
			String Path = CTSWOptions.OptionsSetDir;
			DirectoryInfo SavedSettingsDir= new DirectoryInfo(Path);
			DirectoryInfo[] SavedSets = SavedSettingsDir.GetDirectories("*",SearchOption.TopDirectoryOnly);
			SavedUserSettingsList.Clear();
			foreach (var X in SavedSets)
				{
				SavedUserSettingsList.Add(X);
				}
			}

		public void LoadSettingsInDictionary(FileInfo SettingsFile, FileInfo EngineIniFile = null)
			{
			if (!File.Exists(SettingsFile.FullName))
				{
				Result += CLog.Trace("Settings file " + SettingsFile.FullName + " not found",
					LogEventType.Message);
				return;
				}

			SettingsDictionary.Clear();
			ProcessSettingsFile(SettingsFile);

			if(EngineIniFile==null)
				{
				Result += CLog.Trace("Settings file Engine.ini" + " not specified");
				return;
			}

			if (!File.Exists(EngineIniFile.FullName))
				{
				Result += CLog.Trace("Engine.ini file " + EngineIniFile.FullName + " not found");
				return;
				}
			ProcessSettingsFile(EngineIniFile);
			}

		public void LoadSaveSet()
			{
			if (SaveSetName.Length < 1)
				{
				Result += CLog.Trace("SaveSet name is not valid", LogEventType.Error);
				return; // failed, should never happen
				}

			var Path = CTSWOptions.OptionsSetDir + SaveSetName + "\\"; // Warning order is important here
			var SettingsFile = new FileInfo(Path + "GameUserSettings.ini");
			var EngineIniFile= new FileInfo(Path + "Engine.ini");
			GetSavedSettings();
			LoadSettingsInDictionary(SettingsFile, EngineIniFile);
			Init();
		}

		private void ProcessSettingsFile(FileInfo SettingsFile)
			{
			try
				{
				using (var Reader = new StreamReader(SettingsFile.FullName))
					{
					String Str;
					CurrentSection = SectionEnum.None;
					while ((Str = Reader.ReadLine()) != null)
						{
						if (Str.StartsWith("["))
							{
							var Setting = new CSetting(Str, CurrentSection);
							CurrentSection = Setting.Section;
							if (CurrentSection != SectionEnum.Core)
								{
								}
							}
						else
							{
							if (CurrentSection == SectionEnum.Core)
								{
								SectionCore += Str+"\r\n";
								}
							else
								{
								if (Str.Length > 1)
									{
									var Setting = new CSetting(Str, CurrentSection);
									SettingsDictionary.Add(Setting);
									}
								}
							}
						}
					}
				CurrentUserSettingsFile = SettingsFile;
				}
			catch (Exception E)
				{
				Result += CLog.Trace(
					"Cannot read Settings file " + SettingsFile.FullName + " because " + E.Message,
					LogEventType.Error);
				return;
				}
			}

		public void WriteSettingsToSaveSet()
			{
			if (SaveSetName.Length < 3)
				{
				Result += CLog.Trace("SaveSet name is too short", LogEventType.Error);
				return; // failed, should never happen
				}

			var Path = CTSWOptions.OptionsSetDir + SaveSetName + "\\"; // Warning order is important here
			if (!Directory.Exists(Path))
				{
				var Dir = Directory.CreateDirectory(Path);
				SavedUserSettingsList.Add(Dir);
				}
			var SettingsFile= new FileInfo(Path+"GameUserSettings.ini");
			Update();
			WriteSettingsInDictionary(SettingsFile);
			var EngineIniFile= new FileInfo(Path + "Engine.ini");
			WriteSettingsInDictionary(EngineIniFile,true);
		}

		public void WriteSettingsInDictionary(FileInfo SettingsFile,Boolean EngineIni=false)
			{
			if (SettingsFile == null)
				{
				return;
				}

			try
				{
				using (var Writer = new StreamWriter(SettingsFile.FullName))
					{
					if (EngineIni)
						{
						WriteSection(Writer,SectionEnum.Core);
						if (CTSWOptions.UseAdvancedSettings)
							{
							WriteSection(Writer, SectionEnum.System);
							}
						}
					else
						{
						WriteSection(Writer, SectionEnum.User);
						WriteSection(Writer, SectionEnum.Engine);
						WriteSection(Writer, SectionEnum.Scalability);
						}
					}
				}
			catch (Exception E)
				{
				Result += CLog.Trace(
					"Cannot write Settings file " + SettingsFile.FullName + " because " + E.Message,
					LogEventType.Error);
				return;
				}
			}

		private void WriteSection(StreamWriter Writer, SectionEnum Section)
			{
			if (Section == SectionEnum.Core)
				{
				Writer.WriteLine(Section.ToName());
				Writer.WriteLine(SectionCore);
				return;
				}
			Writer.WriteLine(Section.ToName());
			foreach (var Entry in SettingsDictionary)
				{
				if (Entry.Value.Length > 0 && Section==Entry.Section)
					{
					Writer.WriteLine(Entry.ToString());
					}
				}
			}

		private Int32 GetItemIndex(String Key)
			{
			var Idx = 0;
			foreach (var Setting in SettingsDictionary)
				{
				if (String.CompareOrdinal(Key, Setting.Key) == 0)
					{
					return Idx;
					}

				Idx++;
				}
			return -1;
			}

		public void UpdateSetting(String Key, String Value, SectionEnum Section)
			{
			var Idx = GetItemIndex(Key);
			if (Idx == -1)
				{
				var Setting = new CSetting(Key, Value, Section);
				SettingsDictionary.Add(Setting);
				Result += CLog.Trace("Settings key not found " + Key);
				return;
				}
			var Setting2 = SettingsDictionary[Idx];
			Setting2.Value = Value;
			}

		public void GetSetting(String Key, out String Value)
			{
			var Idx = GetItemIndex(Key);
			if (Idx == -1)
				{
				Result += CLog.Trace("Settings key not found " + Key);
				Value = String.Empty;
				return;
				}

			Value = SettingsDictionary[Idx].Value;
			}

		#endregion
		}
	}
