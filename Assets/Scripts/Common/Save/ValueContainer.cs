using System;

namespace Grigorov.Save {
	[Serializable]
	internal struct ValueContainer<T> {
		public T Value;
	}
}