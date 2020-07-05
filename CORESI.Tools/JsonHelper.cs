// <copyright file="JsonHelper.cs" company="CORESI">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools
{
    using System;
    using CORESI.Data.Tools;
    using Newtonsoft.Json;

    public static class JsonHelper
    {
        public static void Deserialize<T>(this string json, out T deserializedObject)
            where T : class
        {
            if (json.IsValid())
            {
                deserializedObject = JsonConvert.DeserializeObject<T>(json);
            }

            throw new ArgumentException("Invalid Input !");
        }

        public static bool TryDeserialize<T>(this string json, out T deserializedObject)
            where T : class
        {
            if (json.IsValid())
            {
                deserializedObject = JsonConvert.DeserializeObject<T>(json);
                if (deserializedObject != null)
                {
                    return true;
                }
            }

            deserializedObject = null;
            return false;
        }

        public static string Serialize<T>(T instanceToSerialize)
        {
            return JsonConvert.SerializeObject(instanceToSerialize);
        }
    }
}
