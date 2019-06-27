namespace GrowKitApiDTO
{
    /// <summary> The message body used for sending and recieving messages from the sensor stick.</summary>
    /// <typeparam name="T"> The message type atached to the message</typeparam>
    [System.Serializable]
    public class StickMessageDTO<T> where T : class
    {
        /// <summary> The type name of the message to determine the function of the message.</summary>
        public string Type { get; set; }
        /// <summary> The Id of the stick targeted by the message.</summary>
        public long ID_Stick { get; set; }
        /// <summary> The id of the master stick sending the message.</summary>
        public long ID_Master { get; set; }
        /// <summary>The attached message sent.</summary>
        public T Message { get; set; }
    }
}
