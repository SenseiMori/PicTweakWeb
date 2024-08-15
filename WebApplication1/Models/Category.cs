using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{

                    //Здесь создается модель для объявления будущей таблицы и миграции ее в базу данных 
    public class Category
    {
        // Объявление первичного ключа
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [RegularExpression(@"^[a-zA-Zа-яА-Я]+$", ErrorMessage = "Поле может содержать только буквы.")]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range (1,100)]
        public int DisplayOrder  { get; set; }


    }
}
