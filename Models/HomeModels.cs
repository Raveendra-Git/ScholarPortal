using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScholarPortal.Models
{
    public class HomeModels
    {
     
    }
    public class CountyNames
    {
        public int ID { get; set; }
        public string CountryName { get; set; }
    }

    public class States
    {
        public int ID { get; set; }
        public string StateName { get; set; }
    }

    public class Registerinfo
    {
        public int ID { get; set; }
        public string ParticipantID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public Int64 PhoneNo { get; set; }
        public int Gender { get; set; }
        public string Language { get; set; }
        public string OtherLanguage { get; set; }
        public string Address { get; set; }

        //[DataType(DataType.Upload)]
        //[Display(Name = "Upload File")]
        //[Required(ErrorMessage = "Please choose file to upload.")]
        public string file { get; set; }
        public string Picture { get; set; }
        public int UserType { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
    }

    public class LoginSession
    {
        public string Name { get; set; }
        public int UserType { get; set; }
    }
}