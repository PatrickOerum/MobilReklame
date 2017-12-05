﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Newtonsoft.Json;

namespace MobilReklame
{
    class PersistencyServiceOrderArchive
    {
        private static string JsonFileName = "OrderArchive.json";

        public static async void SaveOrderArchiveAsJsonAsync(ObservableCollection<OrderArchiveSingleton> orderarchive)
        {
            string orderarchiveJsonString = JsonConvert.SerializeObject(orderarchive);
            SerializeOrderArchiveFileAsync(orderarchiveJsonString, JsonFileName);
        }

        public static async Task<List<OrderArchiveSingleton>> LoadOrderArchiveFromJsonAsync()
        {
            string orderarchiveJsonString = await DeserializeOrderArchiveFileAsync(JsonFileName);
            if (orderarchiveJsonString != null)
                return (List<OrderArchiveSingleton>)JsonConvert.DeserializeObject(orderarchiveJsonString, typeof(List<OrderArchiveSingleton>));
            return null;
        }



        private static async void SerializeOrderArchiveFileAsync(string orderarchiveJsonString, string fileName)
        {
            StorageFile localFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(localFile, orderarchiveJsonString);
        }


        private static async Task<string> DeserializeOrderArchiveFileAsync(string fileName)
        {
            try
            {
                StorageFile localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return await FileIO.ReadTextAsync(localFile);
            }
            catch (FileNotFoundException ex)
            {
                MessageDialogHelper.Show("Loading for the first time? - Try Add and Save some Notes before trying to Save for the first time", "File not Found");
                return null;
            }
        }


        private class MessageDialogHelper
        {
            public static async void Show(string content, string title)
            {
                MessageDialog messageDialog = new MessageDialog(content, title);
                await messageDialog.ShowAsync();
            }
        }
    }
}
