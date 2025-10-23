using UnityEngine;

public static class ScenarioProgress
{
    //  Seatbelt = true  →  Fasten button circle
    //  Seatbelt = false →  Ignore button circle
    public static bool seenSeatbeltFast = false;
    public static bool seenSeatbeltSlow = false;
    public static bool seenNoSeatbeltFast = false;
    public static bool seenNoSeatbeltSlow = false;

    public static void ResetProgress()
    {
        seenSeatbeltFast = false;
        seenSeatbeltSlow = false;
        seenNoSeatbeltFast = false;
        seenNoSeatbeltSlow = false;
    }
}
