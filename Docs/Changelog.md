# Dialogue Editor - Changelog

### Version 1.1

- Add a "Search All" tool, to look for dialogue nodes based on their workstring and/or conditions/actions/flags.
- Add "Expand All" and "Collapse All" commands on the project explorer.
- Add a "Rename" option on documents context menu.
- Disable root nodes collapsing (dialogue roots + project packages).
- Display +/- icons on collapsed branches.
- Fix the "Reload file" operation not working if a file is not dirty.
- Fix crash when reloading the whole project while some files have been moved outside of the editor.
- Start dialogue nodes IDs at 0 instead of 1 (root node will have ID 0).
- Use Nuget to retrieve dependencies dlls.
- Remove binaries from the repository.

### Version 1.0

- Original public release.
