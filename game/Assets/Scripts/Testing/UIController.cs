using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Text endGame;
    [SerializeField]
    private Text questionText;
    [SerializeField]
    private RawImage questionImage;
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

        //questionText.transform.position = new Vector3(0, 375, 0);
        questionText.rectTransform.localPosition = new Vector2(0, 375);

        if (question.QuestionImage != "")
        {
            questionText.rectTransform.localPosition = new Vector2(0, 500);
            questionImage.gameObject.SetActive(true);
            var txtr = new Texture2D(400, 400);
            txtr.LoadImage(File.ReadAllBytes(question.QuestionImage));
            questionImage.texture = txtr;
        }

        questionText.text = question.Question;

        for (int i = 0; i < question.Answers.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = question.Answers[i];
            answerButtons[i].gameObject.SetActive(true);
        }

        for (int i = question.Answers.Length; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(false);
        }
    }

    public void SetupUIForEnd(int error_count)
    {
        ToggleAnswerButtons(false);
        ToggleImage(false);
        ToggleQuestionText(false);
        correctAnswerPopup.SetActive(false);
        wrongAnswerPopup.SetActive(false);
        endGame.gameObject.SetActive(true);
        if (error_count != 0)
        {
            endGame.text = "?? ?? ?????. ?????????? ?????? = " + error_count.ToString();
        } else
        {
            endGame.text = "?? ?????.";
        }
    }

    public void HandleSubmittedAnswer(bool isCorrect)
    {
        ToggleAnswerButtons(false);
        ToggleImage(false);
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

    private void ToggleQuestionText(bool value)
    {
        questionText.gameObject.SetActive(value);
    }

    private void ToggleImage(bool value)
    {
        questionImage.gameObject.SetActive(value);
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
