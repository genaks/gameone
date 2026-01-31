using System;
using System.Collections.Generic;
using MemoryMatch.Core;
using UnityEngine;

namespace Core.Services
{
	/// <summary>
	/// This is a modified version of the implementation by Will Miller, found here.
	/// http://www.willrmiller.com/?p=87
	/// </summary>
	public class MessageBroker : MonoBehaviour, IMessageBroker
	{
		private delegate void EventDelegate(IGameEvent gameEvent);

		private readonly Dictionary<Type, EventDelegate> _delegates = new();
		private readonly Dictionary<Delegate, EventDelegate> _delegateLookup = new();

		private void Awake()
		{
			ServiceLocator.Instance.Register(this);
		}

		private void OnDestroy()
		{
			ServiceLocator.Instance.Unregister<MessageBroker>();
		}

		public void Subscribe<T>(IMessageBroker.EventDelegate<T> del) where T : IGameEvent
		{
			// Early-out if we've already registered this delegate
			if (_delegateLookup.ContainsKey(del))
			{
				return;
			}

			// Create a new non-generic delegate which calls our generic one.
			// This is the delegate we actually invoke.
			void InternalDelegate(IGameEvent gameEvent) => del((T)gameEvent);
			
			_delegateLookup[del] = InternalDelegate;

			if (_delegates.TryGetValue(typeof(T), out EventDelegate eventDelegate))
			{
				_delegates[typeof(T)] = eventDelegate + InternalDelegate;
			}
			else
			{
				_delegates[typeof(T)] = InternalDelegate;
			}
		}

		public void Unsubscribe<T>(IMessageBroker.EventDelegate<T> del) where T : IGameEvent
		{
			if (!_delegateLookup.TryGetValue(del, out EventDelegate internalDelegate))
			{
				return;
			}

			if (_delegates.TryGetValue(typeof(T), out EventDelegate eventDelegate))
			{
				eventDelegate -= internalDelegate;

				if (eventDelegate == null)
				{
					_delegates.Remove(typeof(T));
				}

				_delegates[typeof(T)] = eventDelegate;
			}

			_delegateLookup.Remove(del);
		}

		public void Publish(IGameEvent gameEvent)
		{
			if (_delegates.TryGetValue(gameEvent.GetType(), out EventDelegate eventDelegate))
			{
				eventDelegate?.Invoke(gameEvent);
			}
		}

		public void WrapUp(bool isAppExit)
		{
		}
	}
}