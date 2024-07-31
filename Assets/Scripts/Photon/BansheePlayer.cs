using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using TMPro;
using Photon.Pun.UtilityScripts;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.U2D.Animation;
using System.Linq;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BansheePlayer : MonoBehaviourPun
{
    //public GameObject PlayerCam;
    public GameObject PlayerCanvas;
    //public GameObject Character;

    private bool isfacingRight = false; 
   
    [HideInInspector] public float HorizontalInput;
    [HideInInspector] public bool Jump;
    [HideInInspector] public bool JumpHeld;
    [HideInInspector] public bool Attack;
    [HideInInspector] public bool InputChanged;
    private PlayerMovementController playerMovementController;

    public WeaponController gunEquipController;
    public HealthController healthController;

    public bool DisableInputs = false;

    public Animator anim;

    public PlayerProfileData playerProfileData;

    private GameObject coin;

    public TextMeshProUGUI PlayerNameText;
    public string PlayerName;
    public GameObject PlayerObj;

    public bool isGunEquipped =  false;
    public bool isIdle = true;
    public bool isSwordEquipped =  false;
    public bool isdead = false;
    public float scaleFactor = 1.2f;
    //CHARACTER CUSTOM
    [SerializeField]
    private SpriteLibrary spriteLibrary = default;

    public SpriteResolver HairResolver = default;
    public SpriteResolver EyesResolver = default;
    public SpriteResolver ArmLeftResolver = default;
    public SpriteResolver ArmRightResolver = default;
    public SpriteResolver TopResolver = default;
    public SpriteResolver ArmForeLeftResolver = default;
    public SpriteResolver ArmForeRightResolver = default;
    public SpriteResolver LeftCalfResolver = default;
    public SpriteResolver LeftThighResolver = default;
    public SpriteResolver RightCalfResolver = default;
    public SpriteResolver RightThighResolver = default;
    public SpriteResolver ShoesLResolver = default;
    public SpriteResolver ShoesRResolver = default;

    public string Hair;
    public string Eyes;
    public string Top;
    public string ArmL;
    public string ArmR;
    public string ArmFL;
    public string ArmFR;
    public string CalfL;
    public string CalfR;
    public string ThighL;
    public string ThighR;
    public string Waist;
    public string ShoesL;
    public string ShoesR;

    public GameObject HairObj;
    public GameObject Mask;
    public GameObject Knicker;
    public GameObject[] Shorts;
    public PhotonView pView;
    public GameObject GroundCheck;
    public LayerMask groundLayer;
    public Sword sword;
    [SerializeField] private GameObject shild;

    private SpriteLibraryAsset LibraryAsset => spriteLibrary.spriteLibraryAsset;

    //private bool IsGrounded;
    private void Awake()
    {
        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 15;

        pView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            Gamemanager.instance.LocalPlayer = this;
            // Gamemanager.instance.SetPlayerState(1);
            //PlayerCam.SetActive(true);
            Gamemanager.instance.CameraTarget(this.gameObject);
            //IsGrounded = true;
            PlayerNameText.text = PhotonNetwork.NickName;
            PlayerName = PhotonNetwork.NickName;
            playerProfileData.username = PlayerName;
            PlayerNameText.color = Color.white;
            PlayerObj = this.gameObject;
        }
        else
        {
            PlayerNameText.text = pView.Owner.NickName;
            PlayerNameText.color = Color.red;
        }
    }



    private void Start()
    {
        gunEquipController = GetComponent<WeaponController>();
        playerMovementController = GetComponent<PlayerMovementController>();
        healthController = GetComponent<HealthController>();
        CharacterCustomise();
    }

   

    // Update is called once per frame
    void Update()
    {
        playerMovementController.SetEnableDisableInputs(DisableInputs);
        if (pView.IsMine && DisableInputs == false)
        {
            //IsGrounded = Physics2D.OverlapCircle(GroundCheck.transform.position, 0.02f, groundLayer);
            PlayerMovementControl();
            GunEquipControl();
            Shoot();
            // CheckInputs(); 
        }
    }

   
    public void CharacterCustomise()
    {
        HairObj.SetActive(true);
        string[] hairlabels = LibraryAsset.GetCategoryLabelNames(Hair).ToArray();
        HairResolver.SetCategoryAndLabel(Hair, hairlabels[(int)pView.Owner.CustomProperties["HairIndex"]]);

        string[] eyelabels = LibraryAsset.GetCategoryLabelNames(Eyes).ToArray();
        EyesResolver.SetCategoryAndLabel(Eyes, eyelabels[(int)pView.Owner.CustomProperties["EyeIndex"]]);

        CustomizeTop();

        string[] shoeLeftlabels = LibraryAsset.GetCategoryLabelNames(ShoesL).ToArray();
        ShoesLResolver.SetCategoryAndLabel(ShoesL, shoeLeftlabels[(int)pView.Owner.CustomProperties["ShoesIndex"]]);
        string[] shoeRightlabels = LibraryAsset.GetCategoryLabelNames(ShoesR).ToArray();
        ShoesRResolver.SetCategoryAndLabel(ShoesR, shoeRightlabels[(int)pView.Owner.CustomProperties["ShoesIndex"]]);





        //if ((bool)pView.Owner.CustomProperties["IsKnickersOn"] == true)
        //{
        //    Mask.gameObject.SetActive(true);
        //    HairObj.SetActive(false);
        //}
        //else
        //{
        //    Mask.gameObject.SetActive(false);
        //    HairObj.SetActive(true);
        //}

        //if ((bool)pView.Owner.CustomProperties["IsShortsOn"] == true)
        //{
        //    foreach (GameObject item in Shorts)
        //    {
        //        item.SetActive(true);
        //    }
        //}
        //else
        //{
        //    foreach (GameObject item in Shorts)
        //    {
        //        item.SetActive(false);
        //    }
        //}

        //if ((bool)pView.Owner.CustomProperties["IsMaskOn"] == true)
        //{
        //    Knicker.gameObject.SetActive(true);
        //}
        //else
        //{
        //    Knicker.gameObject.SetActive(false);
        //}
    }

    private void CustomizeTop()
    {
        string[] Toplabels = LibraryAsset.GetCategoryLabelNames(Top).ToArray();
        string[] RAlabels = LibraryAsset.GetCategoryLabelNames(ArmR).ToArray();
        string[] LAlabels = LibraryAsset.GetCategoryLabelNames(ArmL).ToArray();
        string[] RFAlabels = LibraryAsset.GetCategoryLabelNames(ArmFR).ToArray();
        string[] LFAlabels = LibraryAsset.GetCategoryLabelNames(ArmFL).ToArray();
        string[] LClabels = LibraryAsset.GetCategoryLabelNames(CalfL).ToArray();
        string[] LTlabels = LibraryAsset.GetCategoryLabelNames(ThighL).ToArray();
        string[] RClabels = LibraryAsset.GetCategoryLabelNames(CalfR).ToArray();
        string[] RTlabels = LibraryAsset.GetCategoryLabelNames(ThighR).ToArray();

       

        string strTopIndex = "TopIndex";
        TopResolver.SetCategoryAndLabel(Top, Toplabels[(int)pView.Owner.CustomProperties[strTopIndex]]);
        ArmLeftResolver.SetCategoryAndLabel(ArmL, LAlabels[(int)pView.Owner.CustomProperties[strTopIndex]]);
        ArmRightResolver.SetCategoryAndLabel(ArmR, RAlabels[(int)pView.Owner.CustomProperties[strTopIndex]]);

        ArmForeLeftResolver.SetCategoryAndLabel(ArmFL, LFAlabels[(int)pView.Owner.CustomProperties[strTopIndex]]);
        ArmForeRightResolver.SetCategoryAndLabel(ArmFR, RFAlabels[(int)pView.Owner.CustomProperties[strTopIndex]]);

        LeftCalfResolver.SetCategoryAndLabel(CalfL, LClabels[(int)pView.Owner.CustomProperties[strTopIndex]]);
        LeftThighResolver.SetCategoryAndLabel(ThighL, LTlabels[(int)pView.Owner.CustomProperties[strTopIndex]]);
        RightCalfResolver.SetCategoryAndLabel(CalfR, RClabels[(int)pView.Owner.CustomProperties[strTopIndex]]);
        RightThighResolver.SetCategoryAndLabel(ThighR, RTlabels[(int)pView.Owner.CustomProperties[strTopIndex]]);

    }

    public void ActivateShild(bool state)
    {
        shild.SetActive(state);
    }

   
    //Player Movement
    private void PlayerMovementControl()
    {
        
        // Get the current input states.
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var attack = Input.GetButtonDown("Fire1");



        // Set a boolean (true/false) value to indicate if any input state has changed since the last frame.
        InputChanged = (horizontalInput != HorizontalInput || attack != Attack);

        // Cache the new input states in public variables that can be read elsewhere.
        HorizontalInput = horizontalInput;
        Attack = attack;

        //if(Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        //{
        //    //Jump = true;
        //    //JumpHeld = true;
        //    //playerMovementController.SetJump(Jump);
        //}
        //else
        //{

        //}
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    Jump = false;
        //    JumpHeld = false;
        //}
       
            // Set inputs on Player Controllers.
        playerMovementController.SetHorizontalMovement(HorizontalInput);
        playerMovementController.SetJumpHeld(JumpHeld);
        
        //photonView.RPC("Flip", RpcTarget.AllBuffered);

        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            anim.transform.localScale = new Vector3(-scaleFactor, scaleFactor, scaleFactor);
            isfacingRight = true;
            pView.RPC(nameof(OnDirectionChanged_Right), RpcTarget.Others);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            anim.transform.localScale = Vector3.one * scaleFactor;
            isfacingRight = false;
            pView.RPC(nameof(OnDirectionChanged_Left), RpcTarget.Others);
        }
    }

    [PunRPC]
    void OnDirectionChanged_Right()
    {
        anim.transform.localScale = new Vector3(-scaleFactor, scaleFactor, scaleFactor);
    }

    [PunRPC]
    void OnDirectionChanged_Left()
    {
        anim.transform.localScale = Vector3.one * scaleFactor;
    }


    //Gun Equip
    private void GunEquipControl()
    {
        if (Input.GetKeyDown(KeyCode.E) )
        {
            gunEquipController.EquipWeapon();
            playerMovementController.ChangeAnimation(gunEquipController.animationID);
        }
    }

    //Shoot
    private void Shoot()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F))
        {
            playerMovementController.Shoot();
            if (gunEquipController.IsGunEquiped() == false && gunEquipController.IsSwordEquiped() == true)
                sword.EnableTrigger(true);

            gunEquipController.Shoot(isfacingRight);
        }
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (photonView.IsMine)
    //    {
    //        if (collision.gameObject.CompareTag("Ground"))
    //        {
    //            IsGrounded = true;
    //            playerMovementController.JumpFinised();
    //        }
    //    }
    //}

    //void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (photonView.IsMine)
    //    {
    //        if (collision.gameObject.CompareTag("Ground"))
    //        {
    //            IsGrounded = false;
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Coin")
        {
            ScoreUpdate();
            coin = collision.gameObject;
            PhotonView photonView = PhotonView.Get(this);
            if (PhotonNetwork.IsMasterClient)
            {
                GemCollected(coin);
            }
            else
            {
                int viewID = collision.GetComponent<PhotonView>().ViewID;
                Debug.Log("ViewID of gem: " + viewID);
                photonView.RPC("RPC_GemCollected", RpcTarget.MasterClient, viewID);

            }
        }

        if (collision.gameObject.tag == "Shield")
        {
            //coin = collision.gameObject;
            PhotonView photonView = PhotonView.Get(this);
            healthController.photonView.RPC("ShieldHealth", RpcTarget.AllBuffered);
            photonView.RPC("RPC_GemCollected", RpcTarget.MasterClient);
        }
    }

    public void GemCollected(GameObject Gem)
    {
        Gamemanager.instance.GRemoveSO(Gem);
        Gamemanager.instance.GRemoveOO(Gem);
        PhotonNetwork.Destroy(Gem);
    }

    [PunRPC]
    public void RPC_GemCollected(int viewId)
    {
        if(PhotonView.Find(viewId).gameObject != null)
        {
            PhotonNetwork.Destroy(PhotonView.Find(viewId).gameObject);
        }
        
        //Debug.Log("Coin Destroyed and synced");
    }

    public void ScoreUpdate()
    {
        if (pView.IsMine)
        {
            Gamemanager.instance.UpdateScore();
        }
    }

}


/*
[PunRPC]
private void Flip()
{
    if (isfacingRight && HorizontalInput < 0f || !isfacingRight && HorizontalInput > 0f)
    {
        isfacingRight = !isfacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;

        Vector3 CanvasLocalScale = PlayerCanvas.transform.localScale;
        CanvasLocalScale.x = 2f;
        PlayerCanvas.transform.localScale = CanvasLocalScale;

        if (transform.localScale.x < 0f)
        {
            CanvasLocalScale.x *= -1f;
            PlayerCanvas.transform.localScale = CanvasLocalScale;
        }
    }
}
*/
