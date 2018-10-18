using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class SaveController : MonoBehaviour
{
    public Transform pengin;

    private void Awake()
    {
        LoadGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveGame(false);
        }
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            pengin.position = new Vector3(save.x + (save.faceDirection * 1.25f), save.y, save.z);

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }



    public void SaveGame(bool autosave)
    {
        Save save = new Save();

        save.faceDirection = autosave? pengin.GetComponent<PlayerMovement>().faceDir : 0;

        save.x = pengin.position.x;
        save.y = pengin.position.y;
        save.z = pengin.position.z;

        


        save.autosaved = autosave;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

}
