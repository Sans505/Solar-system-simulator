using UnityEngine;
using System.Collections.Generic;
using System;

public class BodyListMenuManager : MonoBehaviour
{
    [SerializeField] private BodyEntry entryPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private Sprite planetSprite;
    [SerializeField] private Sprite sunSprite;
    [SerializeField] private SelectedBodyManager selectedBodyManager;

    private BodyEntry selectedEntry;

    public class EntryInfo {
        public Sprite Icon;
        public string Text;

        public EntryInfo(Sprite icon, string text) {
            Icon = icon;
            Text = text;
        }
    }

    private List<EntryInfo> entriesInfo = new List<EntryInfo>();
    private List<BodyEntry> entries = new List<BodyEntry>();


    public void CreateEntries() {
        foreach (Transform child in container) {
            Destroy(child.gameObject);
        }

        entries = new List<BodyEntry>();

        foreach(var entryInfo in entriesInfo) {
            var newEntry = Instantiate(entryPrefab, container);
            newEntry.Setup(entryInfo.Icon, entryInfo.Text, this);
            entries.Add(newEntry);
        }
    }

    public void SetEntriesInfoList(List<GameObject> list) {

        entriesInfo = new List<EntryInfo>();
        foreach (GameObject go in list) {
            Sprite sprite;
            if (go.GetComponent<Planet>()) {
                sprite = planetSprite;
            } else {
                sprite = sunSprite;
            }
            EntryInfo entryInfo = new EntryInfo(sprite, go.name);
            entriesInfo.Add(entryInfo);
        }
    }

    public void SelectEntry(BodyEntry entry, bool doubleClick) {

        selectedBodyManager.SelectBody(entry.name, doubleClick);

    }

    public void UpdateSelectedEntry(string name) {
        if (selectedEntry) {
            selectedEntry.DeSelect();
        }
        BodyEntry entry = entries.Find(x => x.name == name);
        selectedEntry = entry;
        selectedEntry.Select();
    }
}
