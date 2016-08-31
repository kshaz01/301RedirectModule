using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSource.RedirectModule
{
    public class SearchContextHelper
    {
        // <summary>
        /// Convert the index list to an item list
        /// </summary>
        /// <param name="indexList">Items</param>
        /// <returns>IList<Item></returns>
        public static IList<Item> IndexToItems(IList<SearchResultItem> indexList)
        {
            return indexList.Select(x => x.GetItem()).Where(x => x != null && x.Name != "__Standard Values").ToList();
        }
    }
}
