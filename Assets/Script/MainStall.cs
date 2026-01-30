using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum MaskType
{
   Sad,
   Anger,
   Confuse,
   Anxiety
}

public enum PlayerState
{
    Idle,
    Talking,
    InAction,
    CheckPaper
}
public class MainStall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static MainStall instance;
    public int currentIndex = 0;
    public GameObject currentPatientGO;
    public Patient currentPatient;
    public TextMeshProUGUI correct, wrong,indexT,info,paperName,paperAge,paperGender,paperContent;
    private int c, w,i;
    public GameObject patientPF;
    public TextAsset firstname,lastname;
    string[] firstNames;
    string[] lastNames;
    public RectTransform paperRect;
    public bool paperOn;
    private Vector2 paperTpos;
    List<string> availableNames = new List<string>();
    void Awake()
    {
        firstNames = firstname.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        lastNames  = lastname.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        GenerateAllNames();
    }
    void Start()
    {
        instance = this;
        SpawnPatient();
    }

    // Update is called once per frame
    void Update()
    {
        indexT.text = i.ToString();
        if (currentPatient != null)
        {
            info.text = currentPatient.correctMask.ToString();
            UpdatePaper();
        }
          
    }
  
    public void UpdatePaper()
    {
        paperName.text = "Name: " +currentPatient.patientName;
        paperAge.text = "Age: " +currentPatient.age.ToString();
        paperGender.text = "Gender: " +currentPatient.gender.ToString();
        paperRect.anchoredPosition =Vector2.Lerp(paperRect.anchoredPosition, paperTpos, Time.deltaTime * 50);
        if (paperOn)
        {
            paperTpos = new  Vector2(-400, 0);
        }
        else{
            paperTpos = new  Vector2(300, 0);
        }

        if (currentPatient.history)
        {
            
        }
        else
        {
            
        }
    }
    void GenerateAllNames()
    {
        foreach (string first in firstNames)
        {
            string f = first.Trim();

            // First name only
            availableNames.Add(f);

            // First + Last
            foreach (string last in lastNames)
            {
                availableNames.Add(f + " " + last.Trim());
            }
        }

        ShuffleList(availableNames);
    }
    void ShuffleList(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }
    public string GetRandomName()
    {
        if (availableNames.Count == 0)
            return "Tewi Inaba :3";

        string name = availableNames[0];
        availableNames.RemoveAt(0);
        return name;
    }
    public void SpawnPatient()
    {
        i++;
    
        GameObject patientGO = Instantiate(patientPF, Vector3.zero, Quaternion.identity);
        Patient patient = patientGO.GetComponent<Patient>();
        currentPatient = patient;
        currentPatientGO =  patientGO;
    }
    public void TalkAction(int i)
    {
        switch (i)
        {
            case 0:
                
                break;
            case 1:
                
                break;
            case 2:
                break;
            
        }
        
    }
    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(1);
        SpawnPatient();
    }
    public void GiveMask(int maskInt)
    {
        if (currentPatient == null)
            return;
        MaskType mask = (MaskType)maskInt;
        if (mask == currentPatient.correctMask)
        {
            c++;
            correct.text = c.ToString();
        }
        else
        {
            w++;
            wrong.text = w.ToString();
        }
        
        Destroy(currentPatientGO);
        currentPatient = null;
        StartCoroutine(SpawnDelay());
    }
}
