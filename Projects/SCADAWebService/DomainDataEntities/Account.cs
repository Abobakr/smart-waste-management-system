//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DomainDataEntities
{
    using System;
    using System.Collections.Generic;

    public partial class Account : IEntityObjectState
    {
        public Account()
        {
            this.AccountTerminals = new HashSet<AccountTerminal>();
        }

        public string Id { get; set; }
        public double LastSavedLatitude { get; set; }
        public double LastSavedLongitude { get; set; }

        public virtual ICollection<AccountTerminal> AccountTerminals { get; set; }
        public EntityObjectState ObjectState { get; set; }
    }
}
