using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 Destination { get; set; }
    private GameObject target;

    [SerializeField]
    private GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        target = new GameObject();
        target.transform.position = new Vector3(0, -10, 0);
        if (gameObject.name.Contains("Clone"))
        {
            if (Destination != null)
            {
                gameObject.transform.position = Vector3.MoveTowards(transform.position, Destination, 30 * Time.deltaTime); //tốc độ đạn bay
            }
        }
        Destroy(gameObject,0.3f);
        //gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Quaternion.RotateTowards(transform.rotation, targetRotation, 70000 * Time.deltaTime);
        }

        if (gameObject.name.Contains("Clone")) gameObject.transform.position = Vector3.MoveTowards(transform.position, Destination, 30 * Time.deltaTime); //tốc độ đạn bay
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);//bắn trúng mèo thì cho nổ
        //gameObject.SetActive(false);
    }
}
