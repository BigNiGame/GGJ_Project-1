using System.Collections.Generic;
using UnityEngine;

public enum Gender
{
    Male,
    Female
}

public enum PatientState
{
    WalkIn,
    Idle,
    Talking,
    Leaving
}

public class Patient : MonoBehaviour
{
    public Sprite photo;
    public int age;
    public Gender gender;
    public string patientName;
    public MaskType correctMask;
    public bool history, randomDia;
    public int questionType;
    [TextArea] public string historytxt, randomtxt, feeltxt, todotxt;

    void Start()
    {
        correctMask = (MaskType)Random.Range(0, 5);
        patientName = MainStall.instance.GetRandomName();
        gender = (Gender)Random.Range(0, 2);
        age = Random.Range(15, 101);
        int possible = Random.Range(1, 8);
        switch (possible)
        {
            case 1:
                // his
                history = true;
                break;
            case 2:
                // rando 
                randomDia = true;
                break;
            case 3:
                // quest
                PickQuestion();
                break;
            case 4:
                // his rando
                history = true;
                randomDia = true;
                break;
            case 5:
                // his quest
                history = true;
                PickQuestion();
                break;
            case 6:
                // rando quest
                randomDia = true;
                PickQuestion();
                break;
            case 7:
                // all 3
                history = true;
                randomDia = true;
                PickQuestion();
                break;
        }

        PickTextData();
    }

    public void PickTextData()
    {
        historytxt = PickHistory();
        PickQuestionAnswer();
        randomtxt = PickRandomDia();
    }

    public string PickHistory()
    {
        if (history)
        {
            string folder = correctMask.ToString();
            string path = $"Dialogues/{folder}/{folder}History";
            TextAsset file = Resources.Load<TextAsset>(path);
            if (file == null)
            {
                Debug.LogError("History file not found: " + path);
                return "";
            }

            string[] lines = file.text.Split(
                new[] { '\n' },
                System.StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
                return "";

            int randomIndex = Random.Range(0, lines.Length);
            return lines[randomIndex].Trim();
        }
        else
        {
            string folder = "Blank";
            string path = $"Dialogues/{folder}/{folder}History";
            TextAsset file = Resources.Load<TextAsset>(path);
            if (file == null)
            {
                Debug.LogError("History file not found: " + path);
                return "";
            }

            string[] lines = file.text.Split(
                new[] { '\n' },
                System.StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
                return "";

            int randomIndex = Random.Range(0, lines.Length);
            return lines[randomIndex].Trim();
        }
    }

    public void PickQuestionAnswer()
    {
        switch (questionType)
        {
            case 1:
            {
                string folderA = correctMask.ToString();
                string pathA = $"Dialogues/{folderA}/{folderA}Feel";
                TextAsset fileA = Resources.Load<TextAsset>(pathA);
                if (fileA == null)
                {
                    Debug.LogError("History file not found: " + pathA);
                }

                string[] linesA = fileA.text.Split(
                    new[] { '\n' },
                    System.StringSplitOptions.RemoveEmptyEntries);


                int randomIndexA = Random.Range(0, linesA.Length);
                feeltxt = linesA[randomIndexA].Trim();
                
                string folderB = "Blank";
                string pathB = $"Dialogues/{folderB}/{folderB}ToDo";
                TextAsset fileB = Resources.Load<TextAsset>(pathB);
                if (fileB == null)
                {
                    Debug.LogError("History file not found: " + pathB);
                }

                string[] linesB = fileB.text.Split(
                    new[] { '\n' },
                    System.StringSplitOptions.RemoveEmptyEntries);


                int randomIndexB = Random.Range(0, linesB.Length);
                todotxt = linesB[randomIndexB].Trim();
                break;
            }
            case 2:
            {
                string folderA = "Blank";
                string pathA = $"Dialogues/{folderA}/{folderA}Feel";
                TextAsset fileA = Resources.Load<TextAsset>(pathA);
                if (fileA == null)
                {
                    Debug.LogError("History file not found: " + pathA);
                }

                string[] linesA = fileA.text.Split(
                    new[] { '\n' },
                    System.StringSplitOptions.RemoveEmptyEntries);


                int randomIndexA = Random.Range(0, linesA.Length);
                feeltxt = linesA[randomIndexA].Trim();
                
                string folderB = correctMask.ToString();
                string pathB = $"Dialogues/{folderB}/{folderB}ToDo";
                TextAsset fileB = Resources.Load<TextAsset>(pathB);
                if (fileB == null)
                {
                    Debug.LogError("History file not found: " + pathB);
                }

                string[] linesB = fileB.text.Split(
                    new[] { '\n' },
                    System.StringSplitOptions.RemoveEmptyEntries);


                int randomIndexB = Random.Range(0, linesB.Length);
                todotxt = linesB[randomIndexB].Trim();
                break;
            }
            case 0:
            {
                string folderA = "Blank";
                string pathA = $"Dialogues/{folderA}/{folderA}Feel";
                TextAsset fileA = Resources.Load<TextAsset>(pathA);
                if (fileA == null)
                {
                    Debug.LogError("History file not found: " + pathA);
                }

                string[] linesA = fileA.text.Split(
                    new[] { '\n' },
                    System.StringSplitOptions.RemoveEmptyEntries);


                int randomIndexA = Random.Range(0, linesA.Length);
                feeltxt = linesA[randomIndexA].Trim();
                
                string folderB = "Blank";
                string pathB = $"Dialogues/{folderB}/{folderB}ToDo";
                TextAsset fileB = Resources.Load<TextAsset>(pathB);
                if (fileB == null)
                {
                    Debug.LogError("History file not found: " + pathB);
                }

                string[] linesB = fileB.text.Split(
                    new[] { '\n' },
                    System.StringSplitOptions.RemoveEmptyEntries);


                int randomIndexB = Random.Range(0, linesB.Length);
                todotxt = linesB[randomIndexB].Trim();
                break;
            }
        }
    }

    public string PickRandomDia()
    {
        if (randomDia)
        {
            string folder = correctMask.ToString();
            string path = $"Dialogues/{folder}/{folder}Rando";
            TextAsset file = Resources.Load<TextAsset>(path);
            if (file == null)
            {
                Debug.LogError("History file not found: " + path);
                return "";
            }

            string[] lines = file.text.Split(
                new[] { '\n' },
                System.StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
                return "";

            int randomIndex = Random.Range(0, lines.Length);
            return lines[randomIndex].Trim();
        }
        else
        {
            string folder = "Blank";
            string path = $"Dialogues/{folder}/{folder}Rando";
            TextAsset file = Resources.Load<TextAsset>(path);
            if (file == null)
            {
                Debug.LogError("History file not found: " + path);
                return "";
            }

            string[] lines = file.text.Split(
                new[] { '\n' },
                System.StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
                return "";

            int randomIndex = Random.Range(0, lines.Length);
            return lines[randomIndex].Trim();
        }
    }

    public void PickQuestion()
    {
        if (Random.value > 0.5f)
        {
            questionType = 1; //feel
        }
        else
        {
            questionType = 2; //do to
        }
    }


    // Update is called once per frame
    void Update()
    {
    }
}