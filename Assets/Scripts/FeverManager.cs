using UnityEngine;
using System.Collections;
public class FeverManager : MonoBehaviour
{
    private const float feverEndTime = 3f; //
    private const int feverscore = 1;
    private bool isfever = false;
    private float feverCurTime = 0f;

    private void SetFever()
    {
        isfever = true;
        feverCurTime = 0f;
    }

    private void EndFever()
    {
        isfever = false;
        feverCurTime = feverEndTime;
    }
    private IEnumerator TimeCount()
    {
        SetFever();
        while (feverEndTime > feverCurTime)
        {
            feverCurTime += Time.deltaTime;
            yield return null;
        }
        EndFever();
    }

    public void StartFever(int _Score) 
    {
        StartCoroutine(TimeCount());
    }

    public int FeverHit(int _Score)
    {
        if (feverEndTime > feverCurTime && isfever)
            _Score += feverscore;
        return _Score;
    }
}
