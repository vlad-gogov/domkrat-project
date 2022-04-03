using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum State
{
    DEFAULT = 0,
    SET_PEREHODNICK = 1,
    CHECK_DOMKRATS = 2,
    SET_DOMKRATS = 3,
    CHECK_TURING_MACHANISM = 4,
    UP_TPK = 5,
    CONFIG_DOMKRAT_TO_FORWARD = 6,
    CONFIG_DOMKRAT_TO_RIGHT = 7
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

    public List<Domkrat> domkrats = new List<Domkrat>();

    public bool OnPause { get; private set; }
    private Dictionary<State, string> states = new Dictionary<State, string>();
    Dictionary<State, string> tutorials = new Dictionary<State, string>();
    private State curState;
    // Переменная, проверяющая завершили ли мы сценарий
    bool finished = false;

    public int countPerehodnick = 0;
    public int countDomkrats = 0;

    void Start()
    {
        gameMode = CrossScenesStorage.gameMode;
        typeArea = CrossScenesStorage.typeArea;
        Debug.Log($"Current mode is: {gameMode} | {typeArea}");

        states.Add(State.DEFAULT, "");
        states.Add(State.SET_PEREHODNICK, "Установите переходники на пакет");

        states.Add(State.CHECK_DOMKRATS, "Выполнить проверку подъема и опускание домкарата в разных режимах");
        states.Add(State.SET_DOMKRATS, "Подкатите и установите домкраты");
        
        states.Add(State.CHECK_TURING_MACHANISM, "Проверьте работу механизма поворота домкрата");
        
        states.Add(State.UP_TPK, "Поднимите ТПК");
        LoadTutorial();

        if (typeArea == TypeArea.FLAT)
        {
            states.Add(State.CONFIG_DOMKRAT_TO_FORWARD, "Установите домкарты для перемещения вперед и нажмите стрелку вверх");
            states.Add(State.CONFIG_DOMKRAT_TO_RIGHT, "Установите домкарты для перемещения вправо и нажмите стрелку вправо");
        }
        else if (typeArea == TypeArea.UP)
        {

        }
        else if (typeArea == TypeArea.DOWN)
        {

        }

        curState = State.DEFAULT;

        if ( gameMode == GameMode.FREEPLAY)
        {
            NotifyAllDomkrats(State.CHECK_DOMKRATS);
            NotifyAllDomkrats(State.SET_DOMKRATS);
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
        if (finished)
        {
            Debug.Log("Calling 'NextState' when 'finished' flag is set to 'true', ignoring this call...");
            return;
        }
        curState = (State)((int)curState + 1);
        Debug.LogError(curState);
        if (curState == State.SET_DOMKRATS)
        {
            NotifyAllDomkrats(curState);
        }
        if (gameMode == GameMode.TRAIN)
        {
            ChangeTextHelper();
        }
    }

    public void ChangeTextHelper()
    {
        int index = (int)curState;
        Singleton.Instance.UIManager.SetHelperText(string.Copy(SafeGetFromDict(states)));
        // InitialStateHack();
        Singleton.Instance.UIManager.OpenTutorial(string.Copy(SafeGetFromDict(tutorials)));
    }

    public void onError(Error error)
    {
        if (gameMode == GameMode.TRAIN)
            errorMessage.OnShow(string.Copy(error.ErrorText));
        counterMistaks += (int)error.Weight;
    }

    void NotifyAllDomkrats(State state)
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
            NotifyAllDomkrats(curState);
        }
        if (countDomkrats == 4)
        {
            NextState();
            countDomkrats++;
        }
    }

    public State GetState()
    {
        return curState;
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
            tutorials[(State)i] = tutorialSteps[i];
        }
    }

    public void InitialStateHack()
    {
        if (curState == State.DEFAULT)
        {
            NextState();
        }
    }

    public string SafeGetFromDict(Dictionary<State, string> dict)
    {
        string value;
        if (!dict.TryGetValue(curState, out value))
        {
            value = $"Для состояния {curState} ещё не написаны подсказки...";
        }
        return value;
    }
}
