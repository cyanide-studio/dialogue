# Dialogue Editor - Changelog

### Version 1.3 _(2023-06-11)_

- Add callbacks for the Dialogue Player on dialogue start/end and on nodes execution (Actions and Conditions), allowing custom builds to simulate a play session.
- Add User.config option "RefreshTreeViewOnEdit" to control TreeView refresh during edition (default to false).
- Add User.config option "DialogueTreeViewFont" to store the selected display font.
- Add User.config options to store preferences for the Dialogue Player.
- Add a first implementation of a localization exporter for Unity (needs test).
- Update default exports directories to regroup them into an "Exports" root directory.

### Version 1.2 _(2021-11-16)_

- Add a new CustomProperties mechanism for custom additional node data and its associated edition panels.
- Add a custom DirectingProperties sample in the demo.
- Deprecate project-specific properties  (should be handled via CustomProperties and/or NodeFlags instead) :
  - Dialogue : Camera, CameraBlendTime.
  - NodeSentence : Camera, CameraBlendTime, PreDelay, PostDelay.
  - NodeChoice : Timer, HideTimer, Blueprint.
  - NodeReply : Repeat, Deduction, AutoSelect.
- Add mechanisms to handle deprecated data migration :
  - ExtensionData dictionnary : used on several serialized classes to store unhandled properties.
  - Dummy types for Conditions/Actions/Flags/CustomProperties : can be used to load deprecated classes.
  - OnDialoguePostLoad callback : can be used to migrate/clean data.
- Fix windows "beep" sounds when using some editor shortcuts.
- Fix a behaviour where changing speaker/listener properties would generate two undo steps instead of one.
- Fix a behaviour where scrolling on a panel would scroll a combobox instead.

### Version 1.1 _(2021-02-03)_

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

### Version 1.0 _(2017-03-31)_

- Original public release.
