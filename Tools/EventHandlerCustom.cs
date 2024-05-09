using System;

public class EventHandlerCustom {

	private EventHandler _eventHandler;
	
	public EventHandler EventsEmpty;

	// Counter to keep track of the number of subscribers
	private int _subscriberCount = 0;
	private int _subscribersRunning = 0;

	// Event property
	public event EventHandler Event
	{
		add
		{
			_eventHandler += value;
			_subscriberCount++;
		}
		remove {
			_eventHandler -= value;
			_subscriberCount--;
			
			CheckIfSubscribersFinished();
		}
	}

	// Method to get the number of subscribers
	public int GetSubscriberCount() {
		return _subscriberCount;
	}
	
	public int GetSubscribersStillRunning() {
		return _subscribersRunning;
	}

	// Method to raise the event
	public void RaiseEvent(object sender, EventArgs e, EventHandler EventsEmptyAction = null) {
		_subscribersRunning = _subscriberCount;
		_eventHandler?.Invoke(sender, e);
		EventsEmpty += EventsEmptyAction;

		CheckIfSubscribersFinished();
	}

	public void SubscriberFinished() {
		_subscribersRunning--;
		CheckIfSubscribersFinished();
	}

	void CheckIfSubscribersFinished() {
		if (_subscribersRunning == 0) {
			EventsEmpty?.Invoke(this, EventArgs.Empty);
			EventsEmpty = null;
		}
	}
}
