using Microsoft.Extensions.Configuration;

namespace Our.Community.Configuration.OptionsProvider.Provider
{
    public class OurConfigurationSource : IConfigurationSource
    {
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
        public string CmsId { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder) => new OurConfigurationProvider(this);
    }
}
