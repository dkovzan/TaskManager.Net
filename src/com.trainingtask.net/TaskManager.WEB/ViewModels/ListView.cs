using System.Collections.Generic;

namespace TaskManager.WEB.ViewModels
{
    public class ListView<T> where T : class
    {
        public IEnumerable<T> EntitiesPerPageList { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}