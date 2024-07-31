using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CharacterCustomisation : MonoBehaviour
{
    [SerializeField]
    private SpriteLibrary spriteLibrary = default;

    public SpriteResolver HairResolver = default;
    public SpriteResolver EyesResolver = default;
    public SpriteResolver ArmLeftResolver = default;
    public SpriteResolver ArmRightResolver = default;
    public SpriteResolver ArmForeLeftResolver = default;
    public SpriteResolver ArmForeRightResolver = default;
    public SpriteResolver TopResolver = default;
    public SpriteResolver LeftCalfResolver = default;
    public SpriteResolver LeftThighResolver = default;
    public SpriteResolver RightCalfResolver = default;
    public SpriteResolver RightThighResolver = default;
    public SpriteResolver ShoesLResolver = default;
    public SpriteResolver ShoesRResolver = default;
    //public SpriteResolver WaistResolver = default;

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


    public TextMeshProUGUI HairtitleLabel;
    public TMP_Dropdown Hairdropdown;

    public TextMeshProUGUI EyestitleLabel;
    public TMP_Dropdown Eyesdropdown;

    public TMP_Dropdown Topsdropdown;
    public TMP_Dropdown Shoesdropdown;

    public Toggle MaskToggle;
    public Text MaskLable;
    public Toggle ShortsToggle;
    public Text ShortsLable;
    public Toggle KnickersToggle;
    public Text KnickersLable;

    public GameObject HT;
    public GameObject HairObj;
    public GameObject Mask;
    public GameObject Knicker;
    public GameObject[] Shorts;

    public int HairIndex;
    public int EyesIndex;
    public int TopsIndex;
    public int ShoesIndex;
    public bool IsKnickersOn;
    public bool IsShortsOn;
    //public bool IsMaskOn;
    private SpriteLibraryAsset LibraryAsset => spriteLibrary.spriteLibraryAsset;
    // Start is called before the first frame update
    void Start()
    {
        HairIndex = 0;
        EyesIndex = 0;
        TopsIndex = 0;
        ShoesIndex = 0;
        IsKnickersOn = true;
        IsShortsOn = true;
        //IsMaskOn = false;
        HairCustomise();
        EyesCustomise();
        TopCustomsie();
        ShoesCustomise();
        Mask.gameObject.SetActive(false);
        HairObj.gameObject.SetActive(true);
        //MaskValue();
        KnickersValue();
        ShortsValue();

        //MaskToggle.onValueChanged.AddListener(delegate { MaskValue(); });
        ShortsToggle.onValueChanged.AddListener(delegate { ShortsValue(); });
        KnickersToggle.onValueChanged.AddListener(delegate { KnickersValue(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HairCustomise()
    {
        // Set title
        //HairtitleLabel.text = Hair;
        string[] labels = LibraryAsset.GetCategoryLabelNames(Hair).ToArray();

        // Populate dropdown
        List<TMP_Dropdown.OptionData> spriteLabels = new List<TMP_Dropdown.OptionData>();
        foreach (string label in labels)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData(label);
            spriteLabels.Add(data);
        }

        Hairdropdown.options = spriteLabels;

        // Handle change
        Hairdropdown.onValueChanged.AddListener(optionIndex =>
        {
            string label = labels[optionIndex]; HairIndex = optionIndex;
            HairResolver.SetCategoryAndLabel(Hair, label);
        });
    }

    public void EyesCustomise()
    {
        // Set title
        EyestitleLabel.text = Eyes;
        string[] labels = LibraryAsset.GetCategoryLabelNames(Eyes).ToArray();

        // Populate dropdown
        List<TMP_Dropdown.OptionData> spriteLabels = new List<TMP_Dropdown.OptionData>();
        foreach (string label in labels)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData(label);
            spriteLabels.Add(data);
        }

        Eyesdropdown.options = spriteLabels;

        // Handle change
        Eyesdropdown.onValueChanged.AddListener(optionIndex =>
        {
            string label = labels[optionIndex]; EyesIndex = optionIndex;
            EyesResolver.SetCategoryAndLabel(Eyes, label);
        });
    }

    public void TopCustomsie()
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
        //string[] Waistlabels = LibraryAsset.GetCategoryLabelNames(Waist).ToArray();

        // Handle change
        Topsdropdown.onValueChanged.AddListener(optionIndex =>
        {

            if (optionIndex == 3)
            {
                foreach (GameObject item in Shorts)
                {
                    item.SetActive(false);
                }
            }
            else
            {
                foreach (GameObject item in Shorts)
                {
                    item.SetActive(true);
                }
            }

            TopsIndex = optionIndex;
            string Toplabel = Toplabels[optionIndex];
            string RAlabel = RAlabels[optionIndex];
            string LAlabel = LAlabels[optionIndex];
            string RFAlabel = RFAlabels[optionIndex];
            string LFAlabel = LFAlabels[optionIndex];

            string LClabel = LClabels[optionIndex];
            string LTlabel = LTlabels[optionIndex];
            string RClabel = RClabels[optionIndex];
            string RTlabel = RTlabels[optionIndex];
            //string Waistlabel = Waistlabels[optionIndex];


            TopResolver.SetCategoryAndLabel(Top, Toplabel);
            ArmLeftResolver.SetCategoryAndLabel(ArmL, LAlabel);
            ArmRightResolver.SetCategoryAndLabel(ArmR, RAlabel);
            ArmForeLeftResolver.SetCategoryAndLabel(ArmFL, LFAlabel);
            ArmForeRightResolver.SetCategoryAndLabel(ArmFR, RFAlabel);

            LeftCalfResolver.SetCategoryAndLabel(CalfL, LClabel);
            LeftThighResolver.SetCategoryAndLabel(ThighL, LTlabel);
            RightCalfResolver.SetCategoryAndLabel(CalfR, RClabel);
            RightThighResolver.SetCategoryAndLabel(ThighR, RTlabel);
            //WaistResolver.SetCategoryAndLabel(Waist, Waistlabel);

        });
    }

    public void ShoesCustomise()
    {
        // Set title
        string[] ShoesL_labels = LibraryAsset.GetCategoryLabelNames(ShoesL).ToArray();
        string[] ShoesR_labels = LibraryAsset.GetCategoryLabelNames(ShoesR).ToArray();

        // Populate dropdown
        List<TMP_Dropdown.OptionData> spriteLabels = new List<TMP_Dropdown.OptionData>();
        foreach (string label in ShoesL_labels)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData(label);
            spriteLabels.Add(data);
        }

        Shoesdropdown.options = spriteLabels;

        // Handle change
        Shoesdropdown.onValueChanged.AddListener(optionIndex =>
        {
            string L_label = ShoesL_labels[optionIndex]; ShoesIndex = optionIndex;
            ShoesLResolver.SetCategoryAndLabel(ShoesL, L_label);
            string R_label = ShoesR_labels[optionIndex]; ShoesIndex = optionIndex;
            ShoesRResolver.SetCategoryAndLabel(ShoesR, R_label);

        });
    }


    public void MaskValue1()
    {
        if (MaskToggle.isOn)
        {
            Mask.gameObject.SetActive(true);
            //IsMaskOn = true;
            HairObj.SetActive(false);
            HT.SetActive(false);
            MaskLable.text = "Mask On";
        }
        else
        {
            Mask.gameObject.SetActive(false);
            //IsMaskOn = false;
            HairObj.SetActive(true);
            HT.SetActive(true);
            MaskLable.text = "Mask Off";
        }
    }

    public void ShortsValue()
    {
        if (ShortsToggle.isOn)
        {
            foreach (GameObject item in Shorts)
            {
                item.SetActive(true);
            }
            IsShortsOn = true;
            ShortsLable.text = "Shorts On";
        }
        else
        {
            foreach (GameObject item in Shorts)
            {
                item.SetActive(false);
            }
            IsShortsOn = false;
            ShortsLable.text = "Shorts Off";
        }
    }

    public void KnickersValue()
    {
        if (KnickersToggle.isOn)
        {
            Knicker.gameObject.SetActive(true);
            IsKnickersOn = true;
            KnickersLable.text = "Knickers On";
        }
        else
        {
            Knicker.gameObject.SetActive(false);
            IsKnickersOn = false;
            KnickersLable.text = "Knickers Off";
        }
    }
}
