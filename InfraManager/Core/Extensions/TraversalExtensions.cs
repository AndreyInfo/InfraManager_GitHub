using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.Core.Extensions
{
	public static class TraversalExtensions
	{
		#region VisitNext
		public static IEnumerable<T> VisitNext<T>(this T root, Func<T, T> selector)
		{
			T current = root;
			while (current != null)
			{
				yield return current;
				current = selector(current);
			}
		}
		#endregion

		#region VisitBreadthFirst
		public static IEnumerable<T> VisitBreadthFirst<T>(this T root, Func<T, IEnumerable<T>> selector)
		{
			return VisitLeftRightBreadthFirst(root, selector);
		}
		#endregion

		#region VisitLeftRightBreadthFirst
		public static IEnumerable<T> VisitLeftRightBreadthFirst<T>(this T root, Func<T, IEnumerable<T>> selector)
		{
			if (root == null) yield break;
			//
			Queue<T> queue = new Queue<T>();
			T current = root;
			queue.Enqueue(current);
			while (queue.Count > 0)
			{
				current = queue.Dequeue();
				yield return current;
				foreach (var child in selector(current) ?? new T[0])
					queue.Enqueue(child);
			}
		}
		#endregion

		#region VisitRightLeftBreadthFirst
		public static IEnumerable<T> VisitRightLeftBreadthFirst<T>(this T root, Func<T, IEnumerable<T>> selector)
		{
			if (root == null) yield break;
			//
			Queue<T> queue = new Queue<T>();
			T current = root;
			queue.Enqueue(current);
			while (queue.Count > 0)
			{
				current = queue.Dequeue();
				yield return current;
				foreach (var child in (selector(current) ?? new T[0]).Reverse())
					queue.Enqueue(child);
			}
		}
		#endregion

		#region VisitDepthFirst
		public static IEnumerable<T> VisitDepthFirst<T>(this T root, Func<T, IEnumerable<T>> selector)
		{
			return VisitPrefixLeftRightDepthFirst(root, selector);
		}
		#endregion

		#region VisitPrefixLeftRightDepthFirst
		public static IEnumerable<T> VisitPrefixLeftRightDepthFirst<T>(this T root, Func<T, IEnumerable<T>> selector)
		{
			if (root == null) yield break;
			//
			Stack<T> stack = new Stack<T>();
			T current = root;
			stack.Push(current);
			while (stack.Count > 0)
			{
				current = stack.Pop();
				yield return current;
				foreach (var child in (selector(current) ?? new T[0]).Reverse())
					if(child!=null)
						stack.Push(child);
			}
		}
		#endregion

		#region VisitPrefixRightLeftDepthFirst
		public static IEnumerable<T> VisitPrefixRightLeftDepthFirst<T>(this T root, Func<T, IEnumerable<T>> selector)
		{
			if (root == null) yield break;
			//
			Stack<T> stack = new Stack<T>();
			T current = root;
			stack.Push(current);
			while (stack.Count > 0)
			{
				current = stack.Pop();
				yield return current;
				foreach (var child in selector(current) ?? new T[0])
					stack.Push(child);
			}
		}
		#endregion

		#region VisitPostfixLeftRightDepthFirst
		public static IEnumerable<T> VisitPostfixLeftRightDepthFirst<T>(this T root, Func<T, IEnumerable<T>> selector)
		{
			if (root == null) yield break;
			//
			Stack<T> stack1 = new Stack<T>(), stack2 = new Stack<T>();
			T current = root;
			stack1.Push(current);
			while (stack1.Count > 0)
			{
				current = stack1.Pop();
				stack2.Push(current);
				foreach (var child in selector(current) ?? new T[0])
					stack1.Push(child);
			}
			while (stack2.Count > 0)
				yield return stack2.Pop();
		}
		#endregion

		#region VisitPostfixRightLeftDepthFirst
		public static IEnumerable<T> VisitPostfixRightLeftDepthFirst<T>(this T root, Func<T, IEnumerable<T>> selector)
		{
			if (root == null) yield break;
			//
			Stack<T> stack1 = new Stack<T>(), stack2 = new Stack<T>();
			T current = root;
			stack1.Push(current);
			while (stack1.Count > 0)
			{
				current = stack1.Pop();
				stack2.Push(current);
				foreach (var child in (selector(current) ?? new T[0]).Reverse())
					stack1.Push(child);
			}
			while (stack2.Count > 0)
				yield return stack2.Pop();
		}
		#endregion
	}
}
