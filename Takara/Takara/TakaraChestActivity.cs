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
    /// Layouts used - TakaraCategories.axml, TakaraCategoriesLVR.axml.
    /// This class opens the TakaraChest and displays all the available categories.
    /// </summary>
    [Activity(Label = "Takara", MainLauncher = true, Icon = "@drawable/treasurechest",
        NoHistory = false, ScreenOrientation = ScreenOrientation.Portrait
       )]
    public class TakaraChestActivity : Activity
    {
        private const string LOG_TAG = "TakaraChestActivity";
        private int TAKARA_CHEST_ACTIVITY = 101;
        private ListView lvCategory = null;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // Generate any additional actions that can be performed on the        
            // overall list.  In a normal install, there are no additional        
            // actions found here, but this allows other applications to extend        
            // our menu with their own actions.
            MenuInflater inflater = this.MenuInflater;
            inflater.Inflate(Resource.Menu.OptionsMenu, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem menuItem)
        {
            int ADD_CATEGORY = 0;

            switch (menuItem.ItemId)
            {
                case Resource.Id.miAdd:
                    // Use StartActityForResult.
                    // Starts a new activity.
                    Intent intent = new Intent(this, typeof(AddTakaraCategory));
                    this.StartActivityForResult(intent, ADD_CATEGORY);
                    break;

                case Resource.Id.miDelete:
                    Toast.MakeText(this, "You pressed Delete menu item.", ToastLength.Long).Show();
                    break;
            }
            
            return base.OnOptionsItemSelected(menuItem);
        }

        /// <summary>
        /// Build UI 
        /// </summary>
        /// <param name="takaraChest"></param>
        private void BuildUI(TakaraChest takaraChest)
        {
            if (takaraChest != null)
            {
                int takaraCatCount = takaraChest.Takaras.Count;
                string toastData = this.Resources.GetQuantityString(Resource.Plurals.NoOfTakarasAvailable, takaraCatCount, takaraCatCount);
                Toast.MakeText(this, toastData, ToastLength.Long).Show();

                List<IDictionary<string, object>> listObject = new List<IDictionary<string, object>>();
                Dictionary<string, object> categoryDict = null;

                int positionIndex = 0;
                // Iterate through every category and bind it to Android list view.
                foreach (TakaraCategory category in takaraChest.Takaras)
                {
                    categoryDict = new Dictionary<string, object>();
                    categoryDict.Add("Name", category.Name + " (" + category.Takaras.Count + ")");
                    categoryDict.Add("Desc", category.Desc);
                    categoryDict.Add("ItemCount", category.Takaras.Count);
                    categoryDict.Add("Index", positionIndex.ToString());

                    listObject.Add((IDictionary<string, object>)categoryDict);
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
            lvCategory = FindViewById<ListView>(Resource.Id.LvCategory);

            if (lvCategory != null)
            {
                lvCategory.TextFilterEnabled = true;

                // Attach the simple adapter to the list view.
                lvCategory.Adapter = new SimpleAdapter(
                    this, // Context
                    list, //
                    Resource.Layout.TakaraCategoriesLVR,
                    new String[] { "Name", "Desc" },   // Data source names
                    new int[] { Resource.Id.categoryName, Resource.Id.categoryDesc }); // Target int ids.

                // Attaching a item click handler to the list view.
                lvCategory.ItemClick += new EventHandler<ItemEventArgs>(lvCategory_ItemClick);
            }

            Log.Debug(LOG_TAG, "BindListView() executed with lvcategory::ID " + lvCategory.Id);
        }

        /// <summary>
        /// Event handler executes when a list view item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">View</param>
        void lvCategory_ItemClick(object sender, ItemEventArgs e)
        {
            TextView tvCategoryName = e.View.FindViewById<TextView>(Resource.Id.categoryName);

            // This is the user selected category dictionary.
            IDictionary<string, object> categoryDictionary = (IDictionary<string, object>)(sender as ListView).GetItemAtPosition(e.Position);
            
            // Start an intent activity to display - wow! 
            Intent intent = new Intent(this, typeof(TakaraCategoryActivity));
            
            // Create a bindle to send data to the intent.
            Bundle dataForActivity = new Bundle();
            // This is the name of it. 
            dataForActivity.PutString("SELECTED_CATEGORY_NAME", tvCategoryName.Text);
            // This is the position of the TakaraCategory in the catalog.
            string position = (string)categoryDictionary["Index"];
            Log.Debug(LOG_TAG, "Bundle LV Category position: " + position);
            
            dataForActivity.PutString("SELECTED_CATEGORY_POSITION", position);

            // Name must be fully qualified class name - recommended.
            intent.PutExtra("Takara.TakaraChestActivity", dataForActivity);

            this.StartActivityForResult(intent, TAKARA_CHEST_ACTIVITY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Log.Debug(LOG_TAG, "Called OnActivityResult()");

            if (requestCode == TAKARA_CHEST_ACTIVITY 
                && resultCode == Result.Ok 
                && data != null 
                && data.GetStringExtra("ADD_TAKARA_CATEGORY") == "OK"
                )
            {
                // TODO: Need to investigate why this is not being called.
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            // Read the file in JSON format and get all the required data for categories.
            TakaraChest takaraChest = TakaraHelper.ReadTakaraChest(this);

            BuildUI(takaraChest);
        }

        protected override void OnPause()
        {
            base.OnPause();
            //Finish();
        }

        /// <summary>
        /// The entry point for the Takara chest activity.
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Check if custom title bar is supported.
            bool isCustomTitleSuuported = this.RequestWindowFeature(WindowFeatures.CustomTitle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.TakaraCategories);

            if (isCustomTitleSuuported)
            {
                this.Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Titlebar);
            }

            // Read the file in JSON format and get all the required data for categories.
            TakaraChest takaraChest = TakaraHelper.ReadTakaraChest(this);

            BuildUI(takaraChest);
        }

    }
}

