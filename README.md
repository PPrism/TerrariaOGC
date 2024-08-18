# TerrariaOGC: A modern port of 'Old-Gen' Console Terraria
# **PLEASE SUPPORT THE OFFICIAL RELEASES OF TERRARIA**

Revived and revamped, TerrariaOGC is a new way to play Terraria, specifically the 'Old-Gen' releases for the Xbox 360 and PS3 console versions that are long since outdated and partially unavailable to play anymore, along with the exclusive content that these versions come with.

Alongside being decompiled from the original versions and ported away from console systems, TerrariaOGC also comes with new enhancements compared to the original versions in order to make your experience more enjoyable.

![Preview](https://i.imgur.com/XiBt7ky.png)

* You will need to provide assets from the official 'Old-Gen' console releases in order to run TerrariaOGC.

* The official releases can be found with the links below:
  * [Xbox Marketplace (X360 Version)](https://marketplace.xbox.com/Product/Terraria-Xbox-360-Edition/66acd000-77fe-1000-9115-d8025841128f)
  * [PlayStation Store (PS3 Version)](https://store.playstation.com/en-au/product/EP4040-NPEB01270_00-TERRARIA00000002)
  * *Yea... sorry for the bait. These links no longer work as their storefronts have been decommissioned, so these links are here for archival reasons. Please still try and get either the physical or digital releases.*
* You can also support the devs in today's age by buying the other releases or their merchandise from:
  * [Terraria's Official Store](https://terraria.org/store)

**TerrariaOGC is not intended to act as a free substitute for the official versions of Terraria**; This project was made with love and care for the source material and was created for purely educational purposes, and would not exist without the work of the marvelous people at Re-Logic, 505 Games, and Engine Software.

# Features:
* You can run all released milestone versions of the game. Including: **1.00** (Console initial release), **1.01** (Console Improvement of PC 1.1.2), **1.03** (Console Improvement of PC 1.2.1.2) & **1.09** (Final 'Old-Gen' update / Console Improvement of PC 1.2.4.1).
  * Versions 1.03 & 1.09 are to come later; See FAQ for details.
* Added variable screen sizing and other user options.
  * Included are some optional gameplay tweaks (Toggleable by the Settings.ini file).
  * This also includes the re-implementation of some debug gameplay features (FPS Counter, 'Always active' GPS features)
* Added an achievement system built off the original Xbox 360 & PS3 version.
* Added the ability to backup worlds during gameplay.
  * You can do this by saving the game in the pause menu.
* You can transfer your saves from the original versions as long as the version matches. Once extracted, you can drop it in the SavedGames folder on your system!
* Auxillary codebase is now built on a hybrid of FNA and MonoGame, with base networking provided by Lidgren.Network.
  * Outside of making the game function, it means very little right now in terms of user experience, but hopefully this changes in the future.
* If you were fortunate enough to buy the PC Collector's Edition of the game back when it was being sold and installed it on the system that you play TerrariaOGC on, new characters will be given some cabbage.
* If provided the XNA framework (Made for the Xbox 360), the auxillary XNA.XDK dependency, and an Xbox 360 Development Kit, you should still be able to run the initial version's code without too much hassle provided you use the `USE_ORIGINAL_CODE` configuration symbol.

# How to Build (Windows):
* See the instructions I have made for setting up on Windows [here](https://github.com/PPrism/TerrariaOGC/blob/main/WINBUILD.md).
* There is not yet direct support for other platforms as you would need to modify the GamerServices, Net, and base functions to support a non-Windows setup. I likely won't do this myself.
* For compilation, some configuration symbols are available upon building with Visual Studio 2022, which you can see below.

## List of configuration symbols:
* `USE_ORIGINAL_CODE`: *This requires an official Xbox 360 SDK setup.* If defined, the code should function on an official XDK setup. No additions are available if this is defined and only the initial version will be playable.
* `VERSION_INITIAL`: Whether or not to build the first release of 'Old-Gen' Terraria for consoles. This is the closest equivalent to Version 1.1.2 for the PC version.
* `VERSION_101`: Whether or not to build the 1.01 release of 'Old-Gen' Terraria for consoles. This is akin to Version 1.1.2 for the PC version, but with some new additions, some of which are from PC 1.2.
* `VERSION_103`: Whether or not to build the 1.03 release of 'Old-Gen' Terraria for consoles. This is the closest equivalent to Version 1.2.1.2 for the PC version.
* `VERSION_FINAL`: Whether or not to build the final release (1.09) of 'Old-Gen' Terraria for consoles. This is the closest equivalent to Version 1.2.4.1 for the PC version.
* `IS_PATCHED`: Whether or not to enable the patches that improved a few things on the initial release of 'Old-Gen' Terraria for consoles. Some were not available to the PS3 version until the 1.01 release. Only applicable if `VERSION_INITIAL` is defined.

# Special Thanks
* **Re-Logic**: They are first and foremost, since without these lot, I could not have experienced such an amazing and fun game, which in turn, makes Terraria a game I will definitely be enjoying in the years to come and as a result, it also made developing this project fun to work on as well.
* **505 Games & Engine Software**: These lot developed the console ports of Terraria, of which I have spent **a lot** of hours playing. Without them, a lot of what made the console versions feel iconic and different in comparison to the PC version would not exist.
* **The [FNA](https://fna-xna.github.io/) Community**: A major player that helped me kick-start this project, since software preservation is a real interest of mine and a project like FNA really piqued my interest due to the possibilities of XNA. The community has also been very helpful, both directly and indirectly, with any issues I had during development.

# Known Issues (for now):
Most of these are purely visual, but there are some that are functional. Currently, I cannot locate the issue for most of these.

* **Some light rendering may look odd in small instances (in other words, a few blocks), and this is mostly noticeable on 720p and 1080p.**
  * This is due to me needing to adjust the lighting code by hand for larger resolutions, since it still uses the initial lighting engine. 
* **The Wall of Flesh occasionally lacks mouth tracking and the wall itself can move with the player's Y-position at times.**
  * Some testing shows that it spawns with varying X-velocity to start with, leading to the mouth's AI to skip targeting.
* **Some flying enemies may spawn with increased velocity.**
* **In some instances, liquid may be magically replicated by causing it to flow.**
* **The HD Xbox glyphs are incomplete and only support the 4 buttons. Similarly, the HD glyphs may be inaccurate in some areas.**
  * I do not yet have the Xbox One game files so I had to make the button glyphs by hand. No chance am I making the rest by hand too.

# FAQ
### Q: What to do if I have a suggestion/bug report/question for TerrariaOGC? 
A: Feel free to make an issue or pull request, and I'll review it as soon as I can. If you need to contact me regarding something major for this project, you can do so with whatever contact I got on my GitHub profile or with Discord through the name: pprism.

### Q: Why does the game start in Trial Mode?
A: It's just a little thing to not promote piracy. I didn't make this project so it would be able to play the full game on the first run without any changes on your end. It can be changed, but you need to do this yourself.

### Q: How do I change the username used?
A: In the `Game` category of the Settings.ini file, change the value attached to the `Gamertag` key to whatever username you want.

### Q: How do I exit or enter full-screen mode?
A: Press 'F' on the keyboard at any time following the Engine Software logo to go into full-screen. You can also start-up in full-screen by turning on the respective setting in the Settings.ini file.

### Q: Only the 1.1 releases are playable, how do I get the 1.2 content?
A: 1.03 and 1.09 content will come in a later update to TerrariaOGC due to the updates being massive and more complex to recreate/decompile. Since I am currently the only person working on this, this will take a very long time, but I have already worked on stuff for those updates (some can be seen in the code already). If enough contributions are made through pull requests for 1.2 additions, I'll probably make a separate branch for it.
