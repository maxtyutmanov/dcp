using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcp
{
    /*
    This problem was asked by Google.
    Given the head of a singly linked list, reverse it in-place.
     */

    public static class Dcp73
    {
        public static void Test()
        {
            LLNode<int> ll = null; 
            Console.WriteLine("Reverse empty list:");
            Console.WriteLine("Before: {0}", ll);
            Console.WriteLine("After: {0}", Reverse(ll));

            ll = new LLNode<int>() { Value = 3 };
            Console.WriteLine("Reverse single item list:");
            Console.WriteLine("Before: {0}", ll);
            Console.WriteLine("After: {0}", Reverse(ll));

            ll = LLNode<int>.Make(new[] { 1, 2, 3, 4, 5 });
            Console.WriteLine("Reverse multi-item list:");
            Console.WriteLine("Before: {0}", ll);
            Console.WriteLine("After: {0}", Reverse(ll));
        }

        public static LLNode<T> Reverse<T>(LLNode<T> head)
        {
            if (head == null)
                return null;

            return head.ReverseTo(null);
        }

        public class LLNode<T>
        {
            public LLNode<T> Next { get; set; }

            public T Value { get; set; }

            public static LLNode<T> Make(IReadOnlyCollection<T> items)
            {
                if (items.Count == 0)
                    return null;

                var current = new LLNode<T>() { Value = items.First() };
                var head = current;

                foreach (var item in items.Skip(1))
                {
                    current.Next = new LLNode<T>() { Value = item };
                    current = current.Next;
                }

                return head;
            }

            public LLNode<T> ReverseTo(LLNode<T> newNext)
            {
                var nextBackup = Next;
                Next = newNext;
                if (nextBackup != null)
                {
                    // making this call as the last operation to enable tail call optimization
                    return nextBackup.ReverseTo(this);
                }
                else
                {
                    // this will be the new head
                    return this;
                }    
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                var head = this;

                while (head != null)
                {
                    if (head != this)
                        sb.Append(", ");
                    sb.Append(head.Value);
                    head = head.Next;
                }

                if (sb.Length > 0)
                    return sb.ToString();
                else
                    return "<nil>";
            }
        }
    }
}
