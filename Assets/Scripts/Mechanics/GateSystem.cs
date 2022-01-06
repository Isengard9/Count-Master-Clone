using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Gate system using for adding player or removing player using with math door
/// </summary>
public class GateSystem : MonoBehaviour
{

    //Is the good or bad gate
    public enum GateType
    {
        good,
        bad,
    }

    public GateType gateType = GateType.good;

    //What is the math function
    public enum MathFunction
    {
        plus,
        minus,
        multiple,
        divide,
    }

    public MathFunction mathFunction = MathFunction.plus;

    public int Value = 0;


    [SerializeField] TMP_Text Text;//Math text 
    [SerializeField] MeshRenderer gateMaterial;//Gate material red or blue
    [SerializeField] Color good, bad;//Can changeble good or bad colors
    [SerializeField] GateSystem otherGate;//Connected left or right gate


    private void Start()
    {
        InitializeGate();
    }

    /// <summary>
    /// Initializing gate system
    /// </summary>
    void InitializeGate()
    {
        switch (mathFunction)
        {
            case MathFunction.plus:
                Text.text = "+" + Value;
                gateMaterial.material.color = good;
                break;
            case MathFunction.minus:
                Text.text = "-" + Value;
                gateMaterial.material.color = bad;
                break;
            case MathFunction.multiple:
                Text.text = "x" + Value;
                gateMaterial.material.color = good;
                break;
            case MathFunction.divide:
                Text.text = "/" + Value;
                gateMaterial.material.color = bad;
                break;
        }
    }

    public bool isUsed = false;//Controlling the player already triggered this gate


    private void OnTriggerEnter(Collider other)
    {
        if (!isUsed  && other.transform.tag.Equals("Player"))
        {
            isUsed = true;
            otherGate.isUsed = true;
            this.GetComponentInParent<Animator>().SetTrigger("Close");
            int value = this.Value;
            int currentTinyPlayers = PlayerManager.instance._Count;

            switch (this.mathFunction)
            {
                case GateSystem.MathFunction.plus:
                    PlayerManager.PlayerAdd(value);
                    //PlayerManager.instance.AddTinyPlayers(value);
                    break;
                case GateSystem.MathFunction.minus:
                    PlayerManager.PlayerRemove(value);
                    //PlayerManager.instance.RemoveTinyPlayers(value);
                    break;
                case GateSystem.MathFunction.multiple:
                    PlayerManager.PlayerAdd(currentTinyPlayers * (value - 1));
                    //PlayerManager.instance.AddTinyPlayers(currentTinyPlayers * (value - 1));
                    break;
                case GateSystem.MathFunction.divide:
                    PlayerManager.PlayerRemove(currentTinyPlayers - Mathf.FloorToInt((float)currentTinyPlayers / (float)value));
                    //PlayerManager.instance.RemoveTinyPlayers(currentTinyPlayers - Mathf.FloorToInt((float)currentTinyPlayers / (float)value));
                    break;
            }
            this.GetComponent<Collider>().enabled = false;
        }
    }
}
