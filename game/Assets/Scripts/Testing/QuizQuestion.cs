using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenuAttribute]
public class QuizQuestion : ScriptableObject
{
    public string Question { get; set; }
    public string[] Answers { get; set; }
    public int CorrectAnswer { get; set; }

    public bool Asked { get; set; }
}
