using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Missions
{
    public string Mission;
    public List<Dialogue> Dialogue;
}

[System.Serializable]
public class Dialogue
{
    public int Day;
    public int Index;
    public List<Line> Lines;
    public string Quest;
}

[System.Serializable]
public class Line {
    public string line;
}

[System.Serializable]
public class DialogueList {
    public string Name;
    public List<Missions> Missions;
}


