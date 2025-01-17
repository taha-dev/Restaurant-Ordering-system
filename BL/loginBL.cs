﻿using Restaurant_Ordering_System.DL;
using Restaurant_Ordering_System.DTO;
using Restaurant_Ordering_System.GUI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_Ordering_System.BL
{
    class loginBL
    {
        loginDL logindl = new loginDL();
        EncryptorDecryptorBL edbl = new EncryptorDecryptorBL();
        public Form verifyUser(loginDTO logindto)
        {
            try
            {
                logindto.Password = edbl.EncryptData(logindto.Password);
                userDTO ud = logindl.VerifyUserFromDB(logindto);
                if (ud == null)
                {
                    return new outputFormGUI("Wrong User");
                }
                if(ud.Status == "blocked")
                {
                    return new outputFormGUI("Your account is blocked.");
                }
                if (ud.Role == "admin")
                {
                    SessionBL.setUserInSession(ud.Id.ToString());
                    return new adminGUI();
                }
                else if (ud.Role == "Manager")
                {
                    SessionBL.setUserInSession(ud.Id.ToString());
                    return new managerGUI(ud);
                }
                else if (ud.Role == "chef")
                {
                    return new chefGUI();
                }
                else if (ud.Role == "waiter")
                {
                    SessionBL.setUserInSession(ud.Id.ToString());
                    return new waiterGUI();
                }
                else
                {
                    return new outputFormGUI("Wrong User");
                }
            }
            catch (SqlException ex)
            {
                throw ex;

            }
            return new outputFormGUI("Wrong User");
        }
    }
}
