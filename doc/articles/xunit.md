# Dapplo.Log.XUnit

XUnit runs your unit tests parallel, which can make reading your log output for the tests extremely hard.
Fortunately Dapplo.Log also has a XUnit logger, which can be found here:@Dapplo.Log.XUnit
This takes care of matching log output to facts, simplifying your ouput.

To use this, you add the nuget package:
```
Install-Package Dapplo.Log.XUnit
```

If you want to log from your test class itself, include a static one-liner in your test class:
```
private static readonly LogSource Log = new LogSource();
```

Add a Constructor to your test class which looks something like:
```
public MyClass(ITestOutputHelper testOutputHelper)
{
	LogSettings.RegisterDefaultLogger<XUnitLogger>(LogLevels.Verbose, testOutputHelper);
}
```

And you are set...