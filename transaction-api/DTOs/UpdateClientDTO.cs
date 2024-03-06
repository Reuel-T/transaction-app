using System.ComponentModel.DataAnnotations;

namespace transaction_api.DTOs
{
    public class UpdateClientDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Provide an ID")]
        public int ClientID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is Required"), MinLength(1, ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Surname is Required"), MinLength(1, ErrorMessage = "Surname is Required")]
        public string Surname { get; set; }
    }
}
