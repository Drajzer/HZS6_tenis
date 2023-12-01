using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class ManageSavingAndData : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public Slider volume;

    [System.Serializable]
    public class Data
    {
        public int Difficulty;
        public float Volume;
    }

    private void Update()
    {
        AudioListener.volume = save.Volume;
    }

    public Data save = new Data();

    private void Start()
    {
        LoadData();
    }

    public void SetDifficulty(TMP_Dropdown Dificulty)
    {
        save.Difficulty = Dificulty.value;
    }
    public void SetVolume(Slider Volume)
    {
        save.Volume = Volume.value;
    }

    public void LoadData()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/save.json"))
        {
            string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/save.json");
            save = JsonUtility.FromJson<Data>(json);
        }
    }

    public void SaveData()
    {
        string output = JsonUtility.ToJson(save);
        File.WriteAllText(Application.persistentDataPath + "/save.json", output);
    }

    public void applyMenuData()
    {
        dropdown.value = save.Difficulty;
        volume.value = save.Volume;
    }
}
