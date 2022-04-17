using MyShop.Models;

namespace MyShop.DTOs
{
    public class InLoanDto
    {
        public string OutDate { get; set; }
        public List<Loan> Lonas { get; set;  }
    }
}
