using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc.Html;

namespace DV.FrenteLoja
{
    public static class CheckBoxListHelper
    {
        public static HtmlString CheckBoxList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Expression<Func<TModel, bool?>> expressionConfirm)
        {
            PropertyInfo member = CheckBoxListHelper.GetPropertyInfo(htmlHelper.ViewData.Model, expression);
            PropertyInfo confirm = CheckBoxListHelper.GetPropertyInfo(htmlHelper.ViewData.Model, expressionConfirm);
          
            var keysEnum = Enum.GetNames(typeof(DV.FrenteLoja.Core.Contratos.Enums.CheckListEnum));

            var result = $@"<div class='row'><div class='col-md-6'><p> {CheckBoxListHelper.GetDisplayName(member)} </p></div>
                             <div class='col-md-6 text-align-end'>";

            foreach (var key in keysEnum)
            {
                var value = member.GetValue(htmlHelper.ViewData.Model);
                string select = "false";

                if (value != null)
                    select = value.ToString() == key ? "true" : "false";

                var radiosHTML = htmlHelper.RadioButtonFor(expression, new { @selected = select });

                var html = $@"<label class='checkList' selected='{select}'>
                            <input id='{ member.Name }' name='{ member.Name }'  value='{key}' type='radio'/> 
                                { CheckBoxListHelper.ResolverIcons(key)}
                            </label>";
                            
                result += html;
            }
        
            var checkHTML = htmlHelper.CheckBox(confirm.Name);

            result += $@"<label class='checkMark' selected='{confirm.GetValue(htmlHelper.ViewData.Model)?.ToString().ToLower()}'>
                                { checkHTML.ToString()} 
                                { CheckBoxListHelper.ResolverIcons("ServicoRealizado")}
                            </label>";
            result += "</div></div>";

            result.Replace("'", "\"");

            return new HtmlString(result);
        }

        private static string ResolverIcons(string checkListEnum)
        {
            switch (checkListEnum)
            {
                case "ValidadoOk":
                    return "<span class=\"icon-icon-check iconRadio\"></span>";
                case "ProgramarTroca":
                    return "<span class=\"icon-icon-warning iconRadio\"></span>";
                case "TrocaImediato":
                    return "<span class=\"icon-icon-danger iconRadio\"></span>";
                case "ServicoRealizado":
                    return "<span class=\"icon-icon-check2 iconRadioCheck\"></span>";
                default: return "<span class=\"icon-icon-angle-left iconRadio\" style='display:none;'></span>";

            }
        }

        private static string GetDisplayName(PropertyInfo property)
        {
            var customAttribute = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            return customAttribute.Name;
                
        }

        private static PropertyInfo GetPropertyInfo<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            if (propertyLambda == null)
                return null;

            MemberExpression member = propertyLambda.Body as MemberExpression;

            PropertyInfo propInfo = member.Member as PropertyInfo;
           
            return propInfo;
        }

        public static HtmlString CheckBoxSimple<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Expression<Func<TModel, bool?>> expressionConfirm, bool lastItem = false)
        {
            PropertyInfo member = CheckBoxListHelper.GetPropertyInfo(htmlHelper.ViewData.Model, expression);
            PropertyInfo confirm = CheckBoxListHelper.GetPropertyInfo(htmlHelper.ViewData.Model, expressionConfirm);

            var servicoRealizado = confirm.GetValue(htmlHelper.ViewData.Model)?.ToString().ToLower();

            switch (servicoRealizado)
            {
                case "true":
                    servicoRealizado = "ServicoRealizado";
                    break;
                case "false":
                    servicoRealizado = "NaoRealizado";
                    break;
                default:
                    servicoRealizado = "null";
                    break;
            };

            var tipoOperacao = member.GetValue(htmlHelper.ViewData.Model).ToString();
            var destacar = false;

            if ((servicoRealizado == "NaoRealizado" && tipoOperacao == "TrocaImediato") && tipoOperacao != "NaoSelecionado")
                destacar = true;

            var result = string.Empty;

            if (lastItem)
                result += $@"<div class='checkbox checkboxReport last' destacar={destacar.ToString().ToLower()}>";
            else
                result += $@" <div class='checkbox checkboxReport' destacar={destacar.ToString().ToLower()}>";

            result += $@"<div class='col-md-4' style='word-break: break-word;'>
                               { htmlHelper.LabelFor(expression)}
                            </div>";

            if (servicoRealizado == "NaoRealizado" && tipoOperacao == "TrocaImediato")
                result += "<div class='col-md-4' id='trocarImediato' style='word-wrap: break-word;'><p style='font-size: .75em;margin-top: 0.79em;'>Trocar imediatamente</p></div>";

            result += $@"<div class='col-md-4 text-align-end'>
                            <label class='checkMark' selected='true'>
                                { CheckBoxListHelper.ResolverIcons(tipoOperacao) }";

            if (servicoRealizado != "null")
                result += $@"{ CheckBoxListHelper.ResolverIcons(servicoRealizado) }";

            result += $@"</label></div></div>";

            result.Replace("'", "\"");

            return new HtmlString(result);
        }

        public static HtmlString CheckBoxSimplePdf<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Expression<Func<TModel, bool?>> expressionConfirm, bool lastItem = false)
        {
            PropertyInfo member = CheckBoxListHelper.GetPropertyInfo(htmlHelper.ViewData.Model, expression);
            PropertyInfo confirm = CheckBoxListHelper.GetPropertyInfo(htmlHelper.ViewData.Model, expressionConfirm);

            var servicoRealizado = confirm.GetValue(htmlHelper.ViewData.Model)?.ToString().ToLower();

            switch (servicoRealizado)
            {
                case "true":
                    servicoRealizado = "ServicoRealizado";
                    break;
                case "false":
                    servicoRealizado = "NaoRealizado";
                    break;
                default:
                    servicoRealizado = "null";
                    break;
            };

            var tipoOperacao = member.GetValue(htmlHelper.ViewData.Model).ToString();
            var destacar = false;

            if ((servicoRealizado == "NaoRealizado" && tipoOperacao == "TrocaImediato") && tipoOperacao != "NaoSelecionado")
                destacar = true;

            var result = string.Empty;

            if (lastItem)
                result += $@"<div class='checkbox checkboxReportPdf last' destacar={destacar.ToString().ToLower()}>";
            else
                result += $@" <div class='checkbox checkboxReportPdf' destacar={destacar.ToString().ToLower()}>";

            result += $@"<div class='pdf-expression-helper'>
                               { htmlHelper.LabelFor(expression)}
                            </div>";

            if (servicoRealizado == "NaoRealizado" && tipoOperacao == "TrocaImediato")
                result += "<div class='pdf-troca-helper'><p>Trocar imediatamente</p></div>";

            result += $@"<div class='pdf-realizado-helper'>
                            <label class='checkMark' selected='true'>
                                { CheckBoxListHelper.ResolverIcons(tipoOperacao) }";

            if (servicoRealizado != "null")
                result += $@"{ CheckBoxListHelper.ResolverIcons(servicoRealizado) }";

            result += $@"</label></div></div>";

            result.Replace("'", "\"");

            return new HtmlString(result);
        }

    }
}