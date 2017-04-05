package md505c6d22fffa1a14ca8a2a83beabb96fe;


public class EventCalendarViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Key_Club.Droid.EventCalendarViewHolder, Key_Club.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", EventCalendarViewHolder.class, __md_methods);
	}


	public EventCalendarViewHolder (android.view.View p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == EventCalendarViewHolder.class)
			mono.android.TypeManager.Activate ("Key_Club.Droid.EventCalendarViewHolder, Key_Club.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
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
