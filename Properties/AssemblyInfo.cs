using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;

[assembly: AssemblyTitle(MasterSeatFix.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(MasterSeatFix.BuildInfo.Company)]
[assembly: AssemblyProduct(MasterSeatFix.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + MasterSeatFix.BuildInfo.Author)]
[assembly: AssemblyTrademark(MasterSeatFix.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(MasterSeatFix.BuildInfo.Version)]
[assembly: AssemblyFileVersion(MasterSeatFix.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(MasterSeatFix.MasterSeatFixMain), MasterSeatFix.BuildInfo.Name, MasterSeatFix.BuildInfo.Version, MasterSeatFix.BuildInfo.Author, MasterSeatFix.BuildInfo.DownloadLink)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]