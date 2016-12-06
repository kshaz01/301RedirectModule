using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSource.RedirectModule
{
   public class ContentSearchReader
    {
        /// <summary>
        /// Find all redirect url
        /// </summary>
        /// <param name="language">language</param>
        /// <returns>IList<Item></returns>
       public static IList<Item> FindRedirectUrl(Language language)
        {
            // Sitecore.Configuration.Settings.GetSetting(Constants.Settings.RedirectRootNode)
            using (var indexSearchContext = ContentSearchManager.GetIndex(Sitecore.Configuration.Settings.GetSetting(Constants.Settings.RedirectsWebIndex)).CreateSearchContext())
            {
                var output = indexSearchContext.GetQueryable<SearchResultItem>()
                            .Where(x => x.TemplateId == ID.Parse(Sitecore.Configuration.Settings.GetSetting(Constants.Settings.RedirectUrlTemplateID))
                            && x.Language == language.Name);

                return SearchContextHelper.IndexToItems(output.ToList());
            }
        }

       /// <summary>
       /// Find all redirect pattern
       /// </summary>
       /// <param name="language">language</param>
       /// <returns>IList<Item></returns>
       public static IList<Item> FindRedirectPattern(Language language)
       {
           using (var indexSearchContext = ContentSearchManager.GetIndex(Sitecore.Configuration.Settings.GetSetting(Constants.Settings.RedirectsWebIndex)).CreateSearchContext())
           {
               var output = indexSearchContext.GetQueryable<SearchResultItem>()
                           .Where(x => x.TemplateId == ID.Parse(Sitecore.Configuration.Settings.GetSetting(Constants.Settings.RedirectPatternTemplateID))
                           && x.Language == language.Name);

               return SearchContextHelper.IndexToItems(output.ToList());
           }
       }


       /// <summary>
       /// Find an item id
       /// </summary>
       /// <param name="itemId">itemId</param>
       /// <returns>Item<Item></returns>
       /// <param name="language">language</param> 
       public static Item FindItembyId(ID itemId, Language language)
       {
           using (var indexSearchContext = ContentSearchManager.GetIndex(Sitecore.Configuration.Settings.GetSetting(Constants.Settings.SitecoreWebIndex)).CreateSearchContext())
           {
               var output = indexSearchContext.GetQueryable<SearchResultItem>()
                           .Where(x => x.ItemId == itemId
                             && x.Language == language.Name);

               return SearchContextHelper.IndexToItems(output.ToList()).FirstOrDefault();
           }
       }

       /// <summary>
       /// Find an item path
       /// </summary>
       /// <param name="itemId">itemId</param>
       /// <returns>Item<Item></returns>
       public static Item FindItembyPath(string path)
       {
           using (var indexSearchContext = ContentSearchManager.GetIndex(Sitecore.Configuration.Settings.GetSetting(Constants.Settings.SitecoreWebIndex)).CreateSearchContext())
           {
               var output = indexSearchContext.GetQueryable<SearchResultItem>()
                           .Where(x => x.Path == path);

               return SearchContextHelper.IndexToItems(output.ToList()).FirstOrDefault();
           }
       }
    }
}
