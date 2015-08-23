TiledGGD - basically it's GGD (General Graphical Dump tool), with some extra features:

### Currently working (extra) features: ###
  * Support for tiled graphics (tiles of any size) _and palettes_ (tile sizes are restricted, because they need to fit a 16\*16 grid)
  * Copy visible graphics and palette directly to clipboard
  * 'Go To Offset' functionality
  * Toggle Endianness of graphics and palette separately
  * Save the entire graphics
  * Built-in NCGR/NCBR and NCLR support (I'm not using all the data from the files, so there are most likely still some bugs)
  * **Lua plugin support**

### Features currently being worked on ###
  * Support for files compressed via the default DS/GBA algorithms

### Planned features ###
  * Palette edit function
  * Support for planar & composite pixel formats
  * Different layout options, including a scrollable graphics window when the graphics are too large for it.
  * Support for tile maps


## Notice ##
Because of the used Lua interpreter, you will probably need the most recent VisualC++ runtime libraries to use the Lua plugins. You can download them [here](http://www.microsoft.com/download/en/details.aspx?id=5555)

## Wanted! ##

The program still has the default logo at the About-window. Therefore I need someone to make me a logo for the About-screen and these pages.