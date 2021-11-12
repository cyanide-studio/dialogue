using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace DialogueEditor
{
    public class DialogueNodeSentence : DialogueNode
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public string Sentence { get; set; }
        public string SpeakerID { get; set; }
        public string ListenerID { get; set; }

        //Voicing
        public string VoicingID { get; set; }
        public DateTime LastEditDate { get; set; }
        public bool HideSubtitle { get; set; }
        public string Context { get; set; }         //Directing details
        public string Comment { get; set; }         //Voicing details
        public string VoiceIntensity { get; set; }  //Voicing Intensity

        //Anim
        public string SpeakerAnimset { get; set; }
        public string ListenerAnimset { get; set; }
        public string SpeakerAnim { get; set; }
        public string ListenerAnim { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogueNodeSentence()
        {
            Sentence = "";
            SpeakerID = "";
            ListenerID = "";

            VoicingID = "";
            LastEditDate = Utility.GetCurrentTime();
            HideSubtitle = false;
            Context = "";
            Comment = "";
            VoiceIntensity = "";

            SpeakerAnimset = "";
            ListenerAnimset = "";
            SpeakerAnim = "";
            ListenerAnim = "";

            if (EditorCore.CustomLists["Intensities"].Count > 0)
            {
                VoiceIntensity = EditorCore.CustomLists["Intensities"].First().Key;
            }
        }

        public DialogueNodeSentence(DialogueNodeSentence other)
            : base(other)
        {
            Sentence = other.Sentence;
            SpeakerID = other.SpeakerID;
            ListenerID = other.ListenerID;

            VoicingID = "";
            LastEditDate = Utility.GetCurrentTime();
            HideSubtitle = other.HideSubtitle;
            Context = other.Context;
            Comment = other.Comment;
            VoiceIntensity = other.VoiceIntensity;

            SpeakerAnimset = other.SpeakerAnimset;
            ListenerAnimset = other.ListenerAnimset;
            SpeakerAnim = other.SpeakerAnim;
            ListenerAnim = other.ListenerAnim;
        }

        public override object Clone()
        {
            return new DialogueNodeSentence(this);
        }
    }
}
