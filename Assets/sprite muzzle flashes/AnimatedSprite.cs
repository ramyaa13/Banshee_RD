using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat.UtilityScripts;

public class AnimatedSprite : MonoBehaviour
{
    public float fps = 30.0f;
    public Sprite[] frames;

    private int frameIndex;
    [SerializeField] private SpriteRenderer rendererMy;
    [SerializeField] private bool isLooped;
    [SerializeField] private bool canDestroy;

    private PhotonView photonView;

    private void Start()
    {
        if(canDestroy)
            photonView = GetComponent<PhotonView>();

    }

    void OnEnable()
    {
        frameIndex = 0;
        //rendererMy = GetComponent<SpriteRenderer>();
        NextFrame();
        InvokeRepeating("NextFrame", 1 / fps, 1 / fps);
    }

    void NextFrame()
    {
        rendererMy.sprite = frames[frameIndex];
        if(isLooped)
            frameIndex = (frameIndex + 1) % frames.Length;
        else
        {
            frameIndex++;
            if (frameIndex >= frames.Length)
            {
                CancelInvoke("NextFrame");
                if(canDestroy)
                    photonView.RPC("DestroyB", RpcTarget.AllBuffered);
                else
                    this.gameObject.SetActive(false);
            }
        }
    }

    [PunRPC]
    void DestroyB()
    {
        Destroy(this.gameObject);
    }
}