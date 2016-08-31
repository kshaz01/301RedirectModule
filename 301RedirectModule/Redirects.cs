using System.Web;
using Sitecore;
using Sitecore.Data;
using System;
using Sitecore.Data.Items;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using Sitecore.Links;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;

namespace SharedSource.RedirectModule
{
    /// <summary>
    ///  Redirection Module which handles 301 redirects.  Both exact matches and regular expression pattern matches are supported.
    /// </summary>
    public class Redirects : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (args.LocalPath != Constants.Paths.VisitorIdentification)
            {
                // Grab the actual requested path for use in both the item and pattern match sections.
                var requestedUrl = HttpContext.Current.Request.Url.ToString();
                var requestedPath = HttpContext.Current.Request.Url.AbsolutePath;
                var requestedPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;

                // First, we check for exact matches because those take priority over pattern matches.
                if (Sitecore.Configuration.Settings.GetBoolSetting(Constants.Settings.RedirExactMatch, true))
                {
                    try
                    {
                        // Loop through the exact match entries to look for a match.
                        foreach (var possibleRedirect in ContentSearchReader.FindRedirectUrl(Context.Language))
                        {
                            //Sitecore.Diagnostics.Log.Info(string.Format("### RedirectUrl: exact match: {0}", possibleRedirect.ID), this);
                            //Sitecore.Diagnostics.Log.Info(string.Format("### RedirectUrl: RequestedUrl: {0}", possibleRedirect[Constants.Fields.RequestedUrl]), this);
                            //Sitecore.Diagnostics.Log.Info(string.Format("### RedirectUrl: RedirectTo: {0}", possibleRedirect[Constants.Fields.RedirectTo]), this);

                            if (possibleRedirect.Versions.Count > 0)
                            {
                                if (requestedUrl.Equals(possibleRedirect[Constants.Fields.RequestedUrl], StringComparison.OrdinalIgnoreCase) ||
                                 requestedPath.Equals(possibleRedirect[Constants.Fields.RequestedUrl], StringComparison.OrdinalIgnoreCase) ||
                                Regex.IsMatch(requestedUrl, possibleRedirect[Constants.Fields.RequestedUrl], RegexOptions.IgnoreCase))
                                {
                                    var redirectTo = possibleRedirect.Fields[Constants.Fields.RedirectTo];
                                    if (redirectTo.HasValue && !string.IsNullOrEmpty(redirectTo.ToString()))
                                    {
                                        Sitecore.Diagnostics.Log.Info(string.Format("### 301 Redirects Found"), this);
                                        Sitecore.Diagnostics.Log.Info(string.Format("### 301 Redirects from {0} to {1}", requestedUrl, redirectTo), this);
                                        var responseStatus = GetResponseStatus(possibleRedirect);
                                        SendResponse(redirectTo.Value, HttpContext.Current.Request.Url.Query, responseStatus, args);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error(string.Format("### SharedSource.RedirectModule : FindRedirectUrl {0}", ex), this);
                    }

                }

                // Finally, we check for pattern matches because we didn't hit on an exact match.
                if (Sitecore.Configuration.Settings.GetBoolSetting(Constants.Settings.RedirPatternMatch, true))
                {
                    try
                    {
                        // Loop through the pattern match items to find a match
                        foreach (var possibleRedirectPattern in ContentSearchReader.FindRedirectPattern(Context.Language))
                        {
                            //Sitecore.Diagnostics.Log.Info(string.Format("### RedirectPattern: didn't hit on an exact match: {0}", possibleRedirectPattern.ID), this);
                            //Sitecore.Diagnostics.Log.Info(string.Format("### RedirectPattern: RequestedExpression: {0}", possibleRedirectPattern[Constants.Fields.RequestedExpression]), this);
                            //Sitecore.Diagnostics.Log.Info(string.Format("### RedirectPattern: SourceItem: {0}", possibleRedirectPattern[Constants.Fields.SourceItem]), this);

                            if (possibleRedirectPattern.Versions.Count > 0)
                            {
                                var redirectPath = string.Empty;
                                if (Regex.IsMatch(requestedUrl, possibleRedirectPattern[Constants.Fields.RequestedExpression], RegexOptions.IgnoreCase))
                                {

                                    redirectPath = Regex.Replace(requestedUrl, possibleRedirectPattern[Constants.Fields.RequestedExpression],
                                                                 possibleRedirectPattern[Constants.Fields.SourceItem], RegexOptions.IgnoreCase);

                                }
                                else if (Regex.IsMatch(requestedPathAndQuery, possibleRedirectPattern[Constants.Fields.RequestedExpression], RegexOptions.IgnoreCase))
                                {
                                    redirectPath = Regex.Replace(requestedPathAndQuery,
                                                                 possibleRedirectPattern[Constants.Fields.RequestedExpression],
                                                                 possibleRedirectPattern[Constants.Fields.SourceItem], RegexOptions.IgnoreCase);
                                }

                                if (string.IsNullOrEmpty(redirectPath)) continue;

                                // Query portion gets in the way of getting the sitecore item.
                                var pathAndQuery = redirectPath.Split('?');
                                var path = pathAndQuery[0];
                                if (LinkManager.Provider != null &&
                                    LinkManager.Provider.GetDefaultUrlOptions() != null &&
                                    LinkManager.Provider.GetDefaultUrlOptions().EncodeNames)
                                {
                                    path = MainUtil.DecodeName(path);
                                }
                                var redirectToItem = ContentSearchReader.FindItembyId(ID.Parse(redirectPath));
                                if (redirectToItem != null)
                                {
                                    Sitecore.Diagnostics.Log.Info(string.Format("### 301 Redirects Found"), this);
                                    Sitecore.Diagnostics.Log.Info(string.Format("### 301 Redirects from {0} to {1}", requestedUrl, redirectToItem), this);
                                    var query = pathAndQuery.Length > 1 ? "?" + pathAndQuery[1] : "";
                                    var responseStatus = GetResponseStatus(possibleRedirectPattern);
                                    SendResponse(redirectToItem, query, responseStatus, args);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error(string.Format("### SharedSource.RedirectModule : FindRedirectPattern {0}", ex), this);
                    }

                }
            }
        }

        /// <summary>
        ///  Once a match is found and we have a Sitecore Item, we can send the 301 response.
        /// </summary>
        private static void SendResponse(Item redirectToItem, string queryString, ResponseStatus responseStatus, HttpRequestArgs args)
        {
            var redirectToUrl = GetRedirectToUrl(redirectToItem);
            SendResponse(redirectToUrl, queryString, responseStatus, args);
        }

        private static void SendResponse(string redirectToUrl, string queryString, ResponseStatus responseStatusCode, HttpRequestArgs args)
        {
            args.Context.Response.Status = responseStatusCode.Status;
            args.Context.Response.StatusCode = responseStatusCode.StatusCode;
            args.Context.Response.AddHeader("Location", redirectToUrl + queryString);
            args.Context.Response.End();
        }

        private static string GetRedirectToUrl(Item redirectToItem)
        {
            if (redirectToItem.Paths.Path.StartsWith(Constants.Paths.MediaLibrary))
            {
                var mediaItem = (MediaItem)redirectToItem;
                var mediaUrl = MediaManager.GetMediaUrl(mediaItem);
                var redirectToUrl = StringUtil.EnsurePrefix('/', mediaUrl);
                return redirectToUrl;
            }

            return LinkManager.GetItemUrl(redirectToItem);
        }

        private static ResponseStatus GetResponseStatus(Item redirectItem)
        {
            var result = new ResponseStatus
            {
                Status = "301 Moved Permanently",
                StatusCode = 301,
            };

            try
            {
                if (redirectItem != null)
                {
                    var responseStatusCodeId = redirectItem.Fields[Constants.Fields.ResponseStatusCode];

                    if (responseStatusCodeId.HasValue && !string.IsNullOrEmpty(responseStatusCodeId.ToString()))
                    {
                        var responseStatusCodeItem = redirectItem.Database.GetItem(ID.Parse(responseStatusCodeId));
                        if (responseStatusCodeItem != null)
                        {
                            result.Status = responseStatusCodeItem.Fields[Constants.Fields.StatusDescription].Value;
                            result.StatusCode = responseStatusCodeItem.GetIntegerFieldValue(Constants.Fields.StatusCode, result.StatusCode);
                        }
                    }
                }

                return result;
            }
            catch( Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(string.Format("### SharedSource.RedirectModule GetResponseStatus {0}", ex), new object());
                
                return result;
            }
            
        }
    }
}
