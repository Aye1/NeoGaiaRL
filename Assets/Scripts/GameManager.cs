using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    private List<Character> _characters;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _characters = new List<Character>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
    }

    public void AddCharacter(Character character)
    {
        if (character != null)
        {
            _characters.Add(character);
        }
    }

    public void FreezGameButDialogs()
    {
        foreach(Character c in _characters)
        {
            c.Freeze();
        }
    }

    public void EndFreezeGameButDialogs()
    {
        foreach (Character c in _characters)
        {
            c.EndFreeze();
        }
    }
}
