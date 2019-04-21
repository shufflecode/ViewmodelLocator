using System;
using System.Collections.Generic;
using System.Reflection;
using ViewModelLocator.ViewModels;

namespace ViewModelLocator.Locators
{
    /// <summary>
    ///     Resolves view models and corresponding DesignDataView models attributed with the <see cref="ViewModelAttribute" />
    ///     Attribute
    /// </summary>
    public class ViewModelLocator : ILocator
    {
        private readonly Dictionary<string, object> _registeredDesignDataObjects = new Dictionary<string, object>();
        private readonly Dictionary<string, object> _registeredViewModels = new Dictionary<string, object>();

        public ViewModelLocator()
        {
            LoadViewModels();
        }

        /// <summary>
        ///     Returns the viewmodel instance with the given name from the locator,
        ///     when the viewmodel could not be resolved, null will be returned
        /// </summary>
        /// <param name="name">The name the Viewmodel was registered with</param>
        /// <returns>Instance of an Viewmodel, attributed with the <see cref="ViewModelAttribute" /> Attribute</returns>
        public object this[string name] => Locate(name);

        public object Locate(string name)
        {
            if (App.IsInDesignMode)
            {
                if (!_registeredDesignDataObjects.ContainsKey(name))
                    return null;

                var vm = (ViewModelBase) _registeredDesignDataObjects[name];
                return vm;
            }
            else
            {
                if (!_registeredViewModels.ContainsKey(name))
                    return null;

                var vm = (ViewModelBase) _registeredViewModels[name];
                return vm;
            }
        }

        /// <summary>
        ///     Registers a viewmodel with the given name in the Locator
        /// </summary>
        /// <param name="name">the name used for resolving the Viewmodel</param>
        /// <param name="o"></param>
        public void Register(string name, object o)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (!_registeredViewModels.ContainsKey(name))
                _registeredViewModels.Add(name, o);

            else throw new Exception($"viewmodel with name '{name}' already registered");
        }

        private void LoadViewModels()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var currentAssembly in assemblies)
            {
                try
                {
                    foreach (var currentType in currentAssembly.GetTypes())
                    {
                        foreach (var customAttribute in currentType.GetCustomAttributes(true))
                        {
                            if (!(customAttribute is ViewModelAttribute viewModelAttribute)) continue;

                            if (!_registeredViewModels.ContainsKey(viewModelAttribute.Name))
                            {
                                var vmInstance = Activator.CreateInstance(currentType);
                                _registeredViewModels.Add(viewModelAttribute.Name, vmInstance);
                            }

                            var designDataType = viewModelAttribute.DesignDataType;
                            if (designDataType == null) continue;

                            var designDataInstance = Activator.CreateInstance(designDataType);
                            _registeredDesignDataObjects.Add(viewModelAttribute.Name, designDataInstance);
                        }
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                    //ignore to get rid of error messages in Blend
                }
            }
        }
    }
}