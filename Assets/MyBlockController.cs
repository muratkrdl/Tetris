using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBlockController : MonoBehaviour
{
    public List<Transform> ListPiece => listPiece;
    [SerializeField] List<Transform> listPiece = new List<Transform>();

    private void Start() 
    {
        GameManager.Instance.Current = this;
        StartCoroutine(MoveDown());
    }


    IEnumerator MoveDown()
    {
        while(true)
        {
            float delay = GameManager.Instance.GetGameSpeed();
            yield return new WaitForSeconds(delay);
            bool isMoveable = GameManager.Instance.IsInside(GetPreviewPosition());

            if(isMoveable)
            {
                Move();
            }
            else
            {
                //New Object spawn
                foreach(var piece in listPiece)
                {
                    int x = Mathf.RoundToInt(piece.position.x);
                    int y = Mathf.RoundToInt(piece.position.y);

                    GameManager.Instance.Grid[x,y] = true;
                }

                GameManager.Instance.UpdateRemoveObjectController();

                GameManager.Instance.SpawnObject();
                break;
            }
        }
    }

    private List<Vector2> GetPreviewPosition()
    {
        var result = new List<Vector2>();
        foreach(var piece in listPiece)
        {
            var position = piece.position;
            position.y--;
            result.Add(position);
        }
        return result;
    }


    void Move()
    {
        var position = transform.position;
        position.y--;
        transform.position = position;
    }

}
