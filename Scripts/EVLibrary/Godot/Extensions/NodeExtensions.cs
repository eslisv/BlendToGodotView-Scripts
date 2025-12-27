using EVLibrary.Extensions;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EVLibrary.Godot.Extensions
{
    public static class NodeExtensions
    {
        /// <summary>
        /// Searches node's children for a child node with a matching type.
        /// </summary>
        /// <typeparam name="T">Child node's type</typeparam>
        /// <param name="node">The parent node</param>
        /// <param name="includeSubChildren">Whether to include child nodes of a child</param>
        /// <returns>First instance of a child node of the given type</returns>
        public static T FindChildOfType<T>(this Node node, bool includeSubChildren = false)
        {
            List<Node> nodes;
            if (includeSubChildren)
            {
                nodes = node.GetAllChildrenInNode();
            }
            else
            {
                nodes = node.GetChildren().ToList();
            }
            foreach (Node n in nodes)
            {
                if (n.GetType() == typeof(T))
                {
                    return (T)(object)n;
                }
            }
            GD.PrintErr($"Could not find Node of type {typeof(T).Name}");
            return default(T);
        }

        /// <summary>
        /// Searches node's parent for a child node with a matching type.
        /// </summary>
        /// <typeparam name="T">Sibling node's type</typeparam>
        /// <param name="node">The sibling node used as a reference point</param>
        /// <returns>First instance of a sibling node of the given type</returns>
        public static T FindSiblingOfType<T>(this Node node)
        {
            Array<Node> nodes = node.GetParent().GetChildren();
            foreach (Node n in nodes)
            {
                if (n.GetType() == typeof(T))
                {
                    return (T)Convert.ChangeType(n, typeof(T));
                }
            }
            GD.PrintErr($"Could not find Node of type {typeof(T).Name}");
            return default(T);
        }

        /// <summary>
        /// Creates a list of all children in a node.
        /// </summary>
        /// <param name="node">The parent node</param>
        /// <returns>A list containing the node's child nodes</returns>
        public static List<Node> GetAllChildrenInNode(this Node node)
        {
            Array<Node> nodes = new Array<Node>();
            foreach (Node n in node.GetChildren())
            {
                if (n.GetChildCount() > 0)
                {
                    nodes.Add(n);
                    nodes.Merge(GetAllChildrenInNode(n));

                }
                else
                {
                    nodes.Add(n);
                }
            }
            return nodes.ToList();
        }

        /// <summary>
        /// Creates a list of all children of the given type in a node.
        /// </summary>
        /// <typeparam name="T">Child node's type</typeparam>
        /// <param name="node">The parent node</param>
        /// <returns>A list containing the node's child nodes of the given type</returns>
        public static List<T> GetAllChildrenInNodeOfType<T>(this Node node)
        {
            List<Node> childList = new List<Node>();
            List<Node> nodeList = GetAllChildrenInNode(node);
            foreach (Node n in nodeList)
            {
                if (n.GetType() == typeof(T))
                {
                    childList.Add(n);
                }
            }
            return childList.Cast<T>().ToList();
        }
    }
}
