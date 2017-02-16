using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueEditor
{
    static public class Extensions
    {
        //--------------------------------------------------------------------------------------------------------------
        // List<>

        static public IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dictionary<>

        //exemple : DialogueStats stats = Dialogues.GetOrAdd(dialogue.GetName(), (key) => new DialogueStats());
        static public TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFactory)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            else
            {
                TValue value = valueFactory(key);
                dict.Add(key, value);
                return value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // String

        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            return (source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        //--------------------------------------------------------------------------------------------------------------
        // ListBox

        public static void RefreshItems(this System.Windows.Forms.ListBox list)
        {
            typeof(System.Windows.Forms.ListBox).InvokeMember("RefreshItems", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                null, list, new object[] { }
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        // TreeNode

        public static System.Windows.Forms.TreeNode GetRootNode(this System.Windows.Forms.TreeNode node)
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
        public static List<System.Windows.Forms.TreeNode> FlattenList(this System.Windows.Forms.TreeNode root, bool includeRoot)
        {
            List<System.Windows.Forms.TreeNode> list = new List<System.Windows.Forms.TreeNode>();
            if (includeRoot)
            {
                list.Add(root);
            }

            Queue<System.Windows.Forms.TreeNode> queue = new Queue<System.Windows.Forms.TreeNode>();
            foreach (System.Windows.Forms.TreeNode top in root.Nodes)
            {
                queue.Enqueue(top);
            }

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node != null)
                {
                    list.Add(node);

                    foreach (System.Windows.Forms.TreeNode child in node.Nodes)
                    {
                        queue.Enqueue(child);
                    }
                }
            }

            return list;
        }
    }
}
