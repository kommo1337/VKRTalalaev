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
    
    public partial class Counterparty
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Counterparty()
        {
            this.Goods = new HashSet<Goods>();
            this.Operation = new HashSet<Operation>();
        }
    
        public int IdCounterparty { get; set; }
        public string CounterpartyName { get; set; }
        public byte[] PasportPhoto { get; set; }
        public Nullable<int> INN { get; set; }
        public Nullable<int> KPP { get; set; }
        public Nullable<int> OGRN { get; set; }
        public int IdAdress { get; set; }
        public int FinancialAccaunt { get; set; }
    
        public virtual Adress Adress { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Goods> Goods { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Operation> Operation { get; set; }
    }
}
