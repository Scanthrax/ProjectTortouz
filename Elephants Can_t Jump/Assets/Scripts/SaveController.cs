using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class SaveController : MonoBehaviour
{
    public Camera cam;
    public Transform pengin;

    public static Dictionary<string, bool> buttonsDict = new Dictionary<string, bool>();
    public static Dictionary<string, bool> breakableDict = new Dictionary<string, bool>();

    public List<AlienObjects> listOfAliens;
    public static Dictionary<string, bool> alienCollectables = new Dictionary<string, bool>();

    Save save = new Save();

    private void Awake()
    {
        cam = Camera.main;

        foreach (var item in listOfAliens)
        {
            alienCollectables.Add(item.name, false);
        }
        //DeleteFile();

        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            buttonsDict = save.buttonsDict;
            breakableDict = save.breakableDict;
            alienCollectables = save.alienCollectables;
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


            // update pengin's position
            int temp = save.lastSave ? 1 : 0;

            pengin.position = new Vector3(save.x + (save.faceDirection * 1.25f * temp), save.y, save.z);
            pengin.GetComponent<PlayerMovement>().faceDir = save.faceDirection;
            cam.transform.position = new Vector3(save.camX, save.camY, save.camZ);

            alienCollectables = save.alienCollectables;
            buttonsDict = save.buttonsDict;
            breakableDict = save.breakableDict;


            Debug.Log("Game Loaded");
            SaveGame(save);
        }
        else
        {
            Debug.Log("No game saved!");

        }
    }

        

    public void SaveGame()
    {
        save = new Save();

        

        // save the xyz coordinates
        save.x = pengin.position.x;
        save.y = pengin.position.y;
        save.z = pengin.position.z;

        save.faceDirection = pengin.GetComponent<PlayerMovement>().faceDir;

        save.camX = cam.transform.position.x;
        save.camY = cam.transform.position.y;
        save.camZ = cam.transform.position.z;

        save.buttonsDict = buttonsDict;
        save.breakableDict = breakableDict;
        save.alienCollectables = alienCollectables;
        save.lastSave = true;

        


        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public void SaveGame(Save save)
    {
        save.lastSave = false;
        save.x = pengin.position.x;
        save.y = pengin.position.y;
        save.z = pengin.position.z;

        save.faceDirection = pengin.GetComponent<PlayerMovement>().faceDir;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("last save");
    }


    void DeleteFile()
    {
        File.Delete(Application.persistentDataPath + "/gamesave.save");
    }
}
