using System.ComponentModel.DataAnnotations;

namespace PharmactMangmentEditeIdea.ViewModel
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "UserName Is Required")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "ConfirmPassword Is Required")]
        [DataType(DataType.Password)]

        [Compare("Password", ErrorMessage = "Password And ConfirmPassword Do Not Match")]
        public string ConfirmPassword { get; set; }


        // =================== Pharmacy ===================
        [Required(ErrorMessage = "يجب إدخال اسم الصيدلية.")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز اسم الصيدلية 100 حرف.")]
        public string NameOfPharmacy { get; set; }


        [Required(ErrorMessage = "يجب إدخال اسم صاحب الصيدلية.")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز اسم صاحب الصيدلية 100 حرف.")]
        public string OwnerName { get; set; }


        [Required(ErrorMessage = "يجب إدخال رقم رخصه الصيدلية.")]

        public string License_Number { get; set; }

        [Required(ErrorMessage = "يجب إدخال رقم الهاتف.")]
        [StringLength(20, ErrorMessage = "يجب ألا يتجاوز رقم الهاتف 20 رقمًا.")]
        [RegularExpression(@"^\+?\d{8,20}$", ErrorMessage = "يجب إدخال رقم هاتف صحيح.")]
        public string PhoneNumber { get; set; }



        [StringLength(500, ErrorMessage = "يجب ألا تتجاوز ساعات العمل 500 حرف.")]
        public string OperatingHours { get; set; }



        [Required(ErrorMessage = "يجب إدخال العنوان.")]
        [StringLength(200, ErrorMessage = "يجب ألا يتجاوز العنوان 200 حرف.")]
        public string FullAddress { get; set; }



        [Required(ErrorMessage = "يجب إدخال اسم المحافظة.")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز اسم المحافظة 50 حرف.")]
        public string Governorate { get; set; }



        [Required(ErrorMessage = "يجب إدخال اسم الحي.")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز اسم الحي 50 حرف.")]
        public string District { get; set; }




        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز اسم المنطقة 50 حرف.")]
        public string Area { get; set; }




        public bool IsAgree { get; set; }

    }
}
