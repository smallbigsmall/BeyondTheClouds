using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string Mission;
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
    public List<Dialogue> Dialogues;
}
