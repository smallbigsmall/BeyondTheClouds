using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SttResult
{
    [System.Serializable]
    public class Token
    {
        public float confidence;
        public int end;
        public int start;
        public string token;
    }

    [System.Serializable]
    public class Speech
    {
        public float confidence;
        public List<Token> tokens;
    }

    [System.Serializable]
    public class Transcription
    {
        public bool is_final;
        public Speech speech;
        public string text;
        public string type;
    }
}
