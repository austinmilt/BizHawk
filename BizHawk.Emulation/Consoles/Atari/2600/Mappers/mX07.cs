﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizHawk.Emulation.Consoles.Atari._2600
{
	/*
	X07 (Atariage)
	-----

	Apparently, this was only used on one cart: Stella's Stocking.
	Similar to EF, there are 16 4K banks, for a total of up to 64K of ROM.  

	The addresses to select banks is below the ROM area, however.

	The following TWO masks are used:

	A13           A0
	----------------
	0 1xxx nnnn 1101

	This means the address 80B selects bank 0, 81B selects bank 1, etc.

	In addition to this, there is another way:

	A13           A0
	----------------
	0 0xxx 0nxx xxxx

	This is somewhat special purpose:  Accessing here does nothing, unless one of the
	last two banks are selected (banks 14 or 15).  In that case, the new bank is:

	111n   i.e. accessing 0000 will select bank 14 (Eh, 1110b) while accessing 0040
	will select bank 15 (Fh, 1111b).  This allows for bankswitching by accessing 
	TIA registers at 00-3F or 40-7F without incurring any overhead.
	*/

	class mX07 : MapperBase
	{

	}
}
