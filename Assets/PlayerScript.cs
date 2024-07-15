using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public GameObject Body;
    public GameObject Torch;
    private bool TorchOn = true;
    public float MoveSpeed = 10f;
    public int Health = 10;
    public float ShootDelay = 0.5f;
    private float CurrentShootDelay = 0f;
    private Plane plane = new Plane(Vector3.up, -1f);
    public GameObject Bullet;
    public GameObject BulletSpawnPoint;
    public AudioClip GunshotSnd;
    public AudioSource AudioSrc;
    void Update()
    {
        this.transform.Translate(Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(plane.Raycast(ray, out float distance))
        {
            Body.transform.LookAt(ray.GetPoint(distance));
        }
        
        if (Input.GetKeyDown(KeyCode.E))
            Torch.SetActive(TorchOn = !TorchOn);

        if(Input.GetMouseButton(0) && CurrentShootDelay <= 0)
        {
            CurrentShootDelay = ShootDelay;
            Instantiate(Bullet, BulletSpawnPoint.transform.position, BulletSpawnPoint.transform.rotation);
            AudioSrc.PlayOneShot(GunshotSnd);
        }

        CurrentShootDelay -= Time.deltaTime;
    }

    public void OnHit(int damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            string currentSceneName =  SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
