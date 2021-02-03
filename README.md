# ![Icon](/Icons/led24/bookmark_book.png) Dialogue Editor 1.1 
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

A Dialogue Editor for branching dialogues, built in C# and mainly based on WinForms.  

The Editor is designed for large scale production, primarily targeting the needs of writers and game designers.  
It is based on individual Json files for easy integration in any game engine, and provides import/export pipelines for localization and voicing.  

The main goal of the tool is to provide consistent and dependable unique IDs on every sentence in the project, to allow easier and safer iterations on localization and voicing, even for already integrated dialogues.  
It is designed as a backbone for automated staged dialogues, with very minimal camera and animation options.  

What it does NOT do, is to provide tools for timelines and advanced staged dialogues.    
You can, however, plug the exported data on top of your game engine timelines.

The tool has been created at Cyanide for its various projects needs.  
It is mostly inspired by the tools used on Cyanide's Game of Thrones RPG, and the Aurora tool used on Neverwinter Nights.

![Screenshot](/Docs/DemoScreenshot.png)

## Features
(You can check the last updates in the [Changelog](/Docs/Changelog.md))

- Individual Json files for each dialogue.
- Edition of narrative flows through a node hierarchy.
- Branching flows, either through interactive choices, conditional branching, and go-to nodes.
- Unique IDs assigned for every sentence in the project, from creation to updates and reorders.
- Localized constants can be used inside sentences.
- Entry points for simple animations, custom conditions, actions and flags on every node.
- Import/Export for localization and voicing (csv files with pre-generated unique files and events IDs).
- Actors settings, including parameters and voice-kit assignation for voicing sessions.

## Dependencies
- Newtonsoft.Json, A Json serializer library : https://github.com/JamesNK/Newtonsoft.Json
- WeifenLuo.WinFormsUI.Docking, A docking library : https://github.com/dockpanelsuite/dockpanelsuite
- Icons from [led24.de](http://www.led24.de/)

## Documentation
- [User Documentation](/Docs/UserDocumentation.pdf)
- [Technical Documentation](/Docs/TechnicalDocumentation.pdf)
- [Changelog](/Docs/Changelog.md)

You can reach the author here : [twitter/Legulysse](https://twitter.com/legulysse) or here : [github/Legulysse](https://github.com/Legulysse)
