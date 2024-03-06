using System.ComponentModel.DataAnnotations;

namespace transaction_api.DTOs
{
    public class CreateClientDTO
    {
        [Required(AllowEmptyStrings =false, ErrorMessage ="Name is Required"), MinLength(1, ErrorMessage = "Name is Required")]
        public  string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Surname is Required"), MinLength(1, ErrorMessage = "Surname is Required")]
        public  string Surname { get; set; }
        [Required(ErrorMessage = "Please provide an opening balance")]
        public  decimal ClientBalance { get; set; }
    }
}
