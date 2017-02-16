using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueEditor
{
    public class VoiceKit
    {
        public string Name { get; set; }
        public string VoiceActor { get; set; }

        public VoiceKit()
        {
            Name = "";
            VoiceActor = "";
        }
    }

    public class VoiceActor
    {
        public string Name { get; set; }
        public List<LocalizedVoiceActor> ListLocalizedVoiceActors { get; set; }

        public VoiceActor()
        {
            Name = "";
            ListLocalizedVoiceActors = new List<LocalizedVoiceActor>();
        }

        public string GetLocalizedActor(Language language)
        {
            var voiceActor = ListLocalizedVoiceActors.Find(item => item.LanguageName == language.Name);
            if (voiceActor != null)
                return voiceActor.Name;
            return "";
        }

        public void SetLocalizedActor(Language language, string name)
        {
            var localizedActor = ListLocalizedVoiceActors.Find(item => item.LanguageName == language.Name);
            if (localizedActor == null)
            {
                localizedActor = new LocalizedVoiceActor() { Name = name, LanguageName = language.Name };
                ListLocalizedVoiceActors.Add(localizedActor);
            }
            localizedActor.Name = name;
        }
    }

    public class LocalizedVoiceActor
    {
        public string Name { get; set; }
        public string LanguageName { get; set; }

        public LocalizedVoiceActor()
        {
            Name = "";
            LanguageName = "";
        }
    }

}
