using System;

namespace RingBuffer
{
	public class RingBuffer<T>
	{
		public delegate T InitializeElementDelegate();

		private static readonly int DefaultSize = 3;

		private T[] buffer;
		private int currentIndex = 0;
		private object ringLock = new object();

		public RingBuffer() : this(DefaultSize, null) { }
		public RingBuffer(int size) : this(size, null) { }
		public RingBuffer(int size, InitializeElementDelegate initializer)
		{
			if(size < 1) { throw new ArgumentOutOfRangeException("size"); }
			this.buffer = new T[size];
			for(int x = 0; x < this.buffer.Length && initializer != null; x++)
			{
				this.buffer[x] = initializer.Invoke();
			}
		}

		public T Current
		{
			get
			{
				lock(this.ringLock)
				{
					return this.buffer[this.currentIndex];
				}
			}
		}
		public T Next
		{
			get
			{
				lock(this.ringLock)
				{
					return this.buffer[(this.currentIndex + 1) % this.buffer.Length];
				}
			}
		}
		public T Previous
		{
			get
			{
				lock(this.ringLock)
				{
					return this.buffer[(this.currentIndex + this.buffer.Length - 1) % this.buffer.Length];
				}
			}
		}
		public int Size
		{
			get
			{
				lock(this.ringLock)
				{
					return this.buffer.Length;
				}
			}
		}

		public RingBuffer<T> Resize(int newSize)
		{
			return this.Resize(newSize, null);
		}
		public RingBuffer<T> Resize(int newSize, InitializeElementDelegate initializer)
		{
			if(newSize < 1) { throw new ArgumentOutOfRangeException("newSize"); }
			lock(this.ringLock)
			{
				Array.Resize<T>(ref this.buffer, newSize);
				if(this.currentIndex > this.buffer.Length - 1) { this.currentIndex = this.buffer.Length - 1; }
				for(int x = 0; x < this.buffer.Length && initializer != null; x++)
				{
					this.buffer[x] = initializer.Invoke();
				}

				return this;
			}
		}
		public RingBuffer<T> MoveNext()
		{
			lock(this.ringLock)
			{
				this.currentIndex = ++this.currentIndex % this.buffer.Length;
				return this;
			}
		}
	}
}
