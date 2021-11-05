namespace DialogueEditor
{
    public static class EditorVersion
    {
        public const int Major = 1;
        public const int Minor = 0;
        public const int Patch = 0;

        public static string GetVersion()
        {
            return string.Format("{0}.{1}.{2}", Major, Minor, Patch);
        }
    }
}
