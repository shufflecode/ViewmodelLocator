using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace ViewModelLocator
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        ///     detects via reflection whether we are in DesignMode or not
        ///     Idea from MVVM Light (https://github.com/lbugnion/mvvmlight/)
        /// </summary>
        public static bool IsInDesignMode => DetectDesignMode();

        private static bool DetectDesignMode()
        {
            try
            {
                var designPropertiesType =
                    Type.GetType($"{typeof(DesignerProperties).FullName}, {typeof(DesignerProperties).Assembly}");

                var dmp = designPropertiesType
                    .GetTypeInfo()
                    .GetDeclaredField(nameof(DesignerProperties.IsInDesignModeProperty))
                    .GetValue(null);

                var propertyDescriptorType =
                    Type.GetType(
                        $"{typeof(DependencyPropertyDescriptor).FullName}, {typeof(DependencyPropertyDescriptor).Assembly}");
                var frameWorkElementType =
                    Type.GetType($"{typeof(FrameworkElement).FullName}, {typeof(FrameworkElement).Assembly}");

                var methodInfos = propertyDescriptorType
                    .GetTypeInfo()
                    .GetDeclaredMethods("FromProperty")
                    .ToList();

                var fromProperty =
                    methodInfos.FirstOrDefault(mi => mi.IsPublic && mi.IsStatic && mi.GetParameters().Length == 2);

                if (fromProperty == null) return false;

                var descriptor = fromProperty.Invoke(null, new[] {dmp, frameWorkElementType});
                var metaDataProperty = propertyDescriptorType.GetTypeInfo().GetDeclaredProperty("Metadata");
                var metadata = metaDataProperty.GetValue(descriptor, null);
                var propertyMetaDataType =
                    Type.GetType($"{typeof(PropertyMetadata).FullName}, {typeof(PropertyMetadata).Assembly}");
                var defaultValueProperty = propertyMetaDataType.GetTypeInfo().GetDeclaredProperty("DefaultValue");

                var inDesignMode = (bool) defaultValueProperty.GetValue(metadata, null);

                return inDesignMode;
            }
            catch
            {
                return false;
            }
        }
    }
}