namespace GameFrameWork
{
    public class GameTime
    {
        public GameTime(float deltaTime)
        {
            DeltaTime = deltaTime;
        }

        // Time elapsed since the last update
        public float DeltaTime { get; set; } = 1f;
    }
}