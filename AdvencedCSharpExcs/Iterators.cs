using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace AdvencedCSharpExcs
{

//    Requirements:
//- Create a BinaryTree<T> class with custom iteration patterns
//- Implement different traversal methods(InOrder, PreOrder, PostOrder)
//- Use yield return for lazy evaluation
//- Allow for filtered iteration

//Learning Objectives:
//- Understanding iterator implementation
//- Yield return usage
//- Custom IEnumerable implementation
    internal class Iterators
    {
    }

    public class BinaryTree<T> where T : IComparable<T>
    {
        public class Node
        {
            public T Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public Node(T value)
            {
                Value = value;
            }
        }
        private Node root;

        public void Add(T value)
        {
            if (root == null)
            {
                root = new Node(value);
                return;
            }

            AddNode(root,value);

        }

        private void AddNode(Node node,T value)
        {
            if (value.CompareTo(node.Value) < 0)
            {
                if (node.Left == null)
                    node.Left = new Node(value);
                else
                    AddNode(node.Left, value);
            }
            else
            {
                if (node.Right == null)
                    node.Right = new Node(value);
                else
                    AddNode(node.Right, value);
            }
        }


        public IEnumerable<T> InOrderTraversal()
        {
            if(root != null)
            {
                foreach(T value in InOrderTraversal(root))
                {
                    yield return value;
                }
            }
        }
        private IEnumerable<T> InOrderTraversal(Node node)
        {
            if (node.Left != null)
            {
                foreach (var item in InOrderTraversal(node.Left))
                    yield return item;
            }

            yield return node.Value;

            if (node.Right != null)
            {
                foreach (var item in InOrderTraversal(node.Right))
                    yield return item;
            }
        }

        public IEnumerable<T> PreOrderTraversal()
        {
            if (root != null)
            {
                foreach (var item in PreOrderTraversal(root))
                    yield return item;
            }
        }

        public IEnumerable<T> PreOrderTraversal(Node node)
        {
            if(node.Left != null)
            {
                foreach (var i in PreOrderTraversal(node.Left))
                    yield return i;
            }
            if (node.Right != null)
            {
                foreach(var i in PreOrderTraversal(node.Right))
                    yield return i;
            }
        }


        public IEnumerable<T> FilteredTraversal (Func<T,bool> predicate)
        {
            foreach(var i in InOrderTraversal())
            {
                if(predicate(i))
                    yield return i;
            }
        }
    }
    public class MyProgram
    {
        public void Mainy()
        {
            var tree = new BinaryTree<int>();
            int[] numbers = { 50, 30, 70, 20, 40, 60, 80 };

            foreach (var num in numbers)
                tree.Add(num);

            Console.WriteLine("In-Order Traversal:");
            foreach (var num in tree.InOrderTraversal())
                Console.Write($"{num} ");  // Output: 20 30 40 50 60 70 80

            Console.WriteLine("\n\nPre-Order Traversal:");
            foreach (var num in tree.PreOrderTraversal())
                Console.Write($"{num} ");  // Output: 50 30 20 40 70 60 80

            Console.WriteLine("\n\nFiltered Traversal (Even numbers):");
            foreach (var num in tree.FilteredTraversal(x => x % 2 == 0))
                Console.Write($"{num} ");  // Output: 20 30 40 50 60 70 80
        }
    }
}
