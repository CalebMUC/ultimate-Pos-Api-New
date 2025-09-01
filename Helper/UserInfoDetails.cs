using Ultimate_POS_Api.DTOS;

namespace Ultimate_POS_Api.Helper
{
    public class UserInfoDetails
    {
        public string strUser;
        public string MachineIP;
        public ushort uAccessLevel;
        public DateTime dtLastAccessTime;
        public bool IsAdmin;
        public string SessionID;

        //public UserInfoDetails(string strUser, string MachineIP, ushort uAccessLevel, bool IsAdmin)
        //   : this(strUser, MachineIP, uAccessLevel, IsAdmin)
        //{

        //}


        public UserInfoDetails(string strUser, string MachineIP, ushort uAccessLevel,  bool IsAdmin)
        {

            this.strUser = strUser;
            this.MachineIP = MachineIP;
            this.uAccessLevel = uAccessLevel;
            this.dtLastAccessTime = DateTime.Now;
            this.IsAdmin = IsAdmin;
            //this.SessionID = strSystem;

        }

        public userdetails UserInfodet() { 
            return new userdetails();
        } 

    }
}
