//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VKRTalalaev.DataFolder
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employer
    {
        public int IdEmployer { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Therdname { get; set; }
        public string Phone { get; set; }
        public byte[] Photo { get; set; }
        public byte[] PhotoPasport { get; set; }
        public int IdAdress { get; set; }
        public int IdGender { get; set; }
        public string Email { get; set; }
        public int IdUser { get; set; }

        public string FullName
        {
            get
            {
                return $"{Surname} {Name} {Therdname}";
            }
        }

        public virtual Adress Adress { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual User User { get; set; }
    }
}
