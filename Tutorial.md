

# Tutorial #

This page will explain how to use the program, and explain some of the features in a bit more detail.


## 1. Program Basiscs ##

If you've ripped sprites with GGD before, you can probably skip this part if you take a look at the shortcuts-list (About->Shortcuts...).

### 1.1 The window layout ###
I will explain the layout of the window first;

<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/2_empty_marked.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/2s_empty_marked.png' /></a>

If you know what GGD looks like, this will probably look a bit familiar;
  * The red area is where you will look most of the time; it's the Graphics Panel. This is where you'll have to look for sprites.
  * The green area is the Palette Panel; this will determine what colours are used in the Graphics Panel most of the time.
  * The blue area is the Data Panel, displaying data about the Graphics and Palette Panels.

### 1.2 Loading data ###

Now we're ready to rip some sprites. Obviously, we'll need some data to get the sprites from first. In this part of the tutorial, I will use examples from the DS game Luminous Arc.
There are two ways to load a file;
  * One is by using the File->Open or File->Open Palette menus,
  * The other by simply dragging the file on either the Graphics or Palette panel, loading it as the respective type of data.
Note that loading the first file will automatically load is as the other type as well. This is so that you have at least something to look at in the Graphics Panel.

We have no idea what's in what file, so let's just open the first one; 1.iear.

<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/3_1iear.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/3s_1iear.png' /></a>

### 1.3 Browse the Palette ###

When we look in the Data Panel, we see that the image format is 4 bits/pixel. This means only the top row of the palette is used to colour the Graphics Panel. Because it's mostly black, we'll scroll down a bit, using the Home and End keys.<br>When you do, you'll notice the palette only scrolling one colour at a time. This is where the Skip Size comes in. You can set it using the Palette->Skip Size menu, or toggle it with Shift+Z. Each time you press Home or End, the Palette Panel will go forward or backward in the data with the amount specified by the Skip Size.<br>
<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/4_palss.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/4s_palss.png' /></a>

You should scroll down until you either reach a palette, or until the currently used palette (in this case the top 16 colours) aren't that similar any more.<br>
<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/5_palscr.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/5s_palscr.png' /></a>

<h3>1.4 Resizing the Graphics Panel</h3>

We've seen that the file is quite large (18 MB), and the current window is quite small, so let's enlarge it, using the arrow keys;<br>
<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/6_largegr.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/6s_largegr.png' /></a>

If you find the enlarging/shrinking goes too fast or too slow, or you need your panel to have a width it cannot get to now (you can see the current size in the Data Panel), you can use the Width Skip Size and Height Skip Size menu's in the Graphics-menu;<br>
<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/7_grwss.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/7s_grwss.png' /></a> <a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/8_grhss.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/8s_grhss.png' /></a>

<h3>1.5 Browsing the Graphics</h3>

Browsing the Graphics is practically the same as browsing the Palette, only you use PageUp and PageDown instead. There is also a Skip Size menu in the Image menu, from which the alternative is Ctrl+Z;<br>
<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/9_grss.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/9s_grss.png' /></a>

If you've browsed through the whole file; you've probably found nothing worth looking at. A closer inspection at the contents of the file, using among other things a HEX editor, reveals that the file contains only sound data, so it's no wonder there was nothing.<br />
Since the next couple of files also contain no graphics, we'll skip ahead to the first file that does; 5.iear.<br>
<br>
<h2>2. Ripping raw data</h2>

When you load the file 5.iear, it's hard to say there is no sprite data inside (after you've changed the width a bit, if necessary);<br>
<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/10_5iear.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/10s_5iear.png' /></a>

<h3>2.1 Determining the Graphics Format</h3>

However, the sprites really don't look like anything yet. Mainly because the is no correct palette, but also because there are vertical lines through them.<br />
The latter generally indicates the graphics format is incorrect. You can set it using Image->Format or by using the B key to toggle it;<br />
(I've already browsed to the correct palette, as you can see)<br>
<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/11_grform.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/11s_grform.png' /></a>

Vertical lines every other column usually means pressing B once should be enough;<br>
<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/12_8bpp.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/12s_8bpp.png' /></a>

The Graphics width is too wide, so the sprites are spread out over the columns. Just resize the panel (and scroll a few bytes) to see the sprites correctly;<br>
<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/13_corr.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/13s_corr.png' /></a>

<h3>2.2 Getting the sprites out of the data</h3>

There are several ways to get the sprites out of the data. By far the easiest is to simply copy the Graphics Panel contents to the clipboard using Ctrl+C. You can also use the menu version; Image->Copy to clipboard.<br />
The same goes for the palette; you can copy it (as an image) to the clipboard using Ctrl+Shift+C, or the menu version Palette->Copy to clipboard.<br>
<br>
You can also save the graphics and/or palette to an image using Ctrl+S or Ctrl+Shift+S respectively. Of course there are also the menu versions File->Save Graphics... and File->Save Palette.... Currently the only possible output is a PNG file. Also, this is the only way to properly copy transparency, as the data sent to the clipboard doesn't handle it properly, if at all.<br>
<br>
<h3>2.3 Handling tiled graphics</h3>

If you look below the plainly visible sprites, you'll see some data that could also be sprites. It could pay off to investigate;<br>
<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/14_tiled1.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/14s_tiled1.png' /></a> <a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/15_tiled2.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/15s_tiled2.png' /></a>

At a width of 8 pixels, we can barely see that the data is indeed more sprites. This is where the Graphics Mode comes in. Increase the width again and press the F key to toggle the Graphics Mode (or use the menu: Image->Mode);<br>
<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/16_tiled3.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/16s_tiled3.png' /></a>

It looks a bit garbled again, but it's nothing some scrolling won't fix;<br>
<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/17_tiled4.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/17s_tiled4.png' /></a>

Unfortunately, it seems this game has two copies of almost every sprite saved; one 'Linear' and one 'Tiled' version, and these sprites are just a copy of the ones we saw before.<br>
<br>
<h2>3. Ripping NCGR/NCBR's and NCLR's</h2>

One of the future features of TiledGGD is the support of plugins, so that you can extract only specific data from the files instead of all the data. The built-in support for NCGR/NCBR and NCLR files is basically an internal plug-in, since it will ignore the header data of the files and only view the data that can possible contain sprite data.<br>
An example: (left is without the use of the support, right with it)<br>
<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/18_ninfiles1.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/18s_ninfiles1.png' /></a> <a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/19_ninfiles2.png'> <img src='http://img.photobucket.com/albums/v175/barubary/tggdtut/19s_ninfiles2.png' /></a>

(You may have noticed that this looks like data in 05.iear. This is because it <i>is</i>. The iear files are archives, which I unpacked. I will not explain how here, since it is not what this tutorial (nor program) is about. There are lots of other DS games that use the same default Nintendo image format, which produce similar results)<br>
<br>
A word of warning, though. Not every NCGR/NCBR file has the proper width and height stored in its file. Therefore it is usually wise to check if there is still more in the file than  currently shown by increasing the panel height a few pixels.<br>
<br>
Also, if you want to see the graphics loaded without the built-in plugin, use the menus Image/Palette ->Reload as;<br>
<ul><li>'Reload as->Generic data' reloads the data without the plugin<br>
</li><li>'Reload as->Specific data' reloads the data with the plugin</li></ul>


<h2>4. Miscellaneous features</h2>

<h3>4.1 Palette Format</h3>

Most games save their palette in a 2 Bytes per colour format. However, some use 3 or 4 bytes per colour. In those cases, use the O key to toggle between the different palette formats, or use the menu Palette->Format.<br>
<br>
<h3>4.2 Palette Order</h3>

As with the palette format, the palette order of a palette is generally B->G->R. Should this happen to be different, use the P key or the menu Palette->Colour order to change the order.<br>
<br>
<h3>4.3 Alpha Settings</h3>

With some games, the palette also contains data about the transparency. Usually, this data is stored before the colour data, but not always. Use the menu Palette->Alpha Settings... to change some settings.<br>
<br>
<h4>4.3.1 Alpha location</h4>

This is where the alpha value is located. 'Start' implies the alpha value is stored in the Most Significant Byte(s), 'End' in the Least Significant Byte(s).<br>
<br>
<h4>4.3.2 Alpha Stretch</h4>

Some formats do not use the full range of 0 - 255 for alpha values. Use this to stretch the used range back to 0 - 255. It at all necessary, the range actually used is generally 0 - 128.<br>
<br>
<h4>4.3.3 Ignore Alpha</h4>

This lets you ignore the alpha value of colours. If enabled, all colours will be fully opaque, regardless of the alpha value.<br>
<br>
<h3>4.4 Go to Offset</h3>

Both the Graphics Panel as the Palette Panel have the option to go to a specific offset. This can either be useful if you know some image data starts at that offset for a set of files (until the plug-ins are working), or when you've written down some offsets for later use/got them from someone else. You can use the menus Image/Palette->Go to offset... or Ctrl+(Shift+)G.<br>
You can enter the offset as either a decimal or hexadecimal number, and you can also choose to treat your current offset as zero (making the number you enter how much you skip forward).<br>
<br>
<h3>4.5 Custom tile size</h3>

Generally, when an image is tiled, the tile size is 8x8. However, as always, there are some exceptions. I've seen tiles of 16x16 and 32x32, but also of 16x8 and 32x8, but almost any dimension is possible. Use Ctrl+T or the menu Image->Set Tile Size... to use this functionality.<br>
<br>
<h4>4.5.1 Palette tiling</h4>

In ever rarer cases, the palette is also tiled. In general, that tile size will be 8x2. Use Ctrl+Shift+T to change the tile size (or the menu Palette->Set Tile Size...), and Shift+F to toggle between a tiled and linear palette (or the menu items in Palette->Mode).<br>
<a href='http://img.photobucket.com/albums/v175/barubary/tggdtut/19_tiledpal.png'>This is an example</a> of a tiled palette, as found in Persona 3 and 4 Bust-ups. The left version is the linear palette, the right one is how it shows up with the tiled mode with a tile size of 8 x 2.<br>
<br>
<h3>4.6 Zooming</h3>

You can zoom up to 800%, using either the NumPad + and -, or the menu Image->Zoom.<br>
<br>
<h3>4.7 Endianness</h3>

99 out of 100 (if not more) of the games have their data stored BigEndian; which means that the bytes that provide the most information are stored at the end of a set of bytes. If a game happens to use a LittleEndian approach, you can use Ctrl+(Shift+)E to toggle the Endianness of the graphics and/or the palette. Of course, you can also use the menus Image/Palette->Endianness<br>
<br>
<h3>4.8 Reloading the Bindings</h3>

If you make an adjustment to the XML file defining the bindings, you can either reload the program for the changes to take effect, but you can also use the menu item File->Reload Bindings.<br>
Plug-ins do not need to be reloaded manually; they are re-read every time a new file using that plug-in is opened.<br>
See the PluginTutorial for a turorial of some sorts for the bindings and plug-ins.