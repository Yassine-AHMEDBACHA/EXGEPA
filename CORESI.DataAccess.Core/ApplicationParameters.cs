﻿
using System;
using System.Collections;
using System.ComponentModel.Composition;
using System.Linq;
using CORESI.Data;

namespace CORESI.DataAccess.Core
{
    [Export(typeof(IParameterProvider)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ApplicationParameters : IParameterProvider
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        GenericDAL<Parameter> SettingsService { get; set; }

        private ApplicationParameters()
        {
            SettingsService = new GenericDAL<Parameter>();
        }

        public T GetValue<T>(string parameterName, T defaultValue = default(T))
        {
            var paramter = this.Get(parameterName);
            if (paramter != null)
            {
                try
                {
                    return (T)Convert.ChangeType(paramter.Value, typeof(T));
                }
                catch (Exception)
                {
                    this.logger.ErrorFormat("Error while trying to convert parameter value, Parameter key : {0}, String value = {1}", paramter.Key, paramter.Value);
                    throw;
                }
            }
            else
            {
                return defaultValue;
            }
        }

        public T GetValue<T>(string parameterName)
        {
            var paramter = this.Get(parameterName);
            if (paramter != null)
            {
                return GetValue<T>(paramter);
            }
            else
            {
                throw new ArgumentNullException("Specified parameter not found");
            }
        }

        private T GetValue<T>(Parameter paramter)
        {
            try
            {
                return (T)Convert.ChangeType(paramter.Value, typeof(T));
            }
            catch (Exception)
            {
                this.logger.ErrorFormat("Error while trying to convert parameter value, Parameter key : {0}, String value = {1}", paramter.Key, paramter.Value);
                throw;
            }
        }

        public Parameter Get(string parameterName)
        {
            return SettingsService.SelectAll().FirstOrDefault(x => x.Key.ToUpper() == parameterName.ToUpper());
        }

        private bool ValuesAreSame(string oldValue, string newValue)
        {
            if (oldValue != null && newValue != null)
            {
                return oldValue.Equals(newValue);
            }
            if (oldValue == null && newValue == null)
            {
                return true;
            }
            return false;
        }

        public bool TrySetOrAdd(string key, object value)
        {
            var stringValue = value.ToString();
            var parameter = Get(key);
            if (parameter != null)
            {
                if (ValuesAreSame(parameter.Value, stringValue))
                {
                    return true;
                }

                parameter.Value = stringValue;
                return Set(parameter) > 0;
            }
            else
            {
                parameter = new Parameter()
                {
                    Key = key,
                    Value = stringValue,
                };
                return Add(parameter) > 0;
            }
        }

        public int Add(Parameter parameter)
        {
            return SettingsService.Add(parameter);
        }

        public int Set(Parameter parameter)
        {
            return SettingsService.Update(parameter);
        }

        public IDictionary GetAllParameters()
        {
            return SettingsService.SelectAll().ToDictionary(x => x.Key, x => x.Value);
        }

        public string GetStringValue(string parameterName)
        {
            return this.Get(parameterName)?.Value;
        }

        public T GetAndSetIfMissing<T>(string parameterName, T value)
        {
            var parameter = this.Get(parameterName);
            if (parameter != null)
            {
                return GetValue<T>(parameter);
            }
            this.TrySetOrAdd(parameterName, value);
            return value;
        }
    }
}