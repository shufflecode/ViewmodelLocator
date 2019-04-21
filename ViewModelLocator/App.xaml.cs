using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace ViewModelLocator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// detects via reflection whether we are in DesignMode or not 
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                return Load1();
                //return Load2();
            }
        }

        private static bool Load1()
        {
            try
            {
                var designPropertiesType = Type.GetType($"{typeof(DesignerProperties).FullName}, {typeof(DesignerProperties).Assembly}");

                var dmp = designPropertiesType
                    .GetTypeInfo()
                    .GetDeclaredField(nameof(DesignerProperties.IsInDesignModeProperty))
                    .GetValue(null);

                var propertyDescriptorType = Type.GetType($"{typeof(DependencyPropertyDescriptor).FullName}, {typeof(DependencyPropertyDescriptor).Assembly}");
                var frameWorkElementType = Type.GetType($"{typeof(FrameworkElement).FullName}, {typeof(FrameworkElement).Assembly}");

                var methodInfos = propertyDescriptorType
                    .GetTypeInfo()
                    .GetDeclaredMethods("FromProperty")
                    .ToList();

                var fromProperty = methodInfos.FirstOrDefault(mi => mi.IsPublic && mi.IsStatic && mi.GetParameters().Length == 2);

                if (fromProperty == null)
                {
                    return false;
                }

                var descriptor = fromProperty.Invoke(null, new[] { dmp, frameWorkElementType });
                var metaDataProperty = propertyDescriptorType.GetTypeInfo().GetDeclaredProperty("Metadata");
                var metadata = metaDataProperty.GetValue(descriptor, null);
                var propertyMetaDataType = Type.GetType($"{typeof(PropertyMetadata).FullName}, {typeof(PropertyMetadata).Assembly}");
                var defaultValueProperty = propertyMetaDataType.GetTypeInfo().GetDeclaredProperty("DefaultValue");

                var inDesignMode = (bool)defaultValueProperty.GetValue(metadata, null);

                return inDesignMode;
            }
            catch
            {
                return false;
            }
        }

        private static bool Load2()
        {
            try
            {
                var dm =
                    Type.GetType(
                        "System.ComponentModel.DesignerProperties, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

                if (dm == null)
                {
                    return false;
                }

                var dmp = dm.GetTypeInfo().GetDeclaredField("IsInDesignModeProperty").GetValue(null);

                var dpd =
                    Type.GetType(
                        "System.ComponentModel.DependencyPropertyDescriptor, WindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                var typeFe =
                    Type.GetType("System.Windows.FrameworkElement, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

                if (dpd == null
                    || typeFe == null)
                {
                    return false;
                }

                var fromPropertys = dpd
                    .GetTypeInfo()
                    .GetDeclaredMethods("FromProperty")
                    .ToList();

                if (fromPropertys == null
                    || fromPropertys.Count == 0)
                {
                    return false;
                }

                var fromProperty = fromPropertys
                    .FirstOrDefault(mi => mi.IsPublic && mi.IsStatic && mi.GetParameters().Length == 2);

                if (fromProperty == null)
                {
                    return false;
                }

                var descriptor = fromProperty.Invoke(null, new[] { dmp, typeFe });

                if (descriptor == null)
                {
                    return false;
                }

                var metaProp = dpd.GetTypeInfo().GetDeclaredProperty("Metadata");

                if (metaProp == null)
                {
                    return false;
                }

                var metadata = metaProp.GetValue(descriptor, null);
                var tPropMeta = Type.GetType("System.Windows.PropertyMetadata, WindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

                if (metadata == null
                    || tPropMeta == null)
                {
                    return false;
                }

                var dvProp = tPropMeta.GetTypeInfo().GetDeclaredProperty("DefaultValue");

                if (dvProp == null)
                {
                    return false;
                }

                var dv = (bool)dvProp.GetValue(metadata, null);
                return dv;
            }
            catch
            {
                return false;
            }
        }
    }
}
