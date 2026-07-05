using UnityEngine;

public class TempDebugUI : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    private void OnEnable()
    {
        gameController.OnZoneChanged += zone =>
            Debug.Log($"[Zone] Index={zone.ZoneIndex} Type={zone.Type} AllowsLeave={zone.AllowsLeave}");

        gameController.OnSpinResolved += result =>
            Debug.Log(result.IsBomb
                ? "[Spin] BOMB HIT!"
                : $"[Spin] Reward: {result.Slice.reward.rewardId}");

        gameController.OnGameOver += () =>
            Debug.Log($"[GameOver] Total collected value = {gameController.GetTotalValue()}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) gameController.Spin();
        if (Input.GetKeyDown(KeyCode.C)) gameController.ContinueToNextZone();
        if (Input.GetKeyDown(KeyCode.L)) gameController.Leave();
        if (Input.GetKeyDown(KeyCode.R)) gameController.StartNewRun();
    }
}