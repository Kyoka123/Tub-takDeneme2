using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Questions : MonoBehaviour
{   //sorularý, yazý hýzýný ve text componentini inspector üzerinden atadýk
    public List<string> questions = new List<string>();
    public float textspeed;
    public TMPro.TextMeshProUGUI textcomponent;

    //courutine bitti mi diye kontrol etmek için bool deðiþkeni oluþturduk
    bool iscoroutinerunning;

    //array þeklinde yaptýðýmýz için kaçýncý array olduðunu tutmak için index deðiþkeni oluþturduk
    private int index;
    void Start()
    {
        textcomponent.text = string.Empty;
        StartQuestions();
    }
    void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame && iscoroutinerunning == false)
        {        
         questions.RemoveAt(index);
            index = Random.Range(0, questions.Count);
         textcomponent.text = string.Empty;
         StartCoroutine(WriteQuestion());     
        }
    }

    void StartQuestions()
    {
        index = Random.Range(0, questions.Count);
        StartCoroutine(WriteQuestion());
    }

    IEnumerator WriteQuestion()
    {
        //yazýyý teker teker yazmamýzý saðlayacak bir coroutine oluþturduk
        
        foreach (char c in questions[index].ToCharArray())
        {
            iscoroutinerunning = true;
            textcomponent.text += c;
            //yazý hýzýný belirlemek için waitforseconds kullandýk
            yield return new WaitForSeconds(textspeed);
        }
            iscoroutinerunning = false;
    }
}
