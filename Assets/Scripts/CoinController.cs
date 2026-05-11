using UnityEngine;

public class CoinController : MonoCache
{
    public bool isCollect = false;
    public GameObject children;
    private void Start()
    {

        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        transform.eulerAngles = new Vector3(0, 0, 0);

        if (transform.parent.localEulerAngles.z > 20)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.2f, transform.localPosition.z);
            transform.eulerAngles = new Vector3(0, 0, 30);
        }
    }
    public override void OnFixedTick()
    {
        children.transform.localRotation = Quaternion.Euler(children.transform.localEulerAngles.x, children.transform.localEulerAngles.y + 120 * Time.fixedDeltaTime, children.transform.localEulerAngles.z);
    }

    private void OnDestroy()
    {
        if (isCollect) 
        {
            MyPlayerController.PlayAudioTakingCoin();
            MyPlayerController.coinsLevel += 1;
            MyPlayerController.needUpdateCoinCounter = true;
        }
    }
}
