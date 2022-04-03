using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum AnswerType
{
    Single = 0,
    Sequence = 1
}

[CreateAssetMenuAttribute]
public class QuizQuestion : ScriptableObject
{
    public int Type { get; set; }
    public string Question { get; set; }
    public string QuestionImage { get; set; }
    public string[] Answers { get; set; }
    public string CorrectAnswer { get; set; }

    public bool Asked { get; set; }
}
