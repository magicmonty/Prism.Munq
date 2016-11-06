#r "packages/build/FAKE/tools/FakeLib.dll"

open Fake

Target "Build" (fun _ ->
  tracefn "Hello World"
)

RunTargetOrDefault "Build"
