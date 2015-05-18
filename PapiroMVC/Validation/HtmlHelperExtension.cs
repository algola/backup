using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PapiroMVC.Validation
{


    public static class RouteValueDictionaryExtensions
    {
        public static IHtmlString ToHtmlAttributes(this RouteValueDictionary dictionary)
        {
            var sb = new StringBuilder();
            foreach (var kvp in dictionary)
            {
                sb.Append(string.Format("{0}=\"{1}\" ", kvp.Key, kvp.Value));
            }
            return new HtmlString(sb.ToString());
        }
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
            catch (Exception e)
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
        public static IHtmlString AlgolaNoDescEditorFor<TModel, TProperty>(
                this HtmlHelper<TModel> html,
                Expression<Func<TModel, TProperty>> expression, object htmlAttribute = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, html.ViewData);


            string tool = "";
            var getTool = String.Empty;

            if (metadata.AdditionalValues.ContainsKey("ToolTip"))
            {
                getTool = ((string)metadata.AdditionalValues["Tooltip"]);
            }
            if (getTool != null && getTool != "")
            {
                tool = "<span class=\"help-button\" data-rel=\"popover\" data-trigger=\"hover\" data-placement=\"right\" data-content=\"" + getTool + "\" title=\"\" data-original-title=\"\">?</span>";
            }

            var algolaEditFor = new TagBuilder("div");
            algolaEditFor.AddCssClass("form-group");

            //    <label class="col-sm-3 control-label no-padding-right" for="form-field-1"> Text Field </label>

            //    <div class="col-sm-9">
            //        <input type="text" id="form-field-1" placeholder="Username" class="col-xs-10 col-sm-5">
            //    </div>
            //</div>

            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            {
                attrs.Add("class", "col-xs-10 col-sm-5");
                attrs.Add("placeholder", "Username");
            }
            htmlAttribute = attrs;

            //var labelFor = new TagBuilder("div");
            //labelFor.AddCssClass("editor-label col-sm-3 control-label no-padding-right");
            //labelFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.LabelExtensions.LabelFor(html, expression);

            var htmlatt = new RouteValueDictionary();
            htmlatt.Add("class", "col-sm-3 control-label no-padding-right");

            algolaEditFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.LabelExtensions.LabelFor(html, expression, htmlatt);


            var editFor = new TagBuilder("div");
            editFor.AddCssClass("controls col-sm-9");

            var valu = (metadata.Model) == null ? false : (bool)metadata.Model;

            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.InputExtensions.CheckBox(html, "", valu);


            var builder = new TagBuilder("span");
            builder.AddCssClass("grey");

            if (metadata.IsRequired)
                builder.InnerHtml = "*";
            else
                builder.InnerHtml = "&nbsp;&nbsp;";

            editFor.InnerHtml += builder;

            editFor.InnerHtml += tool;

            //            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.InputExtensions.TextBoxFor(html, expression, htmlAttribute);
            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(html, expression);

            //            algolaEditFor.InnerHtml += labelFor;


            algolaEditFor.InnerHtml += editFor;

            return new HtmlString(algolaEditFor + Environment.NewLine);
        }


        private const int TextAreaRows = 2;
        private const int TextAreaColumns = 20;

        // ...


        /// <summary>
        /// HtmlHelper Extension
        /// This Method create automaticaly HTML with LabelFor + EditFor + Validation Message + Title displaing ToolTip Attribute
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IHtmlString AlgolaTextAreaFor<TModel, TProperty>(
                this HtmlHelper<TModel> html,
                Expression<Func<TModel, TProperty>> expression, object htmlAttribute = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, html.ViewData);

            string tool = "";
            var getTool = String.Empty;

            if (metadata.AdditionalValues.ContainsKey("ToolTip"))
            {
                getTool = ((string)metadata.AdditionalValues["Tooltip"]);
            }
            if (getTool != null && getTool != "")
            {
                tool = "<span class=\"help-button\" data-rel=\"popover\" data-trigger=\"hover\" data-placement=\"right\" data-content=\"" + getTool + "\" title=\"\" data-original-title=\"\">?</span>";
            }

            var algolaEditFor = new TagBuilder("div");
            algolaEditFor.AddCssClass("form-group");

            //    <label class="col-sm-3 control-label no-padding-right" for="form-field-1"> Text Field </label>

            //    <div class="col-sm-9">
            //        <input type="text" id="form-field-1" placeholder="Username" class="col-xs-10 col-sm-5">
            //    </div>
            //</div>

            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            {
                attrs.Add("class", "col-xs-10 col-sm-5");
                attrs.Add("placeholder", "Username");
            }
            htmlAttribute = attrs;

            //var labelFor = new TagBuilder("div");
            //labelFor.AddCssClass("editor-label col-sm-3 control-label no-padding-right");
            //labelFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.LabelExtensions.LabelFor(html, expression);

            var htmlatt = new RouteValueDictionary();
            htmlatt.Add("class", "col-sm-3 control-label no-padding-right");

            algolaEditFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.LabelExtensions.LabelFor(html, expression, htmlatt);


            var editFor = new TagBuilder("div");
            editFor.AddCssClass("controls col-sm-9 autosize-transition");
            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(html, expression, htmlAttribute);

            var builder = new TagBuilder("span");
            builder.AddCssClass("grey");

            if (metadata.IsRequired)
                builder.InnerHtml = "*";
            else
                builder.InnerHtml = "&nbsp;&nbsp;";

            editFor.InnerHtml += builder;

            editFor.InnerHtml += tool;

            //            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.InputExtensions.TextBoxFor(html, expression, htmlAttribute);
            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(html, expression);

            //            algolaEditFor.InnerHtml += labelFor;


            algolaEditFor.InnerHtml += editFor;

            return new HtmlString(algolaEditFor + Environment.NewLine);
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
                Expression<Func<TModel, TProperty>> expression, object htmlAttribute = null, int txtLength = 12, bool readOnly = false, byte inTheCol = 1)
        {
            var metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, html.ViewData);

            //<div class="form-group">
            // <label class="col-sm-3 control-label no-padding-right" for="Customer">Cliente</label>
            // <div class="controls col-sm-9">
            //  <input class="col-xs-10 col-sm-5 text-box single-line" id="Customer" name="Customer" readonly="true" type="text" value="prova"><span class="grey">&nbsp;&nbsp;</span><span class="help-button" data-rel="popover" data-trigger="hover" data-placement="right" data-html="true" data-delay="{&quot;show&quot;:&quot;0&quot;, &quot;hide&quot;:&quot;3000&quot;}" data-content="Cliente" title="" data-original-title="">?</span>
            //  <span class="field-validation-valid" data-valmsg-for="Customer" data-valmsg-replace="true"></span>
            // </div>
            //</div>

            var mediaQuery = "col-xs-10 col-sm-5";

            if (inTheCol == 2)
            {
                mediaQuery = "col-xs-10 col-sm-7";
            }

            if (inTheCol == 3)
            {
                mediaQuery = "col-xs-10 col-sm-10";
            }


            string tool = "";
            var getTool = String.Empty;

            if (metadata.AdditionalValues.ContainsKey("ToolTip"))
            {
                getTool = ((string)metadata.AdditionalValues["Tooltip"]);
            }

            if (getTool != null && getTool != "")
            {
                tool = "<span class=\"help-button\" data-rel=\"popover\" data-trigger=\"hover\" data-placement=\"right\" data-html=\"true\" data-delay='{\"show\":\"0\", \"hide\":\"3000\"}' data-content=\"" + getTool + "\" title=\"\" data-original-title=\"\">?</span>";
            }

            var algolaEditFor = new TagBuilder("div");
            algolaEditFor.AddCssClass("form-group");

            //    <label class="col-sm-3 control-label no-padding-right" for="form-field-1"> Text Field </label>

            //    <div class="col-sm-9">
            //        <input type="text" id="form-field-1" placeholder="Username" class="col-xs-10 col-sm-5">
            //    </div>
            //</div>


            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            {
                string value;

                if (attrs.ContainsKey("class"))
                {
                    value = (string)attrs["class"];
                    value += " " + mediaQuery  ;
                    attrs["class"] = value;
                    Console.WriteLine(value);
                }
                else
                {
                    attrs.Add("class", mediaQuery);
                }
            }
            htmlAttribute = attrs;

            var htmlatt = new RouteValueDictionary();
            htmlatt.Add("class", "col-sm-3 control-label no-padding-right");

            algolaEditFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.LabelExtensions.LabelFor(html, expression, htmlatt);

            var editFor = new TagBuilder("div");
            editFor.AddCssClass("controls col-sm-9");

            if (readOnly)
            {
                editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.EditorExtensions.EditorFor(html, expression, new { htmlAttributes = new { @class = "col-xs-10 col-sm-5", @readonly = true } });
            }
            else
            {
                editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.EditorExtensions.EditorFor(html, expression, new { htmlAttributes = htmlAttribute });
            }


            var builder = new TagBuilder("span");
            builder.AddCssClass("grey");

            if (metadata.IsRequired)
                builder.InnerHtml = "*";
            else
                builder.InnerHtml = "&nbsp;&nbsp;";

            editFor.InnerHtml += builder;

            editFor.InnerHtml += tool;

            //            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.InputExtensions.TextBoxFor(html, expression, htmlAttribute);
            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(html, expression);

            //            algolaEditFor.InnerHtml += labelFor;


            algolaEditFor.InnerHtml += editFor;

            return new HtmlString(algolaEditFor + Environment.NewLine);
        }

        public static MvcHtmlString AlgolaAutocompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string actionName, string controllerName, object htmlAttributes = null)
        {
            /*
            string autocompleteUrl = UrlHelper.GenerateUrl(null, actionName, controllerName,
                                                           null,
                                                           html.RouteCollection,
                                                           html.ViewContext.RequestContext,
                                                           includeImplicitMvcValues: true);
            */

            string autocompleteUrl = UrlHelper.GenerateUrl(null, actionName, controllerName,
                                               null,
                                               html.RouteCollection,
                                               html.ViewContext.RequestContext,
                                               includeImplicitMvcValues: true);

            var metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, html.ViewData);


            string tool = "";
            var getTool = String.Empty;

            if (metadata.AdditionalValues.ContainsKey("ToolTip"))
            {
                getTool = ((string)metadata.AdditionalValues["Tooltip"]);
            }

            if (getTool != null && getTool != "")
            {
                tool = "<span class=\"help-button\" data-rel=\"popover\" data-trigger=\"hover\" data-placement=\"right\" data-content=\"" + getTool + "\" title=\"\" data-original-title=\"\">?</span>";
            }


            var algolaEditFor = new TagBuilder("div");
            algolaEditFor.Attributes.Add("Title", GetToolTip(html, expression));
            algolaEditFor.AddCssClass("form-group");

            var htmlatt = new RouteValueDictionary();
            htmlatt.Add("class", "col-sm-3 control-label no-padding-right");

            algolaEditFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.LabelExtensions.LabelFor(html, expression, htmlatt);


            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            {
                string value;

                if (attrs.ContainsKey("class"))
                {
                    value = (string)attrs["class"];
                    value += " col-xs-10 col-sm-5";
                    attrs["class"] = value;
                    Console.WriteLine(value);
                }
                else
                {
                    attrs.Add("class", "col-xs-10 col-sm-5");
                }
            }
            attrs.Add("data-autocomplete-url" , autocompleteUrl);
            htmlAttributes = attrs;



            var editFor = new TagBuilder("div");
            editFor.AddCssClass("controls col-sm-9");
//            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.InputExtensions.TextBoxFor(html, expression, new { });
            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.EditorExtensions.EditorFor(html, expression, new { htmlAttributes = htmlAttributes });


            var builder = new TagBuilder("span");
            builder.AddCssClass("grey");

            if (metadata.IsRequired)
                builder.InnerHtml = "*";
            else
                builder.InnerHtml = "&nbsp;&nbsp;";

            editFor.InnerHtml += builder;
            editFor.InnerHtml += tool;

            editFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(html, expression);

            algolaEditFor.InnerHtml += editFor;

            return new MvcHtmlString(algolaEditFor + Environment.NewLine);
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
        public static IHtmlString AlgolaDisplayFor<TModel, TProperty>(
                this HtmlHelper<TModel> html,
                Expression<Func<TModel, TProperty>> expression, object htmlAttribute = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, html.ViewData);

            var algolaEditFor = new TagBuilder("div");
            algolaEditFor.Attributes.Add("Title", GetToolTip(html, expression));

            var labelFor = new TagBuilder("div");
            labelFor.AddCssClass("display-label");
            labelFor.InnerHtml += Environment.NewLine + "\t\t" + System.Web.Mvc.Html.LabelExtensions.LabelFor(html, expression);

            labelFor.InnerHtml += ": " + System.Web.Mvc.Html.DisplayExtensions.DisplayFor(html, expression, htmlAttribute);

            algolaEditFor.InnerHtml += labelFor;

            return new HtmlString(algolaEditFor + Environment.NewLine);
        }

        public static MvcHtmlString DisplayWithBreaksFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var model = html.Encode(metadata.Model).Replace("\r\n", "<br />\r\n");

            if (String.IsNullOrEmpty(model))
                return MvcHtmlString.Empty;

            return MvcHtmlString.Create(model);
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

        // ActionLink con immagine
        public static MvcHtmlString ActionImage(this HtmlHelper html, string actionName, string controllerName, object routeValues, string imagePath, string alt)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            // build the <img> tag
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", url.Content(imagePath));
            imgBuilder.MergeAttribute("alt", alt);
            string imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

            // build the <a> tag
            var anchorBuilder = new TagBuilder("a");
            anchorBuilder.MergeAttribute("href", url.Action(actionName, controllerName, routeValues));
            anchorBuilder.InnerHtml = imgHtml; // include the <img> tag inside
            string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(anchorHtml);

        }

        // ActionLink con immagine da classe css
        public static MvcHtmlString ActionLinkImage(this HtmlHelper html, string linkText, string actionName, string controllerName, object routeValues, string className)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            //dove renderizzare tale link
            /*<a href="@Url.Action("Register", "Account", new { id = "registerLink"})">
                <i class="icon-key"></i>
                @Html.T(strings,"RegistrationAccount")
            </a>*/

            var icoBuilder = new TagBuilder("i");
            icoBuilder.AddCssClass(className);
            string icoHtml = icoBuilder.ToString(TagRenderMode.Normal);

            // build the <a> tag
            var anchorBuilder = new TagBuilder("a");
            anchorBuilder.MergeAttribute("href", url.Action(actionName, controllerName, routeValues));
            anchorBuilder.InnerHtml = icoHtml + " " + linkText; // include the <i> tag inside
            string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(anchorHtml);
        }

        // ActionLink con immagine da classe css e classe span
        public static MvcHtmlString ActionLinkImageSpan(this HtmlHelper html, string linkText, string actionName, string controllerName, object routeValues, string className, string classSpanName)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            //dove renderizzare tale link
            /*<a href="@Url.Action("Register", "Account", new { id = "registerLink"})">
                <i class="icon-key"></i>
                <span class="menu-text"> @Html.T(strings,"RegistrationAccount") </span>
            </a>*/

            var icoBuilder = new TagBuilder("i");
            icoBuilder.AddCssClass(className);
            string icoHtml = icoBuilder.ToString(TagRenderMode.Normal);

            var spanBuilder = new TagBuilder("span");
            spanBuilder.AddCssClass(classSpanName);
            spanBuilder.InnerHtml = linkText;
            string spanHtml = spanBuilder.ToString(TagRenderMode.Normal);

            // build the <a> tag
            var anchorBuilder = new TagBuilder("a");
            anchorBuilder.MergeAttribute("href", url.Action(actionName, controllerName, routeValues));
            anchorBuilder.InnerHtml = icoHtml + " " + spanHtml; // include the <i> and <span> tag inside
            string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(anchorHtml);
        }

        // ActionLink con immagine da classe css e classe del tipo di anchor
        public static MvcHtmlString ActionLinkButton(this HtmlHelper html, string linkText, string actionName, string controllerName, object routeValues, string classNameIcon, string classNameAnchor)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            //dove renderizzare tale link
            /*<a href="@Url.Action("Register", "Account", new { id = "registerLink"})" class="btn btn-app btn-info btn-sm no-radius">
                <i class="icon-key"></i>
                @Html.T(strings,"RegistrationAccount")
            </a>*/

            var icoBuilder = new TagBuilder("i");
            icoBuilder.AddCssClass(classNameIcon);
            string icoHtml = icoBuilder.ToString(TagRenderMode.Normal);

            // build the <a> tag
            var anchorBuilder = new TagBuilder("a");
            anchorBuilder.AddCssClass(classNameAnchor);
            anchorBuilder.MergeAttribute("href", url.Action(actionName, controllerName, routeValues));
            anchorBuilder.InnerHtml = icoHtml + " " + linkText; // include the <i> tag inside
            string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(anchorHtml);
        }

        public static IHtmlString ToolTipFor<TModel, TValue>(
                 this HtmlHelper<TModel> html,
                 Expression<Func<TModel, TValue>> expression)
        {
            var data = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
            if (data.AdditionalValues.ContainsKey("Tooltip"))
                return new HtmlString((string)data.AdditionalValues["Tooltip"]);

            return new HtmlString("");

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

        public static string Seo(this HtmlHelper helper, string path, string key, string type)
        {
            string ret = String.Empty;
            try
            {
                ret = (string)HttpContext.GetLocalResourceObject(path, key + type);
            }
            catch (Exception e)
            {
                ret = "Stringa non definita nel file della lingua";
                Console.WriteLine(e.Message);
            }
            return ret;
        }

        public static MvcHtmlString If(this MvcHtmlString value, bool evaluation)
        {
            return evaluation ? value : MvcHtmlString.Empty;
        }

    }

    public static class LocalizationHelper
    {
        public static IHtmlString MetaAcceptLanguage(this HtmlHelper html)
        {
            var acceptLang = HttpUtility.HtmlAttributeEncode(Thread.CurrentThread.CurrentUICulture.ToString());
            return new HtmlString(string.Format("<meta name=\"accept-language\" content=\"{0}\"/>", acceptLang));
        }
    }


}
