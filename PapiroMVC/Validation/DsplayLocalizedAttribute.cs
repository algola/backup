using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PapiroMVC.Validation
{

    public class DisplayNameLocalizedAttribute : DisplayNameAttribute    
    {
            public DisplayNameLocalizedAttribute(Type resourceName, string resourceKey)
            {
                this.resourceKey = resourceKey;
                this.resourceName = resourceName;
            }

            public override string DisplayName
            {
            get
            {
                var resman = new System.Resources.ResourceManager(resourceName.FullName,resourceName.Assembly);
                string displayName = resman.GetString(resourceKey);

                return string.IsNullOrEmpty(displayName)
                    ? string.Format("[[{0}]]", resourceKey)
                    : displayName;
            }
        }
     
        private string resourceKey { get; set; }
        private Type resourceName { get; set; }
    }

    
        public class TooltipAttribute : DescriptionAttribute
        {
            public TooltipAttribute(Type resourceName, string resourceKey)
            {
                this.resourceKey = resourceKey;
                this.resourceName = resourceName;
            }

            public override string Description
            {
            get
            {
                var resman = new System.Resources.ResourceManager(resourceName.FullName,resourceName.Assembly);
                string displayName = resman.GetString(resourceKey);

                return string.IsNullOrEmpty(displayName)
                    ? string.Format("[[{0}]]", resourceKey)
                    : displayName;
            }
        }
    
            private string resourceKey { get; set; }
            private Type resourceName { get; set; }
        }

 
    public static class HtmlHelperExtension
    {


        private static bool IsPropertyACollection(PropertyInfo property)
        {
            return property.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) != null;
        }

        private static String GetToolTip<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {

            Console.WriteLine(expression.Body);
            MemberExpression exp;

            var ret = String.Empty;
            try
            {
                exp = (MemberExpression)expression.Body;
            }
            catch  (Exception e)
            {
                if (expression.CanReduce)
                    expression.Reduce();
                exp = (MemberExpression)expression.Body;
                Console.WriteLine(e);
            }

            MetadataTypeAttribute[] metadataTypes = typeof(TModel).GetCustomAttributes(typeof(MetadataTypeAttribute), true).OfType<MetadataTypeAttribute>().ToArray();
            MetadataTypeAttribute metadata = metadataTypes.FirstOrDefault();

            if (metadata != null)
            {
                PropertyInfo[] properties = metadata.MetadataClassType.GetProperties();

                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (propertyInfo.Name == exp.Member.Name)
                    {
                        try
                        {
                            TooltipAttribute toolAttrib = (TooltipAttribute)propertyInfo.GetCustomAttributes(typeof(TooltipAttribute), true)[0];
                            ret = ((TooltipAttribute)toolAttrib).Description;
                        }
                        catch
                        {                        
                        }
                    }
                }
            }

            return ret;
        }

        private static TagBuilder createContainer(string tagName, object htmlAttributes)
        {
            TagBuilder tag = new TagBuilder(tagName);
            IDictionary<string, object> htmlAttributesDictionary = new RouteValueDictionary(htmlAttributes);
            tag.MergeAttributes(htmlAttributesDictionary);
            return tag;
        }

        /// <summary>
        /// HtmlHelper Extension
        /// This Method create automaticaly HTML with LabelFor + EditFor + Validation Message + Title displaing ToolTip Attribute
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IHtmlString AlgolaEditorFor<TModel, TProperty>(
                this HtmlHelper<TModel> html,
                Expression<Func<TModel, TProperty>> expression, object htmlAttribute = null )
        {
            var metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, html.ViewData);

            var algolaEditFor = new TagBuilder("div");
            algolaEditFor.Attributes.Add("Title", GetToolTip(html,expression));

            var labelFor = new TagBuilder("div");
            labelFor.AddCssClass("editor-label");           
            labelFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.LabelExtensions.LabelFor(html, expression);

            var editFor = new TagBuilder("div");
            editFor.AddCssClass("editor-field");
            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.EditorExtensions.EditorFor(html, expression, htmlAttribute);
//            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.InputExtensions.TextBoxFor(html, expression, htmlAttribute);
            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(html, expression);

            algolaEditFor.InnerHtml += labelFor;
            algolaEditFor.InnerHtml += editFor;

            return new HtmlString(algolaEditFor + Environment.NewLine);    
        }

        public static MvcHtmlString AlgolaAutocompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string actionName, string controllerName, object htmlAttributes = null)
        {
            string autocompleteUrl = UrlHelper.GenerateUrl(null, actionName, controllerName,
                                                           null,
                                                           html.RouteCollection,
                                                           html.ViewContext.RequestContext,
                                                           includeImplicitMvcValues: true);

            var metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, html.ViewData);

            var algolaEditFor = new TagBuilder("div");
            algolaEditFor.Attributes.Add("Title", GetToolTip(html, expression));

            var labelFor = new TagBuilder("div");
            labelFor.AddCssClass("editor-label");
            labelFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.LabelExtensions.LabelFor(html, expression);

            var editFor = new TagBuilder("div");
            editFor.AddCssClass("editor-field");
            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.InputExtensions.TextBoxFor(html, expression, new { data_autocomplete_url = autocompleteUrl });
            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(html, expression);

            algolaEditFor.InnerHtml += labelFor;
            algolaEditFor.InnerHtml += editFor;

            return new MvcHtmlString(algolaEditFor + Environment.NewLine);
        }


        public static string T(string resPath, string key)
        {
            return (string)HttpContext.GetLocalResourceObject(resPath, key);
        }


        public static MvcHtmlString AutocompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string actionName, string controllerName)
        {
            string autocompleteUrl = UrlHelper.GenerateUrl(null, actionName, controllerName,
                                                           null,
                                                           html.RouteCollection,
                                                           html.ViewContext.RequestContext,
                                                           includeImplicitMvcValues: true);
            return System.Web.Mvc.Html.EditorExtensions.EditorFor(html, expression, new { data_autocomplete_url = autocompleteUrl });
        }

     
        static MergedType<T1, T2> Merge<T1, T2>(T1 t1, T2 t2)
         {
            return new MergedType<T1, T2>(t1, t2);
         }


        public static string AlgolaNameFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            return metadata.DisplayName;
        }

    }


  class MergedType<T1, T2> : DynamicObject
  {
     T1 t1;
     T2 t2;
     Dictionary<string, object> members = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

     public MergedType(T1 t1, T2 t2)
     {
        this.t1 = t1;
        this.t2 = t2;
        foreach (System.Reflection.PropertyInfo fi in typeof(T1).GetProperties())
        {
           members[fi.Name] = fi.GetValue(t1, null);
        }
        foreach (System.Reflection.PropertyInfo fi in typeof(T2).GetProperties())
        {
           members[fi.Name] = fi.GetValue(t2, null);
        }
     }

     public override bool TryGetMember(GetMemberBinder binder, out object result)
     {
        string name = binder.Name.ToLower();
        return members.TryGetValue(name, out result);
     }
  
  }

  public static class THelper
  {
      public static string T(this HtmlHelper helper, string path, string key)
      {
          string ret = String.Empty;
          try
          {
              ret = (string)HttpContext.GetLocalResourceObject(path, key);
          }
          catch (Exception e)
          {
              ret = "Stringa non definita nel file della lingua";
              Console.WriteLine(e.Message);
          }
          return ret;
      }
  }

}

