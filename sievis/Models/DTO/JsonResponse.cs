using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;

namespace sievis.Models
{
    [Serializable]
    public class JsonResponseBase
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    [Serializable]
    public class JsonResponse<T> : JsonResponseBase
    {
        public T Data { get; set; }
    }
}