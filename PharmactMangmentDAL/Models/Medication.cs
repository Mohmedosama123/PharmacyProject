﻿using System.ComponentModel.DataAnnotations;

namespace PharmactMangmentDAL.Models
{
    public class Medication : Base
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "يجب إدخال اسم الدواء.")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز الوصف 500 حرف.")]
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "يجب أن يكون السعر قيمة موجبة.")]
        public decimal Price { get; set; }

        public string Category { get; set; }

        public string? ImageName { get; set; }
    }
}
