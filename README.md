
# Dialogue Editor 1.0
Documentation

# Summary

- [User Documentation](#user-documentation)
    - [Overview](#user-overview)
        - [Project files](#project-files)
        - [Dialogues structure](#dialogues-structure)
        - [Nodes IDs](#nodes-ids)
    - [Getting started](#getting-started)
    - [Dialogue nodes](#dialogue-nodes)
        - [Root](#root)
        - [Sentence](#sentence)
        - [Choice](#choice)
        - [Reply](#reply)
        - [Goto](#goto)
        - [Branch](#branch)
    - [Dialogue edition](#dialogue-edition)
        - [Context menu](#context-menu)
        - [Node properties](#node-properties)
        - [Node attributes (Conditions, Actions and Flags)](#node-attributes)
        - [Filters](#filters)
        - [Constants](#constants)
    - [Project edition](#project-edition)
    - [Tools](#tools)
        - [Viewer](#viewer)
        - [Errors check](#errors-check)
        - [Exporters](#exporters)
        - [Localization](#localization)
        - [Copy / Undo](#copy-undo)
    - [Annex](#annex)
        - [Shortcuts](#shortcuts)
        - [Tips](#tips)
- [Technical Documentation](#technical-documentation)
    - [Overview](##technical-overview)
    - [Project content](##project-content)
        - [Directories](##directories)
        - [Visual Studio Solution](##visual-studio-solution)
    - [Design](##design)
    - [Code](##code) 
        - [Libraries](##libraries)
        - [Naming conventions](##naming-conventions)
        - [Architecture](##architecture)
    - [Integration](##integration)
        - [Editor build](##editor-build)
        - [Game integration](##game-integration)
        
(You can find a list of all the available shortcuts at the end of this document, or by pressing F1 in the Editor.)

[\[Top\]](#summary)

# User Documentation

## Overview

The goal of this editor is to allow quick iterations on dialogues writing, and to provide the necessary tools for gameplay flow and data exports.
It's inspired by the tools used on Cyanide's Game of Thrones RPG, and the Aurora tool used on Neverwinter Nights.

The main functions of this editor pipeline are :
- edition of the dialogues trees in a standalone editor.
- exports for the targeted game engines and localization, voicing, and any external tools.
- samples for easy integration and iteration in game engines, with both editors running in parallel.

[\[Top\]](#summary)

### Project files

The files manipulated by the editor consist of a .project file and several .dlg files.

The .project will hold the general information, such as actors, constants, languages and packages descriptions.
It will also act as a root for the files hierarchy. When you open a .project with the editor, all the folders next to it will be parsed recursively to find all .dlg files and fill the editor view.

Each .dlg file will hold a single dialogue. They are independent from each other, and only need to be situated inside a folder next to a .project for them to be manipulated through the editor.
Dialogues are accessed by their names, not their path. This means that every dialogue in a project must ave different names, even across different folders.

Several exporters have been implemented to provide the necessary data for external tools.

[\[Top\]](#summary)

### Dialogues structure

Dialogues consist of a list of sentences, associated with a speaker and a listener.
Special nodes can be used to fork the dialogue's flow and create multiple paths, either by using interactive choices, conditional branches or "jumps" (goto).

[\[Top\]](#summary)

### Nodes IDs

Every dialogue node will automatically receive a numeric ID on their creation. This ID will be unique and constant for each node, and should never be modified by hand.
IDs are used for cross-reference when exporting dialogues and moving nodes.
Rearranging nodes in the tree will not affect this ID.

This implies that after some time editing a dialogue, nodes will appear as if they are disordered (if you display the ID in the interface). This is perfectly normal and intended.
IDs are not indexes, and nodes will be ordered when exporting.

[\[Top\]](#summary)

## Getting started

Launch the editor with the DialogueEditorBuild.exe executable, or the project specific build executable.

To create a new project, select "File > New Project" then select a folder to host your dialogue files, and enter the name of the project file you wish to create (XXX.project).

To open an existing project, select "File > Open Project" then select the project file you wish to open (XXX.project).

You can edit the project configuration by double-clicking on it on the left panel (Project Explorer).
You will need to create actors in the Actors tab of the project configuration, as well as declare the languages you wish to export.
(See part 5 for more details about the project configuration)

You can open an existing dialogue by double-clicking on it on the left panel (Project Explorer), or create a new dialogue by using the menu "File > New Dialogue", or right-clicking on a folder and select "New Dialogue".

[\[Top\]](#summary)

## Dialogue nodes

Dialogues are built with a succession of nodes. When playing a dialogue, the game will parse the nodes in sequence, top to bottom from the "Root" node, until a Choice, Goto or Branch node creates a fork. The dialogue will stop automatically after playing a node without child.
You can use five types of nodes to edit a dialogue, defined below.

[\[Top\]](#summary)

### Root

The root node allows the edition of the node attributes, as well as global dialogue's attributes.
Additional Actors can be defined on this node. This allows you to include actors in a dialogue that are not used as speaker or listener in any sentence, but should be managed as participants anyway (spectators).

[\[Top\]](#summary)

### Sentence

Represents a single line of dialogue.
It is essentially composed of a workstring, which is the text that will be translated, voiced and displayed in the game.
A speaker must be picked from the Actors list defined in the project file. It will be used for the voicing.
A listener can be defined, to be used by the game's realization.
Additional information can be filled, such as animations, directing, and voicing intensity.

[\[Top\]](#summary)

### Choice

Represents an interactive choice.
It is composed of a list of replies, that must be added as children of the choice.
When parsing a choice node, the game will display the list of all the replies and let the player choose the option he wants to read.
A text can be registered on the choice node for debugging purposes, it is not intended to appear in the game. 

[\[Top\]](#summary)

### Reply

Represents a choice's option.
A workstring must be filled, and will be translated to appear in the game interface, but will not be voiced.
It acts as the new root of a fork.

[\[Top\]](#summary)

### Goto

Represents a jump in the dialogue's flow.
To edit a goto, you have to use "right click > copy reference" on the node you wish to jump to, then use "right click > paste reference" on the goto. Several goto nodes can target the same node. Every goto nodes will display the text of their target as a reminder, and every targeted nodes will be displayed in bold.
You can also use special paste (ctrl+shift+v) to set a goto reference.
Goto nodes are useful to merge different forks back to a common flow, or to organize the nodes hierarchy.

[\[Top\]](#summary)

### Branch

Represents a conditional branch.
It can be used to fork the dialogue's flow.
A branch node needs conditions defined to be effective.
To edit a branch node, a dedicated option will appear on the context menu to add children : "Branch > Add node".
You can also use special paste (ctrl+shift+v) to paste a sequence as branch children.

[\[Top\]](#summary)

## Dialogue edition

### Context menu

When you right-click on a node, a context menu will provide several edition options, depending on the selected node.

- Open Directory : opens the directory containing the dialogue.
- Copy Name : copies the name of the dialogue to the Windows clipboard.
- Add Sentence/Choice/Goto/Branch : inserts a new node of the selected type just after the selected node.
- Add Reply : inserts a new reply child node under the selected choice or reply. 
- Branch > Add xxxx : inserts a new node of the selected type as a child of the selected branch node (effectively creating a branch).
- Copy Reference : stores the reference of the selected node.
- Paste Reference : assigns the stored node reference on the selected goto node.
- Copy ID : copies the ID of the selected node to the Windows clipboard.
- Move Up/Down : moves the selected node upwards/downwards in the tree.
- Delete : deletes the selected node.

[\[Top\]](#summary)

### Node properties

When a node is selected, different properties will be available on the right side, depending on the type of node.

Root properties allow the edition of :
- the dialogue type.
- the specific voice bank.
- a comment used to describe what the dialogue will be about, or the context.

Sentence properties allow the edition of :
- the workstring, which will be translated and voiced in game. The number of characters is displayed next to it, and will change color when hitting the defined max size.
- the speaker actor, who will be used to actually speak the sentence.
- the optional listener actor, who will be used to define the speaker look-at.
- the optional animset and animation used by those actors.
- the Hide Subtitle option can be used to indicate that a sentence should be voiced but not displayed (for cases like "Aaarrg !").
- the Tone used for the voicing.

Reply properties allow the edition of :
- the workstring, which will be displayed but not voiced. The number of characters is displayed next to it, and will change color when hitting the defined max size.

Choice properties allow the edition of :
- the workstring, which will not appear in game and can be used as a comment for edition.

Common properties allow the edition of attributes (conditions, actions, flags) on each nodes.

[\[Top\]](#summary)

### Node attributes (Conditions, Actions and Flags)

It is possible to declare conditions on a node, like testing if the player possesses an item, or if he has information about a specific topic.

When the game hits a node with a condition, it will execute it only if the condition is valid, and skip it otherwise.
When a node is skipped, the game will try to execute the next child of the current hierarchy level.
Skipping a branch will skip its children.
Skipping a goto will skip the teleport.
A skipped reply will not appear in the interface.

Actions will be used to trigger in-game events, like acquiring items or starting an animation in the level.

Flags will be used to store additional information that will be used by the dialogue system, like indicating that a Reply node will use a skill test when selected.

[\[Top\]](#summary)

### Filters

Filters are available on the document view to show/hide additional information on each nodes.
The first group of filters will control the visibility of different bits of information.
The second group allows you to use a user-defined colors for sentences, to use localized Constants, and to change the font size and style.
The third group allows you to select the language used for display.

[\[Top\]](#summary)

### Constants

Constants are a list of IDs associated to a localized workstring. They are useful to use abstract names for characters or places in your sentences, centralize the localization of their names, and easily modify them during production.
For instance, you can use a constant "KingName", and localize it into "King Robert".

You define these constants in the project configuration.
To use them, you have to wrap their names between { }, like this : {KingName}
You can press Tab when editing a sentence to display the list of available constants.
You can toggle the "Constants" filter on the dialogue view to see the localized value of the constants used in your text.

[\[Top\]](#summary)

## Project edition

On the left panel, you can double-click on the project's name to open the dedicated edition panel.
This panel is subdivided in five tabs : "General", "Actors", "Voicing", "Constants" and "Debug".

The General panel allows the configuration of :
- the languages used by the exporters.
- the packages used to regroup dialogues exports.
- the maximum size of the workstrings in sentences and replies.

The Actors panel allows the edition of Actors used by the sentences :
- the ID is the reference of the actor, used by the game.
- the display name is only used by the editor.
- the picture will be displayed in sentences and in the viewer.
- the gender, species, build, age, height, and personality will be used by exporters (voicing).
- the description will only appear here and not be exported.
- the color can be used when working on a dialogue to highlight the actor's sentences.
- the voice kit will be used on voicing export.

The Voicing panel allows the edition of voice kits :
- Voice Actors represent your physical actors, such as Male1 or Female#
- Localized Actors allows you to specify the actual names of your voice actors.
- Voice Kits represent the game voices, such as Guard1, King, or Bartender.
- Voice Actors can be affected and shared between several voice kits, allowing the same voice actor to play different characters with different acting.

The Constants panel allows the edition of the localized constants :
- the ID is the reference of the constant, used in the sentences in the form {ID}
- the workstring is the base text used for localization of this constant.
- the comments section will be exported in the localization spreadsheet.

The Debug panel displays a view of the custom lists used by the editor.

[\[Top\]](#summary)

## Tools

### Viewer

You can use "Tools > Play Dialogue" to open a little viewer and test your dialogue flow.

[\[Top\]](#summary)

### Errors check

You can use "Tools > Check Dialogue" to let the editor execute a quick sanity check on your dialogue. Error will be displayed in the bottom panel, and you can double-click on those to jump to the concerned node.

[\[Top\]](#summary)

### Exporters

Several exporters are built-in in the editor under the "Tools >" menu.
- Import/Export Dialogues can be used to edit dialogues in a spreadsheet and import the modifications back. This exporter is mainly targeted towards localization.
- Export Stats is used to get word-counts on dialogues.
- Export Localization is used to export your localized strings towards a targeted game engine. Currently only .po files targeted for Unreal4 are provided.
- Export Voicing is used to export spreadsheets dedicated to audio studios, containing informations for the voice actors and the localized texts to voice. A Wwise text helper file is also generated, and can be imported in Wwise to pre-generate all the events with the generated VoicingID.
- Export Game Lipsync is used to export lipsync informations towards generation tools. Currently only text helper files targeted for FaceFx are provided.

[\[Top\]](#summary)

### Localization

Each dialogue holds his own translation table. Each dialogue node will have his own section, containing the translated strings of its workstring.
Those table are currently not editable through the editor, but can be viewed on the dialogue tree by selecting the appropriate language in the filters.

To edit the translations, you need to use the "Tools > Export Dialogues" to export a spreadsheet of the selected dialogues with their current localization information.
You can send this spreadsheet to your localization team, and import it back by using "Tools > Import Dialogues".

Once your translation tables are filled, you can either read them directly from the .dlg files when importing them in your game, or use the provided Unreal .po exporter, or any exporter your custom build may provide.
The voicing exporter will also use the translation tables.

[\[Top\]](#summary)

### Voicing

The voicing exporter allows you to export spreadsheets intended for the voicing sessions. This includes a "Loca_" file to handle all the localized sentences, an "Actors_" file to handle all the actors information, and a "Dialogues_" file to handle general dialogues information.

Each sentence will have a unique VoicingID generated, that is guaranteed to never change whatever happens to the associated sentence. This VoicingID should be used as a basis to name the voicing files, allowing the game to dynamically retrieve them during play.

By default, voice banks should be named after the dialogues names. If you want custom bank names or to regroup several dialogues inside a single bank, you can edit the property "Voice Bank" on the Root node to indicate the intended bank name.

A Wwise text file is generated alongside every export. This file can be used inside Wwise to generate all the sound events with the right VoicingID, scene type, actor and text.

Voicing intentions and scene context can be edited on each dialogue and sentence to provide the voicing sessions with as much information as possible. Those will be exported in the spreadsheets.

[\[Top\]](#summary)

### Copy / Undo

You can copy and paste nodes inside a dialogue, as well as into other dialogues.
Copying nodes attributes is also supported.
Special paste (ctrl+shift+v) can be used to set goto references and branch child sequences.

An undo/redo feature is available for the whole dialogue structure by using ctrl+z and ctrl+y, but only when the focus is on the dialogue tree.
You can also use ctrl+z when editing a single text-field.

[\[Top\]](#summary)

## Annex

You can display the Help dialog by pressing F1 in the editor.

[\[Top\]](#summary)

### Shortcuts

- Ctrl+N : Create new dialogue file.
- Ctrl+S : Save current file.
- Ctrl+Shift+S : Save all opened files.
- Ctrl+R : Reload current file.
- Ctrl+Shift+R : Reload all project files.
- Ctrl+Shift+T : Open last closed file.

- Enter : Edit current node Text / edit next node Text.
- F2 : Edit current node text / return on node tree.
- Ctrl+Enter : Insert sentence after selected node with same actors.
- Ctrl+Shift+Enter : Insert sentence after selected node with inversed actors.
- Tab : Open the auto-complete box for Constants (Sentences and replies only).

- F4 : Show current dialogue's stats using the current language.
- Shift+F4 : Show the whole project's stats using the current language.
- F5 : Play current dialogue in a viewer.
- Shift+F5 : Play current dialogue starting at current sentence in a viewer.
- F7 : Check current dialogue.
- Shift+F7 : Check all dialogues.

- Ctrl+C : Copy selected node or attribute.
- Ctrl+X : Cut selected node or attribute.
- Ctrl+V : Paste node or attribute.
- Ctrl+Shift+V (on goto) : Set goto reference towards the copied node.
- Ctrl+Shift+V (on branch) : Paste as branching sequence.
- Ctrl+Z : Undo.
- Ctrl+Y : Redo.
- Del : Delete selected node.

[\[Top\]](#summary)

### Tips

- Use copy on the root node to copy the whole dialogue.
- Use copy on a root attribute to copy all its children.
- Use special paste (ctrl+shift+v) to quickly edit goto and branch nodes.

[\[Top\]](#summary)

# Technical Documentation

## Overview 

The goal of this editor is to allow quick iterations on dialogues writing, and to provide the necessary tools for gameplay flow and data exports. 
It's inspired by the tools used on Cyanide's Game of Thrones RPG, and the Aurora tool used on Neverwinter Nights. 

The main functions of this editor pipeline are : 
- edition of the dialogues trees in a standalone editor. 
- exports for the targeted game engines and localization, voicing, and any external tools. 
- samples for easy integration and iteration in game engines, with both editors running in parallel. 

[\[Top\]](#summary)

## Project content 

### Directories 

- Build : the generated binary builds (release) of the demos. 
- Dev : the Visual Studio solution. 
- Dev/External : the external dependencies' dlls. 
- Docs : the documentations. 
- Icons : the working directory for icons. 
- ProjectDefault : the default "hello world" project assets. 
- ProjectDemo : the demo project assets. 

[\[Top\]](#summary)

### Visual Studio Solution 

When you open DialogueEditor.sln, you will find several projects inside : 
- DialogueEditorDll : the main part of the solution, containing all the editor's components. 
- DefaultBuild : the "hello world" build demo. You can use it to open the ProjectDefault/HelloWorld.project, and use it as a base for your project build. 
- DemoBuild : the sample build demo. You can use it to open the ProjectDemo/Demo.project, and use it as a reference when creating your project build. 
- TestSingleWindow : a small build test using a single dialogue view with its properties. This is intended as a base for projects willing to integrate the editor inside another tool. 

[\[Top\]](#summary)

## Design 

- Unique Files names : all dialogues are referenced by their names, and thus must all be unique accross the whole project. This ensures that all the files can be imported as a single big pack of assets in a common folder in any game engine. It could also allow dialogues to reference each other wherever they are stored in the hierarchy. 

- Directories : paths are not stored in the resources or the project. When the editor opens a project 
file, it will parse the adjacent folders and look for any .dlg files they may contain to fill the ResourcesHandler and the ProjectExplorer. This allows the users to move the files outside of the editor through any versionning software, such as svn or mercurial, without breaking anything in the editor. All they will need to do is reload the project. 

- Nodes IDs : dialogue nodes are stored as a flat list, in their order of creation. They will receive a incremental numeric ID upon creation. Those IDs are meant to be unique for each node inside a dialogue, and must never be modified through a node lifetime. 
The dialogue tree is defined by references between the nodes, through properties like NextNodeID, RepliesIDs, GotoID and BranchID. 
This ID is also used to have a consistent VoicingID and translation ID during production. 
When moved accross the tree, the nodes will not be moved in the serialized flat list. This limits the risks of conflict when using merge tools through svn or mercurial. 
This means that after some time, nodes IDs will appear as if they are disordered on the dialogue tree view. This is perfectly normal and intended. 
IDs are not indexes ! 

- CustomLists : this is a centralized dictionnary of dictionnaries. It is used as a helper to manipulate static lists of data throughout various parts of the editor. 
Several helpers already exist to manipulate fields associated with a specific CustomList, such as fields decorators converting string properties into a combo box (look for PropertyCustomListConverter). 
By default, five lists are registered as samples for necessary project properties (scene types, actors species, actors genders, actors builds, and voicing intensities). 
Those lists can be overriden or completed, and additional lists can be added in your project build (see part 4.1). 

- Nodes attributes : most nodes properties are built-in and specific to each type of node, except for 
nodes attributes. Those are declared in your custom build and can be instanciated on every node. 
There are three types of attributes, each with a different goal. 
Conditions are used to decide if a node should be executed or skipped. 
Actions are triggered at the beginning or end of a node execution. 
Flags act as static parameters that can be read during the node execution to provide more information. 

- Voicing ID : This ID is generated and stored in the .dlg for every sentence, in the form "DialogueName_NodeID". It is exported inside the Wwise text helper file and the voicing spreadsheets. Sound files recorded should match this ID in order for your game to dynamically find 
them during gameplay execution. 

[\[Top\]](#summary)

## Code 

### Libraries 

The editor is written in standard C#. 
It's based on several libraries : 
- WinForms for the UI (Native with Visual Studio) : https://msdn.microsoft.com/en-us/library/dd30h2yb%28v=vs.110%29.aspx 
- Json.Net (Newtonsoft) for the json serialization (provided dll) : http://www.newtonsoft.com/json 
- DockPanel Suite (Weifenluo) for the UI panels docking (recompiled dll) : http://dockpanelsuite.com/ 

The provided builds are compiled with Visual 2013, using .Net 4.5, other versions have not been tested. 

[\[Top\]](#summary)

### Naming conventions 

- Dialog and dialogue disambiguation : a dialog is a popup, while a dialogue is a conversation (uk english). 
- Node properties : this term references all the serialized parameters of a dialogue node. 
- Node attributes : this term is used to reference nodes conditions, actions, and flags under a common term. 
- Concerning the code : all methods and public variables use UpperCamelCase, while private variables use lowerCamelCase. 

[\[Top\]](#summary)

### Architecture 

- EditorCore.cs is a static class holding all the project's static resources and callbacks overrides. It also provides access to most of the editor components, such as windows, panels, logs, and settings. 
- ResourcesHandler.cs is a static class holding all the serialized project resources (dialogues and project). It provides the necessary methods to manipulate and serialize the project's files. 
- EditorHelper.cs is a set of static methods, classes and decorators used to manipulate various parts of the editor's component, such as manipulating listviews and treeviews icons or checking errors in fields and resources. 
- EditorSettings.cs is a serialized class used to store the user's local settings. 
- EditorVersion.cs is a static class holding the editor's version numbers. 
- Extensions.cs is a set of extensions for existing classes in the standard language and WinForms. 
- Utility.cs is a set of static methods used as helpers for the standard language, such as helpers to manipulate file paths or datetimes. 
- WIN32.cs is a set of static methods used for low-level operations. 
- Controls/ contains custom Winforms controls used by other forms. 
- Dialogs/ contains all the dialogs (popups) used by the editor. 
- Documents/ contains the forms used to view and edit the project's resources. They will be used as docked document in the center of the MainWindow. 
- Exporters/ contains static classes for the various import/export formats used by the editor's tools. 
- Panels/ contains the panels docked inside the MainWindow. 
    - PanelProjectExplorer.cs handles the files hierarchy and gives access to the project's resources. 
    - PanelOutputLog.cs handles the log messages sent by other components. 
    - PanelProperties.cs handles the list of editable properties associated with the selected dialogue node. 
    - Properties/ handles the different edition panels dedicated to each type of dialogue nodes. They will appear inside the PanelProperties. 
- Resources/ contains the classes used for serializing all the project's data. 
- Windows/ contains all the top-level forms. 
    - WindowMain.cs is the main window of the editor. It holds the docking area and the top menu. 
    - WindowViewer.cs is a basic dialogue parser, used to test how a dialogue plays out. 

[\[Top\]](#summary)

## Integration 

### Editor build 

To start using the editor efficiently with your project, you will need to create a custom build. This will allow you to declare custom attributes for your nodes, as well as injecting custom data into various properties. 
As a first step, you can use the DefaultBuild to prepare the integration with your game. You will be able to create actors and interactive dialogue trees, but you won't have access to any actions or conditions associated with your nodes. 

You can use the DefaultBuild as a base for your own build, and use the DemoBuild as a reference when iterating on it. You can declare all your overrides in your own Program.cs, and declare your own node attributes classes (conditions/actions/flags) in an adjacent folder. 

[\[Top\]](#summary)

#### Node attributes 

To declare a node attribute, you will first need to declare the class itself, with NodeCondition, NodeAction or NodeFlag as a base (look for NodeConditionHasHonor as a sample). 
You can declare as many public variables on your new condition, it will be serialized inside your dialogues. 
You can provide an override to the GetDisplayText_Impl and GetTreeText_Impl methods, they are used when displaying the attribute in the tree view and on the node properties edition panel. 

To integrate the new attribute in the editor, you have to bind it in the editor menus and serializer. To do that, you can use this method in your Program.cs : 

    EditorCore.BindAttribute(typeof(NodeConditionHasHonor), "ConditionHasHonor", "Has Honor"); 

The first string parameter will be used as the serialized type name. 
The second string parameter will be used as text for the context menu option. 

You can use decorators on your node attributes to define custom behaviours when editing their properties. A decorator already exists for Actors (PropertyCharacterNameConverter), as well as a base for CustomLists (PropertyCustomListConverter) which needs to be overriden for the lists you want to use. Those can be used as a reference for your own decorators. 

Declare like this : 

    public class PropertySkillNameConverter : PropertyCustomListConverter 
    { 
        public PropertySkillNameConverter() 
        { 
            CustomListName = "Skills"; 
        } 
    } 

Use like this : 

    [TypeConverter(typeof(PropertySkillNameConverter))] 
    public string Skill { get; set; } 

[\[Top\]](#summary)

#### Custom data 

You can edit the version number of your project build by editing EditorCore.VersionProject. 
This project version number will be stored in the header of all the saved files, next to the editor version number. 

You can plug a delegate that will be called when the project is loaded through EditorCore.OnProjectLoad. 
Animations are defined through EditorCore.Animations. It's a dictionary of animsets defined by their name and the list of animations they contain. You can also use the delegate EditorCore.GetActorAnimsets to fill the animset combobox depending on the actor. 

To inject lists of values associated to particular properties (a bit like enums), you can use EditorCore.CustomLists. It is aimed to store a list of named dictionaries, with keys being used for serialization, and values used for the editor display. 
You can either modify/replace existing lists (some are used to customize actors), or add your own new lists. The advantage is that some methods exist to make their edition easier through decorators, but you could have your own datasets and decorators instead. 

The delegate EditorCore.OnCheckDialogueErrors can be used to log additional information when the "Check Dialogue" command is used. 

To add your own menu on the top menubar of the editor, you can declare it in Program.cs then add 
it through EditorCore.MainWindow.AddCustomMenu, like this : 

    ToolStripMenuItem menuItemImportGameData = new System.Windows.Forms.ToolStripMenuItem(); 
    menuItemImportGameData.Text = "&Import Game Data"; 
    menuItemImportGameData.Click += delegate 
    { 
            //... 
    }; 
    
    ToolStripMenuItem menuItemGame = new System.Windows.Forms.ToolStripMenuItem(); 
    menuItemGame.Text = "My Game"; 
    menuItemGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { 
             menuItemImportGameData, 
    }); 
    
    EditorCore.MainWindow.AddCustomMenu(menuItemGame); 

[\[Top\]](#summary)

### Game integration 

To use the dialogues in your game, you will need to implement two main parts : 
- the dialogue parser, to read the json files in your game. 
- the dialogue interpreter, to run the dialogues in your game. 

[\[Top\]](#summary)

#### Dialogue parser 

To write the parser, you can use the description of the files structure.
Unreal provides a json parsing helper, and the json.net library used in this editor can be used in any 
engine using csharp, such as Unity. Solutions for json parsers ion other platforms should not be a problem to find. 

The parsing will essentially consists in a loop on the ListNodes. Each node and attribute will have its type defined by the property "$type", allowing you to instanciate objects matching those types. 

[\[Top\]](#summary)

#### Dialogue interpreter 

To write the interpreter, you can look at the WindowViewer inside the editor for inspiration. 

The flow of a dialogue starts from the root node, then plays the node referenced by the NexNodeID property, then the NexNodeID again and again until there is no next node, meaning the dialogue is finished. 
If a node's conditions are false, the node should be skipped, and it's NextNode should be played instead. 
The node's actions should be played at the beginning and the end of its execution (bool property OnNodeStart). 
Special nodes can fork the flow of the dialogue. Those nodes should not use their NextNodeID property unless they are skipped because of their conditions being false. Those nodes include Choice, Goto and Branch, and will use the properties RepliesIDs, GotoID and BranchID respectively. 

Sentences will be the core of the execution. They hold the displayed text, as well as informations about the speaker and the animations. 
The speaker is the actor actually speaking the sentence's text. The listener is the actor looked at by the speaker. 

Your design should include a central manager for all the dialogues played. It would be in charge of starting, updating and closing the dialogues, as well as handling priorities on UI and voicing. 
An interactive dialogue should always be unique and have top priority, while several ambiant dialogues may be played simultaneously. 

[\[Top\]](#summary)

#### Samples 

You can find integration samples in Docs/Samples. 
You will find code extracts and instructions for the Unreal part, and the build sample used for the DialogueEditor. 

[\[Top\]](#summary)

