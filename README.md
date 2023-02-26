# pounds&
easy assembly loader for s&box

## what?
pounds& patches s&box to load custom assemblies, making it possible to code anything you want - you don't even need the editor!

## how?
1. create a folder in the s&box root called `poundsand`
2. patch Microsoft.CodeAnalysis.dll in <s&box root>/bin/managed using the patcher tool
    - do `patcher -v -p <path to dll>`
3. compile the bootstrap - place `bootstrap.dll` in the poundsand folder you made before
4. put the assemblies you want to run in that poundsand folder!
