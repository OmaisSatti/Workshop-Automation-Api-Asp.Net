//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vehicle_Workshop_Automation_Api.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mechanic_Service
    {
        public int Id { get; set; }
        public string Mechanic_Mobile_No { get; set; }
        public string Vehicle_Reg_No { get; set; }
        public Nullable<int> Service_Id { get; set; }
        public Nullable<int> Surveyor_Id { get; set; }
    }
}
