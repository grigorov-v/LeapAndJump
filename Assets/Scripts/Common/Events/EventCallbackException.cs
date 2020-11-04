using System;

namespace Grigorov.Unity.Events {
	public sealed class EventCallbackException : Exception {
		public EventCallbackException(Type eventType, Exception innerException) :
			base(string.Format("Failed to call event callback for {0}", eventType.Name), innerException) {}
	}
}