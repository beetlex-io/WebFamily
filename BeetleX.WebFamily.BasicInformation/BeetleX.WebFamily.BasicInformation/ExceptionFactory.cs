using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.WebFamily.BasicInformation
{
    public class ExceptionFactory
    {
        public static void DELETE_SYSTEM_DATA_ERROR()
        {
            throw new Exception("系统数据无法删除!");
        }

        public static void USER_ADD_EMAIL_EXISTS()
        {
            throw new Exception("邮件地址已存在!");
        }

        public static void ROLS_ADD_NAME_EXISTS()
        {
            throw new Exception("角色名称已存在!");
        }

        public static void USER_ADD_WORKNUMBER_EXISTS()
        {
            throw new Exception("用户编号已存在!");
        }

        public static void PERMISSION_ADD_CODE_EXISTS()
        {
            throw new Exception("权限码已存在!");
        }
    }
}
