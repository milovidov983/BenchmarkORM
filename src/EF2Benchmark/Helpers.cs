namespace EF2Benchmark {
    using System;

    public class Helpers {
        public class ClosedQueue<T> {
            private int pointer = 0;
            private T[] elements;

            public ClosedQueue(T[] elements) {
                if (elements == null || elements.Length < 1) {
                    throw new ArgumentException($"Array parameter {nameof(elements)} must be grather than 0");
                }
                this.elements = elements;
            }

            public T Dequeue() {
                if (pointer < elements.Length) {
                    return elements[pointer++];
                }
                pointer = 0;
                return elements[pointer];
            }
        }
    }
}