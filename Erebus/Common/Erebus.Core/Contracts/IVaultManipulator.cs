using Erebus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Contracts
{
    public interface IVaultManipulator
    {
        Group GetGroupById(Guid groupId);
        void AddGroup(Guid? parentGroupId, Group group);
        void UpdateGroup(Group group);
        void DeleteGroupById(Guid groupId);
        void AddEntry(Guid groupId, Entry entry);
        void UpdateEntry(Entry entry);
        void DeleteEntryById(Guid entryId);
        Entry GetEntryById(Guid entryId);
    }
}
