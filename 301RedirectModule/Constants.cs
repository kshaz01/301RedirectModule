using System;
using System.ComponentModel;

namespace SharedSource.RedirectModule
{
    public static class Constants
    {
        public static class Paths
        {
            public static string VisitorIdentification = "/layouts/system/visitoridentification";
            public static string MediaLibrary = "/sitecore/media library/";
        }

        public static class Settings
        {
            public static string RedirExactMatch = "SharedSource.RedirectModule.RedirectionType.ExactMatch";
            public static string RedirPatternMatch = "SharedSource.RedirectModule.RedirectionType.Pattern";
            public static string QueryExactMatch = "SharedSource.RedirectModule.QueryType.ExactMatch";
            public static string QueryPatternMatch = "SharedSource.RedirectModule.QueryType.PatternMatch";
            public static string RedirectRootNode = "SharedSource.RedirectModule.RedirectRootNode";
            public static string RedirectsWebIndex = "SharedSource.RedirectModule.Redirects.Web.Index";
            public static string SitecoreWebIndex = "SharedSource.RedirectModule.Sitecore.Web.Index";

            public static string RedirectFolderTemplateID = "SharedSource.RedirectModule.Redirect.Folder.Template.ID";
            public static string RedirectPatternTemplateID = "SharedSource.RedirectModule.Redirect.Pattern.Template.ID";
            public static string RedirectStatusTemplateID = "SharedSource.RedirectModule.Redirect.Status.Template.ID";
            public static string RedirectUrlTemplateID = "SharedSource.RedirectModule.Redirect.Url.Template.ID";
            public static string ResponseStatusCodeTemplateID = "SharedSource.RedirectModule.Response.Status.Code.Template.ID";

        }
        public static class Templates
        {
            public static string RedirectUrl = "Redirect Url";
            public static string VersionedRedirectUrl = "Versioned Redirect Url";
            public static string RedirectPattern = "Redirect Pattern";
            public static string VersionedRedirectPattern = "Versioned Redirect Pattern";
            public static string ResponseStatusCodeFolder = "Response Status Code Folder";
            public static string ResponseStatusCode = "Response Status Code";
            public static readonly Guid RedirectFolderTemplateID = new Guid("{8BEF76F0-DEAA-4939-B9C1-F357E1875D5D}");
            public static readonly Guid RedirectPatternTemplateID = new Guid("{94AC4F3A-E888-4557-9504-4AD2560ACC12}");
            public static readonly Guid RedirectStatusTemplateID = new Guid("{F050D923-541E-4B50-BC43-5E1569F08A36}");
            public static readonly Guid RedirectUrlTemplateID = new Guid("{B5967A68-7F70-42D3-9874-0E4D001DBC20}");
            public static readonly Guid ResponseStatusCodeTemplateID = new Guid("{72E10D68-E414-45AD-824E-758B1711B763}");

        }

        public static class Fields
        {
            public static string RequestedUrl = "Requested Url";
            public static string RedirectToUrl = "redirect to url";
            public static string RedirectTo = "Redirect To";
            public static string RequestedExpression = "requested expression";
            public static string SourceItem = "source item";
            public static string ItemProcessRedirects = "Items Which Always Process Redirects";
            public static string ResponseStatusCode = "Response Status Code";
            public static string StatusCode = "Status Code";
            public static string StatusDescription = "Status Description";
            
        }

    }
}