using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using DG.Tweening;


public class ShooterPlayer : MonoBehaviourPun
{
    [SerializeField] float speed = 5;
    [SerializeField] int health = 10;
    [SerializeField] TMP_Text playerName;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Renderer rend;
    [SerializeField] Texture[] avatarTextures;

    Vector2 moveDir;

   private void Start () {
        //damage
        playerName.text = photonView.Owner.NickName + $" ({health})";
        if (photonView.Owner.CustomProperties.TryGetValue(PropertyNames.Player.AvatarIndex, out var AvatarIndex))
            rend.material.mainTexture = avatarTextures[(int)AvatarIndex];
   }

    void Update()
    {
        if(photonView.IsMine == false)
            return;
        
        moveDir = new Vector2 (
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
        
        // transform.Translate(moveDir*Time.deltaTime*speed);

        if(Input.GetKeyDown(KeyCode.Space))
            photonView.RPC("TakeDamage", RpcTarget.All, 1);
    }

    private void FixedUpdate() {
        //if (photonView.IsMine == false)
            //return;
        rb.velocity = moveDir * speed;
    }

    [PunRPC]
    public void TakeDamage (int amount) {
        health -= amount;
        playerName.text = photonView.Owner.NickName + $" ({health})";

        //kasih warna
        GetComponent <SpriteRenderer> ().DOColor(Color.red, 0.2f).SetLoops(1,LoopType.Yoyo).From();
   }
}
