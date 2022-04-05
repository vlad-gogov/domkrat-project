using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class QuizController : MonoBehaviour
{
    private QuestCollection questionCollection;
    private QuizQuestion currentQuestion;
    private UIController uiController;

    private int currentSeqLenght = 0;
    private int errors_cout = 0;
    private int count_question = 0;
    private int[] answer;

    [SerializeField]
    private float delayBetweenQuestions = 2f;

    private void Awake()
    {
        questionCollection = FindObjectOfType<QuestCollection>();
        uiController = FindObjectOfType<UIController>();
    }

    private void Start()
    {
        PresentQuestion();
    }

    private void PresentQuestion()
    {
        count_question++;
        currentQuestion = questionCollection.GetUnaskedQuestion();
        uiController.SetupUIForQuestion(currentQuestion);
        answer = new int[currentQuestion.Answers.Length];
    }

    private void EndTest()
    {
        uiController.SetupUIForEnd(errors_cout);
    }

    public void SubmitAnswer(int answerNumber)
    {
        if (currentQuestion.Type == (int)AnswerType.Single)
        {
            bool isCorrect = answerNumber.ToString().Equals(currentQuestion.CorrectAnswer);
            if (!isCorrect)
            {
                errors_cout++;
            } 
            uiController.HandleSubmittedAnswer(isCorrect, Int32.Parse(currentQuestion.CorrectAnswer), answerNumber);

            StartCoroutine(ShowNextQuestionAfterDelay());
        } else
        {
            answer[currentSeqLenght] = answerNumber;
            currentSeqLenght++;
            if (currentSeqLenght == currentQuestion.Answers.Length)
            {
                bool isCorrect = true;
                for (int i = 0; i < answer.Length; i++)
                {
                    if (!answer[i].ToString()[0].Equals(currentQuestion.CorrectAnswer[i]))
                    {
                        isCorrect = false;
                    }
                }
                if (!isCorrect)
                {
                    errors_cout++;
                }
                uiController.HandleSubmittedAnswer(isCorrect, currentQuestion.CorrectAnswer, answer);

                StartCoroutine(ShowNextQuestionAfterDelay());
            }
        }

    }

    private IEnumerator ShowNextQuestionAfterDelay()
    {
        currentSeqLenght = 0;
        answer = null;
        //uiController.ShowCorrectButton(Int32.Parse(currentQuestion.CorrectAnswer));
        yield return new WaitForSeconds(delayBetweenQuestions);
        if (count_question == questionCollection.allQuestions.Length)
        {
            EndTest();
        } else
        {
            PresentQuestion();
        }
    }
}
