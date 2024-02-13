using Radishmouse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerTemp : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    public Color color;
    public Color resetColor;
    public AudioSource audioSource;

    public Image image;
    public PointerManager manager;


    public bool isNode;
    private bool isRock;
    public bool isFilled;
    public int pathId;
    public int id;

    public UILineRenderer[] uiPath;

    public void Init(PointerManager manager)
    {
        this.manager = manager;
    }

    public void ResetImage()
    {
        if (!this.isNode)
        {
            image.color = resetColor;
            isFilled = false;
            foreach (var item in uiPath)
            {
                item.color = resetColor;
            }
            pathId = -1;
        }
    }

    public void SetNode(int id, Color color)
    {
        foreach (var item in uiPath)
        {
            item.color = color;
        }

        pathId = id;
        isNode = true;
        isFilled = true;
        image.color = color;
        this.color = color;
    }

    public void Rock()
    {
        image.color = Color.black;
        isRock = true;
        isFilled = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isRock)
        {
            return;
        }
        manager.Reset();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isRock)
        {
            return;
        }
        var userPathIndex = manager.GetPath(pathId).IsInUserPath(this);

        if (isNode)
        {
            manager.activePath = manager.GetPath(pathId);
            var path = manager.GetPath(this.pathId);
            path.activePoint = this;
            path.ResetPath();
            manager.SetActive(this);
            path.userPath.Add(this);
        }
        else
        {
            if (userPathIndex != -1)
            {
                var path = manager.GetPath(this.pathId);
                manager.activePath = path;
                path.activePoint = this;
                manager.SetActive(this);
            }
        }


    }

    public void ResetUIPath() {
        foreach (var item in uiPath)
        {
            item.gameObject.SetActive(false) ;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isRock)
        {
            return;
        }
        var path = manager.GetActivePath();
        if (path.activePoint.id == this.id)
        {
            return;
        }
        if (manager.isActive)
        {
            if (!CheckMayPass())
            {
                return;
            }

            if (isNode)
            {
                var indexPath = path.IsInUserPath(this);
                if (indexPath != -1)
                {
                    path.DestroyFromIndex(indexPath);
                    path.activePoint = this;
                    return;
                }
                path.userPath.Add(this);
                
                manager.GetPath(this.pathId).CheckPath();
                manager.Check();
                FillPath();
                PlayClink();
            }
            else
            {

                var indexPath = path.IsInUserPath(this);
                var activePointIndexPath = path.IsInUserPath(path.activePoint);
                if (indexPath != -1)
                {
                    path.DestroyFromIndex(indexPath);
                }
                else
                {
                    if (Mathf.Abs(activePointIndexPath - path.userPath.Count) != 1)
                    {
                        FillPointer();
                        path.DestroyFromIndex(activePointIndexPath);
                        path.userPath.Add(this);
                        PlayClink();

                    }
                    else
                    {
                        FillPointer();
                        path.userPath.Add(this);
                        PlayClink();

                    }
                }
                path.activePoint = this;
            }
        }
    }

    private void FillPath()
    {
        var lastId = manager.activePointer.id;
        if (lastId + manager.xSize == this.id)
        {
            manager.activePointer.uiPath[0].gameObject.SetActive(true);
        }
        else if (lastId - 1 == this.id)
        {
            manager.activePointer.uiPath[1].gameObject.SetActive(true);

        }
        else if (lastId - manager.xSize == this.id)
        {
            manager.activePointer.uiPath[2].gameObject.SetActive(true);

        }
        else if (lastId + 1 == this.id)
        {
            manager.activePointer.uiPath[3].gameObject.SetActive(true);

        }
    }

    private void PlayClink()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
/*            audioSource.pitch =  Random.Range(0.4f, 0.45f);*/

        }
    }

    private bool CheckMayPass()
    {
        if (manager.activePointer.pathId % 10 == this.pathId % 10)
        {
            return true;
        }
        if (pathId != -1 && pathId != manager.activePointer.pathId)
        {
            return false;
        }

        var lastId = manager.activePointer.id;

        if (lastId + manager.xSize == this.id)
        {
            return true;
        }
        else if (lastId - 1 == this.id)
        {
            return true;

        }
        else if (lastId - manager.xSize == this.id)
        {
            return true;

        }
        else if (lastId + 1 == this.id)
        {
            return true;

        }


        return false;
    }

    private void FillPointer()
    {

        pathId = manager.activePointer.pathId;
        image.color = manager.activePointer.color;
        color = manager.activePointer.color;
        isFilled = true;

        foreach (var item in uiPath)
        {
            item.color = manager.activePointer.color;
        }
        FillPath();


        manager.SetActive(this);
    }



}
