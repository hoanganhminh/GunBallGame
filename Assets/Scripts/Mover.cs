using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class Mover : MonoBehaviour
{
    public Vector3 destination;
    protected Bounds screenBounds;

    [SerializeField]
    public TextMeshProUGUI Live;

    [SerializeField]
    private GameObject explosionPrefab;

    public int count = 10;
    protected Vector3 getRandomPoint(Vector3 min, Vector3 max)
    {
        int x = (int)Random.Range(min.x, max.x);
        int y = (int)Random.Range(min.y, max.y);
        return new Vector3(x, y, 0);
    }

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
        Live.text = count.ToString();
        screenBounds = OrthographicBounds(Camera.main);
        destination = getRandomPoint(screenBounds.min, screenBounds.max);
    }

    // Update is called once per frame
    void Update() => MoveRandom();

    protected void MoveRandom()
    {
           Live.text = count.ToString();
          float difX = Mathf.Abs(gameObject.transform.position.x - destination.x);
          float difY = Mathf.Abs(gameObject.transform.position.y - destination.y);
          if (difX < 0.5 || difY < 0.5)
          {
                destination = getRandomPoint(screenBounds.min, screenBounds.max);
          }
          else
          {
                gameObject.transform.position = Vector3.MoveTowards(transform.position, destination, 0.5f * Time.deltaTime);
          }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            count--;
            if (count <= 0)
            {
                foreach (ContactPoint2D missileHit in collision.contacts)
                {
                    Vector2 hitPoint = missileHit.point;
                    GameObject explose = Instantiate(explosionPrefab, new Vector3(hitPoint.x, hitPoint.y, 0), Quaternion.identity);
                }
                GameObject obj = GameObject.FindWithTag("MainCamera");
                MovingBehavior movingBehavior = obj.GetComponent<MovingBehavior>();
                movingBehavior.cats.Remove(gameObject);
                //Destroy(gameObject);
                gameObject.SetActive(false);
                count = 10;
            }
            else
            {
                Live.text = count.ToString();
            }
        }
    }
}
