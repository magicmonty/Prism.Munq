#r "packages/build/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing.XUnit2

let outputDirectory = "out"

Target "Clean" (fun _ ->
  outputDirectory |> DeleteDir
)

Target "Build" (fun _ ->
  !! "src/Prism.Munq.sln"
  |> MSBuildDebug outputDirectory "Clean,Build" 
  |> Log "Build-Output: "
)

Target "Test" (fun _ ->
  !! (outputDirectory @@ "*.Tests.dll")
  |> xUnit2 (fun parameters ->
    { parameters with
        ToolPath = "./packages/test/xunit.runner.console/tools/xunit.console.x86.exe"
        HtmlOutputPath = Some (outputDirectory @@ "TestResults.html")
        XmlOutputPath = Some (outputDirectory @@ "TestResults.xml")
    })
)

"Clean"
  ==> "Build"
  ==> "Test"

RunTargetOrDefault "Build"
