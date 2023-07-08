using System.ComponentModel;

enum Speed : byte
{
    [Description("SlowROM (200 ns)")]
    slow = 0x20,

    [Description("FastROM (120 ns)")]
    fast = 0x30
}