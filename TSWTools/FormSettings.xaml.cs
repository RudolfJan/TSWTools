﻿using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TSWTools
	{
	/// <summary>
	/// Interaction logic for FormSettings.xaml
	/// </summary>
	public partial class FormSettings
		{
		public CSettingsManager SettingsManager { get; set; }

		public FormSettings()
			{
			InitializeComponent();
			SettingsManager = new CSettingsManager();
			DataContext = SettingsManager;
			SortDataGrid(VideoModesDataGrid, 0, ListSortDirection.Descending);
			SortDataGrid(VideoModesDataGrid, 2, ListSortDirection.Descending, true);
			SetControlStates();
			}

		private void SetControlStates()
			{
			//SaveSettings.IsEnabled = SettingsManager.CurrentUserSettingsFile != null;
			SetScreenResButton.IsEnabled = VideoModesDataGrid.SelectedItem != null;
			UpdateSaveSetButton.IsEnabled = SaveSetNameTextBox.Text.Length > 2;
			LoadSaveSetSettingsButton.IsEnabled = SettingFilesDataGrid.SelectedItem != null;
			SetRecommendedSoundButton.IsEnabled =
				SettingsManager != null && SettingsManager.SettingsSound != null;
      SetRecommendedAdvancedButton.IsEnabled =
        SettingsManager != null && SettingsManager.SettingsAdvanced != null;
      // View distance also is an advanced setting with a larger range.
      ViewDistanceComboBox.IsEnabled = UseAdvancedSettingsCheckBox.IsChecked==false;
      }

		private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
			{
			Close();
			}

		private void OnLoadActiveGameSettingsClicked(Object Sender, RoutedEventArgs E)
			{
			SettingsManager.LoadSettingsInDictionary(SettingsManager.GetInGameSettingsLocation(),
				SettingsManager.GetInGameEngineIniLocation());
			SettingsManager.Init();
			SettingFilesDataGrid.Items.SortDescriptions.Add(new SortDescription(NameColumn.SortMemberPath,
				ListSortDirection.Ascending));
			SettingsDictionaryDataGrid.Items.SortDescriptions.Add(
				new SortDescription(KeyColumn.SortMemberPath, ListSortDirection.Ascending));
			SettingsDictionaryDataGrid.Items.SortDescriptions.Add(
				new SortDescription(SectionColumn.SortMemberPath, ListSortDirection.Ascending));
			SetControlStates();
			}

		public static void SortDataGrid(DataGrid MyDataGrid, Int32 ColumnIndex = 0,
			ListSortDirection MySortDirection = ListSortDirection.Ascending, Boolean Add = false)
			{
			var Column = MyDataGrid.Columns[ColumnIndex];

			// Clear current sort descriptions
			if (!Add)
				{
				MyDataGrid.Items.SortDescriptions.Clear();
				}

			// Add the new sort description
			MyDataGrid.Items.SortDescriptions.Add(new SortDescription(Column.SortMemberPath,
				MySortDirection));

			// Apply sort
			foreach (var Col in MyDataGrid.Columns)
				{
				Col.SortDirection = null;
				}

			Column.SortDirection = MySortDirection;

			// Refresh items to display sort
			MyDataGrid.Items.Refresh();
			}

		private void OnSaveSettingsClicked(Object Sender, RoutedEventArgs E)
			{
			var FileName = SettingsManager.GetInGameSettingsLocation();
			SettingsManager.Update();
			SettingsManager.WriteSettingsInDictionary(FileName);
			FileName = SettingsManager.GetInGameEngineIniLocation();
			SettingsManager.WriteSettingsInDictionary(FileName,true);
			SetControlStates();
			}

		private void OnUpdateButtonClicked(Object Sender, RoutedEventArgs E)
			{
			SettingsManager.Update();
			}

		private void OnSetRecommendedSoundButtonClicked(Object Sender, RoutedEventArgs E)
			{
			SettingsManager.SettingsSound.SetRecommendedValues();
			}

		private void OnSetScreenResButtonClicked(Object Sender, RoutedEventArgs E)
			{
			SettingsManager.SettingsScreen.ResolutionSizeX =
				((VideoMode) VideoModesDataGrid.SelectedItem).Width;
			SettingsManager.SettingsScreen.ResolutionSizeY =
				((VideoMode) VideoModesDataGrid.SelectedItem).Height;
			}

		private void OnVideoModesDataGridSelectionChanged(Object Sender, SelectionChangedEventArgs E)
			{
			SetControlStates();
			}

		private void OnLoadSaveSetSettingsButtonClicked(Object Sender, RoutedEventArgs E)
			{
			SettingsManager.LoadSaveSet();
			}

		private void OnUpdateSaveSetButtonClicked(Object Sender, RoutedEventArgs E)
			{
			SettingsManager.WriteSettingsToSaveSet();
			}

		private void OnSaveSetNameTextBoxTextChanged(Object Sender, TextChangedEventArgs E)
			{
			SetControlStates();
			}

		private void OnSettingFilesDataGridSelectionChanged(Object Sender, SelectionChangedEventArgs E)
			{
			if (SettingFilesDataGrid.SelectedItem != null)
				{
				SettingsManager.SaveSetName = ((DirectoryInfo) SettingFilesDataGrid.SelectedItem).Name;
				}
		SetControlStates();
			}

		private void OnSetRecommendedAdvancedButtonClicked(Object Sender, RoutedEventArgs E)
			{
			SettingsManager.SettingsAdvanced.SetRecommendedValues();
			}

		private void UseAdvancedSettingsCheckBoxChanged(Object Sender, RoutedEventArgs E)
			{
			if (UseAdvancedSettingsCheckBox.IsChecked == true)
				{
				SettingsManager.UseAdvanced = true;
				}
			else
				{
				SettingsManager.UseAdvanced = false;
				}
			CTSWOptions.UseAdvancedSettings = SettingsManager.UseAdvanced;
			CTSWOptions.WriteToRegistry();
			}
		}
	}
