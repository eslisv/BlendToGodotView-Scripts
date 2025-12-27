using System.Collections.Generic;

namespace AM.ModelViewerTool
{
    public sealed class CustomPopupMenu_Args
    {
        public IReadOnlyList<CustomPopupMenu_SingleEntry_Args> Entries => _entries;

        private readonly List<CustomPopupMenu_SingleEntry_Args> _entries = new();

        public CustomPopupMenu_Args(params CustomPopupMenu_SingleEntry_Args[] entries)
        {
            _entries.AddRange(entries);
        }

        public CustomPopupMenu_Args(IEnumerable<CustomPopupMenu_SingleEntry_Args> entries)
        {
            _entries.AddRange(entries);
        }
    }
}
