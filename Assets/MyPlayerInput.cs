using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerInput : MonoBehaviour
{
    public bool isPressLeft => Input.GetKeyDown(KeyCode.A);
    public bool isPressRight => Input.GetKeyDown(KeyCode.D);
    public bool isPressUp => Input.GetKeyDown(KeyCode.W);

    private void Update() 
    {
        EveryBlockController();
    }

    private void EveryBlockController()
    {
        GameManager.Instance.UpSpeed();
        if(isPressLeft || isPressRight)
        {
            int hareketGirdisi = isPressLeft ? -1 : 1;
            bool isMoveable = GameManager.Instance.IsInside(GetPreviewHorizontalPosition(hareketGirdisi));
            if(isMoveable)
            {
                MoveHorizontal(hareketGirdisi);
            }
        }
        else if(isPressUp)
        {
            bool isRotateable = GameManager.Instance.IsInside(GetPreviewRotate());
            if(isRotateable)
            {
                Rotate();
            }
        }
    }

    private List<Vector2> GetPreviewHorizontalPosition(int hareketGirdisi)
    {
        List<Vector2> result = new List<Vector2>();
        List<Transform> listPiece = GameManager.Instance.Current.ListPiece;
        foreach(Transform piece in listPiece)
        {
            Vector3 position = piece.position;
            position.x += hareketGirdisi;
            result.Add(position);
        }
        return result;
    }

    private void MoveHorizontal(int value)
    {
        Transform Current = GameManager.Instance.Current.transform;
        Vector3 pos = Current.position;
        pos.x += value;
        Current.position = pos;
    }

    private void Rotate()
    {
        Transform Current = GameManager.Instance.Current.transform;
        Vector3 angles = Current.eulerAngles;
        angles.z += -90;
        Current.eulerAngles = angles;

    }

    private List<Vector2> GetPreviewRotate()
    {
        List<Vector2> result = new List<Vector2>();
        List<Transform> listPiece = GameManager.Instance.Current.ListPiece;
        var pivot = GameManager.Instance.Current.transform.position;
        
        foreach(Transform piece in listPiece)
        {
            Vector3 position = piece.position;
            position = position-pivot;
            position = new Vector3(position.y, -position.x, 0);
            position = position + pivot;

            result.Add(position);
        }
        return result;
    }

}
