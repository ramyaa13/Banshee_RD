using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] PolygonCollider2D polygonCollider2D;
    public PolygonCollider2D PolygonCollider2D { get { return polygonCollider2D; } }

    //public CinemachineVirtualCamera CMvcam1;
    //private void OnEnable()
    //{
    //    CMvcam1.GetComponent<CinemachineConfiner>().m_BoundingShape2D = PolygonCollider2D;
    //}

}

