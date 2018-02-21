# IndexesStartAtOne
A C# [compiler plugin](https://github.com/6A/Cometary) that makes all indexing operations in an assembly start at a given index (by default, 1).

### Usage
Anywhere in your file,
```csharp
[assembly: IndexesStartAtOne]
```

Then, you can use it however you want (see [Tests](./IndexesStartAtOne.Tests/UnitTests.cs)).

### Installation
```powershell
Install-Package IndexesStartAtOne
```

### Why?
[Some men just want to watch the world burn](https://www.goodreads.com/quotes/592287). I'm one of them.

### License
[Do What the Fuck You Want To Public License](./LICENSE.md)
