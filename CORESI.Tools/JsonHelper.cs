using System;
using CORESI.Data.Tools;
using Newtonsoft.Json;

namespace CORESI.Tools
{
    public static class JsonHelper
    {
        public static void Deserialize<T>(this string json, out T deserializedObject) where T : class
        {
            if (json.IsValidData())
            {
                deserializedObject = JsonConvert.DeserializeObject<T>(json);
            }

            throw new ArgumentException("Invalid Input !");
        }

        public static bool TryDeserialize<T>(this string json, out T deserializedObject) where T : class
        {
            deserializedObject = null;
            if (json.IsValidData())
            {
                deserializedObject = JsonConvert.DeserializeObject<T>(json);
                if (deserializedObject != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
