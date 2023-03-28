# Screensaver

This is a simple animated screensaver.

The Nuke build outputs a single .scr executable, a resources folder and a text file.

The screensaver will play a file called "base.mkv" within the resources folder in a 5 second loop. 
Randomly, with a chance of 20%, the program will switch to a different animation within the resources folder.

The file "tips.txt" contains loading screen hints, which are displayed on top of the videos in the bottom left corner.

During Debug, the screensaver is displayed in a 1920x1080 single window, but if "/s" is called as a starting argument (default for windows), the main window expands to fullscreen size on the main monitor. All other monitors are covered with a fully black window.
