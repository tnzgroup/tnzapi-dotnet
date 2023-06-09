using System.Linq.Expressions;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
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

        #region Dispose

        public void Dispose()
        {
            Entity = null;
        }

        #endregion

        #region Options

        /// <summary>
        /// Set group code
        /// </summary>
        /// <param name="groupCode">Group Code</param>
        public void SetGroupCode(string groupCode)
        {
            Entity.GroupCode = groupCode;
        }

        /// <summary>
        /// Set group name
        /// </summary>
        /// <param name="groupName">Group Name</param>
        public void SetGroupName(string groupName)
        {
            Entity.GroupName = groupName;
        }

        /// <summary>
        /// Set group subaccount
        /// </summary>
        /// <param name="subaccount">SubAccount</param>
        public void SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;
        }

        /// <summary>
        /// Set group department
        /// </summary>
        /// <param name="department"></param>
        public void SetDepartment(string department)
        {
            Entity.Department = department;
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
