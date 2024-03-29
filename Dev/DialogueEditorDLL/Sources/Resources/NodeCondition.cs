﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DialogueEditor
{
    public abstract class NodeCondition : ICloneable
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public bool IntendedResult { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public NodeCondition()
        {
            IntendedResult = true;
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        public string GetTreeText()
        {
            return ((IntendedResult) ? "" : "[Not] ") + GetTreeText_Impl();
        }

        protected virtual string GetTreeText_Impl()
        {
            return GetDisplayText_Impl();
        }

        public string GetDisplayText()
        {
            return ((IntendedResult) ? "" : "[Not] ") + GetDisplayText_Impl();
        }

        protected virtual string GetDisplayText_Impl()
        {
            return "[NodeCondition]";
        }

        public bool IsPlayConditionValid(PlayDialogueConditionContext context)
        {
            return OnTestPlayConditionValid(context) == IntendedResult;
        }

        // Called by the PlayDialogue tool when the node is about to be played.
        // Can be overriden to simulate game flow through custom code.
        protected virtual bool OnTestPlayConditionValid(PlayDialogueConditionContext context)
        {
            EditorCore.LogWarning($"Condition type is not implemented for Play simulations: {GetType().Name}");
            return true;
        }
    }

    public abstract class NodeConditionGroup : NodeCondition
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        [Browsable(false)]
        public List<NodeCondition> Conditions { get; set; }
        
        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public NodeConditionGroup()
        {
            Conditions = new List<NodeCondition>();
        }

        public override object Clone()
        {
            //let base class brute-copy everything, then correct what's wrong at this level
            var clone = base.Clone() as NodeConditionGroup;

            clone.Conditions = Conditions.Clone() as List<NodeCondition>;
            return clone;
        }
    }

    public class NodeConditionAnd : NodeConditionGroup
    {
        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public NodeConditionAnd()
        {
        }

        protected override string GetTreeText_Impl()
        {
            return "AND";
        }

        protected override string GetDisplayText_Impl()
        {
            string text = "[";
            bool first = true;
            foreach (var condition in Conditions)
            {
                if (!first)
                    text += " AND ";
                text += condition.GetDisplayText();
                first = false;
            }
            text += "]";
            return text;
        }

        protected override bool OnTestPlayConditionValid(PlayDialogueConditionContext context)
        {
            if (Conditions.Count == 0)
            {
                return true;
            }

            bool result = true;
            foreach (var condition in Conditions)
            {
                result &= condition.IsPlayConditionValid(context);
            }

            return result;
        }
    }

    public class NodeConditionOr : NodeConditionGroup
    {
        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public NodeConditionOr()
        {
        }

        protected override string GetTreeText_Impl()
        {
            return "OR";
        }

        protected override string GetDisplayText_Impl()
        {
            string text = "[";
            bool first = true;
            foreach (var condition in Conditions)
            {
                if (!first)
                    text += " OR ";
                text += condition.GetDisplayText();
                first = false;
            }
            text += "]";
            return text;
        }

        protected override bool OnTestPlayConditionValid(PlayDialogueConditionContext context)
        {
            if (Conditions.Count == 0)
            {
                return true;
            }

            bool result = false;
            foreach (var condition in Conditions)
            {
                result |= condition.IsPlayConditionValid(context);
            }

            return result;
        }
    }
}
