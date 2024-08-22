using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluacionGonzagaJavier.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del producto no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre del Producto")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, 9999999.99, ErrorMessage = "El precio debe estar entre 0.01 y 9,999,999.99.")]
        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Precio Unitario")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        [Display(Name = "Cantidad")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El descuento es obligatorio.")]
        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100.")]
        [Column(TypeName = "decimal(4, 2)")]
        [Display(Name = "Descuento")]
        [DataType(DataType.Currency)]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Monto de Descuento")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Total")]
        public decimal Total { get; set; }

        public void CalculateTotals()
        {
            Subtotal = UnitPrice * Quantity;
            DiscountAmount = Subtotal * (Discount / 100);
            Total = Subtotal - DiscountAmount;
        }
    }
}