using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erebus.Model;

namespace Erebus.Core.Implementations
{
    public class VaultHandler : IVaultHandler
    {
        private Vault Vault;
        private IClockProvider ClockProvider;

        public VaultHandler(Vault vault, IClockProvider clockProvider)
        {
            GuardClauses.ArgumentIsNotNull(nameof(vault), vault);
            GuardClauses.ArgumentIsNotNull(nameof(clockProvider), clockProvider);

            Vault = vault;
            ClockProvider = clockProvider;
        }

        public Group GetGroupById(Guid groupId)
        {
            return GetGroup(groupId, this.Vault.Groups);
        }

        public void AddGroup(Guid? parentGroupId,Group group)
        {
            GuardClauses.ArgumentIsNotNull(nameof(group), group);
            group.CreatedAt = this.ClockProvider.GetNow();
            group.UpdatedAt = this.ClockProvider.GetNow();

            if (parentGroupId == null)
            {
                Vault.Groups.Add(group);
            }
            else
            {
                var parentGroup = GetGroup(parentGroupId.Value, Vault.Groups);
                if (parentGroup == null) throw new ArgumentException("Group not found", nameof(parentGroupId));
                parentGroup.Groups.Add(group);
            }
        }


        private Group GetGroup(Guid groupId, IEnumerable<Group> groups)
        {
            if (groups == null) return null;

            var foundGroup = groups.FirstOrDefault(x => x.Id == groupId);
            if (foundGroup != null) return foundGroup;

            foreach (var group in groups)
            {
                foundGroup = GetGroup(groupId, group.Groups);
                if (foundGroup != null) return foundGroup;
            }

            return null;
        }


        public void AddEntry(Guid groupId, Entry entry)
        {
            GuardClauses.ArgumentIsNotNull(nameof(entry), entry);

            var group = GetGroup(groupId, Vault.Groups);
            if (group == null) throw new ArgumentException("Group not found", nameof(groupId));

            entry.CreatedAt = this.ClockProvider.GetNow();
            entry.UpdatedAt = this.ClockProvider.GetNow();
            group.Entries.Add(entry);

        }


        private Entry GetEntry(Guid entryId, IEnumerable<Group> groups)
        {
            if (groups == null) return null;

            var foundEntry = groups.SelectMany(x=> x.Entries).FirstOrDefault(x => x.Id == entryId);
            if (foundEntry != null) return foundEntry;

            foreach (var group in groups)
            {
                foundEntry = GetEntry(entryId, group.Groups);
                if (foundEntry != null) return foundEntry;
            }

            return null;
        }

        public Entry GetEntryById(Guid entryId)
        {
            return GetEntry(entryId, Vault.Groups);
        }
    }
}
