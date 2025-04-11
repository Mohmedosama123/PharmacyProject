using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmactMangmentDAL.Models
{
    public class Pharmacy : Base
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "يجب إدخال اسم الصيدلية.")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز اسم الصيدلية 100 حرف.")]
        public string NameOfPharmacy { get; set; }


        [Required(ErrorMessage = "يجب إدخال اسم صاحب الصيدلية.")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز اسم صاحب الصيدلية 100 حرف.")]
        public string OwnerName { get; set; }



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



        [Required(ErrorMessage = "يجب إدخال رقم الهاتف.")]
        [StringLength(20, ErrorMessage = "يجب ألا يتجاوز رقم الهاتف 20 رقمًا.")]
        [RegularExpression(@"^\+?\d{8,20}$", ErrorMessage = "يجب إدخال رقم هاتف صحيح.")]
        public string PhoneNumber { get; set; }



        [StringLength(20, ErrorMessage = "يجب ألا يتجاوز رقم الهاتف الآخر 20 رقمًا.")]
        [RegularExpression(@"^\+?\d{8,20}$", ErrorMessage = "يجب إدخال رقم هاتف صحيح.")]
        public string OtherPhoneNumber { get; set; } = "123";



        [Required(ErrorMessage = "يجب إدخال البريد الإلكتروني.")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز البريد الإلكتروني 100 حرف.")]
        [EmailAddress(ErrorMessage = "يجب إدخال بريد إلكتروني صالح.")]
        public string Email { get; set; }



        [StringLength(500, ErrorMessage = "يجب ألا تتجاوز ساعات العمل 500 حرف.")]
        public string OperatingHours { get; set; }


        [Required(ErrorMessage = "يجب إدخال رقم رخصه الصيدلية.")]
        public string License_Number { get; set; }

        [Required]
        public string userId { get; set; } // مفتاح أجنبي لـ AspNetUsers


        public ICollection<Med_Phar> Med_Phars { get; set; } = new HashSet<Med_Phar>();

        public ICollection<Medication> Medications { get; set; } = new HashSet<Medication>();


        public string ImageName { get; set; }= "default.png"; // اسم الصورة الافتراضية



    }
}
