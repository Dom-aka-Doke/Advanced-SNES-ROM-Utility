# Advanced-SNES-ROM-Utility

An easy to use utility for performing some tasks on SNES / SFC ROMs!

## Functions

### Edit ROM Information
Change basic ROM information like title, country/region, version and game code

### Add Header
Adds an empty header

### Remove Header
Removes an existing header

### Fix Checksum
Fixes a broken checksum. You always should do this before saving, if possible. Please note that some beta or demo ROMs have an odd checksum anyway...

### Fix Internal ROM size
Fixes wrong internal ROM size information. Do this after expanding a ROM. Also useful to fix some ROM hacks

### Expand ROM
Expands ROM to a specific size up to 64 Mbit (BS-X up to 32 Mbit). Mirror option should only be used, when there are problems with standard expanding. Always check if your ROM still works after doing this!

### Split ROM
Splits ROM into equal sized multiple parts

### SwapBin ROM
Swaps binaries to get a "Close-to-SNES-Mask-ROM"-layout Use 27C801 EPROM(s) and rewire pins as followed:

      	   27C801 -> Nintendo Mask ROM
      	   	24     ->     31
      	   	31     ->     24


### Deinterleave
Some copy stations like "Game Doctor SF" or "Super UFO" use interleaving when dumping HiROM games. This function will reverse this process. Please use ROMs with copy station header, unless you know which copy station was used!

### Interleave
Some patches or copy stations require interleaved ROMs. This function will interleave your ROM. If you have interleaved a ROM, patched it and want to play it in an emulator or flashcard, don't forget to deinterleave and fix checksum again!
 - Note: Only LoROMs can be interleaved!

### Remove Region Locks
Some games have a copy protection preventing the ROM from running on systems with another region (PAL/NTSC lock). Use this function to remove most known region locks

### Remove SRAM Checks
Some games have a copy protection preventing the ROM from running, when cartridges have more SRAM than needed (copy stations/some cheap flashcards/deprecated emulators). Use this function to remove most known SRAM checks

### Remove Slow ROM Checks
Useful if you want to play your FastROMs on a copy station without FastROM support or burn them on (E)EPROMs slower than 120ns

### Convert HiROM <-> LoROM
Can switch ROM layout between HiROM and LoROM. If the ROM uses (S)RAM or some enhancement chips, you have to do some manual work after conversion
 - Note: Converting from HiROM to LoROM only works, if the ROM was designed to be convertible

### Apply Patch
Patch your loaded ROM!
 - IPS
   - RLE hunks
   - truncating
   - avoids EOF offset bug
 - UPS
   - revert a patch, if applied a second time 
 - BPS
 - BDF
 - XDELTA

### Options
- Miror ROM on expanding
  - Checked
    - ROMs' content gets mirrored when it gets expanded
  - Unchecked
    - Standard expanding will be perfomed to get more space available
- Scan protections on loading
  - Checked
    - ROM will be scanned instantly for copy protections on loading. This might take a few seconds
  - Unchecked
    - You have to manually click the Scan button to check for copy protections

### Commandline (CLI)
- Use Windows Commandline (CMD) for executing batch operations (or just for processing a single file, if you prefer using the commandline)
- Build your own command chain to execute operations in the order you specified!
  - Note: Deinterleaving and interleaving might require user action! This will bring up a windows form and can only work within a Windows environment!

__Example:__

*Advanced_SNES_ROM_Utility -path "C:\ROMs" -recursive -header add -patch "C:\Patches\patch.ips" -header remove -fixromsize -fixchecksum*

This command will add a header to all ROMs in C:\ROMs (subfolders included), apply an IPS patch, remove header afterwards, fix internal ROM size and checksum and save the modfied file with an extension

For more information type

*Advanced_SNES_ROM_Utility -path*

or take a look into the tools' manual

## Download
- RHDN: https://www.romhacking.net/utilities/1638/

## Support
Support my work and [become a Patreon](https://www.patreon.com/user?u=98965851) 
