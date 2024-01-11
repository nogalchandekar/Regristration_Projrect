

namespace Regristration_Project.Models
{
    public class DisplayModel
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
