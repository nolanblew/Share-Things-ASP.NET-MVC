//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShareThings.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Request
    {
        public int id { get; set; }
        public int requestor_id { get; set; }
        public int item_id { get; set; }
        public string itemWant { get; set; }
        public bool isAccepted { get; set; }
        public int location_id { get; set; }
        public bool isCheckedOut { get; set; }
        public System.DateTime dateRequested { get; set; }
        public System.DateTime dateRecieved { get; set; }
        public System.DateTime dateReturned { get; set; }
        public System.DateTime dateWantedReturn { get; set; }
        public int giverRating { get; set; }
        public int recieverRating { get; set; }
    }
}