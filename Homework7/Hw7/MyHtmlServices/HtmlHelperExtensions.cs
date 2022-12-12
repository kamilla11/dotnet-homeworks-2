using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var model = helper.ViewData.Model;
        var type = helper.ViewData.ModelMetadata.ModelType;
        var divForm = new HtmlContentBuilder();

        foreach (var property in type.GetProperties())
        {
            divForm.AppendHtmlLine($"<div>");
            var name = GetDisplayAttribute(property);
            divForm.AppendHtmlLine($"<label for=\"{property.Name}\">{name}</label><br>");
            AddInputField(property, divForm, model);
            divForm.AppendHtmlLine($"</div>");
        }

        return divForm;
    }

    private static string GetDisplayAttribute(PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<DisplayAttribute>();
        if (attribute is not null) return attribute.Name!;

        return Regex.Replace(property.Name, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
    }

    private static void AddInputField(PropertyInfo property, HtmlContentBuilder divForm, object? model)
    {
        var propertyType = property.PropertyType;
        string value = "";
        string extraAttributes = "";
        if (model is not null)
        {
            var modelValue = property.GetValue(model);
            if (modelValue is not null) value = modelValue.ToString()!;
        }

        string mainAttributes = $"id=\"{property.Name}\" name=\"{property.Name}\" value=\"{value}\"";
        var typeAttribute = "text";
        if (propertyType.IsEnum)
        {
            divForm.AppendHtmlLine($"<select {mainAttributes}>");
            foreach (var item in propertyType.GetEnumValues())
            {
                divForm.AppendHtmlLine($"<option>{item}</option>");
            }
            divForm.AppendHtmlLine($"</select>");
            return;
        }

        if (propertyType == typeof(int))
        {
            var minMax = GetRangeAttribute(property);
            extraAttributes = $"min=\"{minMax.Item1}\" max=\"{minMax.Item2}\"";
            typeAttribute = "number";
        }

        divForm.AppendHtmlLine($"<input {mainAttributes} {extraAttributes} type=\"{typeAttribute}\" />");

        if (model is not null) Validate(model, property, divForm);
    }

    private static void Validate(object model, PropertyInfo property, HtmlContentBuilder divForm)
    {
        var attributes = property.GetCustomAttributes<ValidationAttribute>();
        foreach (var attribute in attributes)
        {
            if (!attribute.IsValid(property.GetValue(model)))
            {
                divForm.AppendHtmlLine($"<span>{attribute.ErrorMessage}</span>");
            }
        }
    }

    [ExcludeFromCodeCoverage]
    private static Tuple<string, string> GetRangeAttribute(PropertyInfo property)
    {
        var attr = property.GetCustomAttribute<RangeAttribute>();
        if (attr is not null)
        {
            return new Tuple<string, string>(attr.Minimum.ToString()!, attr.Maximum.ToString()!);
        }

        return new Tuple<string, string>("", "");
    }
}