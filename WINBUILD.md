## Getting Started
In order to get started with TerrariaOGC, you will need to acquire some prerequisites:
- [FNA](https://github.com/FNA-XNA/FNA)
- [Terraria Content Conversion Suite](https://github.com/PPrism/TCCS)

### Directory Preparation
Firstly, you will need to clone the repo recursively. Do not open the .sln or .csproj for TerrariaOGC just yet, as the environment needs to be setup first.
You can clone the repo **recursively**, using the following command in [Git](https://git-scm.com/downloads):
`git clone --recursive https://github.com/PPrism/TerrariaOGC'

Once cloned, go inside of the 'TerrariaOGC' folder and copy the contents inside of 'Dependencies\LidgrenNET4'. 
* These files have been modified to work with .NET 4.0, which TerrariaOGC runs on.

Once copied, go back a folder and then go to the cloned 'Lidgren.Network' folder, and then head into the folder with the same name.
Now that you are inside that directory, paste the 3 files you copied to replace the original versions. You can now open 'Lidgren.Network.csproj'.

Before you build, open the properties of Lidgren.Network.csproj and change the target framework to `net40` so it builds for .NET 4, then you can build a release-version of the .DLL.

### Setting up FNA
Please note that this will only cover what is needed for the project. For instructions to setup FNA, you can get started [here](https://fna-xna.github.io/docs/1:-Setting-Up-FNA/).

Inside of the downloaded FNA .zip, copy the 'FNA' folder and place it in the same directory as the TerrariaOGC.sln. Next, copy the 'GamerServices' and 'Net' folder from the 'Dependencies' folder inside of 'TerrariaOGC' and place them in the 'src' folder inside of 'FNA'.
* These contain the stub functions needed by these versions of Terraria due to it's interfacing with the Xbox system for the XDK release and for some other functions which have been preserved for authenticity purposes.

Now, open the TerrariaOGC.sln at the root directory of this setup, and inside of the solution explorer, click 'Show All Files'. Right-click on both 'GamerServices' and 'Net' in the FNA project, and click on 'Include in project'.
While still in the solution explorer, right-click on 'Dependencies' for 'FNA.NetFramework', click 'Add Project Reference', click 'Browse' in the reference manager and go to the built Lidgren.Network.dll from earlier and then import it. 
Before you click 'Ok', go to 'Assemblies' in the reference manager and be sure to include 'System.Windows.Forms'. You can setup the rest of FNA as per your system needs.

After this, open the drop-down for the 'TerrariaOGC' project, and then right-click on the folder labelled 'Dependencies' and then click 'Exclude from Project'.

Since the project needs to create font resources from the code, you will need to make a small and inconsequential change to FNA's code. Go to 'FNA/src/Graphics/SpriteFont.cs' and change the access modifier for the constructor from `internal` to `public`. 

After you setup FNA, you can now click the first configuration drop-down and choose which version you would like to run, or you can go to the properties of the 'TerrariaOGC' project file, and customise the 'Debug' configuration to your desires.

Once you build, your desired configuration will have been built in the appropriate folder located in the 'TerrariaOGC/bin' directory. Please ensure you have the required libraries for FNA (e.g., FAudio.dll) in those directories as well.

### The 'Content' Folder
Now it is time to setup your 'Content' folder so that the executable can run.

To make things simple, I have developed a conversion program that can setup one's 'Content' folder for TerrariaOGC.
Once the Terraria Content Conversion Suite (TCCS) is downloaded, place the executable and the required 'Prerequisites' folder in the same directory as the 'Content' folder. 
You can then open the executable and follow the instructions to begin conversion. You can find out more about TCCS [here](https://github.com/PPrism/TCCS).

As stated by TCCS, you need to convert the music files found in your 'Content' folder into a wave bank. A recommended tool for this is [QuickWaveBank](https://github.com/trigger-segfault/QuickWaveBank/).

Now your 'Content' folder is ready to go, drag it into the directory where you built your specified version of TerrariaOGC and then you can start the executable without issue. A settings file will be generated after the first run which you can modify afterwards.
