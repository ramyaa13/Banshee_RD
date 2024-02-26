using System.Linq;
using Unity.Collections;
using UnityEngine;

using UnityEngine.U2D;

using UnityEngine.U2D.Animation;

public class Swapper : MonoBehaviour
{
  #region Inspector

  [SerializeField]
  private SpriteLibrary spriteLibrary = default;

  [SerializeField]
  private SpriteResolver HeadResolver = default;

    public SpriteResolver NeckResolver = default;
    public SpriteResolver BodyResolver = default;
    public SpriteResolver ArmFResolver = default;
    public SpriteResolver ArmBResolver = default;
    public SpriteResolver LegFResolver = default;
    public SpriteResolver LegBResolver = default;

    public string Head;
    public string Neck;
    public string Body;
    public string ArmF;
    public string ArmB;
    public string LegF;
    public string LegB;



    [SerializeField]
  private string targetCategory = default;

  #endregion


  #region Properties

  private SpriteLibraryAsset LibraryAsset => spriteLibrary.spriteLibraryAsset;

  #endregion


  #region Methods

  //public void SelectRandom ()
  //{
  //  string[] labels =
  //    LibraryAsset.GetCategoryLabelNames(targetCategory).ToArray();
  //  int index = Random.Range(0, labels.Length);
  //  string label = labels[index];

  //  targetResolver.SetCategoryAndLabel(targetCategory, label);
  //}

    public void SelectCharacter(int c)
    {
        //Head
        string[] Headlabels = LibraryAsset.GetCategoryLabelNames(Head).ToArray();
        string Headlabel = Headlabels[c];
        HeadResolver.SetCategoryAndLabel(Head, Headlabel);

        //Neck
        string[] Necklabels = LibraryAsset.GetCategoryLabelNames(Neck).ToArray();
        string Necklabel = Necklabels[c];
        NeckResolver.SetCategoryAndLabel(Neck, Necklabel);

        //Body
        string[] Bodylabels = LibraryAsset.GetCategoryLabelNames(Body).ToArray();
        string Bodylabel = Bodylabels[c];
        BodyResolver.SetCategoryAndLabel(Body, Bodylabel);

        //Arm-f
        string[] ArmFlabels = LibraryAsset.GetCategoryLabelNames(ArmF).ToArray();
        string ArmFlabel = ArmFlabels[c];
        ArmFResolver.SetCategoryAndLabel(ArmF, ArmFlabel);

        //Arm-b
        string[] ArmBlabels = LibraryAsset.GetCategoryLabelNames(ArmB).ToArray();
        string ArmBlabel = ArmBlabels[c];
        ArmBResolver.SetCategoryAndLabel(ArmB, ArmBlabel);

        //Leg-f
        string[] LegFlabels = LibraryAsset.GetCategoryLabelNames(LegF).ToArray();
        string LegFlabel = LegFlabels[c];
        LegFResolver.SetCategoryAndLabel(LegF, LegFlabel);

        //Leg-b
        string[] LegBlabels = LibraryAsset.GetCategoryLabelNames(LegB).ToArray();
        string LegBlabel = LegBlabels[c];
        LegBResolver.SetCategoryAndLabel(LegB, LegBlabel);

    }

  public void InjectCustom (Sprite customSprite)
  {
    // Duplicate bones and poses
    string referenceLabel = HeadResolver.GetLabel();
    Sprite referenceHead =
      spriteLibrary.GetSprite(targetCategory, referenceLabel);
    SpriteBone[] bones = referenceHead.GetBones();
    NativeArray<Matrix4x4> poses = referenceHead.GetBindPoses();
    customSprite.SetBones(bones);
    customSprite.SetBindPoses(poses);

    // Inject new sprite
    const string customLabel = "customHead";
    spriteLibrary.AddOverride(customSprite, targetCategory, customLabel);
        HeadResolver.SetCategoryAndLabel(targetCategory, customLabel);
  }

  #endregion
}