using System.Reflection;
using UnityEngine;


namespace APPhysics.Utility
{
    public static class APUtility
    {
        // Credits to @Shaffe and to @vexe for this function.
        // https://answers.unity.com/questions/458207/copy-a-component-at-runtime.html
        // https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
        public static T CopyComponentInto<T>(T Original, GameObject Destination) where T : Component
        {
            System.Type Type = Original.GetType();

            Component Copy = Destination.AddComponent(Type);

            BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;
            PropertyInfo[] PropertiesInfo = Type.GetProperties(Flags);
            foreach (var Info in PropertiesInfo)
            {
                if (Info.CanWrite)
                {
                    Info.SetValue(Copy, Info.GetValue(Original, null), null);
                }
            }

            FieldInfo[] Fields = Type.GetFields(Flags);
            foreach (FieldInfo Field in Fields)
            {
                Field.SetValue(Copy, Field.GetValue(Original));
            }

            return Copy as T;
        }
    }
}
