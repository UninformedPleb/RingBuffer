using System;

namespace RingBuffer
{
	public class RingBuffer<T>
	{
		private static readonly int DefaultSize = 3;

		private T[] buffer;
		private int currentIndex = 0;
		private object ringLock = new object();

		public RingBuffer() : this(DefaultSize) { }
		public RingBuffer(int size)
		{
			this.buffer = new T[size];
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

		public RingBuffer<T> Resize(int newSize)
		{
			lock(this.ringLock)
			{
				Array.Resize<T>(ref this.buffer, newSize);
				if(this.currentIndex > this.buffer.Length - 1) { this.currentIndex = this.buffer.Length -1; }
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
