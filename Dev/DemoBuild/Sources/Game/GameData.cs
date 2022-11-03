using DialogueEditor;

namespace DemoBuild
{
    public static class GameData
    {
        public static PlaySession PlaySession { get; private set; }

        public static void StartPlaySession(PlayDialogueContext context)
        {
            PlaySession = new PlaySession();

            // Here you could load some data from a config file, or have a popup to select a save file, etc...

            EditorCore.LogInfo("Demo PlaySession Starts");
        }

        public static void EndPlaySession()
        {
            PlaySession = null;

            EditorCore.LogInfo("Demo PlaySession Ends");
        }
    }
}
