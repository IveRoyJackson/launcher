using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeyoCraft.Reference
{
    public enum ExchangeCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        SUCCESSFUL,
        /////ERROR//////
        /// <summary>
        /// 超时
        /// </summary>
        TIME_OUT,
        /// <summary>
        /// 失败
        /// </summary>
        FAIL,
        /// <summary>
        /// 账号不存在
        /// </summary>
        ID_DOEST_EXIST,
        /// <summary>
        /// 密码错误
        /// </summary>
        WRONG_PASSWORD,
        /// <summary>
        /// 账号已存在
        /// </summary>
        ID_EXIST,
        /// <summary>
        /// 无法写入
        /// </summary>
        CANT_WRITE,
        /// <summary>
        /// 用户退出
        /// </summary>
        CANCEL,
        /////OW/////////
        /// <summary>
        /// 一声问候
        /// </summary>
        HELLO,
        /// <summary>
        /// 登陆
        /// </summary>
        LOGIN,
        /// <summary>
        /// 注册
        /// </summary>
        SIGNUP,
        /// <summary>
        /// 获取用户信息
        /// </summary>
        GET_INFORMATION,
        /// <summary>
        /// 设置用户信息
        /// </summary>
        SET_INFORMATION,
        /// <summary>
        /// 获取用户头像
        /// </summary>
        GET_AVATAR,
        /// <summary>
        /// 设置用户头像
        /// </summary>
        SET_AVATAR,
        /// <summary>
        /// 获取联系人
        /// </summary>
        GET_CONTACTS,
        /// <summary>
        /// 设置联系人
        /// </summary>
        SET_CONTACTS,
        /// <summary>
        /// 搜索联系人
        /// </summary>
        SEARCH_CONTACTS,
        /// <summary>
        /// 发送消息
        /// </summary>
        SEND_MESSAGE,
        /// <summary>
        /// 获取消息
        /// </summary>
        GET_MESSAGE,
        /// <summary>
        /// 发送消息
        /// </summary>
        SEND_IMAGE,
        /// <summary>
        /// 获取消息
        /// </summary>
        GET_IMAGE,
        //////Return//////
        /// <summary>
        /// 有新消息
        /// </summary>
        HAS_NEW_MESSAGES,
        /// <summary>
        /// 联系人信息发生变化
        /// </summary>
        CONTACT_INFORMATION_CHANGE,
    }
    public class ExchangeHelper
    {
        /// <summary>
        /// 传入服务端返回数据，判断是否成功
        /// </summary>
        /// <param name="result">服务端返回数据</param>
        /// <returns></returns>
        public static bool IsSuccessful(string result)
        {
            return (ExchangeCode)int.Parse(result.Substring(0, 4)) == ExchangeCode.SUCCESSFUL ? true : false;
        }
        /// <summary>
        /// 将文本转为ExchangeCode
        /// </summary>
        /// <param name="s">需要转换的文本</param>
        /// <returns>转换后的ExchangeCode</returns>
        public static ExchangeCode ToExchangeCode(string s)
        {
            try
            {
                return (ExchangeCode)int.Parse(s.Substring(0, 4));
            }
            catch
            {
                return ExchangeCode.FAIL;
            }
            
        }
        /// <summary>
        /// 把ExchangeCode转换成文本
        /// </summary>
        /// <param name="exchangeCode">需要转换的ExchangeCode</param>
        /// <returns>转换后的文本</returns>
        public static string ToString(ExchangeCode exchangeCode)
        {
            return ((int)exchangeCode).ToString("0000");
        }
    }
}
