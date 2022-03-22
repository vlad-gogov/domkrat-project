using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    DEFAULT = 0,
    SET_PEREHODNICK = 1,
    CHECK_DOMKRATS = 2,
    SET_DOMKRATS = 3
}

enum GameMode
{
    TRAIN = 0,
    EXAM = 1
}

public class StateManager : MonoBehaviour
{
    //[SerializeField] GameObject DomkratLeft;
    //[SerializeField] GameObject DomkratRight;

    private Dictionary<State, string> states = new Dictionary<State, string>();
    private State curState;

    // Счетчики для кол-ва
    public int countPerehodnick = 0;
    public int countDomkrrats = 0;


    private List<Vector3> positionDomkrat;

    void Awake()
    {
        states.Add(State.DEFAULT, "");
        states.Add(State.CHECK_DOMKRATS, "Выполнить проверку подъема и опускание домкарата в разных режимах");
        states.Add(State.SET_PEREHODNICK, "Установите переходники на пакет");
        states.Add(State.SET_DOMKRATS, "Подкатите и установите домкраты");
        
        curState = State.DEFAULT;

        //positionDomkrat.Add(new Vector3(7f, 0f, -14f));
        //positionDomkrat.Add(new Vector3(7f, 0f, -12f));
        //positionDomkrat.Add(new Vector3(7f, 0f, -10f));
        //positionDomkrat.Add(new Vector3(7f, 0f, -8f));

        NextState();
    }

    public void NextState()
    {
        curState = curState++;
        ChangeTextHelper();
    }

    public void ChangeTextHelper()
    {
        int index = (int)curState;
        Singleton.Instance.UIManager.SetHelperText(string.Copy(states[curState]));
    }

    void Update()
    {
        if (countPerehodnick == 4)
        {
            NextState();
            //Instantiate(DomkratLeft, positionDomkrat[0], Quaternion.identity);
        }
    }
}
