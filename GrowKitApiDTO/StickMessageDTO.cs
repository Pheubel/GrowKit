namespace GrowKitApiDTO
{
    public class StickMessageDTO<T> where T : class
    {
        public string Type { get; set; }
        public long ID_Stick { get; set; }
        public long ID_Master { get; set; }
        public T Message { get; set; }

    }
}
