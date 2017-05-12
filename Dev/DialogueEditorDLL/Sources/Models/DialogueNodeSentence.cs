using System;
using System.Linq;

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

        //Delay
        public float PreDelay { get; set; }
        public float PostDelay { get; set; }

        //Camera
        public string Camera { get; set; }
        public float CameraBlendTime { get; set; }

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

            PreDelay = 0.0f;
            PostDelay = 0.0f;

            Camera = "";
            CameraBlendTime = -1.0f;

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

            PreDelay = other.PreDelay;
            PostDelay = other.PostDelay;

            Camera = other.Camera;
            CameraBlendTime = other.CameraBlendTime;
        }

        public override object Clone()
        {
            return new DialogueNodeSentence(this);
        }
    }
}
