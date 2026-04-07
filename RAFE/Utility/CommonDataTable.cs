using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAFE.Utility
{
    public class jQueryDataTableParamModel
    {
        public string sEcho { get; set; }
        public string sSearch { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public int iColumns { get; set; }
        public int iSortingCols { get; set; }
        public string sColumns { get; set; }
    }

    public class CommonDataTable
    {
        public string draw { get; set; }
        public int? Skip { get; set; }
        public int? PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public long TotalCount { get; set; }
        public string ID { get; set; }
        public string CMD { get; set; }
        public string SearchText { get; set; }
    }

}