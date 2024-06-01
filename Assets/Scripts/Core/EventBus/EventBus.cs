using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.EventBus
{
	public interface IEventReceiver<T> where T : struct
	{
		void ReceiveEvent(in T args);
	}

	public static class EventBus<T> where T : struct
	{
		private static readonly HashSet<IEventReceiver<T>> subscribers = new();

		public static void Subscribe(IEventReceiver<T> receiver)
		{
			if (subscribers.Add(receiver) == false)
			{
				throw new InvalidOperationException(
					$"Receiver '{receiver}' already subscribed for '{nameof(T)}' event");
			}
		}

		//todo: add Unsubscribes
		public static void Unsubscribe(IEventReceiver<T> receiver)
		{
			if (subscribers.Remove(receiver) == false)
			{
				throw new InvalidOperationException($"No receiver '{receiver}' subscribed for '{nameof(T)}' event");
			}
		}

		public static void Broadcast(in T args)
		{
			if (subscribers.Count == 0)
			{
				Debug.LogWarning($"EventBus typeof: '{typeof(T).Name}' dont have any listeners, possible miss subscribe in code");
			}
			
			foreach (IEventReceiver<T> receiver in subscribers)
			{
				receiver.ReceiveEvent(args);
			}
		}
	}
}