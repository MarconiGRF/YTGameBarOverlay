# Youtube GameBar Overlay
![Youtube GameBar Logo](https://github.com/MarconiGRF/YoutubeGameBarOverlay/blob/master/Assets/SplashScreen.scale-200.png)  
An extension developed on top of Windows' Xbox Game Bar SDK, aimed to be used as a pinned window while playing, making it possible to enjoy the video while gaming. 

## Quality ensurance
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/1b625ef3f7fb4f7182c781e5e6b2706d)](https://www.codacy.com/manual/MarconiGRF/YoutubeGameBarOverlay?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=MarconiGRF/YoutubeGameBarOverlay&amp;utm_campaign=Badge_Grade)

## Releases
* [Get on Microsoft Store](https://www.microsoft.com/store/productId/9NK7J3V4LZS6)
* [Github Release](https://github.com/MarconiGRF/YoutubeGameBarOverlay/releases)

## Setting up
* 1: Clone this repository.  
* 2: Clone the [VideoUI](https://github.com/MarconiGRF/YoutubeGameBarVideoUI) repository, it is the core of videos playback on this project.  
* 3: Build the VideoUI (Instructions on its readme), copy the content of `YoutubeGameBarVideoUI/dist/YoutubeGameBarVideoUI` inside `YoutubeGameBarOverlay/VideoUI`.  
* 4: Rename the .env_sample to .env and update the necessary environment variables, which currently are:
    *  YTGBFS_ADDRESS (The address of a running [YTGBFS](https://github.com/MarconiGRF/YoutubeGameBarFeedbackServer) instance.)
    *  YTGBFS_PORT (The port of the previous YTGBFS instance.)
    *  YTGBSS_ADDRESS= (The address of a running [YTGBSS](https://github.com/MarconiGRF/YoutubeGameBarSearchServer) instance.)
    *  YTGBSS_PORT= (The port of the previous YTGBFS instance.)
    *  YTGBWS_PORT= (The port of the Webserver contained in YTGBO)
* 5: Open the `YoutubeGameBarOverlay.sln` on Visual Studio.  
* 6: Add the VideoUI files to the project
    * On VS' Solution explorer, right click `VideoUI` Folder, then `Add...` > `Existing Item...`
    * Select all files inside VideoUI/
* 7: Update/install the NuGet Packages.  
* 8: You're ready to contribute!

## Development tips
### Debugging with Game Bar
Once that this overlay is intended to be used on Xbox Game Bar, and you're probably going to need debugging it inside Game Bar, the application should not be launched as a normal UWP App:  

* 1: With the solution opened, right-click the project `YoutubeGameBarOverlay (Universal Windows)` and go to `Properties` and then `Debug`.  
* 2: Check the `Do not launch, but debug my code when it starts` option.  
* 3: Save the Project Properties.  
* 4: Use the Visual Studio's `Build` Menu Bar option, then select `Deploy Solution.`  
* 5: Once the process of deploy is succeed, press F5 to start debugging on Visual Studio (notice that it will **not** start the Overlay automatically).  
* 6: Now go ahead to Xbox Game Bar (Windows + G), using the `Widget Menu` option select `Youtube GameBar Overlay`.  
* 7: Pin the window and go back to Visual Studio.  
* 8: Since step 7 Visual Studio should have already attached to the YTGBO's process and you are now able to debug it properly.

Everytime you change the code you'll need to redeploy to debug (Steps 4-7).

## Special Thanks
[Arthur Bryan](https://github.com/arthur-bryan) - For adding Spanish resource files.
