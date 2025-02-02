﻿using System;
using System.Diagnostics;
using System.Windows;

namespace TSWTools
	{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
		{
		public CMain MainData { get; set; }
		public CLog LogForm;
		public static CLogEventHandler LogEventHandler { get; set; }

		public MainWindow()
			{
			InitializeComponent();
			// trace setup
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
			Trace.AutoFlush = true;
			Trace.Indent();
			LogEventHandler = new CLogEventHandler();
			LogForm = new CLog();
			MainData = new CMain();
			DataContext = MainData;
			}

		#region Utilities

		private void OnOptionsButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormOptions();
			Form.ShowDialog();
			}

		private void OnLogViewButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormLogViewer(LogForm);
			Form.Show();
			}

		private void OnBackupButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormBackup();
			Form.Show();
			}

		private void OnKeyBindingButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var InputMapperList = new CInputMapperList();
			var Form = new FormInputMapperManager(InputMapperList);
			Form.Show();
			}

		#endregion

		#region Unpack

		private void OnUnpackerButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormUnpackGameFiles();
			Form.Show();
			}

		private void OnViewUnpackedPaksButtonClicked(Object Sender, RoutedEventArgs E)
			{
			CApps.OpenFolder(CTSWOptions.UnpackFolder);
			}

		private void OnUModelLauncherButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormLaunchUModel();
			Form.Show();
			}
		#endregion

		#region Tools

		private void OnLaunchTSWButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormLaunchTSW();
			Form.Show();
			}

		private void OnScreenshotManagerButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormScreenshotManager();
			Form.Show();
			}

		private void OnLiveryManagerButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var LiveryManager = new CLiveryManager();
			var Form = new FormLiveryManager(LiveryManager);
			Form.Show();
			}

		private void OnEditSettingsButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormSettings();
			Form.Show();
			}

    private void OnRadioStationsButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var Form= new FormRadioStationManager();
      Form.Show();
      }

    private void OnPakInstallerButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var Form=new FormPakInstaller();
      Form.Show();
      }

    #endregion

    #region Help

    private void OnAboutButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormAbout();
			Form.Show();
			}

		private void OnManualButtonClicked(Object Sender, RoutedEventArgs E)
			{
			MainData.OpenManual();
			}

		private void OnStartersGuideButtonClicked(Object Sender, RoutedEventArgs E)
			{
			MainData.OpenStartersGuide();
			}

		private void OnRouteGuidesButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormRouteGuides(MainData);
			Form.Show();
			}

		#endregion

		#region Closing

		private void OnOkButtonClicked(Object Sender, RoutedEventArgs E)
			{
			Close();
			}


    #endregion
 
    }
	}
