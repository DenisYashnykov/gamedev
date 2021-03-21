using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SumNumbers : MonoBehaviour
{
    [SerializeField] private Text _answer;

    public void Fold()
    {
        int first = Random.Range(1, 101);
        int second = Random.Range(1, 101);

        Debug.Log("First " + first);
        Debug.Log("Second " + second);

        _answer.text = FoldNumbers(first, second).ToString();
    }

    private int FoldNumbers(int firstNumber, int secondNumber)
    {
        return firstNumber + secondNumber;
    }
}
