using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.Scripts.Services
{
	public class EventBroadcasterService
	{
		private readonly Dictionary<Type, List<Delegate>> eventListeners = new();
		private bool isBroadcasting;

		public async void Subscribe<T>(Action<T> listener) where T : class
		{
			var eventType = typeof(T);

			await UniTask.WaitUntil(() => !isBroadcasting);

			if (!eventListeners.TryGetValue(eventType, out var listeners))
			{
				listeners = new List<Delegate>();
				eventListeners[eventType] = listeners;
			}

			listeners.Add(listener);
		}

		public async void Unsubscribe<T>(Action<T> listener) where T : class
		{
			var eventType = typeof(T);

			await UniTask.WaitUntil(() => !isBroadcasting);

			if (eventListeners.TryGetValue(eventType, out var listeners))
			{
				listeners.Remove(listener);

				if (listeners.Count == 0)
				{
					eventListeners.Remove(eventType);
				}
			}
		}

		public void Broadcast<T>(T eventData) where T : class
		{
			var eventType = typeof(T);

			isBroadcasting = true;
			if (eventListeners.TryGetValue(eventType, out var listeners))
			{
				for (var i = listeners.Count - 1; i >= 0; i--)
				{
					var listener = listeners[i];
					(listener as Action<T>)?.Invoke(eventData);
				}
			}

			isBroadcasting = false;
		}
	}
}
