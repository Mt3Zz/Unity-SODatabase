# Unity-SODatabase

[![license](https://img.shields.io/badge/LICENSE-MIT-green.svg)](LICENSE.md)
[日本語版はこちら](README_JA.md) | [English version here](README.md)

## Overview
SODatabase is a Unity library for managing ScriptableObjects.
It has the following features:
- Minimal database system
- CRUD operations completed through a GUI
- Extensibility

## Setup
**Requirements**
- Unity 2022.3 or higher

**Installation**
1. Open the Package Manager.
2. Click the “+” button and select “Add package from git URL.”
3. Enter the following URL to install:
`https://github.com/Mt3Zz/Unity-SODatabase.git?path=/Assets/SODatabase`

## Usage
1. Open Window > SODatabase Editor.
2. Click the “Preference” button to open package settings.
3. Click the “Create New Storage” button to create a new storage in “Managed Storages.”
4. Return to the SODatabase Editor window.
5. The created storage will be displayed in the sidebar. Create a new object from “Append Record.”

## Extension
1. Create a new assembly definition reference file in any folder.
2. Attach “SODatabase” to the assembly definition of the created file. Be careful not to attach SODatabase.Editor, SODatabase.Editor.Tests, or SODatabase.Tests.PlayMode.
3. Create C# code in the folder. Refer to the sample code below.
4. The defined object will be added to the window. Use “Append Record” to create and use the defined object.

```csharp
// SampleObject.cs

using UnityEngine;

// Add SODatabase.DataObject
using SODatabase.DataObject;

// Inherit from BaseObject
public sealed class SampleObject : BaseObject
{
    [SerializeField]
    private int _sampleIntValue;
    [SerializeField]
    private string _sampleStringValue;
}
```
