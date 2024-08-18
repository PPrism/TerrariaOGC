# The Lore
Akin to the PC version of Terraria, the 'Old-Gen' console versions are based on a .NET Framework-design using a reimplementation of XNA, and TerrariaOGC itself is built using .NET Framework with [FNA](https://github.com/FNA-XNA/FNA). There exists a nice walkthrough on the development of the console versions through a developer's own words that I would highly recommend, and that author can be found [here](https://medium.com/@watsonwelch).

Initially, the Xbox 360 console version was built using XNA, just like the PC version, and as a result, it needed quite a few auxillary files (e.g. XNA DLLs, HostLoader) to run, but starting from version 1.01 on the Xbox 360 and with the initial release of the PS3 version, a different, seemingly proprietary, engine is used to make the game.

## Enter the 'Embedded .NET Engine' (E.NET Engine)
- The engine seemingly known as the 'Embedded .NET Engine', is what these versions of the game were developed on and it would later be reused with other versions of the game published by 505 Games, prior to those versions receiving content from PC's 1.3 version of the game.
- Versions of the game built in the E.NET engine do not use C# for the language, but rather C++, and as such, it has replicated the XNA framework with various system-dependent APIs implemented where necessary.
- The primary reason for using this custom engine is compatibility, since applications made in C# cannot be run on most systems without auxillary additions, and this is why the PS3 version released with this engine.
  - Performance was also a factor, since when observing the decompiled state of the initial version, I found that aside from the requirement of various runtimes, it had code which was 'unsafe' from a C# standpoint, indicating measures were taken to improve performance, but ultimately, C++ has much less allocating and garbage than C#, so the Xbox 360 version was eventually migrated to that codebase in order to take advantage.
- Unfortunately, without any debug information or more information about the engine, decompiling the engine itself becomes a lot more difficult than it would normally be (which is still very difficult), which makes determining what everything is inside of the game a monumental task.

I still would like to decompile this engine one day, especially since the engine is used for the other XNA-designed versions of 'Old-gen Terraria' (Mobile, Wii U, 3DS), and I'm sure one of those versions would have some debug goodies accidentally left in, like a .SYMTAB or a .pdb file.

Ultimately, this means I had to substitute with FNA (which is fine since XNA is basically replicated 1:1 in the E.NET engine) and as such, some workarounds had to be made, primarily around file handling, as various resources like Fonts, Spritesheets, and Shaders, need workarounds or alternatives to be usable with TerrariaOGC.

# Version Explanations
With TerrariaOGC, I am supporting a total of 4 distinct versions of the game, with one of those versions having a patched branch that can be played. These 4 versions are: **1.0, 1.01, 1.03, and 1.09**.

## 1.0:
- Version 1.0 was the initial release of the game that released in March 2013. It contained all of the functionalities that you attribute to the 'Old-Gen' versions of Terraria for consoles. It was most equivalent to the PC's 1.1.2 version of the game.
- The game includes all content from the PC 1.0 series of updates, Hardmode, the christmas update, alongside most things attributable from the release of the Collector's Edition for the PC version of the game.
- This version was patched for the Xbox 360 version in May 2013 which fixed a few bugs and made some optimisations to the inner workings of the game.
  - Some changes in this patch were not featured in the PS3 version of the game until version 1.01.
  - This patch was also responsible for fixing the game-breaking 'Dragon Mask' bug.

## 1.01:
- Version 1.01 was the first major update to the game, released in November 2013 and made a few more adjustments and bug fixes, but with the most notable feature being the inclusion of some PC 1.2 content.
- This version included new graphics for the ambient environment, PC 1.2 Zombie and Demon Eye variants, and Sparkly wings, alongside other neat additions.
  - This version also brought some changes from the Xbox patch to the PS3.
- This is also the version that is responsible for migrating the Xbox 360 version to the 'Embedded .NET Engine' like the PS3 version, rather than have it continue to run as a C# executable, meaning the language it was built in was now C++.
  - Doing this with the update brought many performance improvements to the game for Xbox players and it remained this way until the Console 1.3 update.
- The PS Vita version of Terraria also released on a branch of this version.

## 1.03:
- Version 1.03 was by far the largest hotfix the game had received during it's lifetime, as it was the first update to follow the titanic Console 1.2 update, that being 1.02, which was most equivalent to PC's 1.2.1.2 version.
- This version is responsible for fixing the large number of inconsistencies (such as new characters not having 20 mana) and bugs (such as jungle spores not spawning) that came with the 1.02 update.
- It was also the version that was packaged on initial release for the PS4 & Xbox One versions of the game, which released in November 2014, while the patch for the then-current systems was released almost 6 months before in May.

## 1.09:
- Version 1.09 of Terraria for the console versions was the final release developed by Engine Software which was released in August/September of 2016 and served as the update most equivalent to PC's 1.2.4.1.
- Following this update, almost a year and a half later, PC's 1.3.0.7 version would be ported by Pipeworks Studio to the then-current gen systems, the PS4 & Xbox One, resetting the development team and abandoning prior systems.
  - This means for the PS3 & Xbox 360, version 1.09 is the most recent.
- In regards to the content featured in the update, it provides fixes pertaining to the 2nd half of the ported PC 1.2.4.1 update. 
- From a technical standpoint, this version is the last one to be built in the 'Embedded .NET Engine', as Pipeworks Studio rebuilt the game in Unity after this.