using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace Regristration_Project.Models
{
    public class RegristerModel
    {
        [Required(ErrorMessage ="Name  required")]
        public string Name { get; set; }

        [Required(ErrorMessage ="RollNo Required")]
        public string RollNo { get; set; }


        [Required(ErrorMessage = "Age Required")]
        [Range(18, 60, ErrorMessage = "Age must be between 18 and 60")]
        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public decimal Fees { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }

        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string EmailId { get; set; }
    }

    public class UpdateModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string RollNo { get; set; }


        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public decimal Fees { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }

        public string EmailId { get; set; }
    }

}
