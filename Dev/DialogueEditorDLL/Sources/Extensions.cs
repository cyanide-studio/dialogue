using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DialogueEditor
{
    public static class Extensions
    {
        //--------------------------------------------------------------------------------------------------------------
        // List<>

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dictionary<>

        //exemple : DialogueStats stats = Dialogues.GetOrAdd(dialogue.GetName(), (key) => new DialogueStats());
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue value;
            if (dict.TryGetValue(key, out value))
                return value;

            return dict[key] = valueFactory(key);
        }

        //--------------------------------------------------------------------------------------------------------------
        // String

        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            return source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        // ListBox

        public static void RefreshItems(this ListBox list)
        {
            typeof(ListBox).InvokeMember("RefreshItems",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod,
                null, list, new object[] { }
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        // TreeNode

        public static TreeNode GetRootNode(this TreeNode node)
        {
            if (node != null)
            {
                while (node.Parent != null)
                {
                    node = node.Parent;
                }
            }
            return node;
        }

        //--------------------------------------------------------------------------------------------------------------
        // TreeNodeCollection

        //Based on http://stackoverflow.com/questions/177277/how-to-get-a-list-of-all-child-nodes-in-a-treeview-in-net (Adrian Regan)
        public static List<TreeNode> FlattenList(this TreeNode root, bool includeRoot)
        {
            List<TreeNode> list = new List<TreeNode>();
            if (includeRoot)
            {
                list.Add(root);
            }

            Queue<TreeNode> queue = new Queue<TreeNode>();
            foreach (TreeNode top in root.Nodes)
            {
                queue.Enqueue(top);
            }

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node != null)
                {
                    list.Add(node);

                    foreach (TreeNode child in node.Nodes)
                    {
                        queue.Enqueue(child);
                    }
                }
            }

            return list;
        }
    }
}
