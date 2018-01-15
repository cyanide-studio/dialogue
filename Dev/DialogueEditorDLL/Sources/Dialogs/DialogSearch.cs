using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class DialogSearch : Form
    {
        struct Node
        {
            public object component;
            public SearchTypeDescriptor descriptor;
            public Type type;
            public static Node BuildNode(Type type)
            {
                Node node;
                node.type = type;
                node.component = Activator.CreateInstance(node.type);
                node.descriptor = new SearchTypeDescriptor(node.component);
                return node;
            }
        }
        List<Node> nodes = new List<Node>();

        struct Result
        {
            public string text;
            public Dialogue dialogue;
            public DialogueNode node;
            public static Result BuildResult(string text, Dialogue dialogue, DialogueNode node)
            {
                Result result;
                result.text = text;
                result.dialogue = dialogue;
                result.node = node;
                return result;
            }
        }

        public DialogSearch()
        {
            InitializeComponent();

            checkedListBoxCommonProperties.Items.Clear();
            foreach (var conditionSlot in EditorCore.ConditionSlots)
            {
                checkedListBoxCommonProperties.Items.Add(conditionSlot.Text, false);
                nodes.Add(Node.BuildNode(conditionSlot.ConditionType));
            }
            foreach (var actionSlot in EditorCore.ActionSlots)
            {
                checkedListBoxCommonProperties.Items.Add(actionSlot.Text, false);
                nodes.Add(Node.BuildNode(actionSlot.ActionType));
            }
            foreach(var flagSlot in EditorCore.FlagSlots)
            {
                checkedListBoxCommonProperties.Items.Add(flagSlot.Text, false);
                nodes.Add(Node.BuildNode(flagSlot.FlagType));
            }

            SelectedIndexChanged(checkedListBoxCommonProperties, null);
        }

        private void SelectedIndexChanged(object sender, EventArgs args)
        {
            if (checkedListBoxCommonProperties.SelectedIndex >= 0)
                propertyGridCommon.SelectedObject = nodes[checkedListBoxCommonProperties.SelectedIndex].descriptor;
        }

        private void SearchClick(object sender, EventArgs e)
        {
            string workstring = textBoxWorkstring.Text;
            bool searchSentences = checkBoxSentences.Checked;
            bool searchReplies = checkBoxReplies.Checked;
            bool searchChoices = checkBoxChoices.Checked;

            List<Result> results = new List<Result>();

            var dialogues = ProjectController.GetAllDialogues();
            foreach (var dialogue in dialogues)
            {
                foreach (var node in dialogue.ListNodes)
                {
                    foreach (var condition in node.Conditions)
                    {
                        if (Check(condition))
                            results.Add(Result.BuildResult(String.Format("{0} {1} - {2}", dialogue.Name, node.ID, (condition as NodeCondition).GetDisplayText()), dialogue, node));
                    }
                    foreach (var action in node.Actions)
                    {
                        if (Check(action))
                            results.Add(Result.BuildResult(String.Format("{0} {1} - {2}", dialogue.Name, node.ID, (action as NodeAction).GetDisplayText()), dialogue, node));
                    }
                    foreach (var flag in node.Flags)
                    {
                        if (Check(flag))
                            results.Add(Result.BuildResult(String.Format("{0} {1} - {2}", dialogue.Name, node.ID, (flag as NodeFlag).GetDisplayText()), dialogue, node));
                    }

                    if (workstring.Length > 0)
                    {
                        if (searchSentences)
                        {
                            var sentence = node as DialogueNodeSentence;
                            if (sentence != null && sentence.Sentence.IndexOf(workstring, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                results.Add(Result.BuildResult(String.Format("{0} {1} - {2}", dialogue.Name, node.ID, FormatText(sentence.Sentence)), dialogue, node));
                        }

                        if (searchReplies)
                        {
                            var reply = node as DialogueNodeReply;
                            if (reply != null && reply.Reply.IndexOf(workstring, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                results.Add(Result.BuildResult(String.Format("{0} {1} - {2}", dialogue.Name, node.ID, FormatText(reply.Reply)), dialogue, node));
                        }

                        if (searchChoices)
                        {
                            var choice = node as DialogueNodeChoice;
                            if (choice != null && choice.Choice.IndexOf(workstring, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                results.Add(Result.BuildResult(String.Format("{0} {1} - {2}", dialogue.Name, node.ID, FormatText(choice.Choice)), dialogue, node));
                        }
                    }
                }
            }

            ProjectController.StartSearchResult();
            foreach (var result in results)
            {
                ProjectController.AddSearchResult(result.text, result.dialogue, result.node);
            }
            ProjectController.EndSearchResult();
        }

        private string FormatText(string text)
        {
            text = text.Replace("\n", "");
            text = text.Replace("\r", "");
            //if (text.Length > 30)
            //    text = text.Substring(0, 30) + "...";
            return text;
        }

        private bool Check(object component)
        {
            if (component == null)
                return false;
            Type type = component.GetType();
            int index = nodes.FindIndex((node) => node.type == type);
            if (index >= 0 && checkedListBoxCommonProperties.GetItemChecked(index))
            {
                var node = nodes[index];
                var properties = node.descriptor.GetProperties();
                if (properties == null || properties.Count == 0)
                    return true;
                bool good = true;
                foreach (var property in properties)
                {
                    object instanceValue = (property as PropertyDescriptor).GetValue(component);
                    object filterValue = (property as PropertyDescriptor).GetValue(node.component);
                    if (filterValue.GetType() == typeof(string) && (filterValue as string).Length == 0)
                        continue;
                    if (!instanceValue.Equals(filterValue))
                        good = false;
                }
                return good;
            }
            return false;
        }
    }

    public class SearchTypeDescriptor : ICustomTypeDescriptor
    {
        private Type type;
        private object component;
        private Attribute filter;

        SearchTypeDescriptor() { }
        public SearchTypeDescriptor(object componentInstance)
        {
            if (componentInstance == null)
                throw new ArgumentNullException("component");
            component = componentInstance;
            type = component.GetType();
            filter = new PropertySearchable();
        }
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(type);
        }
        public string GetClassName()
        {
            return type == null ? null : type.Name;
        }
        public string GetComponentName()
        {
            return null;
        }
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(type);
        }
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(type);
        }
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(type);
        }
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(type, editorBaseType);
        }
        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(type);
        }
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(type, attributes);
        }
        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(type, new Attribute[] { filter });
        }
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            if (attributes == null || attributes.Length == 0)
                return GetProperties();
            Attribute[] filters = new Attribute[attributes.Length + 1];
            attributes.CopyTo(filters, 0);
            filters[attributes.Length] = filter;
            return TypeDescriptor.GetProperties(type, filters);
        }
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return component;
        }
    }
}
