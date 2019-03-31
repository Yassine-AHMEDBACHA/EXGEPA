// <copyright file="JsonHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools
{
    using System;
    using CORESI.Data.Tools;
    using Newtonsoft.Json;

    public static class JsonHelper
    {
        public static T Deserialize<T>(this string json)
            where T : class
        {
            if (json.IsValidData())
            {
                return JsonConvert.DeserializeObject<T>(json);
            }

            throw new ArgumentException("Invalid Input !");
        }

        public static bool TryDeserialize<T>(this string json, out T deserializedObject)
            where T : class
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
