﻿using System;
using System.Windows;
using System.Collections.Generic;
using ErikEJ.SqlCeToolbox.Helpers;

namespace ErikEJ.SqlCeToolbox.Dialogs
{
    public partial class PickServerDatabaseDialog
    {
        public KeyValuePair<string, DatabaseInfo> SelectedDatabase { get; private set; }

        private readonly EFCorePowerTools.EFCorePowerToolsPackage _package;

        public PickServerDatabaseDialog(Dictionary<string, DatabaseInfo> serverConnections, EFCorePowerTools.EFCorePowerToolsPackage package)
        {
            _package = package;
            Telemetry.TrackPageView(nameof(PickServerDatabaseDialog));
            InitializeComponent();
            Background = VsThemes.GetWindowBackground();
            comboBox1.DisplayMemberPath = "Value.Caption";
            comboBox1.ItemsSource = serverConnections;
            if (serverConnections.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void comboBox1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (comboBox1.SelectedItem != null)
                SelectedDatabase = (KeyValuePair<string, DatabaseInfo>)comboBox1.SelectedItem;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox1.Focus();
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var info = EnvDteHelper.PromptForInfo(_package);
                if (info.DatabaseType == DatabaseType.SQLCE35) return;

                var databaseList = EnvDteHelper.GetDataConnections(_package);
                comboBox1.DisplayMemberPath = "Value.Caption";
                comboBox1.ItemsSource = databaseList;

                int i = 0;
                foreach (var item in databaseList)
                {
                    if (item.Key == info.ConnectionString)
                    {
                        comboBox1.SelectedIndex = i;
                    }
                    i++;
                }
            }

            catch (Exception exception)
            {
                EnvDteHelper.ShowMessage(exception.ToString());
            }
        }
    }
}