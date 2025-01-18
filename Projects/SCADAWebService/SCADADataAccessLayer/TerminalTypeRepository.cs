using DomainDataEntities;
using SCADADataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCADADataAccessLayer
{
    public interface ITerminalTypeRepository : ISCADARepository<UnitItem>
    {
        IList<TerminalType> GetTerminals(string accountId);

    }

    public class TerminalTypeRepository : SCADARepository<UnitItem>, ITerminalTypeRepository
    {
        public IList<TerminalType> GetTerminals(string accountId)
        {
            using (var context = new SCADADbContext())
            {
                IQueryable<int> terminalIdes = from accountTerminal in context.AccountTerminals
                                               where accountId == accountTerminal.AccountId
                                               select accountTerminal.TerminalId;
                var terminals = from terminal in context.Terminals
                                where terminalIdes.Contains(terminal.Id) //&& terminal.IsStandard == false
                                select new { terminal.Id, terminal.Name };

                IList<TerminalType> terminalTypes = new List<TerminalType>();

                foreach (var item in terminals)
                {
                    terminalTypes.Add(new TerminalType(item.Id, item.Name));
                }
                return terminalTypes;
            }
        }
        
    }
}
