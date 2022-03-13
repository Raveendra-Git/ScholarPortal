using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Web;

namespace ScholarPortal.Models
{
    public class AdminModels
    {
    }

    public class UserDetails
    {
        public long ID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public long PhoneNo { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Gender { get; set; }
        public Int32 Age { get; set; }
        public string Picture { get; set; }
        public string LastModifiedDate { get; set; }
        public long RowNum { get; set; }
        public int RowCnt { get; set; }
    }

    public class UsersReport
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string DOB { get; set; }
        public Int32 Age { get; set; }
        public string Email { get; set; }
        public long PhoneNo { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Gender { get; set; }
        
    }

    public class Stafflist
    {
        public long ID { get; set; }
        public string StaffName { get; set; }
    }

    public class SubjectList
    {
        public long ID { get; set; }
        public string SubjectCode { get; set; }
        public string SubName { get; set; }

        public long RowNum { get; set; }
        public int RowCnt { get; set; }
    }
    public class StaffSubject
    {
        public List<StaffInfo> stafflist { get; set; }
        public List<SubjectInfo> subjectlist { get; set; }
    }
    public class StaffInfo
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }

    public class SubjectInfo
    {
        public long SubjectID { get; set; }
        public string SubjectName { get; set; }
    }

    public class StaffwithSubList
    {
        public long ID { get; set; }
        public string StaffName { get; set; }
        public string SubName { get; set; }

        public long RowNum { get; set; }
        public int RowCnt { get; set; }
    }
}