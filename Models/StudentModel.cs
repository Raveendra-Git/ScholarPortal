using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScholarPortal.Models
{
    public class Questionslist
    {
        public Int64 QuestionNum { get; set; }
        public string Question { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
    }

    public class StudentInfo
    {
        public string FullName{ get; set; }
        public int Score { get; set; }
    }
}