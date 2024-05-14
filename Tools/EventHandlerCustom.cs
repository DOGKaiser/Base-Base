using System;

public class EventHandlerCustom {

	private EventHandler _eventHandler;
	
	public EventHandler OnEventsEmpty;

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
			SubscriberFinished();
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
	public void RaiseEvent(object sender, EventArgs e, Action action) {
		EventHandler wrappedAction = (sender, args) => action?.Invoke();
		RaiseEvent(sender, e, wrappedAction);
	}
	
	public void RaiseEvent(object sender, EventArgs e, EventHandler EventsEmptyAction = null) {
		_subscribersRunning = _subscriberCount;
		_eventHandler?.Invoke(sender, e);
		OnEventsEmpty += EventsEmptyAction;

		CheckIfSubscribersFinished();
	}

	public void SubscriberFinished() {
		_subscribersRunning--;
		CheckIfSubscribersFinished();
	}

	void CheckIfSubscribersFinished() {
		if (_subscribersRunning == 0) {
			OnEventsEmpty?.Invoke(this, EventArgs.Empty);
			OnEventsEmpty = null;
		}
	}
}
