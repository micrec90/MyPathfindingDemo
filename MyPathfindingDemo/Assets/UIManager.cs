using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private GameObject editor;
    [SerializeField]
    private TMP_InputField rowField;
    [SerializeField]
    private TMP_InputField colField;
    [SerializeField]
    private TMP_InputField map;

    int rows = 5;
    int columns = 5;
    private const int mapInputFiledSpacing = 20;
    private float maxMapFieldSize = 30 + 10 * mapInputFiledSpacing;

    public void ShowMapEditor()
    {
        editor.SetActive(toggle.isOn);
        ChangeMapSize();
    }
    public void ChangeMapSize()
    {
        RectTransform rect = map.gameObject.GetComponent<RectTransform>();
        
        bool parsed = int.TryParse(rowField.text, out rows);
        if (!parsed)
        {
            Debug.LogWarning("Wrong input for rows!");
            return;
        }
        parsed = int.TryParse(colField.text,out columns);
        if (!parsed)
        {
            Debug.LogWarning("Wrong input for rows!");
            return;
        }
        float rowTextFieldSize = (float) 30 + ((rows - 1) * mapInputFiledSpacing);
        float columnTextFieldSize = 160 * columns/4;
        rowTextFieldSize = Mathf.Clamp(rowTextFieldSize, 30, maxMapFieldSize);
        rect.sizeDelta = new Vector2(columnTextFieldSize, rowTextFieldSize);

        UpdateMapInputField();
    }
    public void GenerateMap(bool isRandom)
    {
        GraphManager.Instance.ParseMapText(map.text, rows, columns, isRandom);
    }
    private void UpdateMapInputField()
    {
        map.text = string.Empty;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                map.text += "0\t";
            }
            map.text = map.text.TrimEnd('\t');
            map.text += Environment.NewLine;
        }
    }
    public void QuitApplication()
    {
        Application.Quit();
    }
}
