using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Radar : MonoBehaviour
{
    UnityEvent m_MyEvent;
    public Collider2D[] close = new Collider2D[0];
    GameObject closest;
    Gun gun;

    // Start is called before the first frame update
    void Start()
    {
        if (m_MyEvent == null) m_MyEvent = new UnityEvent();
        m_MyEvent.AddListener(Ping);
        gun = FindObjectOfType<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        close = Physics2D.OverlapCircleAll(gameObject.transform.position, 5, LayerMask.GetMask("Default"));
        if (close.Length > 0)
        {
            closest = GetNearestEnemy();
            m_MyEvent.Invoke();
        }
    }

    void Ping() => gun.Shoot(closest);

    private GameObject findClosestEnemy(Collider2D[] close)
    {
        GameObject closestEnemy = close[0].gameObject;
        float closestDistance = float.MaxValue;
        bool first = true;
        for(int i=0; i < close.Length; i++)
        {
            float distance = Vector3.Distance(close[i].gameObject.transform.position, gameObject.transform.position);
            if (first)
            {
                closestDistance = distance;
                first = false;
            }
            else if (distance < closestDistance)
            {
                closestEnemy = close[i].gameObject;
                closestDistance = distance;
            }
        }
        return closestEnemy;
    }

    private GameObject GetNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Cat");
        GameObject closestEnemy = enemies[0];
        if (enemies.Length > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                if (Vector2.Distance(enemy.transform.localPosition, transform.position) < Vector2.Distance(closestEnemy.transform.localPosition, transform.position))
                {
                    closestEnemy = enemy;
                }
            }
            //transform.LookAt(closestEnemy.transform.position);
        }
        Debug.Log("Heloooooooooooooooo" + closestEnemy + "Hiiiiiiiiiiiiiiiii");
        return closestEnemy;
    }
}
