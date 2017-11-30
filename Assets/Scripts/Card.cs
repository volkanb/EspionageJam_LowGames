using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    public static bool DO_NOT = false;

    [SerializeField]
    private int _state;
    [SerializeField]
    private int _cardValue;
    [SerializeField]
    private bool _initialized = false;

    private Sprite _cardBack;
    private Sprite _cardFace;

    private GameManager _manager;

    void Start() {
        _state = 1;
        _manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    public void setupGraphics() {
        _cardBack = _manager.getCardBack();
        _cardFace = _manager.getCardFace(_cardValue);

        flipCard();
    }

    public void flipCard() {

        if (_state == 0 && !DO_NOT)
            _state = 1;
        else if (_state == 1 && !DO_NOT)
            _state = 0;

        if (_state == 0 && !DO_NOT)
            GetComponent<Image>().sprite = _cardBack;
        else if (_state == 1 && !DO_NOT)
            GetComponent<Image>().sprite = _cardFace;

        if (_manager.getMatches() == 0)
        {
            _manager.choseTarget(carddValue);
        }
    }

    public int carddValue {
        get { return _cardValue; }
        set { _cardValue = value; }
    }

    public int state
    {
        get { return _state; }
        set { _state = value; }
    }

    public bool initialized {
        get { return _initialized; }
        set { _initialized = value; }
    }

    public void falseCheck() {
        StartCoroutine(pause());
    }

    IEnumerator pause() {
        yield return new WaitForSeconds(1);
        if (_state == 0)
            GetComponent<Image>().sprite = _cardBack;
        else if (_state == 1)
            GetComponent<Image>().sprite = _cardFace;
        DO_NOT = false;
    }

    public void rewindCard()
    {
        GetComponent<Image>().sprite = _cardBack;
        _state = 0;
    }
}
