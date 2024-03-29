﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace CORESI.Security
{
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class RightManager
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static RightManager()
        {
            Transcoder = new Dictionary<string, string>();
            Transcoder["Ajouter"] = "Insert";
            Transcoder["Consultation"] = "Select";
            Transcoder["Supprimer"] = "Delete";
            Transcoder["Edition"] = "Print";
            Transcoder["Export"] = "Print";
            Transcoder["Selection"] = "Select";
            Transcoder["Modifier"] = "Update";
        }

        public static Dictionary<string, string> Transcoder { get;  }

        public Dictionary<string, bool> Rights { get; set; }


        public bool HasAccess(string actionName)
        {
            string key = string.Join("-", actionName.Split('-').Select(x => Transcode(x)));
            if (!Rights.TryGetValue(key, out bool hasRight))
                return true;
            return hasRight;
        }

        public void Initialize(Role role)
        {
            logger.Info($"loading role : {role?.Key}");
            this.Rights = role.Abilities.ToDictionary(x => $"{x.Resource.Key}-{x.Operation.Key}", x => x.HasAccess);
        }

        public static string Transcode(string code)
        {
            if (Transcoder.TryGetValue(code, out string transcodedCode))
            {
                return transcodedCode;
            }

            return code;
        }
    }
}

