using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Vector3 SpawnPoint;
    public static GameManager Instance { get; private set; }
    public MyBlockController Current { get; set; }

    private const int GridSizeX = 10;
    private const int GridSizeY = 20;

    public bool[,] Grid = new bool[GridSizeX, GridSizeY];

    [SerializeField, Range(.1f, 1f)] public float gameSpeed = 1f;

    [SerializeField] List<MyBlockController> listPrefabs;

    private List<MyBlockController> _listHistory = new List<MyBlockController>();

    public void UpdateDisplayPreview()
    {
        if(IsOpenTest == false) { return;}

        for(int i = 0 ; i < GridSizeX ; i++)
        {
            for(int j = 0 ; j < GridSizeY ; j++)
            {
                var active = Grid[i,j];
                var Sprite = previewDisplay[i,j];

                Sprite.color = active ? Color.green : Color.red;
            }
        }
    }
    #region 
    public bool IsOpenTest;

    [SerializeField] private SpriteRenderer displayDataPrefab;
    private SpriteRenderer[,] previewDisplay = new SpriteRenderer [GridSizeX, GridSizeY];
    #endregion


    private void Awake() 
    {
        Instance = this;
        if(IsOpenTest)
        {
            for(int i = 0 ; i < GridSizeX ; i++)
            {
                for(int j = 0 ; j < GridSizeY ; j++)
                {
                    SpriteRenderer Sprite = Instantiate(displayDataPrefab,transform);
                    Sprite.transform.position = new Vector3(i, j, 0); 

                    previewDisplay [i,j] = Sprite;
                }
            }
        }
    }

    private void Start() 
    {
        SpawnObject();
    }

    public void UpSpeed()
    {
        if(Input.GetKey(KeyCode.S))
        {
            gameSpeed = 0.1f;
        }
        else
        {
            gameSpeed = 1f;
        }
    }
    public float GetGameSpeed()
    {
        return gameSpeed;
    }

    public bool IsInside(List<Vector2> listCoordinate)
    {
        foreach(var coordinate in listCoordinate)
        {
            var x = Mathf.RoundToInt(coordinate.x);
            var y = Mathf.RoundToInt(coordinate.y);


            if(x < 0 || x >= GridSizeX)
            {
                //Alan dışına çıktı
                return false; 
            }

            if(y < 0 || y >= GridSizeY)
            {
                //Alan dışına çıktı
                return false; 
            }

            if (Grid[x,y])
            {
                //Hit Something
                return false;
            }
        }
        return true;
    }

    public void SpawnObject()
    {
        int randomIndex = Random.Range(0, listPrefabs.Count);
        var blockController = listPrefabs[randomIndex];

        MyBlockController newBlock = Instantiate(blockController, SpawnPoint, Quaternion.identity);

        _listHistory.Add(newBlock);

        UpdateDisplayPreview();
    }

    private bool IsFullRow(int index)
    {
        for(int i = 0; i < GridSizeX; i++)
        {
            if(!Grid[i,index])
            {
                return false;
            }
        }
        return true;
    }

    public void UpdateRemoveObjectController()
    {
        for(int i = 0; i < GridSizeY; i++)
        {
            bool isFull = IsFullRow(i);
            if(isFull)
            {
                //Remove Block
                foreach(MyBlockController myBlock in _listHistory)
                {

                    List<Transform> willDestroy = new List<Transform>();
                    foreach(Transform piece in myBlock.ListPiece)
                    {
                        int y = Mathf.RoundToInt(piece.position.y);
                        if(y == i)
                        {
                            //delete
                            willDestroy.Add(piece);
                        }
                        else if(y > i)
                        {
                            //move down
                            var position = piece.position;
                            position.y--;
                            piece.position = position;
                        }
                    }
                    //Remove Block
                    foreach(var parça in willDestroy)
                    {
                        myBlock.ListPiece.Remove(parça);
                        Destroy(parça.gameObject);
                    }
                }

                //Change Data
                for (int j = 0; j < GridSizeX; j++)
					Grid[j, i] = false;

				for (int j = i+1; j < GridSizeY; j++)
				for (int k = 0; k < GridSizeX; k++)
					Grid[k, j - 1] = Grid[k, j];
                

                //Call Again
                UpdateRemoveObjectController();
                return;
            }
        }
    }
}
