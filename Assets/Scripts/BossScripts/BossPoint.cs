using UnityEngine;

public class BossPoint : MonoBehaviour
{
    public GameObject boss;

    public GameObject emiter;

    public int pointToBoss;

    private void Update()
    {
        if (HUD.Instance.Score >= pointToBoss && boss != null)
        {
            emiter.SetActive(false);
            boss.SetActive(true);
        }
        else if (boss == null)
            return;
    }
}