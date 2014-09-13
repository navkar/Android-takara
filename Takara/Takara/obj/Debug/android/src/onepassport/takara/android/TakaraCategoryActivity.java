package onepassport.takara.android;


public class TakaraCategoryActivity
	extends android.app.Activity
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onCreateContextMenu:(Landroid/view/ContextMenu;Landroid/view/View;Landroid/view/ContextMenu$ContextMenuInfo;)V:GetOnCreateContextMenu_Landroid_view_ContextMenu_Landroid_view_View_Landroid_view_ContextMenu_ContextMenuInfo_Handler\n" +
			"n_onContextItemSelected:(Landroid/view/MenuItem;)Z:GetOnContextItemSelected_Landroid_view_MenuItem_Handler\n" +
			"n_onPause:()V:GetOnPauseHandler\n" +
			"n_onResume:()V:GetOnResumeHandler\n" +
			"";
		mono.android.Runtime.register ("OnePassport.Takara.Android.TakaraCategoryActivity, OnePassport.Takara.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TakaraCategoryActivity.class, __md_methods);
	}

	public TakaraCategoryActivity ()
	{
		super ();
		if (getClass () == TakaraCategoryActivity.class)
			mono.android.TypeManager.Activate ("OnePassport.Takara.Android.TakaraCategoryActivity, OnePassport.Takara.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	@Override
	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	@Override
	public void onCreateContextMenu (android.view.ContextMenu p0, android.view.View p1, android.view.ContextMenu.ContextMenuInfo p2)
	{
		n_onCreateContextMenu (p0, p1, p2);
	}

	private native void n_onCreateContextMenu (android.view.ContextMenu p0, android.view.View p1, android.view.ContextMenu.ContextMenuInfo p2);

	@Override
	public boolean onContextItemSelected (android.view.MenuItem p0)
	{
		return n_onContextItemSelected (p0);
	}

	private native boolean n_onContextItemSelected (android.view.MenuItem p0);

	@Override
	public void onPause ()
	{
		n_onPause ();
	}

	private native void n_onPause ();

	@Override
	public void onResume ()
	{
		n_onResume ();
	}

	private native void n_onResume ();

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
