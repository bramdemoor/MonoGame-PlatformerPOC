namespace PlatformerPOC.Domain
{
    public class ParallaxLayer
    {
        public float Speed { get; set; }
        public string BgKey { get; set; }

        public ParallaxLayer(float speed, string bgKey)
        {
            Speed = speed;
            BgKey = bgKey;
        }
    }
}