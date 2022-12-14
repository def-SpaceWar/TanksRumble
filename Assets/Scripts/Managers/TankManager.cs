using UnityEngine;

public class TankManager : MonoBehaviour {

    [SerializeField] private TankType[] _tankTypes;

    public static TankType[] TankTypes { get; private set; }

    private void Awake()
    {
        {
            int i = 1;

            foreach (TankType tankType in _tankTypes)
            {
                Debug.Log($"TankType {i}: {tankType.Name}");
                i++;
            }
        }

        TankTypes = _tankTypes;
    }

}
