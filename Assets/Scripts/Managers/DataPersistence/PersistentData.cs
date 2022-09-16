using System;
using System.Collections.Generic;

[Serializable]
public class PersistentData
{
    public Player.Data player;
    public Dictionary<string, bool> bools = new();
}