# Dialogue Editor - Samples

## Loading a project with obsolete serialized data

### Obsolete properties

By default, Dialogue and DialogueNode classes will read obsolete properties and store them in a special dictionnary named "ExtensionData".   

Once loaded, you can purge ExtensionDatas and node attributes manually through the EditorCore.OnDialoguePostLoad delegate.
When everything is updated, you can just use the "Force Save Full Project" command.


### Obsolete types

To load obsolete types serialized in your dialogue files, like CustomProperties, Conditions, Actions and Flags, you can use dummy types containing ExtensionDatas and bind them to the obsolete type name.  
Those dummy types are defined in the ExporterJson namespace.  

You can set the CustomProperties slot to stop being generated on all nodes through the AddOnEveryNode property in the binding.  

Once loaded, you can purge ExtensionDatas and node attributes manually through the EditorCore.OnDialoguePostLoad delegate.
When everything is updated, you can just use the "Force Save Full Project" command.

```csharp
CustomPropertiesSlot slot = new CustomPropertiesSlot();
slot.DialogueNodeType = typeof(DialogueNodeSentence);
slot.CustomPropertiesName = "DirectingProperties";
slot.CustomPropertiesType = typeof(ExporterJson.DummySerializedCustomProperties);
slot.FormType = null;
slot.AddOnEveryNode = false;
EditorCore.BindCustomProperties(slot);

EditorCore.OnDialoguePostLoad = delegate (Dialogue dialogue)
{
    foreach (DialogueNode node in dialogue.ListNodes)
    {
        node.CustomProperties?.Clear();
        node.ExtensionData?.Clear();
    }

    dialogue.ExtensionData?.Clear();
};
```


