using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

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

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void SetupUIForQuestion(QuizQuestion question)
    {
        correctAnswerPopup.SetActive(false);
        wrongAnswerPopup.SetActive(false);

        //questionText.transform.position = new Vector3(0, 375, 0);
        questionText.rectTransform.localPosition = new Vector2(0, 375);

        if (question.QuestionImage != "")
        {
            questionText.rectTransform.localPosition = new Vector2(-174, 220);
            questionImage.gameObject.SetActive(true);
            var txtr = new Texture2D(400, 400);
            txtr.LoadImage(File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, "Testing", question.QuestionImage)));
            questionImage.texture = txtr;
        }

        questionText.text = question.Question;

        for (int i = 0; i < question.Answers.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = question.Answers[i];
            answerButtons[i].gameObject.GetComponent<Image>().color = Color.white;
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
            endGame.text = "Вы не сдали. Количество ошибок = " + error_count.ToString();
        } else
        {
            endGame.text = "Вы сдали.";
        }
    }

    public void HandleSubmittedAnswer(bool isCorrect, int correctButton, int uncorrectButton)
    {
        ToggleAnswerButtons(false);
        ToggleImage(false);
            if (!isCorrect)
            {
                ShowWrongAnswerPopup(uncorrectButton);
            }
            ShowCorrectButton(correctButton);
    }

    public void HandleSubmittedAnswer(bool isCorrect, string correctButton, int[] uncorrectButton)
    {
        ToggleAnswerButtons(false);
        ToggleImage(false);
        ShowCorrectButtonSeq(correctButton, uncorrectButton);
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

    public void ShowCorrectButton(int correctButton)
    {
        answerButtons[correctButton].gameObject.SetActive(true);
        answerButtons[correctButton].gameObject.GetComponent<Image>().color = Color.green; 
    }
    public void ShowCorrectButtonSeq(string correctButton, int[] uncorrectButton)
    {
        Debug.Log("correct " + correctButton + " unn " + uncorrectButton[0] + uncorrectButton[1] + uncorrectButton[2]);
        for (int i = 0; i < correctButton.Length; i++)
        {
            if (uncorrectButton[i].ToString()[0].Equals(correctButton[i]))
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].gameObject.GetComponent<Image>().color = Color.green;
            } else
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].gameObject.GetComponent<Image>().color = Color.red;
            }
        }
        
    }

    private void ShowCorrectAnswerPopup(int correctButton)
    {
        
        //correctAnswerPopup.SetActive(true);
    }

    private void ShowWrongAnswerPopup(int uncorrectButton)
    {
        answerButtons[uncorrectButton].gameObject.SetActive(true);
        answerButtons[uncorrectButton].gameObject.GetComponent<Image>().color = Color.red;
        //wrongAnswerPopup.SetActive(true);
    }
}
