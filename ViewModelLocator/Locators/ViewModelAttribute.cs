using System;

namespace ViewModelLocator.Locators
{
    /// <summary>
    ///     Attribute for Viewmodel classes
    /// </summary>
    public class ViewModelAttribute : Attribute
    {
        public ViewModelAttribute(string name, Type designDataType = null)
        {
            Name = name;
            DesignDataType = designDataType;
        }

        /// <summary>
        ///     Name to find Viewmodel, use in Xaml DataBinding 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Optional Design Time Viewmodel Class
        /// </summary>
        public Type DesignDataType { get; set; }
    }
}