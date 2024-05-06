using System;

public class EventHandlerCustom {

	private EventHandler _eventHandler;
	
	public EventHandler EventsEmpty;

	// Counter to keep track of the number of subscribers
	private int _subscriberCount = 0;

	// Event property
	public event EventHandler Event
	{
		add
		{
			_eventHandler += value;
			_subscriberCount++;
		}
		remove
		{
			_eventHandler -= value;
			_subscriberCount--;
			
			if (_subscriberCount == 0) {
				EventsEmpty?.Invoke(this, EventArgs.Empty);
				EventsEmpty = null;
			}
		}
	}

	// Method to get the number of subscribers
	public int GetSubscriberCount()
	{
		return _subscriberCount;
	}

	// Method to raise the event
	public void RaiseEvent(object sender, EventArgs e, EventHandler EventsEmptyAction = null)
	{
		_eventHandler?.Invoke(sender, e);
		EventsEmpty += EventsEmptyAction;

		if (GetSubscriberCount() == 0) {
			EventsEmpty?.Invoke(this, e);
			EventsEmpty = null;
		}
	}
}
