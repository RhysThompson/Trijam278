using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float MoveSpeed = 10f;

    public float LifeTime = 10f;
    public string FriendTag = "Player";
    public string EnemyTag = "Enemy";
    public int Damage = 1;
    void Update()
    {
        this.transform.Translate(0, 0, MoveSpeed * Time.deltaTime);

        LifeTime -= Time.deltaTime;

        if(LifeTime <= 0)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (string.Compare(other.transform.root.gameObject.tag, "Bullet") == 0)
            return;

        if(string.Compare(other.transform.root.gameObject.tag, FriendTag) != 0)
        {
            if (string.Compare(other.transform.root.gameObject.tag, EnemyTag) == 0)
                other.transform.root.gameObject.SendMessageUpwards("OnHit", Damage);
            Destroy(this.gameObject);
        }
    }
}
