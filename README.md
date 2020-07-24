# RingBuffer

This is a generic ringbuffer (n-buffer) implementation for .Net Core 3.1 and later. It works like a DirectX SwapChain, but holds any type of payload, not just video buffers.

The n-buffer count can be specified in the constructor (default is 3), and it can be resized with the Resize(int) method.

The Current, Next, and Previous elements are all accessible at any time. Advancing to the next buffer is as simple as using the MoveNext() method.

Both Resize(int) and MoveNext() are fluent, returning a this-pointer for chainability.

The entire class is threadsafe, using a lock to prevent unsafe access.
