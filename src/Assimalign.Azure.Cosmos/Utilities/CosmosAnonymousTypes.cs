using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Assimalign.Azure.Cosmos.Utilities
{
    internal static class CosmosAnonymousTypes
    {
        private static int _typeIndex = 0;
        private static ModuleBuilder assemblyBuilder = null;
        private static AssemblyName assembly = new AssemblyName() { Name = "AssimalignDynamicLinqTypes" };
        private static Dictionary<string, Type> builtTypes = new Dictionary<string, Type>();
        private static IDictionary<string, Type> _propertyCache = new Dictionary<string, Type>();

        static CosmosAnonymousTypes()
        {
            assemblyBuilder = AssemblyBuilder
                .DefineDynamicAssembly(assembly, AssemblyBuilderAccess.Run)
                .DefineDynamicModule(assembly.Name);
        }



        public static TypeBuilder GetTypeBuilder(string name = "Anomymous", TypeAttributes attributes = TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable)
        {
            _typeIndex++;

            return assemblyBuilder.DefineType($"{name}{_typeIndex}", attributes, typeof(object));
        }


        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyType"></param>
        public static void CreateProperty(this TypeBuilder builder, string propertyName, Type propertyType)
        {
            var field = builder.DefineField($"_{propertyName}", propertyType, FieldAttributes.Private);
            var property = builder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            var getMethodBuilder = builder.DefineMethod($"get_{propertyName}",
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.SpecialName,
                propertyType,
                Type.EmptyTypes);

            var setMethodBuilder = builder.DefineMethod($"set_{propertyName}",
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.SpecialName,
                null,
                new Type[] { propertyType });

            var getBuilder = getMethodBuilder.GetILGenerator();
            getBuilder.Emit(OpCodes.Ldarg_0);
            getBuilder.Emit(OpCodes.Ldfld, field);
            getBuilder.Emit(OpCodes.Ret);


            var setBuilder = setMethodBuilder.GetILGenerator();
            setBuilder.Emit(OpCodes.Ldarg_0);
            setBuilder.Emit(OpCodes.Ldarg_1);
            setBuilder.Emit(OpCodes.Stfld, field);
            setBuilder.Emit(OpCodes.Ret);

            property.SetGetMethod(getMethodBuilder);
            property.SetSetMethod(setMethodBuilder);
        }
    }
}
