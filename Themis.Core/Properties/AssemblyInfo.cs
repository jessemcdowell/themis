using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Themis.Core")]
[assembly: AssemblyDescription("A simple resource scheduling service.")]
[assembly: AssemblyProduct("Themis.Core")]
[assembly: AssemblyCopyright("Copyright © McDowell 2010")]
[assembly: AssemblyCompany("Jesse McDowell")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("5ca2298c-3694-4496-a647-79b5409e8b3c")]


// allow unit tests to access internal members
[assembly: InternalsVisibleTo("Themis.Core.Tests")]