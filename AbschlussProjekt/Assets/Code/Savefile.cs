using System;
using System.Collections.Generic;

[Serializable]
public class Savestate
{
    public List<Entity> OwnedCharacters;
    public Entity[] CurrentTeam;

    public int Gold;
    public int Souls;
    [NonSerialized] public int Test;

    public Entity[] InitializeTeam()
    {
        for(int index = 0; index < CurrentTeam.Length; index++)
            if(CurrentTeam[index] != null) CurrentTeam[index].Init();
        return CurrentTeam;
    }
}