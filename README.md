# Advanced-SNES-ROM-Utility

Functions

- Add Header: Adds an empty header

- Remove Header: Removes an existing header

- Edit/Fix ROM Information: Change title, country/region, version and fix wrong internal ROM size

- SwapBin ROM: Swaps binaries to get a "Close-to-SNES-Mask-ROM"-layout Use 27C801 EPROM(s) and rewire pins as followed:

      	   27C801 -> Nintendo Mask ROM
      	   	24     ->     31
      	   	31     ->     24

- Expand ROM: Expands ROM to a specific EPROM size for burning

- Split ROM: Splits ROM into multiple parts for burning on specific EPROMs

- Deinterleave: Some copy stations like "Game Doctor SF" or "Super UFO" use interleaving when dumping HiROM games. This function will reverse this process. Please use ROMs with copy station header, unless you know which copy station was used!

- Fix Checksum: Fixes a broken checksum. You always should do this after using other functionalities. Please note that some beta or demo ROMs have an odd checksum anyway.

- Region Unlock: Some games have a copy protection preventing the ROM from running on systems with another region (PAL/NTSC lock). Use this function to remove most known region locks.

- Fix ROM size: Fixes a wrong internal ROM size.
