using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Text questionText;
    [SerializeField]
    private Button[] answerButtons;

    [SerializeField]
    private GameObject correctAnswerPopup;
    [SerializeField]
    private GameObject wrongAnswerPopup;

    public void SetupUIForQuestion(QuizQuestion question)
    {
        correctAnswerPopup.SetActive(false);
        wrongAnswerPopup.SetActive(false);

        questionText.text = question.Question;

        for (int i = 0; i < question.Answers.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = question.Answers[i];
            answerButtons[i].gameObject.SetActive(true);
            //answerButtons[i].GetComponent<RectTransform>()
        }

        for (int i = question.Answers.Length; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(false);
        }
    }

    public void HandleSubmittedAnswer(bool isCorrect)
    {
        ToggleAnswerButtons(false);
        if (isCorrect)
        {
            ShowCorrectAnswerPopup();
        }
        else
        {
            ShowWrongAnswerPopup();
        }
    }

    private void ToggleAnswerButtons(bool value)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(value);
        }
    }

    private void ShowCorrectAnswerPopup()
    {
        correctAnswerPopup.SetActive(true);
    }

    private void ShowWrongAnswerPopup()
    {
        wrongAnswerPopup.SetActive(true);
    }
}
