using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Addressbook.Group
{
    public sealed class GroupBuilder : IDisposable
    {
        private GroupModel Entity { get; set; }

        public GroupBuilder()
        {
            Entity = new GroupModel();
        }

        public void Dispose()
        {
            Entity = new GroupModel();
        }

        public GroupBuilder SetGroupName(string groupName)
        {
            Entity.GroupName = groupName;

            return this;
        }

        public GroupBuilder SetSubAccount(string subAccount)
        {
            Entity.SubAccount = subAccount;

            return this;
        }

        public GroupBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        public GroupBuilder SetViewEditBy(Enums.ViewEditByOptions viewEditBy)
        {
            Entity.ViewEditBy = viewEditBy;

            return this;
        }

        public GroupBuilder SetAccessControl(Enums.AccessControlLevel accessControl)
        {
            Entity.AccessControl = accessControl;

            return this;
        }

        public GroupModel Build()
        {
            return Entity;
        }
    }
}