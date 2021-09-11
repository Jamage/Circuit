using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelFactory : MonoBehaviour
{
    public List<Panel> PanelPrefabs;
    public Dictionary<PanelType, Panel> PanelDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        PanelDictionary = new Dictionary<PanelType, Panel>();
        foreach(Panel panel in PanelPrefabs)
        {
            if (PanelDictionary.ContainsKey(panel.panelType) == false)
                PanelDictionary.Add(panel.panelType, panel);
        }
    }

    public Panel GetPanelBy(PanelType panelType)
    {
        Panel panel = PanelDictionary[panelType];
        return Instantiate(panel, Vector3.zero, panel.transform.rotation);
    }
}
