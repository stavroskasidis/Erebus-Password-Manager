using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erebus.Model;

namespace Erebus.Core.Implementations
{
    public class VaultManipulator : IVaultManipulator
    {
        private Vault Vault;
        private IClockProvider ClockProvider;

        public VaultManipulator(Vault vault, IClockProvider clockProvider)
        {
            GuardClauses.ArgumentIsNotNull(nameof(vault), vault);
            GuardClauses.ArgumentIsNotNull(nameof(clockProvider), clockProvider);

            Vault = vault;
            ClockProvider = clockProvider;
        }

        public Group GetGroupById(Guid groupId)
        {
            var result = GetGroup(groupId, this.Vault);
            if (result == null) return null;
            return result.Group;
        }

        public void AddGroup(Guid? parentGroupId, Group group)
        {
            GuardClauses.ArgumentIsNotNull(nameof(group), group);
            group.CreatedAt = this.ClockProvider.GetNow();
            group.UpdatedAt = this.ClockProvider.GetNow();
            group.Version++;

            if (parentGroupId == null)
            {
                Vault.Groups.Add(group);
            }
            else
            {
                var getParentResult = GetGroup(parentGroupId.Value, Vault);
                if (getParentResult == null) throw new ArgumentException("Group not found", nameof(parentGroupId));
                getParentResult.Group.Groups.Add(group);
            }
        }

        public void UpdateGroup(Group group)
        {
            GuardClauses.ArgumentIsNotNull(nameof(group), group);

            var result = GetGroup(group.Id, Vault);
            if (result == null) throw new ArgumentException("Group not found", nameof(group));

            result.Group.UpdatedAt = this.ClockProvider.GetNow();
            result.Group.Version++;
            result.Group.Name = group.Name;
        }


        private GetGroupResult GetGroup(Guid groupId, IGroupContainer parent)
        {
            if (parent.Groups == null) return null;

            var foundGroup = parent.Groups.FirstOrDefault(x => x.Id == groupId);
            if (foundGroup != null) return new GetGroupResult(foundGroup, parent);

            foreach (var group in parent.Groups)
            {
                var result = GetGroup(groupId, group);
                if (result != null) return result;
            }

            return null;
        }

        public void DeleteGroupById(Guid groupId)
        {
            var result = this.GetGroup(groupId, Vault);
            if (result == null) throw new ArgumentException("Group not found", nameof(groupId));
            result.Parent.Groups.Remove(result.Group);
        }

        public void AddEntry(Guid groupId, Entry entry)
        {
            GuardClauses.ArgumentIsNotNull(nameof(entry), entry);

            var result = GetGroup(groupId, Vault);
            if (result == null) throw new ArgumentException("Group not found", nameof(groupId));

            entry.CreatedAt = this.ClockProvider.GetNow();
            entry.UpdatedAt = this.ClockProvider.GetNow();
            entry.Version++;
            result.Group.Entries.Add(entry);

        }

        public Entry GetEntryById(Guid entryId)
        {
            var result = GetEntry(entryId, Vault);
            if (result == null) return null;
            return result.Entry;
        }

        private GetEntryResult GetEntry(Guid entryId, IGroupContainer groupContainer)
        {
            if (groupContainer.Groups == null) return null;
            foreach (var group in groupContainer.Groups)
            {
                var foundEntry = group.Entries.FirstOrDefault(x => x.Id == entryId);
                if (foundEntry != null) return new GetEntryResult(foundEntry, group);

                var result = GetEntry(entryId, group);
                if (result != null) return result;
            }

            return null;
        }

        public void UpdateEntry(Entry entry)
        {
            GuardClauses.ArgumentIsNotNull(nameof(entry), entry);

            var result = GetEntry(entry.Id, Vault);
            if (result == null) throw new ArgumentException("Entry not found", nameof(entry));

            var foundEntry = result.Entry;

            foundEntry.UpdatedAt = this.ClockProvider.GetNow();
            foundEntry.Version++;
            foundEntry.Title = entry.Title;
            foundEntry.UserName = entry.UserName;
            foundEntry.Password = entry.Password;
            foundEntry.Url = entry.Url;
            foundEntry.Description = entry.Description;

        }

        public void DeleteEntryById(Guid entryId)
        {
            var result = this.GetEntry(entryId, Vault);
            if (result == null) throw new ArgumentException("Entry not found", nameof(entryId));
            result.Parent.Entries.Remove(result.Entry);
        }
    }
}
