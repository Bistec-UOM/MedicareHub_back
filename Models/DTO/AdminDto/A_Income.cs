using Models; // Assuming your Prescription class is in the Models namespace

namespace Models.DTO.AdminDto
{
    public class A_Income
    {
        public int Id { get; set; }
        public float Total { get; set; }
        public DateTime DateTime { get; set; }
    }

    public static class IncomeMapper
    {
        public static A_Income MapToDTO(Prescription prescription)
        {
            // Create a new Income DTO and set its properties
            A_Income incomeDTO = new A_Income
            {
                Id = prescription.Id,
                DateTime = prescription.DateTime,
                Total = prescription.Total
            };

            return incomeDTO;
        }
    }
}
