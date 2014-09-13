package onepassport.takara.android;


public class TakaraViewActivity
	extends android.app.Activity
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("OnePassport.Takara.Android.TakaraViewActivity, OnePassport.Takara.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TakaraViewActivity.class, __md_methods);
	}

	public TakaraViewActivity ()
	{
		super ();
		if (getClass () == TakaraViewActivity.class)
			mono.android.TypeManager.Activate ("OnePassport.Takara.Android.TakaraViewActivity, OnePassport.Takara.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	@Override
	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
