using System;
using System.Reflection;

namespace RastaMelkor
{
    public class DomainProxy : MarshalByRefObject
    {
        public void LoadAndInvoke(byte[] bytes, string methodName)
        {
            Assembly assembly = Assembly.Load(bytes);

            foreach (var type in assembly.GetTypes())
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    if ((method.Name.ToLower()).Equals(methodName.ToLower()))
                    {
                        try
                        {
                            object instance = Activator.CreateInstance(type);
                            method.Invoke(instance, new object[] { });

                            return;
                        }
                        finally
                        {
                            Array.Clear(bytes, 0, bytes.Length);
                        }
                    }
                }
            }
        }
    }
}
