#r "packages/build/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing.NUnit3

let outputDirectory = "out"

Target "Clean" (fun _ ->
  outputDirectory |> DeleteDir

  !! "**/bin"
  ++ "**/obj"
  |> DeleteDirs
)

Target "Build" (fun _ ->
  !! "src/Prism.Munq.sln"
  |> MSBuildDebug outputDirectory "Clean,Build" 
  |> Log "Build-Output: "
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
  ==> "Build"
  ==> "Test"

RunTargetOrDefault "Build"
