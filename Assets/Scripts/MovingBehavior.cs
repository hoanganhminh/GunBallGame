using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts;

public class MovingBehavior : MonoBehaviour
{
    private Bounds screenBounds;

    [SerializeField]
    public GameObject catPrefab;

    [SerializeField]
    public GameObject gunPrefab;

    public List<GameObject> cats;

    private Timer timer;

    List<Vector3> points;

    bool isPause = false;

    private Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(camera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = OrthographicBounds(Camera.main);
        cats = new List<GameObject>();
        points = new List<Vector3>();
        points.Add(new Vector3(screenBounds.min.x, screenBounds.min.y, 0));//góc trái dưới
        points.Add(new Vector3(screenBounds.min.x % 6, screenBounds.min.y, 0));
        points.Add(new Vector3(screenBounds.min.x, screenBounds.max.y, 0));//góc trái trên
        points.Add(new Vector3(screenBounds.min.x, screenBounds.max.y % 6, 0));
        points.Add(new Vector3(screenBounds.max.x, screenBounds.max.y, 0));//góc phải trên
        points.Add(new Vector3(screenBounds.max.x % 6, screenBounds.max.y, 0));
        points.Add(new Vector3(screenBounds.max.x, screenBounds.min.y, 0));//góc phải dưới
        points.Add(new Vector3(screenBounds.max.x, screenBounds.min.y % 6, 0));
        GameObject gun = Instantiate(gunPrefab);
        gun.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        timer = gameObject.AddComponent<Timer>();
        timer.Duration = 10;
        timer.Run();
        //GameObject cat = Instantiate(catPrefab);
        //cat.transform.position = getRandomPoint();
        //cats.Add(cat);

        GameObject cat = ObjectPool.shareInstance.GetPooledObject(); 
        if (cat != null) {
           cat.transform.position = getRandomPoint();
           cat.SetActive(true);
           cats.Add(cat);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.Finished)
        {
            //GameObject cat = Instantiate(catPrefab);
            //cat.transform.position = getRandomPoint();
            //cats.Add(cat);
            GameObject cat = ObjectPool.shareInstance.GetPooledObject();
            if (cat != null)
            {
                cat.transform.position = getRandomPoint();
                cat.SetActive(true);
                cats.Add(cat);
            }
            timer.Duration = 10;
            timer.Run();
        }
    }

    Vector3 getRandomPoint() => points[Random.Range(0, points.Count - 1)];


    public void ReplayGame()
    {
        SceneManager.LoadScene("MainScene");
        ResumeGame();
    }

    public void PauseGame()
    {
        if (isPause == false)
        {
            Time.timeScale = 0;
            isPause = true;
        }
        else
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPause = false;
    }

    public void SaveGame()
    {
        Time.timeScale = 0;
        GameObject obj = GameObject.FindWithTag("MainCamera");
        SaveManager saveManager = obj.GetComponent<SaveManager>();
        SaveData saveData = new SaveData();
        saveData.Characters = new List<MoverData>();
        foreach (var cat in cats)
        {
            MoverData catData = new MoverData();
            catData.Type = "Cat";
            catData.Power = cat.GetComponent<Mover>().count;
            catData.Position = cat.transform.position;
            catData.Destination = cat.GetComponent<Mover>().destination;
            saveData.Characters.Add(catData);
        }
        if (saveData.Characters.Count > 0)
        {
            saveManager.Save(saveData);
        }
    }

    public void LoadGame()
    {
        Time.timeScale = 0;

        GameObject obj = GameObject.FindWithTag("MainCamera");
        SaveManager saveManager = obj.GetComponent<SaveManager>();
        while (cats.Count > 0)
        {
            Destroy(cats[0]);
            cats.Remove(cats[0]);
        }

        SaveData saveData = saveManager.Load();
        foreach (var moverData in saveData.Characters)
        {
            //GameObject cat = Instantiate(catPrefab);
            //cat.transform.position = moverData.Position;
            //cats.Add(cat);
            GameObject cat = ObjectPool.shareInstance.GetPooledObject();
            if (cat != null)
            {
                cat.transform.position = moverData.Position;
                cat.SetActive(true);
                cats.Add(cat);
            }
        }
    }
}
