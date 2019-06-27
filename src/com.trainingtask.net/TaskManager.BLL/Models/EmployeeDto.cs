namespace TaskManager.BLL.Models
{
    public class EmployeeDto
    {
        public int? Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Position { get; set; }

        public string FullName { get; set; }

        public int IsDeleted { get; set; }

        public override string ToString()
        {
            return $"Id {Id}, FirstName {FirstName}, LastName {LastName}, MiddleName {MiddleName}, Position {Position}";
        }
    }
}