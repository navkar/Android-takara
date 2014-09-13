using System;
using System.IO;
using System.Json;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.Res;
using Android.Util;

using OnePassport.Takara;
using OnePassport.Takara.Model;

namespace OnePassport.Takara.Android
{
    class TakaraHelper
    {
        private const string LOG_TAG = "TakaraHelper";
        private const string FILE_NAME = "takarachest.dat";
        /// <summary>
        /// Reads the Asset 'Takarachest.dat'
        /// </summary>
        internal static TakaraChest ReadTakaraChest(Activity activity)
        {
            TakaraChest takaraChestObject = null;
            StreamReader sr = null;
            bool fileNotFound = false;

            try
            {
                // If file does NOT exist read from Asset.
                try
                {
                    Stream fileStream = activity.OpenFileInput(FILE_NAME);
                    // Use the stream reader.
                    sr = new StreamReader(fileStream);
                    // Get the Takara object after deserialization.
                    takaraChestObject = JsonHelper.DeserializeFromJson(sr);
                }
                catch (Exception ex)
                {
                    Log.Debug(LOG_TAG, ex.Message + ex.StackTrace);
                    fileNotFound = true;
                }

                if (fileNotFound) // This will happen only once per lifetime of the application.
                {
                    // Obtain the file stream from the JSON file.
                    Stream assetStream = activity.Assets.Open(FILE_NAME);
                    // Use the stream reader.
                    sr = new StreamReader(assetStream);
                    // Get the Takara object after deserialization.
                    takaraChestObject = JsonHelper.DeserializeFromJson(sr);
                }
            }
            catch (Exception e)
            {
                Log.Debug(LOG_TAG, e.Message + e.StackTrace);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr = null; // garbage collect.
                }
            }

            Log.Debug(LOG_TAG, "ReadTakaraChest() executed with FileNotFound flag :" + fileNotFound);
            return takaraChestObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        internal static void WriteTakaraChest(Activity activity, TakaraChest takaraChest)
        {
            StreamWriter sw = null;

            try
            {
                // Obtain the file stream from the JSON file.
                Stream fileStream = activity.OpenFileOutput(FILE_NAME, FileCreationMode.Private);
                // Use the stream reader.
                sw = new StreamWriter(fileStream);
                // Get the Takara object after deserialization.
                string jsonData = JsonHelper.SerializeToJson(takaraChest);
                sw.Write(jsonData);
                sw.Flush();
            }
            catch (Exception e)
            {
                Log.Debug(LOG_TAG, e.Message + e.StackTrace);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw = null; // garbage collect.
                }
            }

            Log.Debug(LOG_TAG, "WriteTakaraChest() executed.");
         }
    }
}