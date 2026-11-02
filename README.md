# Shared

Private .NET Windows Forms utilities shared by Sibra-Soft projects.

Keep this checkout alongside its consumers, for example Tower/ and _Shared/.
Tower references ../_Shared/Shared.csproj and pins a commit in its Shared dependency manifest.

Build: dotnet build Shared.csproj -c Release

Commit source changes here independently. Update each consumer's pinned version only after validation.
Generated bin, obj and IDE files are excluded from Git.
