# SibraSoft.Shared

Small, reusable utilities for .NET Windows Forms applications, maintained by Sibra-Soft.

Reorder lists and ListView items, distribute column widths, capitalize the first character of a string, and pass parameters between forms using a shared dialog helper.

[NuGet package](https://www.nuget.org/packages/SibraSoft.Shared) · [Source code](https://github.com/Sibra-Soft/shared) · [Releases](https://github.com/Sibra-Soft/shared/releases)

## Requirements

- Windows.
- A project targeting `net10.0-windows` with Windows Forms enabled, or a compatible newer Windows target.
- The .NET 10 SDK when building this repository from source.

The package is intended for modern .NET Windows Forms applications, not .NET Framework projects.

## Installation

### Visual Studio

1. Right-click your project and select **Manage NuGet Packages**.
2. Select **nuget.org** as the package source.
3. Open **Browse**, search for **SibraSoft.Shared**, and install it.

Enable **Include prerelease** if you want to install a preview version.

### .NET CLI

```shell
dotnet add package SibraSoft.Shared
```

### Package Manager Console

```powershell
Install-Package SibraSoft.Shared
```

The package name is `SibraSoft.Shared`; its namespaces remain `Shared.Extensions` and `Shared.Utils`.

If you previously referenced `Shared.csproj` directly, remove that project reference before adding the package. Existing namespace imports can stay unchanged.

## List extensions

Use `Shared.Extensions` to move elements in an `IList<T>`:

```csharp
using Shared.Extensions;

var items = new List<string> { "Alpha", "Bravo", "Charlie" };

items.MoveUp(2);    // Alpha, Charlie, Bravo
items.MoveDown(0);  // Charlie, Alpha, Bravo
items.Move(2, 0);   // Bravo, Charlie, Alpha
```

| Method | Behavior |
| --- | --- |
| `MoveUp(index)` | Swaps an element with the previous element. |
| `MoveDown(index)` | Swaps an element with the next element. |
| `Move(oldIndex, newIndex)` | Removes an element and inserts it at the requested index. |

Indices are zero-based. The methods return `false` for invalid indices or a move past a list boundary. `Move` returns `true` when both valid indices are equal. A null list throws `ArgumentNullException`. Use a mutable list; `Move` requires support for removal and insertion.

## ListView extensions

### Move the selected item

In your form, where `listView1` is an existing ListView control:

```csharp
using Shared.Extensions;

private void MoveUpButton_Click(object? sender, EventArgs e)
{
    listView1.MoveSelectedItemUp();
}

private void MoveDownButton_Click(object? sender, EventArgs e)
{
    listView1.MoveSelectedItemDown();
}
```

These helpers move the first selected item by one position and restore its selection and focus. They do nothing if no item is selected or the item is already at the relevant boundary. For manual ordering, disable automatic sorting and use a non-virtual ListView.

### Distribute column widths

Mark columns that should keep their current width with `Tag = "fixed"`. The remaining columns share the available width equally.

```csharp
using Shared.Extensions;

// Run after initializing the form and its columns.
listView1.View = View.Details;
listView1.Columns[0].Width = 80;
listView1.Columns[0].Tag = "fixed";

listView1.ResizeColumns();
listView1.ClientSizeChanged += (_, _) => listView1.ResizeColumns();
```

The helper reserves space for a vertical scrollbar whenever the ListView contains items. It does not measure text or resize columns to their content. Call it again after changes that affect the layout.

## String extensions

```csharp
using Shared.Extensions;

string title = "hello world".UCFirst(); // Hello world
```

`UCFirst()` uppercases the first character using the current culture and leaves the rest unchanged. It is not a title-case conversion. Null input throws `ArgumentNullException`; an empty string throws `ArgumentException`.

## Dialog utilities

`Shared.Utils.Dialogs` provides shared request and response parameters, a dialog mode, and helpers for opening forms.

### Pass values to a modal dialog

The following example assumes your application defines an `EditItemForm` with a `textBoxName` control and a save button.

In the calling form:

```csharp
using Shared.Utils;

Dialogs.ClearRequest();
Dialogs.ClearResponse();
Dialogs.SetDialogType(Dialogs.Types.Edit);
Dialogs.AddRequestParameter("Name", "Original name");

using var dialog = new EditItemForm();

if (Dialogs.ShowAndWait(dialog, "Edit item", this) == DialogResult.OK)
{
    string updatedName = Dialogs.GetResponseParameter<string>("Name");
    // Use updatedName in your application.
}
```

In `EditItemForm`, after `InitializeComponent()`:

```csharp
textBoxName.Text = Dialogs.GetRequestParameter<string>("Name");
```

In its save button handler:

```csharp
Dialogs.AddResponseParameter("Name", textBoxName.Text);
DialogResult = DialogResult.OK;
Close();
```

### Available helpers

| Method | Purpose |
| --- | --- |
| `ClearRequest()` | Initializes or clears request parameters and resets the mode to `Default`. |
| `ClearResponse()` | Initializes or clears response parameters. |
| `AddRequestParameter(name, value)` | Adds a named input value. |
| `GetRequestParameter<T>(name)` | Reads an input value as a supported type. |
| `AddResponseParameter(name, value)` | Adds a named output value. |
| `GetResponseParameter<T>(name)` | Reads an output value as a supported type. |
| `SetDialogType(type)` / `GetDialogType()` | Sets or reads `Dialogs.Types.Default` or `Dialogs.Types.Edit`. |
| `Show(dialog, title)` | Sets the title and opens a modeless form. Returns its current `DialogResult` immediately. |
| `ShowAndWait(dialog, title, parent)` | Opens a modal form with an owner and returns its result after it closes. |

### Parameter behavior

Both getters support `string`, `int`, `bool`, `Guid`, and `List<T>`. The request getter additionally supports `Dictionary<TKey, TValue>`; the response getter does not.

- Call the corresponding clear method before adding or reading parameters.
- Set the dialog type **after** `ClearRequest()`, which resets it.
- Adding a duplicate name throws; adding a parameter does not replace an existing value.
- Reading an unknown name throws `KeyNotFoundException`.
- Unsupported result types throw `InvalidOperationException`.
- Boolean and collection values must have the expected runtime type. These helpers are not a general-purpose object mapper.

Parameter storage and dialog mode are static and shared across the application. Use this helper for one dialog exchange at a time; simultaneous or nested exchanges can overwrite each other's state. Access Windows Forms controls on the UI thread, and manage modeless form lifetime in the calling application.

## Build from source

```shell
git clone https://github.com/Sibra-Soft/shared.git
cd shared
dotnet build Shared.csproj --configuration Release
```

To create a local package:

```shell
dotnet pack Shared.csproj --configuration Release --output artifacts -p:Version=1.0.0
```

For source-based development, you can still reference `Shared.csproj` directly from a neighboring checkout. Use either a project reference or a package reference in each consumer.

## Releases

Publishing a GitHub release runs the repository's `release.yml` workflow. It builds the tagged source, derives the package version from the release tag, and publishes to NuGet.org using Trusted Publishing.

Use a new version tag such as `v1.0.1` for each release. For a preview, use a tag such as `v1.1.0-beta.1` and mark the GitHub release as a prerelease. A tag push alone does not publish a package.

The publishing job uses the GitHub environment `production` and the repository variable `NUGET_USER`. Its NuGet Trusted Publishing policy must match the repository, workflow filename, and environment, and permit publication of the intended package. No long-lived NuGet API-key secret is required.
