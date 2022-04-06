using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum NameState
{
    DEFAULT = 0,
    SET_PEREHODNICK = 1,
    CHECK_DOMKRATS = 2,
    SET_DOMKRATS = 3,
    CHECK_TURING_MACHANISM = 4,
    UP_TPK = 5,
    CHECK_BREAK_MECHANISM = 6,
    // Flat
    MOVE_TPK_FLAT = 7, // Перемещение в точку
    // Up
    MOVE_TPK_UP = 8,
    MOVE_TPK_DOWN = 12,
    // Down
    CHECK_CONFIG = 9,
    SET_TORMOZ = 10,
    ROLLING_TPK = 11
}

public struct State
{
    public NameState state;
    public string disctiption;
}

public enum GameMode
{
    TRAIN = 0,
    EXAM = 1,
    FREEPLAY = 2,
}

public enum TypeArea
{
    FLAT = 0,
    UP = 1,
    DOWN = 2
}

public enum ErrorWeight
{
    MINOR = 0,
    LOW = 1,
    MEDIUM = 2,
    HIGH = 3,
    CRITICAL = 100
}

public struct Error
{
    public string ErrorText;
    public ErrorWeight Weight;
}

//Singleton.Instance.StateManager.onError(new Error() { ErrorText = "WORK", Weight = 0 });


public class StateManager : MonoBehaviour
{
    [SerializeField] ErrorMessage errorMessage;

    public GameMode gameMode;
    public TypeArea typeArea;
    public int counterMistaks = 0;
    public GameObject player;

    private List<Domkrat> domkrats = new List<Domkrat>();

    public bool OnPause { get; private set; }
    private List<State> states = new List<State>();
    Dictionary<NameState, string> tutorials = new Dictionary<NameState, string>();
    private int indexCurState;
    // Переменная, проверяющая завершили ли мы сценарий
    bool finished = false;

    public int countPerehodnick = 0;
    public int countDomkrats = 0;

    public bool isTuringMech = false;
    public int ruchkaIsUp = 0;

    void Awake()
    {
        GameObject[] ObjDomkrats = GameObject.FindGameObjectsWithTag("Domkrat");

        foreach (GameObject obj in ObjDomkrats)
        {
            domkrats.Add(obj.GetComponent<Domkrat>());
            obj.SetActive(false);
        }
    }

    void Start()
    {
        gameMode = CrossScenesStorage.gameMode;
        // typeArea = CrossScenesStorage.typeArea;
        Debug.Log($"Current mode is: {gameMode} | {typeArea}");

        states.Add(new State() { state = NameState.DEFAULT, disctiption = "" });
        states.Add(new State() { state = NameState.SET_PEREHODNICK, disctiption = "Установите переходники на пакет" });
        states.Add(new State() { state = NameState.CHECK_DOMKRATS, disctiption = "Выполнить проверку подъема и опускание домкарата в разных режимах" });
        states.Add(new State() { state = NameState.SET_DOMKRATS, disctiption = "Подкатите и установите домкраты" });
        states.Add(new State() { state = NameState.CHECK_TURING_MACHANISM, disctiption = "Проверьте работу механизма поворота домкрата и верните ручку домкрата в исходное положение" });
        states.Add(new State() { state = NameState.UP_TPK, disctiption = "Поднимите ТПК" });
        states.Add(new State() { state = NameState.CHECK_BREAK_MECHANISM, disctiption = "Проверьте работу тормозного механизма и отключите тормоз" });

        LoadTutorial();

        if (typeArea == TypeArea.FLAT)
        {
            states.Add(new State() { state = NameState.MOVE_TPK_FLAT, disctiption = "Переместите ТПК в красную точку" });
        }
        else if (typeArea == TypeArea.UP)
        {
            states.Add(new State() { state = NameState.MOVE_TPK_UP, disctiption = "Переместите ТПК по наклонной поверхности вверх" });
        }
        else if (typeArea == TypeArea.DOWN)
        {

        }

        indexCurState = 0;

        if ( gameMode == GameMode.FREEPLAY)
        {
            NotifyAllDomkrats(NameState.CHECK_DOMKRATS);
            NotifyAllDomkrats(NameState.SET_DOMKRATS);
            // Переходим в самое последнее состояние, чтобы открыть все части для свободной игры
            for (int i = 0; i < states.Count - 1; i++)
            {
                NextState();
            }
            finished = true;
        }
        ChangeTextHelper();
    }

    public void NextState()
    {
        indexCurState++;
        if (indexCurState >= states.Count)
        {
            finished = true;
        }

        if (finished)
        {
            Debug.Log("Calling 'NextState' when 'finished' flag is set to 'true', ignoring this call...");
            return;
        }
        Debug.Log(states[indexCurState].state);

        if (typeArea != TypeArea.FLAT && states[indexCurState].state > NameState.CHECK_BREAK_MECHANISM)
        {
            TPK.TPKObj.SwitchMovingThings(true);
        }

        if (states[indexCurState].state == NameState.SET_DOMKRATS)
        {
            NotifyAllDomkrats(states[indexCurState].state);
        }
        if (gameMode == GameMode.TRAIN)
        {
            ChangeTextHelper();
        }
    }

    public void ChangeTextHelper()
    {
        Singleton.Instance.UIManager.SetHelperText(string.Copy(states[indexCurState].disctiption));
        // InitialStateHack();
        Singleton.Instance.UIManager.OpenTutorial(string.Copy(SafeGetFromDict(tutorials)));
    }

    public void onError(Error error)
    {
        if (gameMode == GameMode.TRAIN)
            errorMessage.OnShow(string.Copy(error.ErrorText));
        counterMistaks += (int)error.Weight;
    }

    void NotifyAllDomkrats(NameState state)
    {
        foreach(Domkrat domkrat in domkrats)
        {
            domkrat.Notify(state);
        }
    }

    void Update()
    {
        if (countPerehodnick == 4)
        {
            NextState();
            countPerehodnick++;
            NotifyAllDomkrats(states[indexCurState].state);
        }
        if (countDomkrats == 4)
        {
            NextState();
            countDomkrats++;
        }
    }

    public NameState GetState()
    {
        return states[indexCurState].state;
    }

    public void Pause()
    {
        OnPause = true;
        player.GetComponent<PlayerMove>().enabled = false;
        player.GetComponent<PlayerRay>().enabled = false;
        // если установить time в 0, то перестают работать видосы...
        // Time.timeScale = 0;
    }

    public void Resume()
    {
        OnPause = false;
        player.GetComponent<PlayerMove>().enabled = true;
        player.GetComponent<PlayerRay>().enabled = true;
        // Time.timeScale = 1;
    }

    void LoadTutorial()
    {
        string filepath = @"Assets\\Texts\\flatTutorial.txt";
        string tutorial = File.ReadAllText(filepath);
        string[] tutorialSteps = tutorial.Split(new string[] { "<br>" }, StringSplitOptions.None);
        for (int i=0; i<tutorialSteps.Length; i++)
        {
            tutorials[(NameState)i] = tutorialSteps[i];
        }
    }

    public void InitialStateHack()
    {
        if (states[indexCurState].state == NameState.DEFAULT)
        {
            NextState();
        }
    }

    public string SafeGetFromDict(Dictionary<NameState, string> dict)
    {
        string value;
        if (!dict.TryGetValue(states[indexCurState].state, out value))
        {
            value = $"Для состояния {states[indexCurState].state} ещё не написаны подсказки...";
        }
        return value;
    }
}
