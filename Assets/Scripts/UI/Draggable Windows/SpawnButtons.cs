// Sifaka Game Studios (C) 2017

using UnityEngine;
using SimpleJSON;

public class SpawnButtons : MonoBehaviour
{
    public GameObject dragScrollBar;
    public GameObject buttonPrefab;
    private GameObject[] buttons;
    public bool animalButtons = false;
    private int x = 3;
    private JSONNode jsonInfo;
    private TextAsset jsonText;
    void Start()
    {
        GameObject TMPButton;
        if (animalButtons) { jsonText = Resources.Load<TextAsset>("Animals/Animals"); }
        else { jsonText = Resources.Load<TextAsset>("Animals/Animals"); }
        jsonInfo = JSON.Parse(jsonText.text);
        buttons = new GameObject[jsonInfo["animalTemplates"].Count];
        for (int i = 0; i < jsonInfo["animalTemplates"].Count; i++)
        {
            TMPButton = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TMPButton.transform.SetParent(this.transform);
            TMPButton.GetComponent<RectTransform>().localPosition = new Vector3(-100 + (i % x * 80), -55 - ((Mathf.Floor(i / x - 1)) * 80), 0);
            TMPButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            buttons[i] = TMPButton;
        }
    }

    public void OnScrollChange()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<RectTransform>().localPosition = new Vector3(buttons[i].GetComponent<RectTransform>().localPosition.x, -55 - ((Mathf.Floor(i / x - 1)) * 80) + dragScrollBar.GetComponent<UnityEngine.UI.Scrollbar>().value * (Mathf.Floor(jsonInfo["animalTemplates"].Count / 3) * 80), 0);
        }
    }
}
