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
    [Activity(Label = "Takara - Add Category", MainLauncher = false, Icon = "@drawable/treasurechest", NoHistory=true)]
    public class AddTakaraCategory : Activity
    {
        private const string LOG_TAG = "AddTakaraCategory";
        private TakaraChest takaraChest = null;
        private EditText tCatName = null;
        private EditText tCatDesc = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            bool isCustomTitleSupported = this.RequestWindowFeature(WindowFeatures.CustomTitle);

            // Set to Add Category View
            SetContentView(Resource.Layout.AddTakaraCategory);

            if (isCustomTitleSupported)
            {
                this.Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Titlebar);

                // Get the ListView and try to bind it.
                TextView tvcustomTitle = FindViewById<TextView>(Resource.Id.customTitle);

                tvcustomTitle.Text = Resources.GetString(Resource.String.TitleBar1_0)
                    + Resources.GetString(Resource.String.TitleBarSeparator)
                    + Resources.GetString(Resource.String.TitleBar2_0);
            }

            // Read the file in JSON format and get all the required data for categories.
            takaraChest = TakaraHelper.ReadTakaraChest(this);
            Log.Debug(LOG_TAG, "Add Takara Category");

            tCatName = FindViewById<EditText>(Resource.Id.tCatName);
            tCatDesc = FindViewById<EditText>(Resource.Id.tCatDesc);

            // Get our button from the layout resource,
            // and attach an event to it
            Button okButton = FindViewById<Button>(Resource.Id.BtnOK);
            okButton.Click += new EventHandler(OK_Click);

            Button cancelButton = FindViewById<Button>(Resource.Id.BtnCancel);
            cancelButton.Click += new EventHandler(Cancel_Click);
        }

        private void OK_Click(object sender, EventArgs e)
        {
            TakaraCategory item = new TakaraCategory();
            item.Name = tCatName.Text;
            item.Desc = tCatDesc.Text;
            takaraChest.Takaras.Add(item);

            TakaraHelper.WriteTakaraChest(this, takaraChest);

            Toast.MakeText(this, "Created category " + tCatName.Text, ToastLength.Long).Show();

            // Notify that the Add takara category is complete.
            Intent intent = new Intent();
            intent.PutExtra("ADD_TAKARA_CATEGORY", "OK");

            if (Parent == null)
            {
                SetResult(Result.Ok, intent);
            }
            else
            {
                Parent.SetResult(Result.Ok, intent);
            }

            Finish();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            // Notify that the Add takara category is Cancelled.
            Intent intent = new Intent();
            intent.PutExtra("ADD_TAKARA_CATEGORY", "CANCEL");

            if (Parent == null)
            {
                SetResult(Result.Ok, intent);
            }
            else
            {
                Parent.SetResult(Result.Ok, intent);
            }
            
            Finish();
        }
    }
}

