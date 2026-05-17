## Requirements:
- The .NET 10 SDK ([Download](https://dotnet.microsoft.com/en-us/download))
- Visual C++ Redistributable ([Download](https://aka.ms/vs/17/release/vc_redist.x64.exe))

## The 'Content' Folder
To make things simple, I have developed a conversion program that can setup one's 'Content' folder for TerrariaOGC.
Once the Terraria Content Conversion Suite (TCCS) is downloaded, place the executable and the required 'Prerequisites' folder in the same directory as the 'Content' folder. 
You can then open the executable and follow the instructions to begin conversion. You can find out more about TCCS [here](https://github.com/PPrism/TCCS).

## Building:

1. Clone the repository: ``git clone --recursive https://github.com/sjoerdev/TerrariaOGC``
2. Put your ``Content/`` folder in the same folder as the ``TerrariaOGC.slnx`` solution file
3. Compile with: ``dotnet publish -p:Configuration="Debug" -p:Platform="x64"``
4. You can now find the build inside ``./TerrariaOGC/bin/x64/Debug/net10.0-windows/publish/``
