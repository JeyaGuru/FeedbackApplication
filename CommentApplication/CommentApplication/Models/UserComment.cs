//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CommentApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTimeOffset> CreatedTime { get; set; }
        public Nullable<System.DateTimeOffset> UpdatedTime { get; set; }
        public int UserId { get; set; }
    
        public virtual User User { get; set; }
    }
}
