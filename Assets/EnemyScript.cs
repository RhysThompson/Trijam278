using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private bool IsAwareOfPlayer = false;
    private bool IsAnimating = false;
    private bool IsDead = false;
    public int Health = 10;
    public float AttackDelay = 1.5f;
    public float AttackDelayRandomVariation = 0f;
    private float CurrentAttackDelay = 0f;
    private GameObject Player;
    public GameObject Body;
    public GameObject LookTarget;
    public GameObject SpitBall;
    public GameObject ProjectileSpawnPoint;
    public float RotateSpeed = 10;
    public float AwarenessRange = 15;
    public bool InAir = false;
    public Animator Anim;
    public AudioClip SpitSnd;
    public AudioClip JumpSnd;
    public AudioClip AlertSnd;
    public AudioClip DeathSnd;
    public AudioSource AudioSrc;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (IsDead)
            return;

        if(!InAir)
            LookTarget.transform.LookAt(Player.transform, this.transform.up);

        if (!IsAnimating && IsAwareOfPlayer)
        {
            if (Quaternion.Angle(LookTarget.transform.rotation, Body.transform.rotation) > 25)
            {
                IsAnimating = true;
                Anim.Play("EnemyBounce");
                AudioSrc.PlayOneShot(JumpSnd);
            }
            else
            {
                if (CurrentAttackDelay <= 0)
                {
                    Anim.Play("EnemySpit");
                    CurrentAttackDelay = AttackDelay + Random.Range(0, AttackDelayRandomVariation);
                    AudioSrc.PlayOneShot(SpitSnd);
                    IsAnimating = true;
                }
            }
        }

        if (!IsAwareOfPlayer && Vector3.Distance(this.transform.position, Player.transform.position) < AwarenessRange)
        {
            IsAwareOfPlayer = true;
            AudioSrc.PlayOneShot(AlertSnd);
        }
        if (IsAwareOfPlayer && Vector3.Distance(this.transform.position, Player.transform.position) > 50)
            IsAwareOfPlayer = false;

        if (InAir)
        {
            Body.transform.rotation = Quaternion.RotateTowards(Body.transform.rotation, LookTarget.transform.rotation, RotateSpeed * Time.deltaTime);
        }
        if (CurrentAttackDelay > 0)
            CurrentAttackDelay -= Time.deltaTime;
    }

    public void OnHit(int damage)
    {
        if (IsDead)
            return;

        if(!IsAwareOfPlayer)
            AudioSrc.PlayOneShot(AlertSnd);

        IsAwareOfPlayer = true;
        Health -= damage;

        if (Health <= 0)
        {
            AudioSrc.PlayOneShot(DeathSnd);
            IsDead = true;
            Anim.Play("EnemyDie");
        }
    }

    public void AnimEnded()
    {
        IsAnimating = false;
    }

    public void Spit()
    {
        if (IsDead)
            return;
        Vector3 PlayerPos = Player.transform.position;
        PlayerPos.y = 1;
        ProjectileSpawnPoint.transform.LookAt(PlayerPos);
        Instantiate(SpitBall, ProjectileSpawnPoint.transform.position, ProjectileSpawnPoint.transform.rotation);
    }

    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
