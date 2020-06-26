using System;
using Newtonsoft.Json;

namespace Shapematch_Restful_Service.Contracts
{
    //TODO: Create nested json object with its corresponding children

    [JsonObject, Serializable]
    public class GameLog
    {
        public int Id { get; set; }

        public float xTouch { get; set; }
    }


}

