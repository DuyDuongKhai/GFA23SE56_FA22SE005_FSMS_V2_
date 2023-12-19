namespace FSMS.WebAPI.Configurations
{
    public class RedisConfiguration
    {
        public bool Enable { get; set; }
        public bool AbortConnect { get; set; }

        public string ConnectionString { get; set; }
    }
}
