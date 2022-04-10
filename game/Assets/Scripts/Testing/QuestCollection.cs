using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System;
using UnityEngine;
public class QuestCollection : MonoBehaviour
{
    public QuizQuestion[] allQuestions;
    bool isShufle = false;
    int questionIndex = 0;

    private void Start()
    {
        LoadAllQuestions();
    }

    private void LoadAllQuestions()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(QuizQuestion[]));
        using (StreamReader streamReader = new StreamReader(Path.Combine(Application.streamingAssetsPath, "Testing", "Questions.xml")))
        {
            allQuestions = (QuizQuestion[])serializer.Deserialize(streamReader);
        }
    }

    public QuizQuestion GetUnaskedQuestion()
    {
        ResetQuestionsIfAllHaveBeenAsked();
        if (!isShufle)
        {
            for (int k = 0; k < allQuestions.Length; k++)
            {
                QuizQuestion temp = allQuestions[k];
                int randomIndex = UnityEngine.Random.Range(0, allQuestions.Length);
                allQuestions[k] = allQuestions[randomIndex];
                allQuestions[randomIndex] = temp;
            }
            isShufle = true;
        }
        allQuestions[questionIndex].Asked = true;
        return allQuestions[questionIndex++];
    }

    private void ResetQuestionsIfAllHaveBeenAsked()
    {
        if (questionIndex == 0)
        {
            ResetQuestions();
        }
    }

    private void ResetQuestions()
    {   
        for (int i = 0; i < allQuestions.Length; i++)
        {
            string correct = null;
            if (allQuestions[i].Type == (int)AnswerType.Single)
            {
                correct = allQuestions[i].Answers[Int32.Parse(allQuestions[i].CorrectAnswer)];

            }
            string[] new_question = allQuestions[i].Answers;
            char[] new_seq_answer = allQuestions[i].CorrectAnswer.ToArray();

            for (int k = 0; k < new_question.Length; k++)
            {
                string temp = new_question[k];
                int randomIndex = UnityEngine.Random.Range(0, new_question.Length);
                new_question[k] = new_question[randomIndex];
                new_question[randomIndex] = temp;

                if (allQuestions[i].Type == (int)AnswerType.Sequence)
                {
                    char tmp = new_seq_answer[k];
                    new_seq_answer[k] = new_seq_answer[randomIndex];
                    new_seq_answer[randomIndex] = tmp;
                }

            }

            
            if (allQuestions[i].Type == (int)AnswerType.Sequence)
            {
                string tmp = null;
                foreach(char temp in new_seq_answer)
                {
                    tmp += temp;
                }
                allQuestions[i].CorrectAnswer = tmp;
            }
            else
            {
                for (int j = 0; j < new_question.Length; j++)
                {

                    if (new_question[j].Equals(correct))
                    {
                        allQuestions[i].CorrectAnswer = j.ToString();
                    }
                }
            }

            allQuestions[i].Answers = new_question;
            allQuestions[i].Asked = false;
        }
    }
}
