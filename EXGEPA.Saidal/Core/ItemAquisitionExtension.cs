using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORESI.Data;
using CORESI.IoC;
using CORESI.Tools;
using System.Linq.Expressions;
using EXGEPA.Model;
using EXGEPA.Core.Tools;

namespace EXGEPA.Saidal.Core
{
    internal static class ItemAquisitionExtension
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static IParameterProvider parameterProvider;
        private static readonly string separator;
        private static readonly string additionalCharacter;

        private static IDictionary<string, Func<Item, object>> properties;
        private static IDictionary<string, int> valueLenghs;

        static ItemAquisitionExtension()
        {
            ServiceLocator.Resolve(out parameterProvider);
            separator = parameterProvider.GetAndSetIfMissing("InterfaceSerializerSeparator", ",");
            additionalCharacter = parameterProvider.GetAndSetIfMissing("InterfaceAdditionalCharacter", " ");
            var valueExtractor = new List<Expression<Func<Item, object>>>
            {
                x=>x.InputSheet.Key,
                x =>21,
                x =>x.AquisitionDate,
                x=>x.Key,
                x=>x.Description,
                x=>x.Brand,
                x=>x.Model,
                
            };

            properties = valueExtractor.ToDictionary(x => PropertyTools.GetPropertyName(x), x => x.Compile());
            valueLenghs = properties.Keys.ToDictionary(x => x, x => parameterProvider.GetAndSetIfMissing($"InterfaceColumn-{x}-Lenght", 15));
        }

       
    }
}
