#### WPF  global viewmodel locator demo

Shows the resolving of Viewmodels works during Runtime and DesignTime via global ViewmodelResolver
so the view itself does knot need to know the Viewmodel.

Decorate Viewmodel Class with Optional DesignTimeData:

```csharp
 [ViewModel("MainWindow", designDataType:typeof(MainWindowDesignData))]
 internal class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
 {
    ...
 }
```

set Datacontext to Locator in Xaml:

```xaml
<Window.DataContext>
        <Binding Source="{StaticResource GlobalVmLocator}" Path="[MainWindow]" />
</Window.DataContext>
```

DesignTime vs Runtime :

![Error](result.jpg)



----
Sadly this produces an Resharper error, because of the ILocator interface:

```csharp
 public interface ILocator
 {        
    void Register(string name, object o);
    object Locate(string name);
    object this[string name] { get; }
 }

````

![Error](Resharper.png)




When changed to IMainWindowViewModel, R# can lookup the Properties and the warnings are gone. 
This way the Locator obviously cannot be used globally. :(

```csharp
 public interface ILocator
 {        
    void Register(string name, object o);
    IMainWindowViewModel Locate(string name);
    object this[string name] { get; }
 }

````
![No more Warnings](Resharper_no_warnings.png)


Debugging tipps

- Start a second instance of Visual Studio 
- Attach the debugger to *XDesProc* Process (Xaml Designer Process)
- Close all Xaml designer windows and Reopen the one in which you wanna try to Debug the resolving of Design Instance Viewmodel
