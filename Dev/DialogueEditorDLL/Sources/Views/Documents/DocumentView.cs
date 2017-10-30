using WeifenLuo.WinFormsUI.Docking;

namespace DialogueEditor
{
    public class DocumentView : DockContent
    {
        public bool ForceClose = false;

        public DocumentView() { }

        // The views should implement this to react to unspecified model change (full refresh)
        public virtual void RefreshDocument() { }

        public virtual void RefreshTitle() { }
        public virtual void OnPostReload() { }
        public virtual void ResolvePendingDirty() { }
    };
}
