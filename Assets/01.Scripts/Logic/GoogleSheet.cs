using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheet : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(LoadSheet());
    }

    private IEnumerator LoadSheet()
    {
        var sheetID = "15vD7mGcQxETdjB6v1G9ebRKpYnHNO3ZU4ClRqzpRE4Q";
        var url = $"https://docs.google.com/spreadsheets/d/{sheetID}/export?format=csv";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(req.error);
                yield break;
            }

            var csv = req.downloadHandler.text;
            //Service.Log(csv);
        }
    }
}