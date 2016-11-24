#r "packages/build/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing.NUnit3
open ReleaseNotesHelper
open BuildServerHelper
open AssemblyInfoHelper

let outputDirectory = @"out"
let releaseNotes = LoadReleaseNotes "ReleaseNotes.md"

let version = 
  match buildServer with
  | AppVeyor -> { releaseNotes.SemVer with Build = appVeyorBuildVersion }
  | _ -> releaseNotes.SemVer
let buildNumber = match version.Build with | "" -> "" | v -> "." + v
let asmVersion = sprintf "%i.%i.%i%s" version.Major version.Minor version.Patch (match version.Build with | "" -> "" | v -> "." + v)
let nugetVersion = sprintf "%i.%i.%i%s" version.Major version.Minor version.Patch (match version.Build with | "" -> "" | v -> "-build" + v)

tracefn "Assembly Version: %s" asmVersion
tracefn "NuGet Version: %s" nugetVersion

Target "Clean" (fun _ ->
  !! "**/bin"
  ++ "**/obj"
  ++ "**/out"
  |> DeleteDirs
)

Target "PatchAssemblyInfos" (fun _ ->
  let assemblyInfos = !!"src/**/AssemblyInfo.cs"
  ReplaceAssemblyInfoVersionsBulk assemblyInfos (fun f -> { f with AssemblyVersion = asmVersion
                                                                   AssemblyInformationalVersion = asmVersion
                                                                   AssemblyFileVersion = asmVersion }) 
)

Target "Build" (fun _ ->
  let setParams defaults = 
    { defaults with
        Verbosity = Some(Quiet)
        NodeReuse = false
        NoLogo = true
        Targets = ["Build"]
        Properties = 
          [
            "Optimize", "True"
            "DebugSymbols", "True"
            "Configuration", "Debug"
            "OutputPath", outputDirectory |> FullName |> trimSeparator
          ]
    }
  
  build setParams "./src/Prism.Munq.sln" |> ignore
)

Target "Test" (fun _ ->
  !! (outputDirectory @@ "*.Tests.dll")
  |> NUnit3 (fun parameters ->
    { parameters with
        ToolPath = "./packages/test/Nunit.ConsoleRunner/tools/nunit3-console.exe"
        OutputDir = (outputDirectory @@ "TestResults.xml")
    })
)

"Clean"
  =?> ("PatchAssemblyInfos", not isLocalBuild)
  ==> "Build"
  =?> ("Test", outputDirectory |> directoryExists |> not)

RunTargetOrDefault "Build"
