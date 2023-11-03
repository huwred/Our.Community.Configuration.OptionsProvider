# Our.Community.Configuration.OptionsProvider
IOptions provider add-on for Our.Community.Configuration

## Setup

In order to use the IOptions provider you will need to add a class to hold your Model
```
    public class OurConfigOptions
    {
        //Add your Configuration properties added in the back office here
        //The name should match the name in the config setting (minus spaces)
        public string? TestEncryptedString { get; set; }

        public string? TestBool{ get; set; }
    }
```

Add the following line to the ConfigureServices method in Startup.cs

```
services.Configure<OurConfigOptions>(_config);
```

That's it, you can now access the settings using

```
@inject IOptions<OurConfigOptions>  ConfigOptions
@{

    var test = ConfigOptions.Value.TestBool;
}
```