using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CreateAPath : MonoBehaviour
{
    [SerializeField]
    private GameObject Shadow;
    [SerializeField]
    private Transform ParentObject;

    private float TimeLeft;
    private float TimeInBetwen;
    private float DestroyIn;
    private float T = 0;
    private GameObject[] Objects = new GameObject[50];
    int i = 0;
    int g = 0;


    // Update is called once per frame
    void Update()
    {
        DestroyMeshes();
        TimeLeft -= Time.deltaTime;
    }

    public void StartCreating(float time, int amountPerSecond, float destoyDelta)
    {
        TimeInBetwen = 1f / (float)amountPerSecond;
        TimeLeft = time;
        DestroyIn = destoyDelta;
        StartCoroutine(CreateMeshes());
    }

    void CreateMesh()
    {
        Objects[i] = Instantiate(Shadow);
        Objects[i].transform.position = ParentObject.position;
        Objects[i].transform.rotation = ParentObject.rotation;
        Objects[i].transform.localScale = ParentObject.lossyScale;
        i++;
    }

    IEnumerator CreateMeshes()
    {
        yield return new WaitForSeconds(TimeInBetwen);
        CreateMesh();
        if (TimeLeft > 0)
        {
            StartCoroutine(CreateMeshes());
        }
    }

    void DestroyMeshes()
    {
        if (g <= i)
        {
            if (T <= 0)
            {
                Destroy(Objects[g]);

                for (int j = 0; j < g; j++)
                {
                    if (Objects[j] != null)
                    {
                        Destroy(Objects[j]);
                    }
                }

                T = DestroyIn;
                g++;
            }
            else
            {
                T -= Time.deltaTime;
            }
        }
        else
        {
            i = 0;
            g = 0;
        }
    }

}
