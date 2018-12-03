using System;
using System.Collections.Generic;

[Serializable]
public class Savefile
{
    public List<Entity> OwnedCharacters;
    public Entity[] CurrentTeam;

    public int Gold;
    public int Souls;
    [NonSerialized] public int Test;
}