﻿using System.Web;
using System.Web.Optimization;

namespace PapiroMVC
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/bundles/wizardCss").Include(
                "~/Content/acetheme/assets/css/select2.css"));

            bundles.Add(new ScriptBundle("~/bundles/wizard").Include(
                "~/Content/acetheme/assets/js/fuelux/fuelux.wizard.min.js",
                "~/Content/acetheme/assets/js/additional-methods.min.js",
                "~/Content/acetheme/assets/js/bootbox.min.js",
                "~/Content/acetheme/assets/js/jquery.maskedinput.min.js",
                "~/Content/acetheme/assets/js/select2.min.js"));

            /*            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                            "~/Scripts/jquery.unobtrusive-ajax.js",
                            "~/Scripts/jquery.validate.js",
                            "~/Scripts/jquery.validate.unobtrusive.js",
                            "~/Scripts/autocomplete.js"));
                        */

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/autocomplete").Include("~/Scripts/autocomplete.js"));


            /*
                        #region jqgrid
                        bundles.Add(new ScriptBundle("~/bundles/jqgrid").Include(
                                    "~/Scripts/jquery.jqGrid.min.js",
                                    "~/Scripts/i18n/grid.locale-en.js"));

                        bundles.Add(new StyleBundle("~/Content/jqGridcss").Include(
                            "~/Content/jquery.jqGrid/ui.jqgrid.css",
                            "~/Content/themes/smoothness/jquery-ui-1.8.custom.css"));
                        #endregion
            */

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                //           "~/Content/Site.css",        
                //         "~/MetroUI/css/modern.css",
                        "~/Content/DivShowHide.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}