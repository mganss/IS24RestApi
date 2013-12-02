using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IS24RestApi.Tests
{
    class AssertEx
    {
        public static async Task ThrowsAsync<TException>(Func<Task> func)
        {
            var expected = typeof(TException);
            Type actual = null;
            try
            {
                await func();
            }
            catch (Exception e)
            {
                actual = e.GetType();
            }
            Assert.Equal(expected, actual);
        }

        public static void Equal(object o1, object o2)
        {
            if (o1 == null && o2 == null) return;
            Assert.NotNull(o1);
            Assert.NotNull(o2);

            var type1 = o1.GetType();
            var type2 = o2.GetType();
            Assert.Equal(type1, type2);

            foreach (var prop in type1.GetProperties(BindingFlags.Public|BindingFlags.Instance).Where(p => p.CanRead))
            {
                var val1 = prop.GetValue(o1);
                var val2 = prop.GetValue(o2);

                if (prop.PropertyType.IsPrimitive || prop.PropertyType.IsEnum || prop.PropertyType == typeof(string) || prop.PropertyType == typeof(System.DateTime))
                {
                    Assert.Equal(val1, val2);
                }
                else if (prop.PropertyType.IsArray)
                {
                    if (val1 == null && val2 == null) return;

                    var arr1 = (Array)val1;
                    var arr2 = (Array)val2;
                    Assert.Equal(arr1.Length, arr2.Length);

                    for (int i = 0; i < arr1.Length; i++)
                    {
                        Equal(arr1.GetValue(i), arr2.GetValue(i));
                    }
                }
                else
                {
                    Equal(val1, val2);
                }
            }
        }
    }
}
