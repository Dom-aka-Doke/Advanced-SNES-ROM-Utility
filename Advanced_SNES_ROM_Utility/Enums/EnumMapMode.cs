using System.ComponentModel;

enum MapMode : byte
{
    bitmask = 0x37,

    // All LoROMs must contain this text in description!
    [Description("LoROM")]
    lorom_1 = 0x20,

    // All HiROMs must contain this text in description!
    [Description("HiROM")]
    hirom_1 = 0x21,

    [Description("LoROM (SDD-1)")]
    lorom_sdd1 = 0x22,
    
    [Description("LoROM (SA-1)")]
    lorom_sa1_1 = 0x23,

    [Description("ExHiROM")]
    exhirom_1 = 0x25,

    [Description("LoROM")]
    lorom_2 = 0x30,

    [Description("HiROM")]
    hirom_2 = 0x31,

    [Description("ExLoROM")]
    exlorom = 0x32,

    [Description("LoROM (SA-1)")]
    lorom_sa1_2 = 0x33,

    [Description("ExHiROM")]
    exhirom_2 = 0x35,

    [Description("HiROM (SPC7110)")]
    hirom_spc7110 = 0x3A
}