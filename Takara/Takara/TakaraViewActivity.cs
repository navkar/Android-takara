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
    [Activity(Label = "Takara View", MainLauncher = false, Icon = "@drawable/treasurechest", NoHistory = true)]
    public class TakaraViewActivity : Activity
    {
        private const string LOG_TAG = "TakaraViewActivity";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            bool isCustomTitleSupported = this.RequestWindowFeature(WindowFeatures.CustomTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.TakaraView);

            if (isCustomTitleSupported)
            {
                this.Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Titlebar);

                // Get the ListView and try to bind it.
                TextView tvcustomTitle = FindViewById<TextView>(Resource.Id.customTitle);

                tvcustomTitle.Text = Resources.GetString(Resource.String.TitleBar1_0)
                    + Resources.GetString(Resource.String.TitleBarSeparator)
                    + Resources.GetString(Resource.String.TitleBar1_1)
                    + Resources.GetString(Resource.String.TitleBarSeparator)
                    + Resources.GetString(Resource.String.TitleBar1_2);
            }

            // Read the file in JSON format and get all the required data for categories.
            TakaraChest chestObject = TakaraHelper.ReadTakaraChest(this);
            Log.Debug(LOG_TAG, "Check chestObject ");

            // Create a new intent and get the bundle from it. 
            Intent activityIntent = Intent;
            // This bundle shall hold the context data.
            Bundle dataBundle = activityIntent.GetBundleExtra("Takara.TakaraCategoryActivity");

            if (chestObject != null)
            {
                string selectedTakaraPosition = dataBundle.GetString("SELECTED_TAKARA_POSITION");
                Log.Debug(LOG_TAG, "selectedTakaraPosition:" + selectedTakaraPosition);

                string selectedCategoryPosition = dataBundle.GetString("SELECTED_CATEGORY_POSITION");
                Log.Debug(LOG_TAG, "selectedCategoryPosition:" + selectedCategoryPosition);

                int iCategoryIndex = 0, iTakaraIndex = 0;
                Int32.TryParse(selectedCategoryPosition, out iCategoryIndex);
                Int32.TryParse(selectedTakaraPosition, out iTakaraIndex);

                TakaraCategory category = chestObject.Takaras[iCategoryIndex];
                OnePassport.Takara.Model.Takara takara = category.Takaras[iTakaraIndex];

                BindToView(takara);
            }

        }

        /// <summary>
        /// Binds the view to Takara object.
        /// </summary>
        /// <param name="takara"></param>
        private void BindToView(OnePassport.Takara.Model.Takara takara)
        {
            // Get the TextView and try to bind it.
            TextView tvTakaraName = FindViewById<TextView>(Resource.Id.tvName);
            tvTakaraName.Text = takara.Name;

            // Get the TextView and try to bind it.
            TextView tvTakaraDesc = FindViewById<TextView>(Resource.Id.tvDesc);
            tvTakaraDesc.Text = takara.Desc;

            // Get the TextView and try to bind it.
            TextView tvTakaraValue = FindViewById<TextView>(Resource.Id.tvValue);
            tvTakaraValue.Text = takara.Value;
        }
    }
}


