﻿using System.Web;
using System.Web.Optimization;

namespace WR.Blog.Mvc
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/Scripts/tiny_mce/tiny_mce_js").Include(
                        "~/Scripts/tiny_mce/tiny_mce_src.js",
                        "~/Scripts/tiny_mce/tiny_mce_init.js"));

            // Scripts and styles for syntax highlighting
            bundles.Add(new ScriptBundle("~/bundles/syntax").Include(
                        "~/Scripts/syntaxhighlighter/scripts/shCore.js",
                        "~/Scripts/syntaxhighlighter/scripts/shBrushCSharp.js",
                        "~/Scripts/syntaxhighlighter/scripts/shBrushCss.js",
                        "~/Scripts/syntaxhighlighter/scripts/shBrushJScript.js",
                        "~/Scripts/syntaxhighlighter/scripts/shBrushPlain.js",
                        "~/Scripts/syntaxhighlighter/scripts/shBrushSql.js",
                        "~/Scripts/syntaxhighlighter/scripts/shBrushVb.js",
                        "~/Scripts/syntaxhighlighter/scripts/shBrushXml.js",
                        "~/Scripts/syntaxhighlighter/scripts/initSyntax.js"));

            // Skeleton
            bundles.Add(new StyleBundle("~/Content/css/styles").Include(
                "~/content/css/base.css",
                "~/content/css/skeleton.css",
                "~/content/css/layout.css",
                "~/Content/css/Site.css"));


            bundles.Add(new StyleBundle("~/Content/css/syntax").Include(
                        "~/Scripts/syntaxhighlighter/styles/shCoreRDark.css",
                        "~/Scripts/syntaxhighlighter/styles/shThemeRDark.css"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

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