using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TooltipManager : MonoBehaviour
{
    [Serializable]
    public class TooltipEvent
    {
        public string id;          // tooltip key to watch
        public UnityEvent onShow;  // invoked from ShowTooltip
        public UnityEvent onHide;  // invoked from HideTooltip
    }

    [Header("Per-tooltip events")]
    [SerializeField] private List<TooltipEvent> tooltipEvents = new();
    public static TooltipManager Instance { get; private set; }

    private Dictionary<string, ToolTipController> controllersDict = new Dictionary<string, ToolTipController>();
    private HashSet<string> usedTooltips = new HashSet<string>();
    

    void Awake()
    {
        Instance = this;
    }

    public void RegisterTooltip(string id, ToolTipController controller)
    {
        if (string.IsNullOrEmpty(id)) return;
        if (controllersDict.ContainsKey(id))
        {
            Debug.LogWarning($"Tooltip with ID: {id} already registered");
            return;       
        }
        else
        {
            controllersDict[id] = controller;
        }
    }

    public void ShowTooltip(string id)
    {
        if (!controllersDict.ContainsKey(id))
        {
            Debug.LogWarning($"No tooltip registered with ID: {id}");
            return;
        }

        var ctrl = controllersDict[id];
        if (ctrl.enableOnlyOnce && usedTooltips.Contains(id))
            return;

        // HideAll();

        ctrl.AnimateToolTipUp();

        if (ctrl.enableOnlyOnce)
            usedTooltips.Add(id);
            
        foreach (var e in tooltipEvents)
            if (e.id == id)
                e.onShow?.Invoke();
    }

    public void HideTooltip(string id)
    {
        if (controllersDict.TryGetValue(id, out var ctrl))
            ctrl.AnimateToolTipDown();

        foreach (var e in tooltipEvents)
            if (e.id == id)
                e.onHide?.Invoke();
    }

    public void HideAll()
    {
        foreach (var kv in controllersDict)
            kv.Value.AnimateToolTipDown();
    }
}
