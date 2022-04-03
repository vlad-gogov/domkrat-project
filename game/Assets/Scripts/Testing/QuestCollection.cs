using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
public class QuestCollection : MonoBehaviour
{
    private QuizQuestion[] allQuestions;

    private void Awake()
    {
        Debug.Log(Path.GetFullPath(".\\Assets\\Scripts\\Testing\\Questions.xml"));
        
        if(!File.Exists(".\\Assets\\Scripts\\Testing\\Questions.xml") 
        {
            throw new System.Exception("Gde xml dayn");
        }

        LoadAllQuestions();
    }

    private void LoadAllQuestions()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(QuizQuestion[]));
        using (StreamReader streamReader = new StreamReader(".\\Assets\\Scripts\\Testing\\Questions.xml"))
        {
            allQuestions = (QuizQuestion[])serializer.Deserialize(streamReader);
        }
    }

    public QuizQuestion GetUnaskedQuestion()
    {
        ResetQuestionsIfAllHaveBeenAsked();

        var question = allQuestions
            .Where(t => t.Asked == false)
            .OrderBy(t => UnityEngine.Random.Range(0, int.MaxValue))
            .FirstOrDefault();

        question.Asked = true;
        return question;
    }

    private void ResetQuestionsIfAllHaveBeenAsked()
    {
        if (allQuestions.Any(t => t.Asked == false) == false)
        {
            ResetQuestions();
        }
    }

    private void ResetQuestions()
    {   
        for (int i = 0; i < allQuestions.Length; i++)
        {
            string correct = allQuestions[i].Answers[allQuestions[i].CorrectAnswer];
            string[] new_question = allQuestions[i].Answers;

            for (int k = 0; k < new_question.Length; k++)
            {
                string temp = new_question[i];
                int randomIndex = UnityEngine.Random.Range(0, new_question.Length);
                new_question[i] = new_question[randomIndex];
                new_question[randomIndex] = temp;
            }

            for(int j = 0; j < new_question.Length; j++)
            {
                if (new_question[j].Equals(correct))
                {
                    allQuestions[i].CorrectAnswer = j;
                }
            }

            allQuestions[i].Answers = new_question;
            allQuestions[i].Asked = false;
        }
    }
}
