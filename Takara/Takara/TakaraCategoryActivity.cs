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
    /// <summary>
    /// Layouts used - Takaras.axml, TakarasLVR.axml.
    /// This class opens the Takara category and displays all the available Takaras.
    /// </summary>
    [Activity(Label = "Takara", MainLauncher = false, Icon = "@drawable/treasurechest",
        NoHistory = false, ScreenOrientation = ScreenOrientation.Portrait
       )]
    public class TakaraCategoryActivity : Activity
    {
        private const string LOG_TAG = "TakaraCategoryActivity";
        private int TAKARA_CAT_ACTIVITY = 102;
        private int _categoryPosition = 0;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            bool isCustomTitleSupported = this.RequestWindowFeature(WindowFeatures.CustomTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Takaras);

            if (isCustomTitleSupported)
            {
                this.Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Titlebar);

                // Get the ListView and try to bind it.
                TextView tvcustomTitle = FindViewById<TextView>(Resource.Id.customTitle);

                tvcustomTitle.Text = Resources.GetString(Resource.String.TitleBar1_0) + Resources.GetString(Resource.String.TitleBarSeparator) + Resources.GetString(Resource.String.TitleBar1_1);
            }

            // Read the file in JSON format and get all the required data for categories.
            TakaraChest chestObject = TakaraHelper.ReadTakaraChest(this);
            Log.Debug(LOG_TAG, "Check chestObject ");

            // Create a new intent and get the bundle from it. 
            Intent activityIntent = Intent;
            // This bundle shall hold the context data.
            Bundle dataBundle = activityIntent.GetBundleExtra("Takara.TakaraChestActivity");

            if (chestObject != null)
            {
                string selectedCategoryName = dataBundle.GetString("SELECTED_CATEGORY_NAME");
                Log.Debug(LOG_TAG, "selectedCategoryName:" + selectedCategoryName);

                string selectedCategoryPosition = dataBundle.GetString("SELECTED_CATEGORY_POSITION");
                Log.Debug(LOG_TAG, "selectedCategoryPosition:" + selectedCategoryPosition);

                int iCategoryIndex = 0; 
                Int32.TryParse(selectedCategoryPosition, out iCategoryIndex);
                _categoryPosition = iCategoryIndex;

                TakaraCategory category = chestObject.Takaras[iCategoryIndex];
                int takaraCatCount = category.Takaras.Count;
                string toastData = this.Resources.GetQuantityString(Resource.Plurals.NoOfTakarasAvailable, takaraCatCount, takaraCatCount);
                Toast.MakeText(this, toastData, ToastLength.Long).Show();

                List<IDictionary<string, object>> listObject = new List<IDictionary<string, object>>();
                Dictionary<string, object> takaraDictionary = null;

                int positionIndex = 0;
                // Iterate through every category and bind it to Android list view.
                foreach (OnePassport.Takara.Model.Takara takara in category.Takaras)
                {
                    takaraDictionary = new Dictionary<string, object>();
                    takaraDictionary.Add("Name", takara.Name);
                    takaraDictionary.Add("Desc", takara.Desc);
                    takaraDictionary.Add("Value", takara.Value);
                    takaraDictionary.Add("Index", positionIndex.ToString());

                    listObject.Add((IDictionary<string, object>) takaraDictionary);
                    positionIndex++;
                }

                IList<IDictionary<string, object>> list = (IList<IDictionary<string, object>>)listObject;

                BindListView(list);
            }
        }

        /// <summary>
        /// Binds the ListView to the simple adapter (used for LV customization).
        /// </summary>
        /// <param name="list"></param>
        private void BindListView(IList<IDictionary<string, object>> list)
        {
            // Get the ListView and try to bind it.
            ListView lvTakaras = FindViewById<ListView>(Resource.Id.LvTakaras);
            
            // Registering the ListView for a context menu.
            RegisterForContextMenu(lvTakaras);
            
            if (lvTakaras != null)
            {
                lvTakaras.TextFilterEnabled = true;

                // Attach the simple adapter to the list view.
                lvTakaras.Adapter = new SimpleAdapter(
                    this, // Context
                    list, //
                    Resource.Layout.TakarasLVR,
                    new String[] { "Name", "Desc" },   // Data source names
                    new int[] { Resource.Id.takaraName, Resource.Id.takaraDesc }); // Target int ids.

                // Attaching a item click handler to the list view.
                lvTakaras.ItemClick += new EventHandler<ItemEventArgs>(lvTakaras_ItemClick);
            }

            Log.Debug(LOG_TAG, "BindListView() executed with lvcategory::ID " + lvTakaras.Id);
        }

        /// <summary>
        /// Event handler executes when a list view item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">View</param>
        void lvTakaras_ItemClick(object sender, ItemEventArgs e)
        {
            // This is the user selected category dictionary.
            IDictionary<string, object> takaraDictionary = (IDictionary<string, object>)(sender as ListView).GetItemAtPosition(e.Position);

            // Start an intent activity to display - wow! 
            Intent intent = new Intent(this, typeof(TakaraViewActivity));

            // Create a bindle to send data to the intent.
            Bundle dataForActivity = new Bundle();

            // This is the position of the Takara in the TakaraCategory.
            string position = (string)takaraDictionary["Index"];
            Log.Debug(LOG_TAG, "Bundle Takara position: " + position);

            dataForActivity.PutString("SELECTED_TAKARA_POSITION", position);
            dataForActivity.PutString("SELECTED_CATEGORY_POSITION", _categoryPosition.ToString());

            // Name must be fully qualified class name - recommended.
            intent.PutExtra("Takara.TakaraCategoryActivity", dataForActivity);

            this.StartActivityForResult(intent, TAKARA_CAT_ACTIVITY);
        }

        /// <summary>
        /// Context menu callback method.
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="v"></param>
        /// <param name="menuInfo"></param>
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
            menu.SetHeaderTitle(Resource.String.TakarasCtxMnuTitle);

            MenuInflater inflater = this.MenuInflater;
            inflater.Inflate(Resource.Menu.TakarasCtxMenu, menu);
        }

        /// <summary>
        /// Context Menu item selection - event handler.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnContextItemSelected(IMenuItem item)
        {
            AdapterView.AdapterContextMenuInfo contextMenuInfo = (AdapterView.AdapterContextMenuInfo) item.MenuInfo;

            switch (item.ItemId)
            {
                case Resource.Id.ctxMenuAdd:
                    Toast.MakeText(this, "Context Add", ToastLength.Long).Show();
                    return true;
                case Resource.Id.ctxMenuDelete:

                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle(Resource.String.TitleL2);
                    builder.SetMessage(Resource.String.DeleteAlert);
                    builder.SetCancelable(false);
                    // EventHandler<DialogClickEventArgs>
                    //builder.SetNegativeButton(
                    //builder.SetPositiveButton(Resource.String.DeleteAlertYES,
                    //    delegate(<DialogClickEventArgs>)
                    //        {
                               
                    //        }


                    //builder.SetNegativeButton(Resource.String.DeleteAlertNO, null);
                    
                    return true;
                default:
                    return base.OnContextItemSelected(item);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
//            Finish();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}