using System.Linq.Expressions;

namespace TNZAPI.NET.Helpers
{
    public class PropertyHelper
    {
        public static T UpdatePropertyValue<T>(T obj, string propertyName, object newValue)
        {
            var propInfo = typeof(T).GetProperty(propertyName);
            if (propInfo != null && propInfo.CanWrite)
            {
                propInfo.SetValue(obj, newValue);
            }
            else
            {
                throw new ArgumentException($"Property '{propertyName}' cannot be written or does not exist on type '{typeof(T).Name}'.");
            }
            return obj;
        }

        public static T UpdatePropertyValue<T>(T obj, KeyValuePair<string, object> data)
        {
            return UpdatePropertyValue(obj, data.Key, data.Value);
        }

        public static void SetProperty<T>(T obj, Expression<Func<T, object>> propertyExpression, object value)
        {
            string propertyName = GetPropertyName(propertyExpression);
            UpdatePropertyValue(obj, propertyName, value);
        }

        private static string GetPropertyName<T>(Expression<Func<T, object>> propertyExpression)
        {
            MemberExpression memberExpression;
            if (propertyExpression.Body is UnaryExpression unaryExpression)
            {
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)propertyExpression.Body;
            }

            return memberExpression.Member.Name;
        }
        /*
        public static bool IsNewObject<T>(T obj)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (!prop.GetValue(obj).Equals(Activator.CreateInstance(prop.PropertyType)))
                {
                    return false;
                }
            }
            return true;
        }
        */

        public static bool IsNewObject<T>(T obj)
        {
            var defaultInstance = Activator.CreateInstance<T>();
            return obj.Equals(defaultInstance);
        }
    }
}
