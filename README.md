# Advanced-SNES-ROM-Utility

An easy to use utility for performing some tasks on SNES / SFC ROMs!

## Functions

### Edit Header Information
Change basic header information like title, company, country/region, version, game code and (S)RAM size.
- **Title:** Japanese (Shift-JIS) characters are supported. The title is limited by bytes, not by characters (21 bytes for most ROMs). A double byte character is never cut in half.
- **Game Code:** only available for ROMs with an extended header; only `A-Z` and `0-9` are accepted.

### Change Company
- **Old header:** one byte at `$FFDA` – only companies with a one byte code can be chosen.
- **Extended header** (`$FFDA` = `0x33`): a two character maker code at `$FFB0`.

The tool converts between both formats automatically (e.g. `01` = Nintendo, `A4` = Konami).

### Change (S)RAM Size
Choose the size of the battery backed SRAM (none, 2 kByte … 128 kByte). Normal ROMs store it at `$FFD8`, Super FX games with an extended header at `$FFBD`. Early Super FX games without an extended header (e.g. **Star Fox / Star Wing**) have no field for it – here the option is locked.

### Add Header
Adds an empty header.

### Remove Header
Removes an existing header.

### Fix Checksum
Fixes a broken checksum. You always should do this before saving, if possible. Please note that some beta or demo ROMs have an odd checksum anyway...

The light next to **CHECKSUMS** shows the state: green = internal checksum is correct, red = it doesn't match.

### Fix Internal ROM size
Fixes wrong internal ROM size information. Do this after expanding a ROM or removing an overdump. Also useful to fix some ROM hacks.

### Expand ROM
Expands ROM to a specific size up to 64 Mbit (BS-X up to 32 Mbit). Choose how the new space is filled with **Expand fill**:

| Expand fill | Use it for |
|---|---|
| **Empty (0x00)** | ROM hacks – free space for your own code and data (default) |
| **Empty (0xFF)** | Same, but filled like an erased (E)EPROM / flash chip |
| **Mirror data** | Writing a smaller game to a larger chip, e.g. an 8 Mbit game to a 16 Mbit EPROM |

- *Mirror data* follows the address decoding of the SNES, so it also works for games whose size is no power of two. Target sizes that cannot be mirrored correctly (12, 20, 24, 28 Mbit) are not offered.
- **48 / 64 Mbit:** LoROM and HiROM games are converted to ExLoROM / ExHiROM; the map mode is changed and the parts the SNES reads from the upper area are copied. Games with enhancement chips (SA-1, S-DD1, SPC7110, Super FX) and interleaved ROMs only get free space.

> **Note**<br>
> Expanding doesn't change the header for you – use **Fix Internal ROM Size** and **Fix Checksum** afterwards. Always check if your ROM still works after doing this!

### Split ROM
Splits ROM into equal sized multiple parts.

### SwapBin ROM (27C801)
Swaps binaries to get a *"Close-to-SNES-Mask-ROM"*-layout. <br>
Use 27C801 (E)EPROM and rewire pins as followed:

|  27C801       | Nintendo Mask ROM |
|:-------------:|:-----------------:|
| 24            | 31                |
| 31            | 24                |

### Deinterleave
Some copy stations like "Game Doctor SF" or "Super UFO" use interleaving when dumping HiROM games. This function will reverse this process. Please use ROMs with copy station header, unless you know which copy station was used!

### Interleave
Some patches or copy stations require interleaved HiROMs. This function will interleave your HiROM. If you have interleaved a ROM, patched it and want to play it in an emulator, flashcard or make a physical copy, don't forget to deinterleave and fix checksum again!
> **Note**<br>
Only HiROMs can be interleaved!

### Remove Region Locks
Some games have a copy protection preventing the ROM from running on systems with another region (PAL/NTSC lock). Use this function to remove most known region locks.

### Remove SRAM Checks
Some games have a copy protection preventing the ROM from running, when cartridges have more SRAM than needed (copy stations/some cheap flashcards/deprecated emulators). Use this function to remove most known SRAM checks.

### Remove SlowROM Checks
Useful if you want to play your FastROM on a copy station without FastROM support or make a physical copy using an (E)EPROM/FlashROM slower than 120ns.

### Convert HiROM <-> LoROM
Can switch ROM layout between HiROM and LoROM. If the ROM uses (S)RAM or some enhancement chips, you have to do some manual work after conversion.
> **Note**<br>
Converting from HiROM to LoROM only works, if the ROM was designed to be convertible

### Remove Overdump
Detects and removes data beyond the real ROM size:
- **Padded data** – the file is larger than its internal size and the rest only contains `0x00` or `0xFF`
- **Mirrored data** – the end of the file repeats data from before, exactly as the SNES would see it on a smaller chip

### ROM Heatmap
Shows all ROM banks (32 kByte LoROM / 64 kByte HiROM) as a map: unique, identical and empty banks, or the entropy of each bank. Useful to spot overdumps, free space for ROM hacks or bad dumps.

### Apply Patch
Patch your loaded ROM!
 - IPS
   - RLE hunks
   - truncating
   - avoids EOF offset bug
 - UPS
   - revert a patch, if applied a second time (the result is verified)
 - BPS
 - BDF
 - XDELTA
> **Note**<br>
> If you have a __BDF__ patch file with __.bdiff__ extension, try to change this into __.bdf__ <br>
> Also try renaming __XDELTA__ patches ending to __.vcdiff__ into __.xdelta__

### Scan
- Scan/Rescan ROM for copy protections.

### Open and Save
- Open ROMs with the button, *File → Open*, by dragging a file onto the window or by starting the program with a ROM file ("Open with")
- ROMs from 32 kByte (homebrew and test ROMs) up to 16 MByte can be loaded
- The open dialog starts in the folder of the last opened ROM
- When closing the window or opening another ROM with unsaved changes, the tool asks whether to save them

### Options
- Expand fill
  - How new space is filled when expanding a ROM (see Expand ROM)
- Scan protections on loading
  - Checked
    - ROM will be scanned instantly for copy protections on loading. This might take a few seconds
  - Unchecked
    - You have to manually click the Scan button to check for copy protections

### Commandline (CLI)
- Use Windows Commandline (CMD) for executing batch operations (or just for processing a single file, if you prefer using the commandline)
- Build your own command chain to execute operations in the order you specified!
> **Note**<br>
Deinterleaving and interleaving might require user action! This will bring up a windows form and can only work within a Windows environment!

__Example:__

*Advanced_SNES_ROM_Utility.exe -path "C:\ROMs" -recursive -header add -patch "C:\Patches\patch.ips" -header remove -fixromsize -fixchecksum*

This command will add a header to all ROMs in C:\ROMs (subfolders included), apply an IPS patch, remove header afterwards, fix internal ROM size and checksum and save the modfied file with an extension.

New options: `-removeoverdump` (removes mirrored or padded data), `-company <CODE>` (e.g. `01` = Nintendo, `A4` = Konami) and `-sram <0-7>` (`0` = none, `n` = 2^n kByte).

For more information type

*Advanced_SNES_ROM_Utility.exe -help*

or take a look into the tool's manual.

## Some notes about BS-X ROMs
- checksum doesn’t include header information (it won't change if you edit values)
- There's no region code (JP only)
- ROM size is limited to a maximum of 32 Mbit

## Other platforms
A port for **Windows, macOS and Linux** (JavaScript/Tauri) with the same functions is available as *Advanced SNES ROM Utility JS*.

## Download
- [Get it on RHDN](https://www.romhacking.net/utilities/1638)
