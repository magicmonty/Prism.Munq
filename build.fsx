#r "packages/build/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing.NUnit3
open ReleaseNotesHelper
open BuildServerHelper
open AssemblyInfoHelper
open PaketTemplate
open Paket

let outputDirectory = @"out"
let deployDirectory = @"deploy"
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
  ++ outputDirectory
  ++ deployDirectory
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
        MaxCpuCount = Some (Some 1)
        NoLogo = true
        Targets = ["Build"]
        Properties = 
          [
            "Optimize", "True"
            "DebugSymbols", "True"
            "Configuration", "Release"
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

Target "CreateNugetPackage" (fun _ ->
  deployDirectory |> CleanDir

  PaketTemplate (fun p -> { p with TemplateFilePath = Some "paket.template"
                                   TemplateType = File
                                   Id = Some "Prism.Munq"
                                   Description = [ "Use these extensions to build Prism applications based on Munq." ]
                                   Summary =  [ "Munq extensions for Prism" ]
                                   Tags = [ "prism"; "mvvm"; "wpf"; "munq"; "dependency injection"; "di" ]
                                   RequireLicenseAcceptance = Some false
                                   ReleaseNotes = releaseNotes.Notes  
                                   Version = Some nugetVersion 
                                   Authors = ["Martin Gondermann"] 
                                   Owners = ["Martin Gondermann"]
                                   IconUrl = Some "http://prismlibrary.github.io/images/prism-logo-graphic-128.png"
                                   Language = Some "en-US"
                                   Files = 
                                    [
                                      Include (outputDirectory @@ "Prism.Munq.Wpf.dll", @"lib\net45")
                                    ] 
                                   Dependencies = 
                                    [
                                      "Prism.Wpf", GreaterOrEqualSafe LOCKEDVERSION
                                      "Prism.Core", GreaterOrEqualSafe LOCKEDVERSION
                                      "Munq.IocContainer", GreaterOrEqualSafe LOCKEDVERSION
                                      "CommonServiceLocator", GreaterOrEqualSafe LOCKEDVERSION ] } )
  Pack (fun p -> { p with TemplateFile = "paket.template"
                          OutputPath = deployDirectory 
                          Version = nugetVersion }) 
)

"Clean"
  =?> ("PatchAssemblyInfos", not isLocalBuild)
  ==> "Build"
  =?> ("Test", outputDirectory |> directoryExists |> not)

"Test" ==> "CreateNugetPackage"

RunTargetOrDefault "Build"
