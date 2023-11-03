using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Configuration.Provider;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Extensions;

namespace Our.Community.Configuration.OptionsProvider.Provider
{
    public static class ConfigurationBuilderExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder">IConfigurationBuilder</param>
        /// <param name="databasedsn">DataSource name</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddOurConfiguration(this IConfigurationBuilder builder, string databasedsn)
        {
            var configroot = builder.Build();
            
            return builder.Add(new OurConfigurationSource { ConnectionString = configroot.GetConnectionString(databasedsn), ProviderName = configroot.GetConnectionStringProviderName(databasedsn), CmsId = configroot.GetValue<string>("Umbraco:CMS:Global:Id") });
        }
    }
}
