using System.Linq.Expressions;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Addressbook.Group
{
    public class GroupBuilder : IDisposable
    {
        private GroupModel Entity { get; set; }

        public GroupBuilder()
        {
            Entity = new GroupModel();
        }

        public GroupBuilder(string groupCode)
        {
            Entity = new GroupModel()
            { 
                GroupCode = groupCode
            };
        }

        #region Dispose

        public void Dispose()
        {
            Entity = null;
        }

        #endregion

        #region Options

        /// <summary>
        /// Set group name
        /// </summary>
        /// <param name="groupName">Group Name</param>
        public GroupBuilder SetGroupName(string groupName)
        {
            Entity.GroupName = groupName;

            return this;
        }

        /// <summary>
        /// Set group subaccount
        /// </summary>
        /// <param name="subaccount">SubAccount</param>
        public GroupBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        /// <summary>
        /// Set group department
        /// </summary>
        /// <param name="department"></param>
        public GroupBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        /// <summary>
        /// Sets the visibility and edit permissions for the group. 
        /// </summary>
        /// <param name="viewEditBy">Enums.ViewEditByOptions</param>
        /// <returns></returns>
        public GroupBuilder SetViewEditBy(Enums.ViewEditByOptions viewEditBy)
        {
            Entity.ViewEditBy = viewEditBy;

            return this;
        }

        public void Set<T>(Expression<Func<T, object>> propertyExpression, object value)
        {
            Expression<Func<GroupModel, object>> convertedExpression = ExpressionHelper.ConvertExpressionParameterType<T, GroupModel>(propertyExpression);
            PropertyHelper.SetProperty(Entity, convertedExpression, value);
        }
        #endregion

        #region Build / BuildAsync

        /// <summary>
        /// Build Group
        /// </summary>
        /// <returns>Group</returns>
        public GroupModel Build()
        {
            return Entity;
        }

        /// <summary>
        /// Build Group Async
        /// </summary>
        /// <returns>Task<Group></returns>
        public async Task<GroupModel> BuildAsync()
        {
            return await Task.Run(() =>
            {
                return Entity;
            });
        }
        #endregion Build / BuildAsync
    }
}
