﻿using System;

namespace Microsoft.Maui.Automation.Driver;

public class AppDriverBuilder
{
	public static AppDriverBuilder WithConfig(string configFile)
	{
		var f = new FileInfo(configFile);
		if (!f.Exists)
			throw new FileNotFoundException(f.FullName);

		var config = new AutomationConfiguration();

		if (f.Extension.Equals(".yaml", StringComparison.OrdinalIgnoreCase))
			config = AutomationConfiguration.FromYaml(f.FullName);
		else if (f.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
			config = AutomationConfiguration.FromJson(f.FullName);
		else
			throw new NotSupportedException("Unsupported configuration file type.  Must be .json or .yaml");

		return new AppDriverBuilder(config);
    }

	public AppDriverBuilder()
	{
		Configuration = new AutomationConfiguration();
	}

    public AppDriverBuilder(IAutomationConfiguration configuration)
    {
		Configuration = configuration;
    }

    public readonly IAutomationConfiguration Configuration;

	public AppDriverBuilder WithAppId(string appId)
	{
		Configuration.AppId = appId;
		return this;
	}

    public AppDriverBuilder WithAppFilename(string appFilename)
    {
		Configuration.AppFilename = appFilename;
        return this;
    }

    public AppDriverBuilder UsingDevice(string device)
    {
		Configuration.Device = device;
        return this;
    }

    public AppDriverBuilder OnDevicePlatform(Platform devicePlatform)
    {
		Configuration.DevicePlatform = devicePlatform;
        return this;
    }

    public AppDriverBuilder WithAutomationPlatform(Platform automationPlatform)
    {
		Configuration.AutomationPlatform = automationPlatform;
        return this;
    }

	public IDriver Build()
		=> new AppDriver(Configuration);
}

public class AppDriver : Driver
{
	

	public AppDriver(IAutomationConfiguration configuration)
		: base(configuration)
	{
		Driver = configuration.DevicePlatform switch
		{
			Platform.Android => new AndroidDriver(configuration),
			Platform.Ios => new iOSDriver(configuration),
			Platform.Tvos => new iOSDriver(configuration),
			Platform.Maccatalyst => new iOSDriver(configuration),
			Platform.Winappsdk => new WindowsDriver(configuration),
			_ => throw new NotSupportedException($"Unsupported Device Platform: '{configuration.DevicePlatform}'")
		};
	}

	public readonly IDriver Driver;

	public override string Name => Driver.Name;

	public override Task Back()
		=> Driver.Back();

	public override Task ClearAppState()
		=> Driver.ClearAppState();

	public override Task<IEnumerable<Element>> FindElements(string propertyName, string pattern, bool isExpression = false, string ancestorId = "")
		=> Driver.FindElements(propertyName, pattern, isExpression, propertyName);

	public override Task<IDeviceInfo> GetDeviceInfo()
		=> Driver.GetDeviceInfo();

	public override Task<IEnumerable<Element>> GetElements()
		=> Driver.GetElements();

	public override Task<string> GetProperty(string elementId, string propertyName)
		=> Driver.GetProperty(elementId, propertyName);

	public override Task<PerformActionResult> PerformAction(string action, string elementId, params string[] arguments)
		=> Driver.PerformAction(action, elementId, arguments);

	public override Task InputText(string text)
		=> Driver.InputText(text);

	public override Task InstallApp()
		=> Driver.InstallApp();

	public override Task KeyPress(char keyCode)
		=> Driver.KeyPress(keyCode);

	public override Task LaunchApp()
		=> Driver.LaunchApp();

	public override Task LongPress(int x, int y)
		=> Driver.LongPress(x, y);

	public override Task OpenUri(string uri)
		=> Driver.OpenUri(uri);

	public override Task PullFile(string remoteFile, string localDirectory)
		=> Driver.PullFile(remoteFile, localDirectory);

	public override Task PushFile(string localFile, string destinationDirectory)
		=> Driver.PushFile(localFile, destinationDirectory);

	public override Task RemoveApp()
		=> Driver.RemoveApp();

	public override Task StopApp()
		=> Driver.StopApp();

	public override Task Swipe((int x, int y) start, (int x, int y) end)
		=> Driver.Swipe(start, end);

	public override Task Tap(int x, int y)
		=> Driver.Tap(x, y);

	public override Task Tap(Element element)
		=> Driver.Tap(element);

	public override void Dispose()
	{
		Driver.Dispose();
	}
}
