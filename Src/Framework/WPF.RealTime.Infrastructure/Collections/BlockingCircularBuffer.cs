using System;
using System.Threading;
using System.Threading.Tasks;

namespace WPF.RealTime.Infrastructure.Collections
{
    public class BlockingCircularBuffer<T>
    {
        private readonly T[] _buffer;
        private readonly int _size = 1024;
        private int _readerIndex = 0;
        private int _writerIndex = 0;
        private bool _writingSuspended;
        private readonly object _lock = new object();

        public BlockingCircularBuffer(int size)
        {
            _size = size;
            _buffer = new T[_size];
        }

        private void Clear()
        {
            _readerIndex = 0;
            _writerIndex = 0;
            Array.Clear(_buffer, 0, _size);
        }

        public int Count()
        {
            int count;
            lock (_lock)
            {
                count = ((_writerIndex % _size) - _readerIndex);
            }
            return (count < 0) ? (-1) * count : count;
        }

        public void Enqueue(T item)
        {
            lock (_lock)
            {
                while (IsFull() || _writingSuspended)
                {
                    Monitor.Wait(_lock);
                }
                _buffer[_writerIndex] = item;
                _writerIndex++;
                _writerIndex %= _size;
                Monitor.Pulse(_lock);
            }
        }

        public T Peek()
        {
            T item;
            lock (_lock)
            {
                if (IsEmpty())
                {
                    return default(T);
                }
                item = _buffer[_readerIndex];
            }
            return item;
        }

        public T Dequeue()
        {
            T item;
            lock (_lock)
            {
                while (IsEmpty())
                {
                    Monitor.Wait(_lock);
                }
                item = _buffer[_readerIndex];
                _readerIndex++;
                _readerIndex %= _size;
                Monitor.Pulse(_lock);
            }
            return item;
        }

        public void SuspendEnqueue()
        {
            if (_writingSuspended) return;
            Task.Factory.StartNew(() =>
            {
                _writingSuspended = true;
                while (true)
                {
                    lock (_lock)
                    {
                        if (!IsEmpty()) continue;
                        _writingSuspended = false;
                        Monitor.PulseAll(_lock);
                    }
                    return;
                }
            });
        }

        private bool IsEmpty()
        {
            return (_readerIndex == _writerIndex);
        }

        private bool IsFull()
        {
            return (((_writerIndex + 1) % _size) == _readerIndex);
        }
    }

}
