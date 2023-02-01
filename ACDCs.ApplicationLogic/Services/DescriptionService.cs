using ACDCs.ApplicationLogic.Interfaces;
using ACDCs.Data.ACDCs.Components.BJT;
using ACDCs.Data.ACDCs.Components.Inductor;
using ACDCs.Data.ACDCs.Components.Resistor;
using ACDCs.Data.ACDCs.Interfaces;
using SpiceSharp.Attributes;

namespace ACDCs.ApplicationLogic.Services;

public class DescriptionService : IDescriptionService
{
    private static readonly Dictionary<Type, Type> s_descriptionTypes = new()
    {
        {typeof(Resistor), typeof(SpiceSharp.Components.Resistors.Parameters)},
        {typeof(Bjt), typeof(SpiceSharp.Components.Bipolars.Parameters)},
        {typeof(Inductor), typeof(SpiceSharp.Components.Inductors.Parameters)}
    };

    public string GetComponentDescription(Type parentType, string propertyName)
    {
        if (parentType.GetInterfaces().All(i => i != typeof(IElectronicComponent)))
        {
            return "";
        }

        if (!s_descriptionTypes.ContainsKey(parentType))
        {
            return "";
        }

        Type targetNamespaceType = s_descriptionTypes[parentType];
        List<Type> targetTypes = targetNamespaceType.Assembly.GetTypes()
            .Where(type => type.Namespace == targetNamespaceType.Namespace).ToList();

        foreach (var parameterInfo in targetTypes.Select(targetType => targetType.GetProperty(propertyName))
                     .Where(property => property != null)
                     .Select(property =>
                         property?.CustomAttributes.FirstOrDefault(attr =>
                             attr.AttributeType == typeof(ParameterInfoAttribute)))
                     .Where(parameterInfo => parameterInfo != null))
        {
            return Convert.ToString(parameterInfo?.ConstructorArguments.First().Value) ?? string.Empty;
        }

        return "";
    }

    public int GetComponentPropertyOrder(Type parentType, string propertyName)
    {
        if (parentType.GetInterfaces().All(i => i != typeof(IElectronicComponent)))
        {
            return 0;
        }

        if (!s_descriptionTypes.ContainsKey(parentType))
        {
            return 0;
        }

        Type targetNamespaceType = s_descriptionTypes[parentType];
        List<Type> targetTypes = targetNamespaceType.Assembly.GetTypes()
            .Where(type => type.Namespace == targetNamespaceType.Namespace).ToList();

        foreach (var parameterInfo in targetTypes.Select(targetType => targetType.GetProperty(propertyName))
                     .Where(property => property != null)
                     .Select(property =>
                         property.CustomAttributes.FirstOrDefault(attr =>
                             attr.AttributeType == typeof(ParameterInfoAttribute)))
                     .Where(parameterInfo => parameterInfo != null))
        {
            int order = 10;
            if (parameterInfo != null)
            {
                bool interesting = Convert.ToBoolean(parameterInfo.NamedArguments.FirstOrDefault(argument => argument.MemberName == "Interesting").TypedValue.Value ?? true);
                order += interesting ? -2 : 2;
                bool isPrincipal = Convert.ToBoolean(parameterInfo.NamedArguments
                    .FirstOrDefault(argument => argument.MemberName == "IsPrincipal").TypedValue.Value ?? false);
                order += isPrincipal ? -5 : 1;
            }
            return order;
        }

        return 0;
    }
}
