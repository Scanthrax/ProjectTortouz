using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class SaveController : MonoBehaviour
{
    public Camera cam;
    public Transform pengin;
    public List<GameObject> objects;

    Dictionary<int, GameObject> listOfGameObjects = new Dictionary<int, GameObject>();

    public static Dictionary<string, bool> alienCollectables = new Dictionary<string, bool>();

    Save save = new Save();

    private void Awake()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Alien"))
        {
            string temp = item.GetComponent<AlienCollectable>().alien.name;
            alienCollectables.Add(temp, false);
        }
        //DeleteFile();
        //InitDictionary();
        LoadGame();
    }


    void InitDictionary()
    {
        foreach (var obj in objects)
        {
            listOfGameObjects.Add(obj.GetInstanceID(), obj);
        }
    }

    public void LoadGame()
    {
        //if (false)
        //{
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

                foreach (var obj in objects)
                {
                    if (!save.activatedItems.ContainsKey(obj.GetInstanceID()))
                        continue;

                    var breakableWall = obj.GetComponent<BreakableWall>();
                    if (breakableWall != null)
                    {
                        listOfGameObjects[obj.GetInstanceID()].GetComponent<BreakableWall>().isBroken = save.activatedItems[obj.GetInstanceID()];
                    }

                    var button = obj.GetComponentInChildren<Button>();
                    if (button != null)
                    {
                        listOfGameObjects[obj.GetInstanceID()].GetComponentInChildren<Button>().press = save.activatedItems[obj.GetInstanceID()];
                    }
                }

                alienCollectables = save.alienCollectables;


                Debug.Log("Game Loaded");
                SaveGame(save);
        }
            else
            {
                Debug.Log("No game saved!");

            }
        //}
    }

        

    public void SaveGame()
    {
        save = new Save();

        save.faceDirection = pengin.GetComponent<PlayerMovement>().faceDir;

        // save the xyz coordinates
        save.x = pengin.position.x;
        save.y = pengin.position.y;
        save.z = pengin.position.z;

        save.camX = cam.transform.position.x;
        save.camY = cam.transform.position.y;
        save.camZ = cam.transform.position.z;

        foreach (var item in objects)
        {
            var breakableWall = item.GetComponent<BreakableWall>();
            if (breakableWall != null)
            {
                save.activatedItems.Add(item.GetInstanceID(), breakableWall.isBroken);
                print("updated wall");
                continue;
            }

            var button = item.GetComponentInChildren<Button>();
            if (button != null)
            {
                save.activatedItems.Add(item.GetInstanceID(), button.isPressed);
                print("updated button");
            }
        }


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
