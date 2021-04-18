using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(Bitzophrenia.BuildInfo.Name)]
[assembly: AssemblyDescription(Bitzophrenia.BuildInfo.Description)]
[assembly: AssemblyCompany(Bitzophrenia.BuildInfo.Company)]
[assembly: AssemblyProduct(Bitzophrenia.BuildInfo.Name)]
[assembly: AssemblyCopyright("Copyright © " + Bitzophrenia.BuildInfo.Author + " 2021")]
[assembly: AssemblyTrademark(Bitzophrenia.BuildInfo.Company)]
[assembly: AssemblyVersion(Bitzophrenia.BuildInfo.Version)]
[assembly: AssemblyFileVersion(Bitzophrenia.BuildInfo.Version)]
[assembly: MelonInfo(typeof(Bitzophrenia.Main), Bitzophrenia.BuildInfo.Name, Bitzophrenia.BuildInfo.Version, Bitzophrenia.BuildInfo.Author, Bitzophrenia.BuildInfo.DownloadLink)]
[assembly: MelonGame("Kinetic Games", "Phasmophobia")]
