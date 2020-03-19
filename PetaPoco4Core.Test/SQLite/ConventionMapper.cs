using System;
using System.Linq;
using System.Reflection;
using PetaPoco;

namespace PetaPoco4Core.Test.SQLite
{
    /// <summary>
    ///     Represents a configurable convention mapper.
    /// </summary>
    /// <remarks>
    ///     By default this mapper replaces <see cref="StandardMapper" /> without change, which means backwards compatibility
    ///     is kept.
    /// </remarks>
    public class ConventionMapper : DefaultMapper
    {
        /// <summary>
        ///     Gets or sets the from db convert logic.
        /// </summary>
        public Func<PropertyInfo, Type, Func<object, object>> FromDbConverter { get; set; }

        /// <summary>
        ///     Constructs a new instance of convention mapper.
        /// </summary>
        public ConventionMapper()
        {
            FromDbConverter = (pi, t) =>
            {
                if (pi != null)
                {
                    if (pi.GetCustomAttributes(typeof(ValueConverterAttribute), true).FirstOrDefault() is ValueConverterAttribute valueConverter)
                    {
                        return valueConverter.ConvertFromDb;
                    }
                }

                return null;
            };
        }

        /// <summary>
        ///     Supply a function to convert a database value to the correct property value
        /// </summary>
        /// <param name="targetProperty">The target property</param>
        /// <param name="sourceType">The type of data returned by the DB</param>
        /// <returns>A Func that can do the conversion, or null for no conversion</returns>
        public override Func<object, object> GetFromDbConverter(PropertyInfo targetProperty, Type sourceType)
        {
            return FromDbConverter?.Invoke(targetProperty, sourceType);
        }


    }

    /// <summary>
    ///     Represents an attribute which can decorate a POCO property to provide
    ///     functions to convert a value from database type to property type and
    ///     vice versa.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValueConverterAttribute : Attribute
    {
        /// <summary>
        ///     Function to convert property value to database type value.
        /// </summary>
        /// <param name="value">Property value</param>
        /// <returns>Converted database value</returns>
        public abstract object ConvertToDb(object value);

        /// <summary>
        ///     Function to convert database value to property type value.
        /// </summary>
        /// <param name="value">Database value</param>
        /// <returns>Converted property type value</returns>
        public abstract object ConvertFromDb(object value);
    }
}
